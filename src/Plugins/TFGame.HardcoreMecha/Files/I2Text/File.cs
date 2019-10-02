using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TF.Core.Files;
using TF.Core.TranslationEntities;
using TF.IO;

namespace TFGame.HardcoreMecha.Files.I2Text
{
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
                input.Skip(0x38);
                var termCount = input.ReadInt32();

                for (var termIndex = 0; termIndex < termCount; termIndex++)
                {
                    var termLength = input.ReadInt32();
                    var term = input.ReadBytes(termLength);
                    input.SkipPadding(0x04);

                    var termType = input.ReadInt32();

                    var descriptionLength = input.ReadInt32();
                    if (descriptionLength > 0)
                    {
                        var description = input.ReadBytes(descriptionLength);
                        input.SkipPadding(0x04);
                    }

                    var languageCount = input.ReadInt32();

                    for (var i = 0; i < languageCount; i++)
                    {
                        var subLength = input.ReadInt32();
                        if (subLength > 0)
                        {
                            var offset = input.Position;
                            var sub = input.ReadBytes(subLength);
                            input.SkipPadding(0x04);

                            if (i == 1) // Inglés
                            {
                                var str = FileEncoding.GetString(sub);

                                var subtitle = new Subtitle
                                {
                                    Offset = offset,
                                    Text = str,
                                    Loaded = str,
                                    Translation = str
                                };

                                subtitle.PropertyChanged += SubtitlePropertyChanged;
                                result.Add(subtitle);
                            }
                        }
                    }

                    input.Skip(0x10);
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
            using (var input = new ExtendedBinaryReader(fsInput, FileEncoding))
            using (var fsOutput = new FileStream(outputPath, FileMode.Create))
            using (var output = new ExtendedBinaryWriter(fsOutput, FileEncoding))
            {
                output.Write(input.ReadBytes(0x38));
                var termCount = input.ReadInt32();
                output.Write(termCount);
                for (var termIndex = 0; termIndex < termCount; termIndex++)
                {
                    var termLength = input.ReadInt32();
                    var term = input.ReadBytes(termLength);
                    input.SkipPadding(0x04);

                    output.Write(termLength);
                    output.Write(term);
                    output.WritePadding(0x04);

                    var termType = input.ReadInt32();
                    output.Write(termType);

                    var descriptionLength = input.ReadInt32();
                    output.Write(descriptionLength);
                    if (descriptionLength > 0)
                    {
                        var description = input.ReadBytes(descriptionLength);
                        input.SkipPadding(0x04);
                        output.Write(description);
                        output.WritePadding(0x04);
                    }

                    var languageCount = input.ReadInt32();
                    output.Write(languageCount);

                    for (var i = 0; i < languageCount; i++)
                    {
                        if (i != 1)
                        {
                            var subLength = input.ReadInt32();
                            output.Write(subLength);
                            if (subLength > 0)
                            {
                                var txt = input.ReadBytes(subLength);
                                input.SkipPadding(0x04);
                                output.Write(txt);
                                output.WritePadding(0x04);
                            }
                        }
                        else
                        {
                            var subLength = input.ReadInt32();
                            if (subLength > 0)
                            {
                                var offset = input.Position;
                                input.ReadBytes(subLength);
                                input.SkipPadding(0x04);

                                var sub = subtitles.First(x => x.Offset == offset);
                                var data = FileEncoding.GetBytes(sub.Translation);
                                output.Write(data.Length);
                                output.Write(data);
                                output.WritePadding(0x04);
                            }
                            else
                            {
                                output.Write(subLength);
                            }
                        }
                    }

                    output.Write(input.ReadBytes(0x10));
                }

                var remainderLength = (int)(input.Length - input.Position);
                var remainder = input.ReadBytes(remainderLength);
                output.Write(remainder);
            }
        }
    }
}
