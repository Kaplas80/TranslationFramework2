namespace TFGame.DiscoElysium.Files.I2Text
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
                input.Skip(0x0C);

                var termCount = input.ReadInt32();

                for (var termIndex = 0; termIndex < termCount; termIndex++)
                {
                    var term = input.ReadStringSerialized(0x04);
                    var termType = input.ReadInt32();

                    var description = input.ReadStringSerialized(0x04);

                    var languageCount = input.ReadInt32();

                    for (var i = 0; i < languageCount; i++)
                    {
                        var sub = input.ReadStringSerialized(0x04);

                        if (i == 0 && !string.IsNullOrEmpty(sub)) // Inglés
                        {
                            var subtitle = new DiscoElysiumSubtitle
                            {
                                Id = term,
                                Offset = 0,
                                Text = sub,
                                Loaded = sub,
                                Translation = sub
                            };

                            subtitle.PropertyChanged += SubtitlePropertyChanged;
                            result.Add(subtitle);
                        }
                    }

                    var flagCount = input.ReadInt32();
                    input.Skip(flagCount);
                    input.SkipPadding(0x04);

                    var languageTouchCount = input.ReadInt32();
                    for (var i = 0; i < languageTouchCount; i++)
                    {
                        input.ReadStringSerialized(0x04);
                    }
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
                output.Write(input.ReadBytes(0x0C));

                var termCount = input.ReadInt32();
                output.Write(termCount);
                for (var termIndex = 0; termIndex < termCount; termIndex++)
                {
                    var term = input.ReadStringSerialized(0x04);
                    output.WriteStringSerialized(term, 0x04);

                    var termType = input.ReadInt32();
                    output.Write(termType);

                    var description = input.ReadStringSerialized(0x04);
                    output.WriteStringSerialized(description, 0x04);

                    var languageCount = input.ReadInt32();
                    output.Write(languageCount);

                    for (var i = 0; i < languageCount; i++)
                    {
                        var sub = input.ReadStringSerialized(0x04);
                        if (i != 0)
                        {
                            output.WriteStringSerialized(sub, 0x04);
                        }
                        else
                        {
                            if (dictionary.TryGetValue(term, out DiscoElysiumSubtitle subtitle))
                            {
                                output.WriteStringSerialized(subtitle.Translation, 0x04);
                            }
                            else
                            {
                                output.WriteStringSerialized(sub, 0x04);
                            }
                        }
                    }

                    var flagCount = input.ReadInt32();
                    output.Write(flagCount);
                    output.Write(input.ReadBytes(flagCount));
                    input.SkipPadding(0x04);
                    output.WritePadding(0x04);

                    var languageTouchCount = input.ReadInt32();
                    output.Write(languageTouchCount);
                    for (var i = 0; i < languageTouchCount; i++)
                    {
                        var sub = input.ReadStringSerialized(0x04);
                        output.WriteStringSerialized(sub, 0x04);
                    }
                }

                var remainderLength = (int)(input.Length - input.Position);
                var remainder = input.ReadBytes(remainderLength);
                output.Write(remainder);
            }
        }
    }
}
