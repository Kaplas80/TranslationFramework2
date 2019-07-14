using System;
using System.Collections.Generic;

namespace TFGame.YakuzaKiwami.Files.Dll
{
    public class File : YakuzaGame.Files.Exe.PEFile
    {
        protected override string PointerSectionName => ".data\0\0\0";
        protected override string StringsSectionName => ".rdata\0\0";

        protected override List<Tuple<long, long>> AllowedStringOffsets => new List<Tuple<long, long>>
        {
            new Tuple<long, long>(0x16C670, 0x16D7A0),
            new Tuple<long, long>(0x186830, 0x1A5730),

        };

        public File(string path, string changesFolder, System.Text.Encoding encoding) : base(path, changesFolder, encoding)
        {
        }
    }
}
