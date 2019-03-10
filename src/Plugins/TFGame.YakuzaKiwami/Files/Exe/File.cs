using System;
using System.Collections.Generic;
using YakuzaCommon.Files.Exe;

namespace TFGame.YakuzaKiwami.Files.Exe
{
    public class File : YakuzaCommon.Files.Exe.File
    {
        protected override long FontTableOffset => 0xD05FD0;
        protected override string PointerSectionName => ".data\0\0\0";
        protected override string StringsSectionName => ".rdata\0\0";

        protected override List<Tuple<ulong, ulong>> AllowedStringOffsets => new List<Tuple<ulong, ulong>>()
        {
            new Tuple<ulong, ulong>(0x00C9A128, 0x00C9A128),
            new Tuple<ulong, ulong>(0x00C9F5F8, 0x00C9F610),
            new Tuple<ulong, ulong>(0x00CA8800, 0x00CA8CF0),
            new Tuple<ulong, ulong>(0x00CAA228, 0x00CAB070),
            new Tuple<ulong, ulong>(0x00CAC528, 0x00CAC540),
            new Tuple<ulong, ulong>(0x00CAD0B0, 0x00CB0A00),
            new Tuple<ulong, ulong>(0x00D5B5B0, 0x00D5BE58),
            new Tuple<ulong, ulong>(0x00D5CEB0, 0x00D5CF20),
            new Tuple<ulong, ulong>(0x00D5D090, 0x00D5D090),
            new Tuple<ulong, ulong>(0x00D8CA90, 0x00D8CA90),
            new Tuple<ulong, ulong>(0x00DA09C0, 0x00DA09C0),
            new Tuple<ulong, ulong>(0x00DA2A00, 0x00DA2A20),
            new Tuple<ulong, ulong>(0x00DA68E0, 0x00DA6E70),
            new Tuple<ulong, ulong>(0x00DA7048, 0x00DA7170),
            new Tuple<ulong, ulong>(0x00DABA10, 0x00DACDB0),
            new Tuple<ulong, ulong>(0x00E133F0, 0x00E2D2E8),
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
                    new Tuple<long, byte[]>(0xE3DE3C, new byte[] {0x25, 0x73, 0x5c})
                },
            },
        };

        public File(string path, string changesFolder, System.Text.Encoding encoding) : base(path, changesFolder, encoding)
        {
        }
    }
}
