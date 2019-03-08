using System;
using System.Collections.Generic;

namespace TFGame.YakuzaKiwami.Files.Exe
{
    public class File : YakuzaCommon.Files.Exe.File
    {
        protected override long FontTableOffset => 0xD05FD0;
        protected override string PointerSectionName => ".data\0\0\0";
        protected override string StringsSectionName => ".rdata\0\0";

        protected override List<Tuple<ulong, ulong>> AllowedStringOffsets => new List<Tuple<ulong, ulong>>()
        {
            new Tuple<ulong, ulong>(0x00c9a128, 0x00c9a128),
            new Tuple<ulong, ulong>(0x00c9f5f8, 0x00c9f610),
            new Tuple<ulong, ulong>(0x00ca8800, 0x00ca8cf0),
            new Tuple<ulong, ulong>(0x00caa228, 0x00cab070),
            new Tuple<ulong, ulong>(0x00cac528, 0x00cac540),
            new Tuple<ulong, ulong>(0x00cad0b0, 0x00cb0a00),
            new Tuple<ulong, ulong>(0x00d5b5b0, 0x00d5be58),
            new Tuple<ulong, ulong>(0x00d5ceb0, 0x00d5cf20),
            new Tuple<ulong, ulong>(0x00d5d090, 0x00d5d090),
            new Tuple<ulong, ulong>(0x00d8ca90, 0x00d8ca90),
            new Tuple<ulong, ulong>(0x00da09c0, 0x00da09c0),
            new Tuple<ulong, ulong>(0x00da2a00, 0x00da2a20),
            new Tuple<ulong, ulong>(0x00da68e0, 0x00da6e70),
            new Tuple<ulong, ulong>(0x00da7048, 0x00da7170),
            new Tuple<ulong, ulong>(0x00daba10, 0x00dacdb0),
            new Tuple<ulong, ulong>(0x00e133f0, 0x00e2d2e8),
        };

        public File(string path, string changesFolder, System.Text.Encoding encoding) : base(path, changesFolder, encoding)
        {
        }
    }
}
