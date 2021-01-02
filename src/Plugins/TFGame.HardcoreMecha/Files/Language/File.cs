namespace TFGame.HardcoreMecha.Files.Language
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using TF.Core.Files;
    using TF.Core.TranslationEntities;
    using TF.IO;

    public class File : BinaryTextFile
    {
        public File(string gameName, string path, string changesFolder, System.Text.Encoding encoding) : base(gameName, path, changesFolder, encoding)
        {
        }

        protected override IList<Subtitle> GetSubtitles()
        {
            var result = new List<Subtitle>();
            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding))
            {
                input.Skip(0x180);

                string str = input.ReadStringSerialized(0x04);

                var subtitle = new Subtitle
                {
                    Offset = 0x180,
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
            string outputPath = System.IO.Path.Combine(outputFolder, RelativePath);
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath));

            IList<Subtitle> subtitles = GetSubtitles();

            using (var fsInput = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fsInput, FileEncoding))
            using (var fsOutput = new FileStream(outputPath, FileMode.Create))
            using (var output = new ExtendedBinaryWriter(fsOutput, FileEncoding))
            {
                output.Write(input.ReadBytes(0x180));
                
                string str = input.ReadStringSerialized(0x04);
                
                Subtitle sub = subtitles.First(x => x.Offset == 0x180);

                output.WriteStringSerialized(sub.Translation, 0x04);

                int remainderLength = (int)(input.Length - input.Position);
                byte[] remainder = input.ReadBytes(remainderLength);
                output.Write(remainder);
            }
        }
    }
}
