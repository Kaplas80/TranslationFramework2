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


                input.Skip(3);
                var count = input.ReadByte();
                input.Skip(4);
                var pointer1 = input.ReadInt32();
                var countPointer1 = input.ReadInt16();

                var numTalkers = input.ReadInt16();
                var pointerTalkers = input.ReadInt32();
                var pointerRemainder = input.ReadInt32();

                for (var i = 0; i < count; i++)
                {
                    input.Skip(4);
                    var groupOffset = input.ReadInt32();
                    input.Skip(1);
                    var stringCount = input.ReadByte();
                    input.Skip(6);

                    var subs = ReadSubtitles(input, groupOffset, stringCount);

                    if (subs.Count > 0)
                    {
                        result.AddRange(subs);
                    }
                }

                if (pointerTalkers > 0 && result.Count > 0)
                {
                    input.Seek(pointerTalkers, SeekOrigin.Begin);
                    for (var i = 0; i < numTalkers; i++)
                    {
                        var offset = input.ReadInt32();
                        var subtitle = ReadSubtitle(input, offset, true);
                        if (subtitle != null)
                        {
                            subtitle.PropertyChanged += SubtitlePropertyChanged;
                            result.Add(subtitle);
                        }
                    }
                }
            }

            LoadChanges(result);

            return result;
        }
    }
}
