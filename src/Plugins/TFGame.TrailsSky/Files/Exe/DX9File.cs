using System;
using System.Collections.Generic;

namespace TFGame.TrailsSky.Files.Exe
{
    public class DX9File : File
    {
        protected override long FontTableOffset => 0x117DA8;

        protected override List<Tuple<long, long>> AllowedStringOffsets => new List<Tuple<long, long>>()
        {
            new Tuple<long, long>(0x117754, 0x11C394),
            new Tuple<long, long>(0x11EDA8, 0x11F168),
        };

        public DX9File(string path, string changesFolder, Encoding encoding) : base(path, changesFolder, encoding)
        {
        }
    }
}
