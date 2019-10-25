namespace TFGame.DiscoElysium.Files.UnityUIText
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using TF.Core.TranslationEntities;
    using TF.IO;
    using TFGame.DiscoElysium.Files.Common;

    public class File : DiscoElysiumTextFile
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
                // Material
                input.Skip(0x04);
                input.Skip(0x08);

                // ColorRGBA
                input.Skip(4 * 0x04);

                // RaycastTarget
                input.Skip(0x04);

                // CullStateChangedEvent
                input.Skip(0x04);

                // PersistentCall
                input.ReadStringSerialized(0x04);

                // FontData
                input.Skip(0x04);
                input.Skip(0x08);
                input.Skip(0x04); // Font size
                input.Skip(0x04); // Font style
                input.Skip(0x04); // Best Fit
                input.Skip(0x04); // Min size
                input.Skip(0x04); // Max size
                input.Skip(0x04); // Alignment
                input.Skip(0x04); // Align by geometry
                input.Skip(0x04); // Rich Text
                input.Skip(0x04); // Horizontal Overflow
                input.Skip(0x04); // Vertical Overflow
                input.Skip(0x04); // Line spacing

                string sub = input.ReadStringSerialized(0x04);

                var subtitle = new DiscoElysiumSubtitle
                {
                    Id = sub,
                    Offset = 0,
                    Text = sub,
                    Loaded = sub,
                    Translation = sub
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
            var subs = subtitles.Select(subtitle => subtitle as DiscoElysiumSubtitle).ToList();
            var dictionary = new Dictionary<string, DiscoElysiumSubtitle>(subs.Count);
            foreach (DiscoElysiumSubtitle subtitle in subs)
            {
                dictionary.Add(subtitle.Id, subtitle);
            }

            using (var fsInput = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fsInput, FileEncoding))
            using (var fsOutput = new FileStream(outputPath, FileMode.Create))
            using (var output = new ExtendedBinaryWriter(fsOutput, FileEncoding))
            {
                // Material
                output.Write(input.ReadBytes(0x04));
                output.Write(input.ReadBytes(0x08));

                // ColorRGBA
                output.Write(input.ReadBytes(4 * 0x04));

                // RaycastTarget
                output.Write(input.ReadBytes(0x04));

                // CullStateChangedEvent
                output.Write(input.ReadBytes(0x04));

                // PersistentCall
                output.WriteStringSerialized(input.ReadStringSerialized(0x04), 0x04);

                // FontData
                output.Write(input.ReadBytes(0x04));
                output.Write(input.ReadBytes(0x08));
                int fontSize = input.ReadInt32();
                output.Write(fontSize <= 0x00000012 ? 0x00000011 : fontSize);
                output.Write(input.ReadBytes(0x04)); // Font style
                input.ReadBytes(0x04);
                output.Write(0x00000001); // Best Fit
                output.Write(input.ReadBytes(0x04)); // Min size
                output.Write(input.ReadBytes(0x04)); // Max size
                output.Write(input.ReadBytes(0x04)); // Alignment
                output.Write(input.ReadBytes(0x04)); // Align by geometry
                output.Write(input.ReadBytes(0x04)); // Rich Text
                output.Write(input.ReadBytes(0x04)); // Horizontal Overflow
                output.Write(input.ReadBytes(0x04)); // Vertical Overflow
                output.Write(input.ReadBytes(0x04)); // Line spacing

                string sub = input.ReadStringSerialized(0x04);

                output.WriteStringSerialized(
                    dictionary.TryGetValue(sub, out DiscoElysiumSubtitle subtitle) ? subtitle.Translation : sub, 0x04);
            }
        }
    }
}
