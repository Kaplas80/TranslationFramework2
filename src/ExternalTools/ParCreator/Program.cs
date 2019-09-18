using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using TF.IO;

namespace ParCreator
{
    class Program
    {
        public static void Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        [Argument(0, "input")]
        [Required]
        public string Input { get; }

        [Option("-nc|--no-compression")]
        public bool NotUseCompression { get; }

        [Option("-v|--verbose")]
        public bool Verbose { get; }

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private void OnExecute()
        {
            ConfigureNLog();

            var useCompression = !NotUseCompression;

            var sw = new Stopwatch();

            var isDirectory = Directory.Exists(Input);

            if (isDirectory)
            {
                _logger.Info("MODE: CREATE");
                _logger.Info("INPUT: {0}", Input);
                _logger.Info("USE COMPRESSION: {0}", useCompression);

                var filename = Input.Replace(".unpack", string.Empty);

                sw.Start();
                CreateParFile(Input, filename, useCompression);

                sw.Stop();

                _logger.Info("Time elapsed: {0}ms", sw.ElapsedMilliseconds);
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }

            _logger.Error("{0} not found", Input);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private void ConfigureNLog()
        {
            var nLogConfig = new LoggingConfiguration();

            var logconsole = new ConsoleTarget("logconsole");
            logconsole.Layout = Layout.FromString("${message}");
            nLogConfig.AddRule(Verbose ? LogLevel.Debug : LogLevel.Info, LogLevel.Fatal, logconsole);

            LogManager.Configuration = nLogConfig;
        }

        private class ParFile
        {
            public string Path { get; set; }
            public string Name => System.IO.Path.GetFileName(Path);
            public byte[] UncompressedData { get; set; }
            public byte[] CompressedData { get; set; }

            public int UncompressedSize => UncompressedData.Length;
            public int CompressedSize => CompressedData.Length;

            public DateTime FileDate { get; set; }

            public uint Date
            {
                get
                {
                    var baseDate = new DateTime(1970, 1, 1);

                    return (uint)(FileDate - baseDate).TotalSeconds;
                }
            }
        }

        private class ParFolder
        {
            public int FirstFolderIndex { get; set; }
            public string Path { get; set; }
            public string Name { get; set; }
            public List<ParFolder> Folders { get; set; }

            public List<ParFile> Files { get; set; }

            public int TotalFoldersCount { get; set; }
            public int TotalFilesCount { get; set; }

            public int FirstFileIndex { get; set; }
            public ParFolder()
            {
                Folders = new List<ParFolder>();
                Files = new List<ParFile>();
                TotalFoldersCount = 0;
                TotalFilesCount = 0;
            }
        }

        private void CreateParFile(string inputFolder, string outputFile, bool useCompression)
        {
            var rootFolder = new ParFolder {Path = inputFolder, Name = "."};

            ProcessFolder(rootFolder);
            var totalFoldersCount = rootFolder.TotalFoldersCount + 1;
            var totalFilesCount = rootFolder.TotalFilesCount;

            using (var output = new ExtendedBinaryWriter(new FileStream(outputFile, FileMode.Create), Encoding.GetEncoding(1252), Endianness.BigEndian))
            {
                output.Write(0x50415243);
                output.Write(0x02010000);
                output.Write(0x00020001);
                output.Write(0x00000000);

                var headerSize = (32 + 64 * totalFoldersCount + 64 * totalFilesCount);
                var folderTableOffset = headerSize;
                var fileTableOffset = (folderTableOffset + totalFoldersCount * 32);
                var dataOffset = (fileTableOffset + totalFilesCount * 32);
                if (dataOffset % 2048 != 0)
                {
                    var padding = 2048 + (-dataOffset % 2048);
                    dataOffset += padding;
                }

                output.Write(totalFoldersCount);
                output.Write(folderTableOffset);
                output.Write(totalFilesCount);
                output.Write(fileTableOffset);

                WriteFolderNames(output, rootFolder);
                WriteFilesNames(output, rootFolder);

                WriteFolderInfos(output, rootFolder);

                WriteFiles(output, rootFolder, dataOffset, useCompression);
                
                output.Seek(0, SeekOrigin.End);
                output.WritePadding(2048);
            }
        }

        private void WriteFolderNames(ExtendedBinaryWriter output, ParFolder rootFolder)
        {
            var queue = new Queue<ParFolder>();
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

        private void WriteFilesNames(ExtendedBinaryWriter output, ParFolder rootFolder)
        {
            var queue = new Queue<ParFolder>();
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

        private void WriteFolderInfos(ExtendedBinaryWriter output, ParFolder rootFolder)
        {
            var queue = new Queue<ParFolder>();
            queue.Enqueue(rootFolder);
            while (queue.Count != 0)
            {
                var parFolder = queue.Dequeue();
                output.Write(parFolder.Folders.Count);
                output.Write(parFolder.FirstFolderIndex);
                output.Write(parFolder.Files.Count);
                output.Write(parFolder.FirstFileIndex);
                output.Write(0x10);
                output.Write(0x00);
                output.Write(0x00);
                output.Write(0x00);
                foreach (var childFolder in parFolder.Folders)
                {
                    queue.Enqueue(childFolder);
                }
            }
        }

        [Flags]
        private enum FileFlags : uint
        {
            None = 0u,
            IsCompressed = 1u << 31
        }

        private void WriteFiles(ExtendedBinaryWriter output, ParFolder rootFolder, long offset, bool useCompression)
        {
            var blockSize = 0;

            var queue = new Queue<ParFolder>();
            queue.Enqueue(rootFolder);
            while (queue.Count != 0)
            {
                var parFolder = queue.Dequeue();
                foreach (var file in parFolder.Files)
                {
                    byte[] data;
                    if (useCompression)
                    {
                        output.Write((uint) FileFlags.IsCompressed);
                        data = file.CompressedData;
                    }
                    else
                    {
                        output.Write((uint) FileFlags.None);
                        data = file.UncompressedData;
                    }
                    
                    if (data.Length > 2048)
                    {
                        blockSize = 2048 + (-data.Length % 2048);
                        if (offset % 2048 != 0)
                        {
                            var padding = 2048 + (-offset % 2048);
                            offset += padding;
                        }
                    }
                    else
                    {
                        if (data.Length < blockSize)
                        {
                            blockSize -= data.Length;
                        }
                        else
                        {
                            blockSize = 2048 + (-data.Length % 2048);
                            if (offset % 2048 != 0)
                            {
                                var padding = 2048 + (-offset % 2048);
                                offset += (uint)padding;
                            }
                        }
                    }

                    output.Write(file.UncompressedSize);
                    output.Write(useCompression ? file.CompressedSize : file.UncompressedSize);
                    output.Write((uint)offset);
                    output.Write(0x2020);
                    output.Write(0x00);
                    output.Write(0x00);
                    output.Write(file.Date);

                    var returnPosition = output.Position;
                    output.Seek(offset, SeekOrigin.Begin);
                    output.Write(useCompression ? file.CompressedData : file.UncompressedData);
                    offset = output.Position;
                    output.Seek(returnPosition, SeekOrigin.Begin);
                }

                foreach (var childFolder in parFolder.Folders)
                {
                    queue.Enqueue(childFolder);
                }
            }
        }

        private void ProcessFolder(ParFolder rootFolder)
        {
            var comparer = new LowerCaseComparer();
            var queue = new Queue<ParFolder>();
            queue.Enqueue(rootFolder);
            var folderIndex = 1;
            var fileIndex = 0;
            while (queue.Count != 0)
            {
                var parFolder = queue.Dequeue();
                parFolder.FirstFolderIndex = folderIndex;
                parFolder.FirstFileIndex = fileIndex;

                var folders = Directory.GetDirectories(parFolder.Path);
                Array.Sort(folders, comparer);
                var files = Directory.GetFiles(parFolder.Path);
                Array.Sort(files, comparer);

                rootFolder.TotalFoldersCount += folders.Length;
                rootFolder.TotalFilesCount += files.Length;

                foreach (var file in files)
                {
                    if (Path.GetFileName(file) == "Extract_Data.tf")
                    {
                        rootFolder.TotalFilesCount--;
                        continue;
                    }

                    var data = File.ReadAllBytes(file);
                    var parFile = new ParFile
                    {
                        Path = file,
                        UncompressedData = data,
                        FileDate = File.GetCreationTime(file)
                    };
                    parFolder.Files.Add(parFile);
                    fileIndex++;
                }

                Parallel.ForEach(parFolder.Files,
                    file =>
                    {
                        file.CompressedData = SllzCompressor.Compress(file.UncompressedData);
                    });

                folderIndex += folders.Length;
                foreach (var folder in folders)
                {
                    var childFolder = new ParFolder
                    {
                        Path = folder,
                        Name = Path.GetFileName(folder)
                    };

                    parFolder.Folders.Add(childFolder);
                    queue.Enqueue(childFolder);
                }
            }
        }

    }
}
