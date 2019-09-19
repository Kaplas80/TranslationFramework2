using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TF.IO;

namespace ParLib
{
    partial class ParFile
    {
        public void Save(string inputPath, string outputFile, CompressionType compression, bool recursive, bool lowMemory)
        {
            using (var output = new ExtendedBinaryWriter(new FileStream(outputFile, FileMode.Create), Encoding.GetEncoding(1252), Endianness.BigEndian))
            {
                output.Write(0x50415243);
                output.Write(0x02010000);
                output.Write(0x00020001);
                output.Write(0x00000000);

                var headerSize = (32 + 64 * TotalFolderCount + 64 * TotalFileCount);
                var folderTableOffset = headerSize;
                var fileTableOffset = (folderTableOffset + TotalFolderCount * 32);
                var dataOffset = (fileTableOffset + TotalFileCount * 32);
                if (dataOffset % 2048 != 0)
                {
                    var padding = 2048 + (-dataOffset % 2048);
                    dataOffset += padding;
                }

                output.Write(TotalFolderCount);
                output.Write(folderTableOffset);
                output.Write(TotalFileCount);
                output.Write(fileTableOffset);

                WriteFolderNames(output, RootFolder);
                WriteFilesNames(output, RootFolder);

                WriteFolderInfos(output, RootFolder);

                WriteFiles(output, inputPath, RootFolder, dataOffset, compression, recursive, lowMemory);
                
                output.Seek(0, SeekOrigin.End);
                output.WritePadding(2048);
            }
        }

        private static void WriteFolderNames(ExtendedBinaryWriter output, ParFolderInfo rootFolder)
        {
            var queue = new Queue<ParFolderInfo>();
            queue.Enqueue(rootFolder);
            while (queue.Count != 0)
            {
                var parFolder = queue.Dequeue();
                output.WriteString(parFolder.Name, 64);

                foreach (var childFolder in parFolder.Folders)
                {
                    queue.Enqueue(childFolder);
                }
            }
        }

        private static void WriteFilesNames(ExtendedBinaryWriter output, ParFolderInfo rootFolder)
        {
            var queue = new Queue<ParFolderInfo>();
            queue.Enqueue(rootFolder);
            while (queue.Count != 0)
            {
                var parFolder = queue.Dequeue();
                foreach (var file in parFolder.Files)
                {
                    output.WriteString(file.Name, 64);
                }

                foreach (var childFolder in parFolder.Folders)
                {
                    queue.Enqueue(childFolder);
                }
            }
        }

        private static void WriteFolderInfos(BinaryWriter output, ParFolderInfo rootFolder)
        {
            var queue = new Queue<ParFolderInfo>();
            queue.Enqueue(rootFolder);
            while (queue.Count != 0)
            {
                var parFolder = queue.Dequeue();
                output.Write(parFolder.Folders.Count);
                output.Write(parFolder.FirstFolderIndex);
                output.Write(parFolder.Files.Count);
                output.Write(parFolder.FirstFileIndex);
                output.Write(parFolder.Unknown1);
                output.Write(parFolder.Unknown2);
                output.Write(parFolder.Unknown3);
                output.Write(parFolder.Unknown4);
                foreach (var childFolder in parFolder.Folders)
                {
                    queue.Enqueue(childFolder);
                }
            }
        }

        private void WriteFiles(ExtendedBinaryWriter output, string inputPath, ParFolderInfo rootFolder, long offset, CompressionType compression, bool recursive, bool lowMemoryUsage)
        {
            if (lowMemoryUsage)
            {
                WriteFilesLowMemory(output, inputPath, rootFolder, offset, compression, recursive);
            }
            else
            {
                WriteFilesHighMemory(output, inputPath, rootFolder, offset, compression, recursive);
            }
        }

        private void WriteFilesLowMemory(ExtendedBinaryWriter output, string inputPath, ParFolderInfo rootFolder, long offset, CompressionType compression, bool recursive)
        {
            var blockSize = 0;

            var queue = new Queue<ParFolderInfo>();
            queue.Enqueue(rootFolder);
            while (queue.Count != 0)
            {
                var parFolder = queue.Dequeue();
                foreach (var file in parFolder.Files)
                {
                    var path = Path.Combine(inputPath, file.RelativePath);

                    if (recursive && file.Name.EndsWith(".par"))
                    {
                        var parFolderPath = $"{path}.unpack";
                        if (Directory.Exists(parFolderPath))
                        {
                            var parFile = ReadFolder(parFolderPath, false);
                            parFile?.Save(parFolderPath, path, compression, true, true);
                        }
                    }
                    
                    var readData = File.ReadAllBytes(path);
                    byte[] data;
                    var effectiveCompression = compression;

                    if (effectiveCompression == CompressionType.Default)
                    {
                        effectiveCompression = file.CompressionFlags == FileFlags.IsCompressed ? CompressionType.V1Compression : CompressionType.Uncompressed;
                    }

                    if (file.Name.EndsWith(".par"))
                    {
                        effectiveCompression = CompressionType.Uncompressed;
                    }

                    switch (effectiveCompression)
                    {
                        case CompressionType.Uncompressed:
                            output.Write((uint) FileFlags.None);
                            data = readData;
                            break;
                        case CompressionType.V1Compression:
                            output.Write((uint) FileFlags.IsCompressed);
                            data = SllzCompressor.Compress(readData);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(compression), compression, null);
                    }

                    CalculateOffset(data.Length, ref offset, ref blockSize);

                    output.Write(readData.Length);
                    output.Write(data.Length);
                    output.Write((uint)offset);
                    output.Write(file.Unknown1);
                    output.Write(file.Unknown2);
                    output.Write(file.Unknown3);
                    output.Write(file.Date);

                    var returnPosition = output.Position;
                    output.Seek(offset, SeekOrigin.Begin);
                    output.Write(data);
                    offset = output.Position;
                    output.Seek(returnPosition, SeekOrigin.Begin);
                }

                foreach (var childFolder in parFolder.Folders)
                {
                    queue.Enqueue(childFolder);
                }
            }
        }

        private void WriteFilesHighMemory(ExtendedBinaryWriter output, string inputPath, ParFolderInfo rootFolder, long offset, CompressionType compression, bool recursive)
        {
            var dictionary = new ConcurrentDictionary<string, Tuple<int, byte[]>>();
            var queue = new Queue<ParFolderInfo>();
            queue.Enqueue(rootFolder);

            // En la primera pasada, comprimo los ficheros (si hace falta)
            while (queue.Count != 0)
            {
                var parFolder = queue.Dequeue();
                Parallel.ForEach(parFolder.Files, file =>
                {
                    var path = Path.Combine(inputPath, file.RelativePath);

                    if (recursive && file.Name.EndsWith(".par"))
                    {
                        var parFolderPath = $"{path}.unpack";
                        if (Directory.Exists(parFolderPath))
                        {
                            var parFile = ReadFolder(parFolderPath, false);
                            parFile?.Save(parFolderPath, path, compression, true, false);
                            File.SetCreationTime(path, file.FileDate);
                            File.SetLastWriteTime(path, file.FileDate);
                        }
                    }

                    var readData = File.ReadAllBytes(path);
                    byte[] data;
                    var effectiveCompression = compression;

                    if (effectiveCompression == CompressionType.Default)
                    {
                        effectiveCompression = file.CompressionFlags == FileFlags.IsCompressed
                            ? CompressionType.V1Compression
                            : CompressionType.Uncompressed;
                    }

                    if (file.Name.EndsWith(".par"))
                    {
                        effectiveCompression = CompressionType.Uncompressed;
                    }

                    switch (effectiveCompression)
                    {
                        case CompressionType.Uncompressed:
                            data = readData;
                            break;
                        case CompressionType.V1Compression:
                            data = SllzCompressor.Compress(readData);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(compression), compression, null);
                    }

                    dictionary[path] = new Tuple<int, byte[]>(readData.Length, data);
                });

                foreach (var childFolder in parFolder.Folders)
                {
                    queue.Enqueue(childFolder);
                }
            }

            // En la segunda pasada es cuando escribo
            var blockSize = 0;
            queue = new Queue<ParFolderInfo>();
            queue.Enqueue(rootFolder);
            while (queue.Count != 0)
            {
                var parFolder = queue.Dequeue();
                foreach (var file in parFolder.Files)
                {
                    var path = Path.Combine(inputPath, file.RelativePath);

                    var (uncompressedLength, data) = dictionary[path];

                    CalculateOffset(data.Length, ref offset, ref blockSize);

                    var effectiveCompression = compression;

                    if (effectiveCompression == CompressionType.Default)
                    {
                        effectiveCompression = file.CompressionFlags == FileFlags.IsCompressed
                            ? CompressionType.V1Compression
                            : CompressionType.Uncompressed;
                    }

                    if (file.Name.EndsWith(".par"))
                    {
                        effectiveCompression = CompressionType.Uncompressed;
                    }

                    switch (effectiveCompression)
                    {
                        case CompressionType.Uncompressed:
                            output.Write((uint) FileFlags.None);
                            break;
                        case CompressionType.V1Compression:
                            output.Write((uint) FileFlags.IsCompressed);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(compression), compression, null);
                    }

                    output.Write(uncompressedLength);
                    output.Write(data.Length);
                    output.Write((uint)offset);
                    output.Write(file.Unknown1);
                    output.Write(file.Unknown2);
                    output.Write(file.Unknown3);
                    output.Write(file.Date);

                    var returnPosition = output.Position;
                    output.Seek(offset, SeekOrigin.Begin);
                    output.Write(data);
                    offset = output.Position;
                    output.Seek(returnPosition, SeekOrigin.Begin);
                }

                foreach (var childFolder in parFolder.Folders)
                {
                    queue.Enqueue(childFolder);
                }
            }
        }

        private static void CalculateOffset(int dataLength, ref long offset, ref int blockSize)
        {
            if (dataLength > 2048)
            {
                blockSize = 2048 + (-dataLength % 2048);
                if (offset % 2048 != 0)
                {
                    var padding = 2048 + (-offset % 2048);
                    offset += padding;
                }
            }
            else
            {
                if (dataLength < blockSize)
                {
                    blockSize -= dataLength;
                }
                else
                {
                    blockSize = 2048 + (-dataLength % 2048);
                    if (offset % 2048 != 0)
                    {
                        var padding = 2048 + (-offset % 2048);
                        offset += (uint)padding;
                    }
                }
            }
        }
    }
}
