using System.Collections.Generic;
using System.IO;
using System.Text;
using TF.Core.Files;
using TF.Core.TranslationEntities;
using TF.IO;

namespace TFGame.Yakuza0.Files.PocketCircuit
{
    public class File : YakuzaGame.Files.PocketCircuit.File
    {
        public File(string path, string changesFolder, Encoding encoding) : base(path, changesFolder, encoding)
        {
        }

        protected override IList<Subtitle> GetSubtitles()
        {
            IList<Subtitle> result;

            if (Name.StartsWith("pokecir_chara.bin"))
            {
                result = GetSubtitlesPOCB();
                LoadChanges(result);

                return result;
            }

            return base.GetSubtitles();
        }

        private IList<Subtitle> GetSubtitlesPOCB()
        {
            var temp = new List<Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding, Endianness.BigEndian))
            {
                input.Skip(16);
                var numItems = input.ReadInt32();

                for (var i = 0; i < numItems; i++)
                {
                    input.Skip(32); // id
                    input.Skip(2); // unknown

                    var subtitle = ReadSubtitle(input, 64);
                    if (!string.IsNullOrEmpty(subtitle.Text))
                    {
                        temp.Add(subtitle);
                    }

                    input.Skip(0x3A); // unknown
                }
            }

            return temp;
        }
    }
}
