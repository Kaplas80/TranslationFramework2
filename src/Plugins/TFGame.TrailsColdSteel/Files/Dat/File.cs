using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TF.Core.Files;
using TF.Core.TranslationEntities;
using TF.IO;

namespace TFGame.TrailsColdSteel.Files.Dat
{
    public class File : BinaryTextFile
    {
        public File(string path, string changesFolder, Encoding encoding) : base(path, changesFolder, encoding)
        {
        }

        protected override IList<TF.Core.TranslationEntities.Subtitle> GetSubtitles()
        {
            var result = new List<Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding))
            {
                input.Seek(0x18, SeekOrigin.Begin);
                var startOffset = input.ReadInt32();
                input.Seek(startOffset, SeekOrigin.Begin);


            }

            LoadChanges(result);

            return result;
        }
    }
}
