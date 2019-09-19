using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using TF.IO;

namespace ParLib
{
    partial class ParFile
    {
		public void Extract(string outputPath, bool recursive = false)
        {
            var queue = new Queue<ParFolderInfo>();
            queue.Enqueue(RootFolder);

            using (var fs = new FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var input = new ExtendedBinaryReader(fs, Encoding.GetEncoding(1252), Endianness.BigEndian))
            {
                while (queue.Count != 0)
                {
                    var current = queue.Dequeue();
                    Logger.Debug("Processing \"{0}\"...", current.RelativePath);
                    foreach (var fileInfo in current.Files)
                    {

                        var outputFile = Path.Combine(outputPath, fileInfo.RelativePath);
                        Extract(input, fileInfo, outputFile);

                        if (recursive && fileInfo.Name.EndsWith(".par"))
                        {
                            var parFile = ParFile.ReadPar(outputFile);
                            parFile.Extract($"{outputFile}.unpack", true);
                        }
                    }

                    foreach (var child in current.Folders)
                    {
                        queue.Enqueue(child);
                    }
                }
            }

            var formatter = new BinaryFormatter {AssemblyFormat = FormatterAssemblyStyle.Simple};
            using (var stream = new FileStream($"{outputPath}\\ParInfo.tf", FileMode.Create))
            {
                formatter.Serialize(stream, this);
            }
        }

        private static void Extract(ExtendedBinaryReader input, ParFileInfo file, string path)
        {
            Logger.Debug("Extracting \"{0}\"...", file.RelativePath);
            input.Seek(file.Offset, SeekOrigin.Begin);
            var data = input.ReadBytes(file.CompressedSize);

            var outputPath = Path.GetDirectoryName(path);
            Directory.CreateDirectory(outputPath);

            if (file.CompressionFlags == FileFlags.IsCompressed)
            {
                var uncompressedData = SllzCompressor.Decompress(data);
                File.WriteAllBytes(path, uncompressedData);
            }
            else
            {
                File.WriteAllBytes(path, data);
            }

			File.SetCreationTime(path, file.FileDate);
            File.SetLastWriteTime(path, file.FileDate);
        }
    }
}
