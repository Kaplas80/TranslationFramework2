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

        public File(string path, string changesFolder, System.Text.Encoding encoding) : base(path, changesFolder, encoding)
        {
        }

        protected override IList<Subtitle> GetSubtitles()
        {
            var result = new List<Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding))
            {
                var patternPos = new List<int>();
                var pos = input.FindPattern(SearchPattern);
                while (pos != -1)
                {
                    patternPos.Add(pos);
                    pos = input.FindPattern(SearchPattern);
                }

                if (patternPos.Count > 0)
                {
                    input.Seek(patternPos[patternPos.Count - 1] + 3, SeekOrigin.Begin);
                    var count = input.ReadByte();

                    for (var i = 0; i < count; i++)
                    {
                        input.Skip(0x1C);
                        var txt = ReadSubtitle(input);
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
                    var subtitle = ReadSubtitle(input);
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
            var outputPath = System.IO.Path.Combine(outputFolder, RelativePath);
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath));

            var subtitles = GetSubtitles();

            using (var fsInput = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fsInput, FileEncoding))
            using (var fsOutput = new FileStream(outputPath, FileMode.Create))
            using (var output = new ExtendedBinaryWriter(fsOutput, FileEncoding))
            {
                var patternPos = new List<int>();
                var pos = input.FindPattern(SearchPattern);
                while (pos != -1)
                {
                    patternPos.Add(pos);
                    pos = input.FindPattern(SearchPattern);
                }

                if (patternPos.Count > 0)
                {
                    input.Seek(0, SeekOrigin.Begin);
                    output.Write(input.ReadBytes(patternPos[patternPos.Count - 1] + 3));

                    var count = input.ReadByte();
                    output.Write(count);

                    for (var i = 0; i < count; i++)
                    {
                        output.Write(input.ReadBytes(0x1C));
                        var txt = ReadSubtitle(input);

                        var sub = subtitles.FirstOrDefault(x => x.Offset == txt.Offset);
                        output.WriteString(sub != null ? sub.Translation : txt.Text);

                        txt = ReadSubtitle(input);
                        sub = subtitles.FirstOrDefault(x => x.Offset == txt.Offset);
                        output.WriteString(sub != null ? sub.Translation : txt.Text);
                    }

                    output.Write(input.ReadBytes(4));
                    var subtitle = ReadSubtitle(input);
                    var sub2 = subtitles.FirstOrDefault(x => x.Offset == subtitle.Offset);
                    output.WriteString(sub2 != null ? sub2.Translation : subtitle.Text);

                    subtitle = ReadSubtitle(input);
                    sub2 = subtitles.FirstOrDefault(x => x.Offset == subtitle.Offset);
                    output.WriteString(sub2 != null ? sub2.Translation : subtitle.Text);
                }
            }
        }

    }
}
