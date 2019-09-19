using System;

namespace ParLib
{
    [Flags]
    internal enum FileFlags : uint
    {
        None = 0u,
        IsCompressed = 1u << 31
    }

    public enum CompressionType
    {
        Default,
        Uncompressed,
        V1Compression,
    }

    [Serializable]
    internal class ParFileInfo
    {
        public FileFlags CompressionFlags
        {
            get => _compressionFlags;
            set => _compressionFlags = value;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public int UncompressedSize
        {
            get => _uncompressedSize;
            set => _uncompressedSize = value;
        }

        public int CompressedSize
        {
            get => _compressedSize;
            set => _compressedSize = value;
        }

        public int Offset
        {
            get => _offset;
            set => _offset = value;
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

        public int Date
        {
            get => _date;
            set => _date = value;
        }

        public DateTime FileDate
        {
            get
            {
                var baseDate = new DateTime(1970, 1, 1);
                return baseDate.AddSeconds(_date);
            }
            set
            {
                var baseDate = new DateTime(1970, 1, 1);
                _date = (int) (value - baseDate).TotalSeconds;
            }
        }

        private FileFlags _compressionFlags;
        private string _name { get; set; }
        private int _uncompressedSize;
        private int _compressedSize;
        private int _offset;
        private int _unknown1;
        private int _unknown2;
        private int _unknown3;
        private int _date;

        public string RelativePath { get; set; }
    }
}
