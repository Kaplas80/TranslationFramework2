using System.Collections.Generic;
using System.IO;
using System.Linq;
using TF.Core.Files;
using TF.Core.TranslationEntities;
using TF.IO;

namespace TFGame.HardcoreMecha.Files.Language
{
    public class File : BinaryTextFile
    {
        public override string LineEnding => "\n";

        public File(string path, string changesFolder, System.Text.Encoding encoding) : base(path, changesFolder, encoding)
        {
        }

        protected override IList<Subtitle> GetSubtitles()
        {
            var result = new List<Subtitle>();
            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding))
            {
                input.Skip(0x1A0);
                var length = input.ReadInt32();
                var sub = input.ReadBytes(length);
                var str = FileEncoding.GetString(sub);

                var subtitle = new Subtitle
                {
                    Offset = 0x1A0,
                    Text = str,
                    Loaded = str,
                    Translation = str
                };

                subtitle.PropertyChanged += SubtitlePropertyChanged;
                result.Add(subtitle);
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
            using (var input = new ExtendedBinaryReader(fsInput, FileEncoding))
            using (var fsOutput = new FileStream(outputPath, FileMode.Create))
            using (var output = new ExtendedBinaryWriter(fsOutput, FileEncoding))
            {
                output.Write(input.ReadBytes(0x1A0));
                
                var length = input.ReadInt32();
                input.ReadBytes(length);
                input.SkipPadding(0x04);
                
                var sub = subtitles.First(x => x.Offset == 0x1A0);
                var data = FileEncoding.GetBytes(sub.Translation);
                output.Write(data.Length);
                output.Write(data);
                output.WritePadding(0x04);

                var remainderLength = (int)(input.Length - input.Position);
                var remainder = input.ReadBytes(remainderLength);
                output.Write(remainder);
            }
        }
    }
}
