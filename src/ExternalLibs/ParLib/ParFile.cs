using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using NLog;
using TF.IO;

namespace ParLib
{
    [Serializable]
    public partial class ParFile
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public int TotalFolderCount { get; set; }
        public int TotalFileCount { get; set; }
        private ParFolderInfo RootFolder { get; set; }

        [NonSerialized] private int _folderInfoOffset;
        [NonSerialized] private int _fileInfoOffset;
        [NonSerialized] private string _path;

        private ParFile()
        {

        }

        public static ParFile ReadFolder(string path, bool canCreate = true)
        {
            var filePath = Path.Combine(path, "ParInfo.tf");
            if (File.Exists(filePath))
            {
                var formatter = new BinaryFormatter {AssemblyFormat = FormatterAssemblyStyle.Simple};
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    return (ParFile) formatter.Deserialize(stream);
                }
            }

            filePath = Path.Combine(path, "Extract_Data.tf");
            if (File.Exists(filePath))
            {
                return ReadOldFormat(filePath);
            }

            return canCreate ? CreateInfoFromFolder(path) : null;
        }

        private static ParFile ReadOldFormat(string path)
        {
            var result = new ParFile();

            var folders = new ParFolderInfo[100000];
            var files = new ParFileInfo[100000];
            var subFolders = new Dictionary<int, List<int>>();
            var subFiles = new Dictionary<int, List<int>>();
            
            var folderOrder = new List<int>();
            
            var folderCount = 0;
            var fileCount = 0;
            using (var log = new ExtendedBinaryReader(new FileStream(path, FileMode.Open), Encoding.GetEncoding(1252), Endianness.BigEndian))
            {
                while (log.Position < log.Length)
                {
                    var type = log.ReadByte();
                    if (type == 0)
                    {
                        // folder
                        var folder = new ParFolderInfo();
                        folder.Name = log.ReadString();
                        var index = (int)log.ReadUInt32(); //index
                        folder.FolderCount = (int) log.ReadUInt32();
                        folder.FirstFolderIndex = (int) log.ReadUInt32();
                        folder.FileCount = (int) log.ReadUInt32();
                        folder.FirstFileIndex = (int) log.ReadUInt32();

                        folder.Unknown1 = (int) log.ReadUInt32();
                        folder.Unknown2 = (int) log.ReadUInt32();
                        folder.Unknown3 = (int) log.ReadUInt32();
                        folder.Unknown4 = (int) log.ReadUInt32();

                        subFolders[index] = new List<int>();
                        subFiles[index] = new List<int>();
                        for (var i = 0u; i < folder.FolderCount; i++)
                        {
                            var folderId = (int)log.ReadUInt32();
                            subFolders[index].Add(folderId);
                        }

                        for (var i = 0u; i < folder.FileCount; i++)
                        {
                            var fileId = (int)log.ReadUInt32();
                            subFiles[index].Add(fileId);
                        }

                        folderCount++;
                        folders[index] = folder;
                    }
                    else
                    {
                        var file = new ParFileInfo();

                        file.Name = log.ReadString();
                        var index = (int)log.ReadUInt32(); //index
                        file.CompressionFlags = (FileFlags) log.ReadUInt32();
                        file.UncompressedSize = (int) log.ReadUInt32();
                        file.CompressedSize = (int) log.ReadUInt32();
                        file.Offset = (int) log.ReadUInt32();
                        file.Unknown1 = (int) log.ReadUInt32();
                        file.Unknown2 = (int) log.ReadUInt32();
                        file.Unknown3 = (int) log.ReadUInt32();
                        file.Date = (int) log.ReadUInt32();

                        fileCount++;
                        files[index] = file;
                    }
                }
            }

            folders[0].RelativePath = ".";
            result.RootFolder = folders[0];

            for (var i = 0; i < folderCount; i++)
            {
                var current = folders[i];
                var fileList = subFiles[i];
                var folderList = subFolders[i];

                foreach (var fileId in fileList)
                {
                    files[fileId].RelativePath = $"{current.RelativePath}\\{files[fileId].Name}";
                    current.Files.Add(files[fileId]);
                }

                foreach (var folderId in folderList)
                {
                    folders[folderId].RelativePath = $"{current.RelativePath}\\{folders[folderId].Name}";
                    current.Folders.Add(folders[folderId]);
                }
            }

            result.TotalFolderCount = folderCount;
            result.TotalFileCount = fileCount;
            return result;
        }

        private static ParFile CreateInfoFromFolder(string path)
        {
            var comparer = new LowerCaseComparer();
            var queue = new Queue<ParFolderInfo>();

            var result = new ParFile
            {
                TotalFileCount = 0,
                TotalFolderCount = 1
            };

            var rootFolder = new ParFolderInfo
            {
                Name = ".",
                RelativePath = "."
            };

            result.RootFolder = rootFolder;

            queue.Enqueue(rootFolder);
            var folderIndex = 1;
            var fileIndex = 0;
            while (queue.Count != 0)
            {
                var parFolder = queue.Dequeue();
                parFolder.FirstFolderIndex = folderIndex;
                parFolder.FirstFileIndex = fileIndex;

                var currentPath = Path.Combine(path, parFolder.RelativePath);
                var folders = Directory.GetDirectories(currentPath);
                Array.Sort(folders, comparer);
                var files = Directory.GetFiles(currentPath);
                Array.Sort(files, comparer);

                result.TotalFolderCount += folders.Length;
                result.TotalFileCount += files.Length;

                foreach (var file in files)
                {
                    var name = Path.GetFileName(file);
                    var parFile = new ParFileInfo
                    {
                        CompressionFlags = FileFlags.None,
                        Name = name,
                        RelativePath = $"{parFolder.RelativePath}\\{name}",
                        Unknown1 = 0x2020,
                        Unknown2 = 0x00,
                        Unknown3 = 0x00,
                        FileDate = File.GetCreationTime(file)
                    };
                    parFolder.Files.Add(parFile);
                    fileIndex++;
                }

                folderIndex += folders.Length;
                foreach (var folder in folders)
                {
                    var name = Path.GetFileName(folder);
                    var childFolder = new ParFolderInfo
                    {
                        Name = name,
                        RelativePath = $"{parFolder.RelativePath}\\{name}"
                    };

                    parFolder.Folders.Add(childFolder);
                    queue.Enqueue(childFolder);
                }
            }

            return result;
        }

        public static ParFile ReadPar(string path)
        {
            Logger.Debug("Reading \"{0}\" Info...", path);
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var input = new ExtendedBinaryReader(fs, Encoding.GetEncoding(1252), Endianness.BigEndian))
            {
                var magic = input.ReadInt32();

                if (magic != 0x50415243)
                {
                    throw new Exception("Invalid par file.");
                }

                var unknown1 = input.ReadInt32();
                if (unknown1 != 0x02010000)
                {
                    throw new Exception("Invalid par file.");
                }

                var unknown2 = input.ReadInt32();
                if (unknown2 != 0x00020001)
                {
                    throw new Exception("Invalid par file.");
                }

                var unknown3 = input.ReadInt32();
                if (unknown3 != 0x00000000)
                {
                    throw new Exception("Invalid par file.");
                }

                var result = new ParFile
                {
                    _path = path,
                    TotalFolderCount = input.ReadInt32(),
                    _folderInfoOffset = input.ReadInt32(),
                    TotalFileCount = input.ReadInt32(),
                    _fileInfoOffset = input.ReadInt32()
                };

                var folderNames = ReadNames(input, 0x20, result.TotalFolderCount);
                var fileNames = ReadNames(input, 0x20 + result.TotalFolderCount * 0x40, result.TotalFileCount);

                input.Seek(result._folderInfoOffset, SeekOrigin.Begin);
                var folderInfos = ReadFolderInfos(input, folderNames);

                input.Seek(result._fileInfoOffset, SeekOrigin.Begin);
                var fileInfos = ReadFileInfos(input, fileNames);
                
                Fill(folderInfos, fileInfos);

                result.RootFolder = folderInfos[0];

                return result;
            }
        }

        private static IList<string> ReadNames(ExtendedBinaryReader input, long baseOffset, int count)
        {
            var result = new List<string>();
            for (var i = 0; i < count; i++)
            {
                input.Seek(baseOffset + i * 0x40, SeekOrigin.Begin);
                result.Add(input.ReadString());
            }

            return result;
        }

        private static IList<ParFolderInfo> ReadFolderInfos(BinaryReader input, IList<string> names)
        {
            var result = new List<ParFolderInfo>();
            for (var i = 0; i < names.Count; i++)
            {
                var folderInfo = new ParFolderInfo
                {
                    Name = names[i],
                    FolderCount = input.ReadInt32(),
                    FirstFolderIndex = input.ReadInt32(),
                    FileCount = input.ReadInt32(),
                    FirstFileIndex = input.ReadInt32(),
                    Unknown1 = input.ReadInt32(),
                    Unknown2 = input.ReadInt32(),
                    Unknown3 = input.ReadInt32(),
                    Unknown4 = input.ReadInt32(),
                    RelativePath = names[i]
                };

                result.Add(folderInfo);
            }

            return result;
        }

        private static IList<ParFileInfo> ReadFileInfos(BinaryReader input, IList<string> names)
        {
            var result = new List<ParFileInfo>();
            for (var i = 0; i < names.Count; i++)
            {
                var fileInfo = new ParFileInfo
                {
                    Name = names[i],
                    CompressionFlags = (FileFlags) input.ReadUInt32(),
                    UncompressedSize = input.ReadInt32(),
                    CompressedSize = input.ReadInt32(),
                    Offset = input.ReadInt32(),
                    Unknown1 = input.ReadInt32(),
                    Unknown2 = input.ReadInt32(),
                    Unknown3 = input.ReadInt32(),
                    FileDate = new DateTime(1970, 1, 1).AddSeconds(input.ReadInt32()),
                    RelativePath = names[i]
                };

                result.Add(fileInfo);
            }

            return result;
        }

        private static void Fill(IList<ParFolderInfo> folderInfos, IList<ParFileInfo> fileInfos)
        {
            foreach (var folderInfo in folderInfos)
            {
                var startIndex = folderInfo.FirstFolderIndex;
                for (var j = 0; j < folderInfo.FolderCount; j++)
                {
                    folderInfo.Folders.Add(folderInfos[startIndex + j]);
                    folderInfos[startIndex + j].RelativePath = $"{folderInfo.RelativePath}\\{folderInfos[startIndex + j].Name}";
                }

                startIndex = folderInfo.FirstFileIndex;
                for (var j = 0; j < folderInfo.FileCount; j++)
                {
                    folderInfo.Files.Add(fileInfos[startIndex + j]);
                    fileInfos[startIndex + j].RelativePath = $"{folderInfo.RelativePath}\\{fileInfos[startIndex + j].Name}";
                }
            }
        }
    }
}
