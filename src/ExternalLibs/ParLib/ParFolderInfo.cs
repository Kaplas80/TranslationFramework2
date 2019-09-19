using System;
using System.Collections.Generic;
using ParLib;

namespace ParLib
{
    [Serializable]
    internal class ParFolderInfo
    {
        public int FolderCount
        {
            get => _folderCount;
            set => _folderCount = value;
        }

        public int FirstFolderIndex
        {
            get => _firstFolderIndex;
            set => _firstFolderIndex = value;
        }

        public int FileCount
        {
            get => _fileCount;
            set => _fileCount = value;
        }

        public int FirstFileIndex
        {
            get => _firstFileIndex;
            set => _firstFileIndex = value;
        }

        public int Unknown1
        {
            get => _unknown1;
            set => _unknown1 = value;
        }

        public int Unknown2
        {
            get => _unknown2;
            set => _unknown2 = value;
        }

        public int Unknown3
        {
            get => _unknown3;
            set => _unknown3 = value;
        }

        public int Unknown4
        {
            get => _unknown4;
            set => _unknown4 = value;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public IList<ParFolderInfo> Folders => _folders;

        public IList<ParFileInfo> Files => _files;

        public ParFolderInfo()
        {
            _folders = new List<ParFolderInfo>();
            _files = new List<ParFileInfo>();
        }

        private int _folderCount;
        private int _firstFolderIndex;
        private int _fileCount;
        private int _firstFileIndex;
        private int _unknown1;
        private int _unknown2;
        private int _unknown3;
        private int _unknown4;
        private string _name;
        private IList<ParFolderInfo> _folders;
        private IList<ParFileInfo> _files;

        public string RelativePath { get; set; }
    }
}
