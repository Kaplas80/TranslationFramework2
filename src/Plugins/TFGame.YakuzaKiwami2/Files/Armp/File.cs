using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TF.Core.Files;
using TF.Core.TranslationEntities;
using TF.IO;

namespace TFGame.YakuzaKiwami2.Files.Armp
{
    public class File : BinaryTextFile
    {
        public File(string path, string changesFolder, System.Text.Encoding encoding) : base(path, changesFolder, encoding)
        {
        }

        protected override IList<Subtitle> GetSubtitles()
        {
            var result = new List<Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding, Endianness.LittleEndian))
            {
                var magic = input.ReadInt32(); // 61726D70
                if (magic != 0x706D7261)
                {
                    // No es un fichero armp
                    return result;
                }

                input.Seek(0x10, SeekOrigin.Begin);
                var tablesSectionPointer = input.ReadInt32();

                var table = Table.ReadTable(input, tablesSectionPointer);
            }

            LoadChanges(result);

            return result;
        }
    }
}
