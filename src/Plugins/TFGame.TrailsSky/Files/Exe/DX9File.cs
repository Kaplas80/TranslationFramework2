using System;
using System.Collections.Generic;

namespace TFGame.TrailsSky.Files.Exe
{
    public class DX9File : File
    {
        protected override long FontTableOffset => 0x117DA8;

        protected override List<Tuple<long, long>> AllowedStringOffsets => new List<Tuple<long, long>>()
        {
            new Tuple<long, long>(0x11567C, 0x115818),
            new Tuple<long, long>(0x115880, 0x1158D0),
            new Tuple<long, long>(0x115A80, 0x115B0C),
            new Tuple<long, long>(0x115B7C, 0x115BB0),
            new Tuple<long, long>(0x115C64, 0x116EB8),
            new Tuple<long, long>(0x117514, 0x117530),
            new Tuple<long, long>(0x117754, 0x11780C),
            new Tuple<long, long>(0x117B94, 0x117BBC),
            new Tuple<long, long>(0x118DB8, 0x118E00),
            new Tuple<long, long>(0x118E64, 0x118F14),
            new Tuple<long, long>(0x118F24, 0x11902C),
            new Tuple<long, long>(0x11908C, 0x11A774),
            new Tuple<long, long>(0x11AC80, 0x11B49C),
            new Tuple<long, long>(0x11B664, 0x11B7BC),
            new Tuple<long, long>(0x11B8D0, 0x11C1D4),
            new Tuple<long, long>(0x11C358, 0x11CCDC),
            new Tuple<long, long>(0x11CF08, 0x11D190),
            new Tuple<long, long>(0x11E444, 0x11E8EC),
            new Tuple<long, long>(0x11EDA8, 0x11F168),
            new Tuple<long, long>(0x11F460, 0x11F47C),
            new Tuple<long, long>(0x11FBB0, 0x1201DC),
        };

        public DX9File(string path, string changesFolder, Encoding encoding) : base(path, changesFolder, encoding)
        {
        }
    }
}
