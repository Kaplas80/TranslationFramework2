using System.Collections.Generic;
using System.IO;
using System.Linq;
using TF.Core.Files;
using TF.Core.TranslationEntities;
using TF.IO;

namespace TFGame.YakuzaIshin.Files.Scenario
{
    public class File : BinaryTextFile
    {

        public File(string path, string changesFolder, Encoding encoding) : base(path, changesFolder, encoding)
        {
        }

        protected override IList<Subtitle> GetSubtitles()
        {
            var result = new List<Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding, Endianness.BigEndian))
            {
                input.Skip(0x38);
                var stringsOffset = input.ReadInt32();

                input.Seek(stringsOffset, SeekOrigin.Begin);

                Subtitle subtitle;

                while (input.Position < input.Length)
                {
                    subtitle = ReadSubtitle(input);
                    subtitle.PropertyChanged += SubtitlePropertyChanged;
                    result.Add(subtitle);
                }
            }

            LoadChanges(result);

            return result;
        }

        public override void Rebuild(string outputFolder)
        {
            var outputPath = System.IO.Path.Combine(outputFolder, RelativePath);
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath));

            var subtitles = GetSubtitles();

            using (var fsInput = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fsInput, FileEncoding, Endianness.BigEndian))
            using (var fsOutput = new FileStream(outputPath, FileMode.Create))
            using (var output = new ExtendedBinaryWriter(fsOutput, FileEncoding, Endianness.BigEndian))
            {
                input.Skip(0x38);
                var stringsOffset = input.ReadInt32();

                input.Seek(0, SeekOrigin.Begin);

                output.Write(input.ReadBytes(stringsOffset));

                Subtitle subtitle;
                var dict = new Dictionary<long, long>(subtitles.Count);

                while (input.Position < input.Length)
                {
                    subtitle = ReadSubtitle(input);

                    var newSubtitle = subtitles.FirstOrDefault(x => x.Offset == subtitle.Offset);
                    if (newSubtitle != null)
                    {
                        dict.Add(subtitle.Offset, output.Position);
                        output.WriteString(newSubtitle.Translation);
                    }
                }

                input.Seek(0x10, SeekOrigin.Begin);
                var count = input.ReadInt32();
                var offset = input.ReadInt32();

                input.Seek(offset, SeekOrigin.Begin);
                output.Seek(offset, SeekOrigin.Begin);

                for (var i = 0; i < count; i++)
                {
                    input.Skip(4);
                    output.Skip(4);

                    var stringOffset = input.ReadInt32();
                    output.Write((int)dict[stringOffset]);

                    input.Skip(16);
                    output.Skip(16);
                }
            }
        }
    }
}
