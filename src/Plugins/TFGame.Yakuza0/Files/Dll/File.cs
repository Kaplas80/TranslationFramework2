using System;
using System.Collections.Generic;

namespace TFGame.Yakuza0.Files.Dll
{
    public class File : YakuzaGame.Files.Exe.PEFile
    {
        protected override string PointerSectionName => ".data\0\0\0";
        protected override string StringsSectionName => ".rdata\0\0";

        protected override List<Tuple<long, long>> AllowedStringOffsets => new List<Tuple<long, long>>
        {
            new Tuple<long, long>(0x15A3C8, 0x15C2E8),
            new Tuple<long, long>(0x166000, 0x16DBB0),
            new Tuple<long, long>(0x175158, 0x1936E8),
        };

        public File(string path, string changesFolder, System.Text.Encoding encoding) : base(path, changesFolder, encoding)
        {
        }
    }
}
