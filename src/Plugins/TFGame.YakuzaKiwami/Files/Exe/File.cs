using System;
using System.Collections.Generic;
using YakuzaGame.Files.Exe;

namespace TFGame.YakuzaKiwami.Files.Exe
{
    public class File : YakuzaGame.Files.Exe.File
    {
        // Buscar "data/font". La tabla empieza 0x1900 bytes antes
        protected override long FontTableOffset => 0xCFF560;
        protected override string PointerSectionName => ".data\0\0\0";
        protected override string StringsSectionName => ".rdata\0\0";

        protected override int ChangesFileVersion => 2;
        protected override List<Tuple<long, long>> AllowedStringOffsets => new List<Tuple<long, long>>()
        {
            new Tuple<long, long>(0x00C93710, 0x00C93710),
            new Tuple<long, long>(0x00C98B78, 0x00C98B90),
            new Tuple<long, long>(0x00CA2020, 0x00CA2260),
            new Tuple<long, long>(0x00CA37C8, 0x00CA4600),
            new Tuple<long, long>(0x00CA6640, 0x00CA9F90),
            new Tuple<long, long>(0x00D56050, 0x00D560C0),
            new Tuple<long, long>(0x00D99F90, 0x00D99F90),
            new Tuple<long, long>(0x00D9BED0, 0x00D9BEF0),
            new Tuple<long, long>(0x00D9FEC0, 0x00DA6390),
            new Tuple<long, long>(0x00E0D0A0, 0x00E26928),
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
                    new Tuple<long, byte[]>(0x195007, new byte[] {0xEB, 0x1E, 0x90}),
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
                    new Tuple<long, byte[]>(0xE374BC, new byte[] {0x25, 0x73, 0x5c})
                },
            },
        };

        public File(string path, string changesFolder, System.Text.Encoding encoding) : base(path, changesFolder, encoding)
        {
        }
    }
}
