namespace UnityGame.Files.I2Text
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using TF.Core.Files;
    using TF.Core.TranslationEntities;
    using TF.IO;

    public abstract class File : BinaryTextFileWithIds
    {
        protected abstract int LanguageIndex { get; }

        protected File(string gameName, string path, string changesFolder, Encoding encoding) : base(gameName, path, changesFolder, encoding)
        {
        }

        protected override IList<Subtitle> GetSubtitles()
        {
            var result = new List<Subtitle>();
            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding))
            {
                input.Skip(0x0C);

                int termCount = input.ReadInt32();

                for (int termIndex = 0; termIndex < termCount; termIndex++)
                {
                    string term = input.ReadStringSerialized(0x04);
                    int termType = input.ReadInt32();

                    string description = input.ReadStringSerialized(0x04);

                    int languageCount = input.ReadInt32();

                    for (int i = 0; i < languageCount; i++)
                    {
                        string sub = input.ReadStringSerialized(0x04);

                        if (i == LanguageIndex && !string.IsNullOrEmpty(sub)) 
                        {
                            var subtitle = new SubtitleWithId
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

                    int flagCount = input.ReadInt32();
                    input.Skip(flagCount);
                    input.SkipPadding(0x04);

                    int languageTouchCount = input.ReadInt32();
                    for (int i = 0; i < languageTouchCount; i++)
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
            string outputPath = System.IO.Path.Combine(outputFolder, RelativePath);
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath));

            IList<Subtitle> subtitles = GetSubtitles();
            List<SubtitleWithId> subs = subtitles.Select(subtitle => subtitle as SubtitleWithId).ToList();
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
                output.Write(input.ReadBytes(0x0C));

                int termCount = input.ReadInt32();
                output.Write(termCount);
                for (int termIndex = 0; termIndex < termCount; termIndex++)
                {
                    string term = input.ReadStringSerialized(0x04);
                    output.WriteStringSerialized(term, 0x04);

                    int termType = input.ReadInt32();
                    output.Write(termType);

                    string description = input.ReadStringSerialized(0x04);
                    output.WriteStringSerialized(description, 0x04);

                    int languageCount = input.ReadInt32();
                    output.Write(languageCount);

                    for (int i = 0; i < languageCount; i++)
                    {
                        string sub = input.ReadStringSerialized(0x04);
                        if (i != LanguageIndex)
                        {
                            output.WriteStringSerialized(sub, 0x04);
                        }
                        else
                        {
                            if (dictionary.TryGetValue(term, out SubtitleWithId subtitle))
                            {
                                output.WriteStringSerialized(subtitle.Translation, 0x04);
                            }
                            else
                            {
                                output.WriteStringSerialized(sub, 0x04);
                            }
                        }
                    }

                    int flagCount = input.ReadInt32();
                    output.Write(flagCount);
                    output.Write(input.ReadBytes(flagCount));
                    input.SkipPadding(0x04);
                    output.WritePadding(0x04);

                    int languageTouchCount = input.ReadInt32();
                    output.Write(languageTouchCount);
                    for (int i = 0; i < languageTouchCount; i++)
                    {
                        string sub = input.ReadStringSerialized(0x04);
                        output.WriteStringSerialized(sub, 0x04);
                    }
                }

                int remainderLength = (int)(input.Length - input.Position);
                byte[] remainder = input.ReadBytes(remainderLength);
                output.Write(remainder);
            }
        }
    }
}
