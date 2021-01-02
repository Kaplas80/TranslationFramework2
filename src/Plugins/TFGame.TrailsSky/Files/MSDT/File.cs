using System.Collections.Generic;
using System.IO;
using System.Linq;
using TF.Core.Files;
using TF.Core.TranslationEntities;
using TF.IO;

namespace TFGame.TrailsSky.Files.MSDT
{
    public class File : BinaryTextFile
    {
        protected virtual byte[] SearchPattern => new byte[] { 0x00, 0x00, 0x00, 0x08 };
        private readonly System.Text.Encoding customEncoding;

        public File(string gameName, string path, string changesFolder, System.Text.Encoding encoding) : base(gameName, path, changesFolder, encoding)
        {
            customEncoding = new Encoding();
        }

        protected override IList<Subtitle> GetSubtitles()
        {
            var result = new List<Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, customEncoding))
            {
                var patternPos = new List<int>();
                int pos = input.FindPattern(SearchPattern);
                while (pos != -1)
                {
                    patternPos.Add(pos);
                    pos = input.FindPattern(SearchPattern);
                }

                if (patternPos.Count > 0)
                {
                    input.Seek(patternPos[patternPos.Count - 1] + 3, SeekOrigin.Begin);
                    byte count = input.ReadByte();

                    for (int i = 0; i < count; i++)
                    {
                        input.Skip(0x1C);
                        Subtitle txt = ReadSubtitle(input);
                        if (!string.IsNullOrWhiteSpace(txt.Text))
                        {
                            txt.PropertyChanged += SubtitlePropertyChanged;
                            result.Add(txt);
                        }

                        txt = ReadSubtitle(input);
                        if (!string.IsNullOrWhiteSpace(txt.Text))
                        {
                            txt.PropertyChanged += SubtitlePropertyChanged;
                            result.Add(txt);
                        }
                    }

                    input.Skip(4);
                    Subtitle subtitle = ReadSubtitle(input);
                    if (!string.IsNullOrWhiteSpace(subtitle.Text))
                    {
                        subtitle.PropertyChanged += SubtitlePropertyChanged;
                        result.Add(subtitle);
                    }

                    subtitle = ReadSubtitle(input);
                    if (!string.IsNullOrWhiteSpace(subtitle.Text))
                    {
                        subtitle.PropertyChanged += SubtitlePropertyChanged;
                        result.Add(subtitle);
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

            using (var fsInput = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fsInput, customEncoding))
            using (var fsOutput = new FileStream(outputPath, FileMode.Create))
            using (var output = new ExtendedBinaryWriter(fsOutput, customEncoding))
            {
                var patternPos = new List<int>();
                int pos = input.FindPattern(SearchPattern);
                while (pos != -1)
                {
                    patternPos.Add(pos);
                    pos = input.FindPattern(SearchPattern);
                }

                if (patternPos.Count > 0)
                {
                    input.Seek(0, SeekOrigin.Begin);
                    output.Write(input.ReadBytes(patternPos[patternPos.Count - 1] + 3));

                    byte count = input.ReadByte();
                    output.Write(count);

                    for (int i = 0; i < count; i++)
                    {
                        output.Write(input.ReadBytes(0x1C));
                        Subtitle txt = ReadSubtitle(input);

                        Subtitle sub = subtitles.FirstOrDefault(x => x.Offset == txt.Offset);
                        output.WriteString(sub != null ? sub.Translation : txt.Text);

                        txt = ReadSubtitle(input);
                        sub = subtitles.FirstOrDefault(x => x.Offset == txt.Offset);
                        output.WriteString(sub != null ? sub.Translation : txt.Text);
                    }

                    output.Write(input.ReadBytes(4));
                    Subtitle subtitle = ReadSubtitle(input);
                    Subtitle sub2 = subtitles.FirstOrDefault(x => x.Offset == subtitle.Offset);
                    output.WriteString(sub2 != null ? sub2.Translation : subtitle.Text);

                    subtitle = ReadSubtitle(input);
                    sub2 = subtitles.FirstOrDefault(x => x.Offset == subtitle.Offset);
                    output.WriteString(sub2 != null ? sub2.Translation : subtitle.Text);
                }
            }
        }
    }
}
