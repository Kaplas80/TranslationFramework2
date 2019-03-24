using System;
using System.Collections.Generic;
using YakuzaCommon.Files.Exe;

namespace TFGame.YakuzaKiwami.Files.Exe
{
    public class File : YakuzaCommon.Files.Exe.File
    {
        protected override long FontTableOffset => 0xCFF290;
        protected override string PointerSectionName => ".data\0\0\0";
        protected override string StringsSectionName => ".rdata\0\0";

        protected override List<Tuple<long, long>> AllowedStringOffsets => new List<Tuple<long, long>>()
        {
            new Tuple<long, long>(0x00C934C8, 0x00C934C8),
            new Tuple<long, long>(0x00C98948, 0x00C98960),
            new Tuple<long, long>(0x00CA1E00, 0x00CA2040),
            new Tuple<long, long>(0x00CA2A68, 0x00CA2BE0),
            new Tuple<long, long>(0x00CA3588, 0x00CA43D0),
            new Tuple<long, long>(0x00CA6410, 0x00CA8F40),
            new Tuple<long, long>(0x00CA8FC0, 0x00CA9D60),
            new Tuple<long, long>(0x00CD0A20, 0x00CD0FA0),
            new Tuple<long, long>(0x00D54860, 0x00D55110),
            new Tuple<long, long>(0x00D55D50, 0x00D55DC0),
            new Tuple<long, long>(0x00D56320, 0x00D56320),
            new Tuple<long, long>(0x00D85D38, 0x00D85D38),
            new Tuple<long, long>(0x00D99CD0, 0x00D99CD0),
            new Tuple<long, long>(0x00D9B150, 0x00D9BC08),
            new Tuple<long, long>(0x00D9FC40, 0x00DA6120),
            new Tuple<long, long>(0x00E0C7D0, 0x00E266C8),
        };

        protected override List<ExePatch> Patches => new List<ExePatch>()
        {
            new ExePatch
            {
                Name = "Cambiar posición de ¥",
                Description = "Cambia la posición del símbolo ¥ a la derecha de la cifra (de ¥1000 a 1000¥)",
                Enabled = false,
                Patches = new List<Tuple<long, byte[]>>
                {
                    new Tuple<long, byte[]>(0xE3725C, new byte[] {0x25, 0x73, 0x5c})
                },
            },
        };

        public File(string path, string changesFolder, System.Text.Encoding encoding) : base(path, changesFolder, encoding)
        {
        }
    }
}
