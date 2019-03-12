using System;
using System.Collections.Generic;
using YakuzaCommon.Files.Exe;

namespace TFGame.Yakuza0.Files.Exe
{
    public class File : YakuzaCommon.Files.Exe.File
    {
        protected override long FontTableOffset => 0xD8FA60;
        protected override string PointerSectionName => ".data\0\0\0";
        protected override string StringsSectionName => ".rdata\0\0";

        protected override List<Tuple<ulong, ulong>> AllowedStringOffsets => new List<Tuple<ulong, ulong>>()
        {
            new Tuple<ulong, ulong>(0xD2A770, 0xD2A770),
            new Tuple<ulong, ulong>(0xD2C688, 0xD2D560),
            new Tuple<ulong, ulong>(0xD2FA28, 0xD2FAC0),
            new Tuple<ulong, ulong>(0xDC8F24, 0xDC98A8),
            new Tuple<ulong, ulong>(0xDCA3C8, 0xDCABA8),
            new Tuple<ulong, ulong>(0xDCABB8, 0xDCD7B0),
            new Tuple<ulong, ulong>(0xDCEB1C, 0xDCFE38),
            new Tuple<ulong, ulong>(0xDD0608, 0xDD4300),
            new Tuple<ulong, ulong>(0xE88C88, 0xEA40F0),
        };

        protected override List<ExePatch> Patches => new List<ExePatch>()
        {
            new ExePatch
            {
                Name = "Usar codificación ISO-8895-1",
                Description = "Cambia la codificación de los textos a ISO-8895-1 (NO SE REPRESENTARÁN CARACTERES UTF-8)",
                Enabled = false,
                Patches = new List<Tuple<long, byte[]>>
                {
                    new Tuple<long, byte[]>(0x39754D, new byte[] {0xEB, 0x1C}),
                    new Tuple<long, byte[]>(0x39BA08, new byte[] {0xEB, 0x47}),
                    new Tuple<long, byte[]>(0x6FAED6, new byte[] {0x90, 0x90}),
                },
            },
            new ExePatch
            {
                Name = "Cambiar posición de ¥",
                Description = "Cambia la posición del símbolo ¥ a la derecha de la cifra (de ¥1000 a 1000¥)",
                Enabled = false,
                Patches = new List<Tuple<long, byte[]>>
                {
                    new Tuple<long, byte[]>(0xEBBFE8, new byte[] {0x25, 0x73, 0x5c})
                },
            },
        };

        public File(string path, string changesFolder, System.Text.Encoding encoding) : base(path, changesFolder, encoding)
        {
        }
    }
}
