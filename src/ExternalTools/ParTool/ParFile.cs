using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TF.IO;

namespace ParTool
{
    public partial class ParFile
    {
        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        private ParFile()
        {
        }

        public static void Extract(string inputPath, string outputFolder)
        {
            Directory.CreateDirectory(outputFolder);

            var parFile = new ParFile();
            parFile.ReadInfo(inputPath);

            if (parFile.Root != null)
            {
                parFile.Dump(inputPath, outputFolder);
            }
        }

        public static void Repack(string inputFolder, string outputPath, bool useCompression)
        {
            var dir = Path.GetDirectoryName(outputPath);
            Directory.CreateDirectory(dir);

            ParFile.Pack(inputFolder, outputPath, useCompression);
        }
    }

    // Extraction
    public partial class ParFile
    {
        [Flags]
        private enum FileFlags : uint
        {
            None = 0u,
            IsCompressed = 1u << 31,
        }

        private uint Magic { get; set; }    // 0x50415243 (PARC)
        private uint UnknownA { get; set; } // 0x02010000
        private uint UnknownB { get; set; } // 0x00020001
        private uint UnknownC { get; set; } // 0x00000000

        private ParFolderInfo Root { get; set; }

        private class ParFolderInfo
        {
            public string Name { get; set; }
            public uint Index { get; set; }

            public uint FolderCount { get; set; }
            public uint FolderIndex { get; set; }
            public uint FileCount { get; set; }
            public uint FileIndex { get; set; }
            public uint UnknownE { get; set; }
            public uint UnknownF { get; set; }
            public uint UnknownG { get; set; }
            public uint UnknownH { get; set; }

            public List<ParFolderInfo> Folders { get; }
            public List<ParFileInfo> Files { get; }

            public List<uint> FoldersId { get; }
            public List<uint> FilesId { get; }

            public ParFolderInfo()
            {
                Folders = new List<ParFolderInfo>();
                Files = new List<ParFileInfo>();

                FoldersId = new List<uint>();
                FilesId = new List<uint>();
            }
        }

        private class ParFileInfo
        {
            public string Name { get; set; }
            public uint Index { get; set; }

            public FileFlags CompressionFlags { get; set; }
            public uint UncompressedSize { get; set; }
            public uint CompressedSize { get; set; }
            public uint FileOffset { get; set; }
            public uint UnknownE { get; set; }
            public uint UnknownF { get; set; }
            public uint UnknownG { get; set; }
            public uint UnknownH { get; set; }

            public bool IsCompressed()
            {
                return CompressionFlags.HasFlag(FileFlags.IsCompressed);
            }
        }

        private void ReadInfo(string inputPath)
        {
            using (var fs = new FileStream(inputPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var input = new ExtendedBinaryReader(fs, Encoding.GetEncoding(1252), Endianness.BigEndian))
            {
                Magic = input.ReadUInt32();

                if (Magic != 0x50415243)
                {
                    throw new InvalidDataException("El fichero no es un PAR válido");
                }

                UnknownA = input.ReadUInt32();
                UnknownB = input.ReadUInt32();
                UnknownC = input.ReadUInt32();

                var folderCount = input.ReadUInt32();
                var folderTableOffset = input.ReadUInt32();
                var fileCount = input.ReadUInt32();
                var fileTableOffset = input.ReadUInt32();

                var folderNamesOffset = 32u;
                var fileNamesOffset = folderNamesOffset + folderCount * 64u;

                if (folderCount > 0)
                {
                    // Siempre hay al menos un directorio
                    Root = ReadFolderInfo(0, input, folderNamesOffset, folderTableOffset);
                    ProcessFolder(Root, input, folderNamesOffset, folderTableOffset, fileNamesOffset, fileTableOffset);
                }
            }
        }

        private static ParFolderInfo ReadFolderInfo(uint index, ExtendedBinaryReader input, uint namesOffset, uint infoOffset)
        {
            input.Seek(namesOffset + index * 64, SeekOrigin.Begin);

            var folder = new ParFolderInfo
            {
                Name = input.ReadString(),
                Index = index
            };

            input.Seek(infoOffset + index * 32, SeekOrigin.Begin);

            folder.FolderCount = input.ReadUInt32();
            folder.FolderIndex = input.ReadUInt32();
            folder.FileCount = input.ReadUInt32();
            folder.FileIndex = input.ReadUInt32();
            folder.UnknownE = input.ReadUInt32();
            folder.UnknownF = input.ReadUInt32();
            folder.UnknownG = input.ReadUInt32();
            folder.UnknownH = input.ReadUInt32();

            return folder;
        }

        private static void ProcessFolder(ParFolderInfo folder, ExtendedBinaryReader input, uint folderNamesOffset, uint folderTableOffset, uint fileNamesOffset, uint fileTableOffset)
        {
            for (uint i = 0; i < folder.FileCount; i++)
            {
                var file = ReadFileInfo(folder.FileIndex + i, input, fileNamesOffset, fileTableOffset);
                folder.Files.Add(file);
            }

            for (uint i = 0; i < folder.FolderCount; i++)
            {
                var f = ReadFolderInfo(folder.FolderIndex + i, input, folderNamesOffset, folderTableOffset);
                ProcessFolder(f, input, folderNamesOffset, folderTableOffset, fileNamesOffset, fileTableOffset);

                folder.Folders.Add(f);
            }
        }

        private static ParFileInfo ReadFileInfo(uint index, ExtendedBinaryReader input, uint namesOffset, uint infoOffset)
        {
            input.Seek(namesOffset + index * 64, SeekOrigin.Begin);

            var f = new ParFileInfo
            {
                Name = input.ReadString(),
                Index = index
            };

            input.Seek(infoOffset + index * 32, SeekOrigin.Begin);

            f.CompressionFlags = (FileFlags)input.ReadUInt32();
            f.UncompressedSize = input.ReadUInt32();
            f.CompressedSize = input.ReadUInt32();
            f.FileOffset = input.ReadUInt32();
            f.UnknownE = input.ReadUInt32();
            f.UnknownF = input.ReadUInt32();
            f.UnknownG = input.ReadUInt32();
            f.UnknownH = input.ReadUInt32();

            return f;
        }

        private void Dump(string inputPath, string outputFolder)
        {
            using (var fs = new FileStream(inputPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var input = new ExtendedBinaryReader(fs, Encoding.GetEncoding(1252), Endianness.BigEndian))
            {
                var logFile = System.IO.Path.Combine(outputFolder, "Extract_Data.tf");
                Dump(input, Root, outputFolder, logFile);
            }
        }

        private static void Dump(ExtendedBinaryReader input, ParFolderInfo folder, string parentFolder, string logFile)
        {
            _logger.Debug("Extracting {0}\\{1}", parentFolder, folder.Name);
            using (var log = new ExtendedBinaryWriter(new FileStream(logFile, FileMode.Append), Encoding.GetEncoding(1252), Endianness.BigEndian))
            {
                log.Write((byte)0);
                log.WriteString(folder.Name);
                log.Write(folder.Index);
                log.Write(folder.FolderCount);
                log.Write(folder.FolderIndex);
                log.Write(folder.FileCount);
                log.Write(folder.FileIndex);
                log.Write(folder.UnknownE);
                log.Write(folder.UnknownF);
                log.Write(folder.UnknownG);
                log.Write(folder.UnknownH);

                foreach (var folderInfo in folder.Folders)
                {
                    log.Write(folderInfo.Index);
                }

                foreach (var fileInfo in folder.Files)
                {
                    log.Write(fileInfo.Index);
                }
            }

            var outputFolder = Path.Combine(parentFolder, folder.Name);

            foreach (var f in folder.Files)
            {
                Dump(input, f, outputFolder, logFile);
            }
            
            foreach (var f in folder.Folders)
            {
                Dump(input, f, outputFolder, logFile);
            }
        }

        private static void Dump(ExtendedBinaryReader input, ParFileInfo file, string outputFolder, string logFile)
        {
            _logger.Debug("Extracting {0}\\{1}", outputFolder, file.Name);
            using (var log = new ExtendedBinaryWriter(new FileStream(logFile, FileMode.Append), Encoding.GetEncoding(1252), Endianness.BigEndian))
            {
                log.Write((byte)1);
                log.WriteString(file.Name);
                log.Write(file.Index);
                log.Write((uint)file.CompressionFlags);
                log.Write(file.UncompressedSize);
                log.Write(file.CompressedSize);
                log.Write(file.FileOffset);
                log.Write(file.UnknownE);
                log.Write(file.UnknownF);
                log.Write(file.UnknownG);
                log.Write(file.UnknownH);
            }

            Directory.CreateDirectory(outputFolder);

            var name = Path.Combine(outputFolder, file.Name);

            input.Seek(file.FileOffset, SeekOrigin.Begin);

            byte[] data;
            if (file.IsCompressed())
            {
                var compressedData = input.ReadBytes((int)file.CompressedSize);
                data = SllzCompressor.Decompress(compressedData);
            }
            else
            {
                data = input.ReadBytes((int)file.UncompressedSize);
            }

            File.WriteAllBytes(name, data);

            if (Path.GetExtension(name) == ".par")
            {
                ParFile.Extract(name, $"{name}.unpack");
            }
            else if (Path.GetExtension(name) == ".bin" && Path.GetFileNameWithoutExtension(name).StartsWith("pac_"))
            {
                //PacFile.Extract(name, $"{name}.unpack");
            }
        }
    }

    // Packing
    public partial class ParFile
    {
        private static void Pack(string inputFolder, string outputFile, bool useCompression)
        {
            var logFile = Path.Combine(inputFolder, "Extract_Data.tf");

            var folderDict = new Dictionary<uint, ParFolderInfo>();
            var fileDict = new Dictionary<uint, ParFileInfo>();

            LoadLogFile(logFile, folderDict, fileDict);

            var root = folderDict[0];

            using (var output = new ExtendedBinaryWriter(new FileStream(outputFile, FileMode.Create), Encoding.GetEncoding(1252), Endianness.BigEndian))
            {
                output.Write(0x50415243);
                output.Write(0x02010000);
                output.Write(0x00020001);
                output.Write(0x00000000);

                var headerSize = (uint)(32 + 64 * folderDict.Count + 64 * fileDict.Count);
                var folderTableOffset = headerSize;
                var fileTableOffset = (uint)(folderTableOffset + folderDict.Count * 32);
                var dataOffset = (uint)(fileTableOffset + fileDict.Count * 32);
                if (dataOffset % 2048 != 0)
                {
                    var padding = 2048 + (-dataOffset % 2048);
                    dataOffset += (uint)padding;
                }

                output.Write((uint)folderDict.Count);
                output.Write(folderTableOffset);
                output.Write((uint)fileDict.Count);
                output.Write(fileTableOffset);

                for (var i = 0u; i < folderDict.Count; i++)
                {
                    var f = folderDict[i];
                    output.WriteString(f.Name, 64);
                }

                for (var i = 0u; i < fileDict.Count; i++)
                {
                    var f = fileDict[i];
                    output.WriteString(f.Name, 64);
                }

                var blockSize = 0;
                Pack(root, output, inputFolder, folderDict, fileDict, folderTableOffset, fileTableOffset, dataOffset, useCompression, ref blockSize);

                output.Seek(0, SeekOrigin.End);
                output.WritePadding(2048);
            }
        }

        private static uint Pack(ParFolderInfo folder, ExtendedBinaryWriter output, string path,
            IDictionary<uint, ParFolderInfo> folderDict,
            IDictionary<uint, ParFileInfo> fileDict, uint folderTableOffset, uint fileTableOffset, uint dataOffset, bool useCompression, ref int blockSize)
        {
            _logger.Debug("Packing {0}\\{1}", path, folder.Name);
            var newPath = Path.Combine(path, folder.Name);

            output.Seek(folderTableOffset + folder.Index * 32, SeekOrigin.Begin);

            output.Write(folder.FolderCount);
            output.Write(folder.FolderIndex);
            output.Write(folder.FileCount);
            output.Write(folder.FileIndex);

            output.Write(folder.UnknownE);
            output.Write(folder.UnknownF);
            output.Write(folder.UnknownG);
            output.Write(folder.UnknownH);

            var newOffset = dataOffset;

            var compressedData = new ConcurrentDictionary<uint, CompressResult>();

            Parallel.ForEach(folder.FilesId, fileId =>
            {
                var f = fileDict[fileId];
                _logger.Debug("Packing {0}\\{1}", newPath, f.Name);
                var data = GetData(f, newPath, useCompression);
                compressedData[fileId] = data;
            });

            foreach (var fileId in folder.FilesId)
            {
                var f = fileDict[fileId];
                
                newOffset = Pack(f, compressedData[fileId], output, fileTableOffset, newOffset, useCompression, ref blockSize);
            }

            foreach (var folderId in folder.FoldersId)
            {
                var f = folderDict[folderId];
                newOffset = Pack(f, output, newPath, folderDict, fileDict, folderTableOffset, fileTableOffset, newOffset, useCompression, ref blockSize);
            }

            return newOffset;
        }

        private class CompressResult
        {
            public byte[] Data;
            public uint UncompressedSize;
        }

        private static CompressResult GetData(ParFileInfo file, string path, bool useCompression)
        {
            var result = new CompressResult();
            
            var newPath = Path.Combine(path, file.Name);

            if (Path.GetExtension(newPath) == ".par")
            {
                if (Directory.Exists($"{newPath}.unpack"))
                {
                    ParFile.Repack($"{newPath}.unpack", newPath, useCompression);
                }
            }
            else if (Path.GetExtension(newPath) == ".bin" && Path.GetFileNameWithoutExtension(newPath).StartsWith("pac_"))
            {
                if (Directory.Exists($"{newPath}.unpack"))
                {
                    //PacFile.Repack($"{newPath}.unpack", newPath, useCompression);
                }
            }

            if (useCompression && file.IsCompressed())
            {
                var uncompressedData = File.ReadAllBytes(newPath);
                result.Data = SllzCompressor.Compress(uncompressedData);
                result.UncompressedSize = (uint)uncompressedData.Length;
            }
            else
            {
                result.Data = File.ReadAllBytes(newPath);
                result.UncompressedSize = (uint)result.Data.Length;
            }

            return result;
        }

        private static uint Pack(ParFileInfo file, CompressResult compress, ExtendedBinaryWriter output, uint fileTableOffset, uint dataOffset, bool useCompression, ref int blockSize)
        {
            output.Seek(fileTableOffset + file.Index * 32, SeekOrigin.Begin);

            if (useCompression && file.IsCompressed())
            {
                output.Write((uint)FileFlags.IsCompressed);
                output.Write(compress.UncompressedSize);
            }
            else
            {
                output.Write((uint)FileFlags.None);
                output.Write(compress.UncompressedSize);
            }

            output.Write((uint)compress.Data.Length);

            if (compress.Data.Length > 2048)
            {
                blockSize = 2048 + (-compress.Data.Length % 2048);
                if (dataOffset % 2048 != 0)
                {
                    var padding = 2048 + (-dataOffset % 2048);
                    dataOffset += (uint)padding;
                }
            }
            else
            {
                if (compress.Data.Length < blockSize)
                {
                    blockSize -= compress.Data.Length;
                }
                else
                {
                    blockSize = 2048 + (-compress.Data.Length % 2048);
                    if (dataOffset % 2048 != 0)
                    {
                        var padding = 2048 + (-dataOffset % 2048);
                        dataOffset += (uint)padding;
                    }
                }
            }

            output.Write(dataOffset);

            output.Write(file.UnknownE);
            output.Write(file.UnknownF);
            output.Write(file.UnknownG);
            output.Write(file.UnknownH);

            output.Seek(dataOffset, SeekOrigin.Begin);

            output.Write(compress.Data);

            return (uint)output.Position;
        }

        private static void LoadLogFile(string logFileName, IDictionary<uint, ParFolderInfo> folderDict, IDictionary<uint, ParFileInfo> fileDict)
        {
            using (var log = new ExtendedBinaryReader(new FileStream(logFileName, FileMode.Open), Encoding.GetEncoding(1252), Endianness.BigEndian))
            {
                while (log.Position < log.Length)
                {
                    var type = log.ReadByte();
                    if (type == 0)
                    {
                        // folder
                        var folder = new ParFolderInfo
                        {
                            Name = log.ReadString(),
                            Index = log.ReadUInt32(),
                            FolderCount = log.ReadUInt32(),
                            FolderIndex = log.ReadUInt32(),
                            FileCount = log.ReadUInt32(),
                            FileIndex = log.ReadUInt32(),

                            UnknownE = log.ReadUInt32(),
                            UnknownF = log.ReadUInt32(),
                            UnknownG = log.ReadUInt32(),
                            UnknownH = log.ReadUInt32(),
                        };

                        for (var i = 0u; i < folder.FolderCount; i++)
                        {
                            var folderId = log.ReadUInt32();
                            folder.FoldersId.Add(folderId);
                        }

                        for (var i = 0u; i < folder.FileCount; i++)
                        {
                            var fileId = log.ReadUInt32();
                            folder.FilesId.Add(fileId);
                        }

                        folderDict.Add(folder.Index, folder);
                    }
                    else
                    {
                        var file = new ParFileInfo()
                        {
                            Name = log.ReadString(),
                            Index = log.ReadUInt32(),
                            CompressionFlags = (FileFlags)log.ReadUInt32(),
                            UncompressedSize = log.ReadUInt32(),
                            CompressedSize = log.ReadUInt32(),
                            FileOffset = log.ReadUInt32(),
                            UnknownE = log.ReadUInt32(),
                            UnknownF = log.ReadUInt32(),
                            UnknownG = log.ReadUInt32(),
                            UnknownH = log.ReadUInt32(),
                        };

                        fileDict.Add(file.Index, file);
                    }
                }
            }
        }
    }
}
