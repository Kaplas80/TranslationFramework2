using System;
using System.Collections.Generic;
using YakuzaGame.Files.Exe;

namespace TFGame.Yakuza0.Files.Exe
{
    public class File : YakuzaGame.Files.Exe.File
    {
        // Buscar "data/font". La tabla empieza 0x1900 bytes antes
        protected override long FontTableOffset => 0xD8F9E0;
        protected override string PointerSectionName => ".data\0\0\0";
        protected override string StringsSectionName => ".rdata\0\0";

        protected override List<Tuple<long, long>> AllowedStringOffsets => new List<Tuple<long, long>>()
        {
            new Tuple<long, long>(0xD2A740, 0xD2A740),
            new Tuple<long, long>(0xD2C658, 0xD2D520),
            new Tuple<long, long>(0xD2F9E8, 0xD2FA80),
            new Tuple<long, long>(0xDCA2D8, 0xDCD6B0),
            new Tuple<long, long>(0xDD0508, 0xDD4200),
            new Tuple<long, long>(0xE898D8, 0xEA4D40),
        };

        protected override List<ExePatch> Patches => new List<ExePatch>()
        {
            /*
            Buscar 33 D2 83 F0 20 2D A1 00 00 00 83 F8 3C
            Avanzar 0x1B bytes
            Sustituir 770A -> EB1C

            Buscar 66 0F 1F 44 00 00 42 0F B6 14 3B 80 FA E0
            Sustituir 7242 -> EB47

            Buscar B8 02 00 00 00 C3 84 D2
            Sustituir 7806 -> 9090
            */
            new ExePatch
            {
                Name = "Usar codificación ISO-8895-1",
                Description = "Cambia la codificación de los textos a ISO-8895-1 (NO SE REPRESENTARÁN CARACTERES UTF-8)",
                Enabled = false,
                Patches = new List<Tuple<long, byte[]>>
                {
                    new Tuple<long, byte[]>(0x396EBD, new byte[] {0xEB, 0x1C}),
                    new Tuple<long, byte[]>(0x39B378, new byte[] {0xEB, 0x47}),
                    new Tuple<long, byte[]>(0x6FA8D6, new byte[] {0x90, 0x90}),
                },
            },

            // Buscar el primer "CoInitialize". La cadena está justo antes
            new ExePatch
            {
                Name = "Cambiar posición de ¥",
                Description = "Cambia la posición del símbolo ¥ a la derecha de la cifra (de ¥1000 a 1000¥)",
                Enabled = false,
                Patches = new List<Tuple<long, byte[]>>
                {
                    new Tuple<long, byte[]>(0xEBCC28, new byte[] {0x25, 0x73, 0x5C})
                },
            },
        };

        public File(string gameName, string path, string changesFolder, System.Text.Encoding encoding) : base(gameName, path, changesFolder, encoding)
        {
        }
    }
}
