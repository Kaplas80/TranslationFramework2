namespace TFGame.NightCry.Files.UILabel
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using TF.Core.Files;
    using TF.Core.TranslationEntities;
    using TF.IO;

    public class File : BinaryTextFileWithIds
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
                // AnchorPoints
                for (int i = 0; i < 4; i++)
                {
                    input.Skip(0x0C);
                    input.Skip(0x08);
                }

                input.Skip(0x04); // updateAnchors
                input.Skip(4 * 0x04); // Color
                input.Skip(0x04); // pivot
                input.Skip(0x04); // width
                input.Skip(0x04); // height
                input.Skip(0x04); // depth
                input.Skip(0x04); // autoresizeBoxCollider
                input.Skip(0x04); // hideIfOffScreen
                input.Skip(0x04); // keepAspectRatio
                input.Skip(0x04); // aspectRatio
                input.Skip(0x04); // keepCrispWhenShrunk
                input.Skip(0x0C); // truetypefont
                input.Skip(0x0C); // font
 
                var str = input.ReadStringSerialized(0x04);

                var subtitle = new SubtitleWithId
                {
                    Id = str,
                    Offset = 0,
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
            var subs = subtitles.Select(subtitle => subtitle as SubtitleWithId).ToList();
            var dictionary = new Dictionary<string, SubtitleWithId>(subs.Count);
            foreach (SubtitleWithId subtitle in subs)
            {
                dictionary.Add(subtitle.Id, subtitle);
            }

            using (var fsInput = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fsInput, FileEncoding))
            using (var fsOutput = new FileStream(outputPath, FileMode.Create))
            using (var output = new ExtendedBinaryWriter(fsOutput, FileEncoding))
            {
                // AnchorPoints
                for (int i = 0; i < 4; i++)
                {
                    output.Write(input.ReadBytes(0x0C));
                    output.Write(input.ReadBytes(0x08));
                }

                output.Write(input.ReadBytes(0x04));
                output.Write(input.ReadBytes(4 * 0x04));
                output.Write(input.ReadBytes(0x04));
                output.Write(input.ReadBytes(0x04));
                output.Write(input.ReadBytes(0x04));
                output.Write(input.ReadBytes(0x04));
                output.Write(input.ReadBytes(0x04));
                output.Write(input.ReadBytes(0x04));
                output.Write(input.ReadBytes(0x04));
                output.Write(input.ReadBytes(0x04));
                output.Write(input.ReadBytes(0x04));
                output.Write(input.ReadBytes(0x0C));
                output.Write(input.ReadBytes(0x0C));

                var str = input.ReadStringSerialized(0x04);
                output.WriteStringSerialized(dictionary.TryGetValue(str, out SubtitleWithId subtitle) ? subtitle.Translation : str, 0x04);

                output.Write(input.ReadBytes(0x84));
            }
        }
    }
}
