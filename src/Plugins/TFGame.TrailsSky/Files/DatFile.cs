using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TF.IO;

namespace TFGame.TrailsSky.Files
{
    public class DatFile
    {
        private static byte[] DIR_HEADER = {0x4c, 0x42, 0x20, 0x44, 0x49, 0x52, 0x1A, 0x00};
        private static byte[] DAT_HEADER = {0x4c, 0x42, 0x20, 0x44, 0x41, 0x54, 0x1A, 0x00};

        public static void Extract(string inputPath, string outputFolder)
        {
            var inputFolder = Path.GetDirectoryName(inputPath);
            var fileName = Path.GetFileNameWithoutExtension(inputPath);

            var dirFile = Path.Combine(inputFolder, $"{fileName}.dir");

            if (!File.Exists(dirFile))
            {
                throw new FileNotFoundException($"No se ha encontrado el archivo {dirFile}");
            }

            Directory.CreateDirectory(outputFolder);

            var logFile = Path.Combine(outputFolder, "Extract_Data.tf");

            using (var dirFs = new FileStream(dirFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var dirInput = new ExtendedBinaryReader(dirFs, Encoding.GetEncoding(1252)))
            using (var datFs = new FileStream(inputPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var datInput = new ExtendedBinaryReader(datFs, Encoding.UTF8))
            using (var log = new ExtendedBinaryWriter(new FileStream(logFile, FileMode.Create), Encoding.GetEncoding(1252)))
            {
                var dirHeader = dirInput.ReadBytes(8);
                var datHeader = datInput.ReadBytes(8);

                if (!dirHeader.SequenceEqual(DIR_HEADER) || !datHeader.SequenceEqual(DAT_HEADER))
                {
                    throw new InvalidDataException("El fichero no es válido");
                }

                var maxNumFiles = dirInput.ReadInt64();
                log.Write(maxNumFiles);

                var currentPos = dirInput.Position;
                var endPos = dirInput.Length;

                while (currentPos < endPos)
                {
                    var name = dirInput.ReadString();
                    log.WriteString(name);

                    dirInput.Skip(3);
                    var compressedSize = dirInput.ReadInt32();
                    var maxSize = dirInput.ReadInt32();
                    var unknown2 = dirInput.ReadInt32();
                    var timestamp = dirInput.ReadInt32();

                    log.Write(compressedSize);
                    log.Write(maxSize);
                    log.Write(unknown2);
                    log.Write(timestamp);

                    var datOffset = dirInput.ReadInt32();
                    log.Write(datOffset);

                    if (datOffset != 0 && compressedSize != 0)
                    {
                        var outputFile = Path.Combine(outputFolder, name);

                        datInput.Seek(datOffset, SeekOrigin.Begin);
                        var data = datInput.ReadBytes(compressedSize);
                        try
                        {
                            var data2 = FalcomCompressor.Decompress(data);
                            File.WriteAllBytes(outputFile, data2);
                            log.Write(1); // Para saber si estaba comprimido o no
                        }
                        catch (Exception e)
                        {
                            File.WriteAllBytes(outputFile, data);
                            log.Write(0);
                        }
                    }
                    else
                    {
                        log.Write(0);
                    }

                    currentPos = dirInput.Position;
                }
            }
        }

        private class FileInfo
        {
            public string FileName;
            public int CompressedSize;
            public int MaxSize;
            public int Unknown;
            public int TimeStamp;
            public int OriginalOffset;
            public bool IsCompressed;
        }

        private static IList<FileInfo> ReadFileInfo(ExtendedBinaryReader log)
        {
            var result = new List<FileInfo>();

            while (log.Position < log.Length)
            {
                var fileName = log.ReadString();
                var originalCompressedSize = log.ReadInt32();
                var maxSize = log.ReadInt32();
                var unknown = log.ReadInt32();
                var timestamp = log.ReadInt32();
                var originalOffset = log.ReadInt32();
                var compressed = log.ReadInt32() == 1;

                var fi = new FileInfo
                {
                    FileName = fileName,
                    CompressedSize = originalCompressedSize,
                    MaxSize = maxSize,
                    Unknown = unknown,
                    TimeStamp = timestamp,
                    OriginalOffset = originalOffset,
                    IsCompressed = compressed
                };

                result.Add(fi);
            }

            return result;
        }
        public static void Repack(string inputFolder, string outputPath, bool useCompression)
        {
            var dir = Path.GetDirectoryName(outputPath);
            Directory.CreateDirectory(dir);

            var datFileName = Path.GetFileNameWithoutExtension(outputPath);
            var dirFile = Path.Combine(dir, $"{datFileName}.dir");

            var logFile = Path.Combine(inputFolder, "Extract_Data.tf");

            using (var dirOutput = new ExtendedBinaryWriter(new FileStream(dirFile, FileMode.Create), Encoding.GetEncoding(1252)))
            using (var datOutput = new ExtendedBinaryWriter(new FileStream(outputPath, FileMode.Create), Encoding.UTF8))
            using (var log = new ExtendedBinaryReader(new FileStream(logFile, FileMode.Open), Encoding.GetEncoding(1252)))
            {
                dirOutput.Write(DIR_HEADER);
                datOutput.Write(DAT_HEADER);

                var maxNumFiles = log.ReadInt64();
                dirOutput.Write(maxNumFiles);
                datOutput.Write(maxNumFiles);

                var files = ReadFileInfo(log);

                var compressedData = new ConcurrentDictionary<string, byte[]>();

                Parallel.ForEach(files, file =>
                {
                    if (file.CompressedSize > 0)
                    {
                        var fileToInsert = Path.Combine(inputFolder, file.FileName);
                        var data = GetData(fileToInsert, file.IsCompressed, useCompression);
                        compressedData[file.FileName] = data;
                    }
                });

                var fileOffset = 0x10 + maxNumFiles * 4 + 0x04; // Posición en la que hay que empezar a insertar los ficheros

                for (var i = 0; i < files.Count; i++)
                {
                    var file = files[i];

                    dirOutput.WriteString(file.FileName, 16);

                    datOutput.Seek(0x10 + i * 4, SeekOrigin.Begin);

                    if (file.OriginalOffset != 0)
                    {
                        datOutput.Write((int) fileOffset);
                    }
                    else
                    {
                        datOutput.Write(0);
                    }

                    if (file.CompressedSize == 0)
                    {
                        // Solo tiene ceros
                        dirOutput.Write(file.CompressedSize);
                        dirOutput.Write(file.MaxSize);
                        dirOutput.Write(file.Unknown);
                        dirOutput.Write(file.TimeStamp);
                        if (file.OriginalOffset != 0)
                        {
                            dirOutput.Write((int)fileOffset);
                        }
                        else
                        {
                            dirOutput.Write(0);
                        }

                        fileOffset += file.MaxSize;
                    }
                    else
                    {
                        datOutput.Seek(fileOffset, SeekOrigin.Begin);

                        var data = compressedData[file.FileName];

                        datOutput.Write(data);

                        dirOutput.Write(data.Length);

                        if (data.Length <= file.MaxSize)
                        {
                            dirOutput.Write(file.MaxSize);
                            dirOutput.Write(file.Unknown);
                            dirOutput.Write(file.TimeStamp);
                            dirOutput.Write((int)fileOffset);

                            fileOffset += file.MaxSize;
                        }
                        else
                        {
                            dirOutput.Write(file.MaxSize * 2);
                            dirOutput.Write(file.MaxSize * 2);
                            dirOutput.Write(file.TimeStamp);
                            dirOutput.Write((int)fileOffset);

                            fileOffset += file.MaxSize * 2;
                        }
                    }
                }
            }
        }

        private static byte[] GetData(string file, bool wasCompressed, bool useCompression)
        {
            if (useCompression && wasCompressed)
            {
                var uncompressedData = File.ReadAllBytes(file);
                var data = FalcomCompressor.Compress(uncompressedData);
                return data;
            }
            else
            {
                return File.ReadAllBytes(file);
            }
        }
    }
}
