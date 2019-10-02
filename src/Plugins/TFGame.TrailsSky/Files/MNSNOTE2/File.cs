using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TF.Core.Files;
using TF.Core.TranslationEntities;
using TF.IO;

namespace TFGame.TrailsSky.Files.MNSNOTE2
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
                while (input.Position < input.Length)
                {
                    input.Skip(4);

                    var fileSize = input.ReadInt32();

                    var offsetBase = (int)input.Position;
                    var bytes = input.ReadBytes(fileSize);
                    result.AddRange(ReadSubtitles(bytes, offsetBase));
                }
            }

            LoadChanges(result);

            return result;
        }

        private IList<Subtitle> ReadSubtitles(byte[] bytes, int offsetBase)
        {
            var result = new List<Subtitle>();

            using (var fs = new MemoryStream(bytes))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding))
            {
                var magic = input.ReadInt32();
                var count = 0x08;
                var searchPattern = new byte[]{ 0x00, 0x00, 0x00, 0x08 };
                if (magic == 0x00100187)
                {
                    count = 0x0E;
                    searchPattern = new byte[]{ 0x00, 0x00, 0x00, 0x0E };
                }
                var patternPos = new List<int>();
                var pos = input.FindPattern(searchPattern);
                while (pos != -1)
                {
                    patternPos.Add(pos);
                    pos = input.FindPattern(searchPattern);
                }

                if (patternPos.Count > 0)
                {
                    input.Seek(patternPos[patternPos.Count - 1] + 4, SeekOrigin.Begin);

                    for (var i = 0; i < count; i++)
                    {
                        input.Skip(0x1C);
                        var txt = ReadSubtitle(input);
                        if (!string.IsNullOrWhiteSpace(txt.Text))
                        {
                            txt.Offset += offsetBase;
                            txt.PropertyChanged += SubtitlePropertyChanged;
                            result.Add(txt);
                        }

                        txt = ReadSubtitle(input);
                        if (!string.IsNullOrWhiteSpace(txt.Text))
                        {
                            txt.Offset += offsetBase;
                            txt.PropertyChanged += SubtitlePropertyChanged;
                            result.Add(txt);
                        }
                    }

                    input.Skip(4);
                    var subtitle = ReadSubtitle(input);
                    if (!string.IsNullOrWhiteSpace(subtitle.Text))
                    {
                        subtitle.Offset += offsetBase;
                        subtitle.PropertyChanged += SubtitlePropertyChanged;
                        result.Add(subtitle);
                    }

                    subtitle = ReadSubtitle(input);
                    if (!string.IsNullOrWhiteSpace(subtitle.Text))
                    {
                        subtitle.Offset += offsetBase;
                        subtitle.PropertyChanged += SubtitlePropertyChanged;
                        result.Add(subtitle);
                    }
                }
            }

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
                while (input.Position < input.Length)
                {
                    output.Write(input.ReadBytes(4));

                    var fileSize = input.ReadInt32();
                    var offsetBase = (int)input.Position;
                    var bytes = input.ReadBytes(fileSize);
                    var outputBytes = Rebuild(bytes, offsetBase, subtitles);

                    output.Write(outputBytes.Length);
                    output.Write(outputBytes);
                }

            }
        }

        private byte[] Rebuild(byte[] inputBytes, int offsetBase, IList<Subtitle> subtitles)
        {
            using (var fsInput = new MemoryStream(inputBytes))
            using (var input = new ExtendedBinaryReader(fsInput, FileEncoding))
            using (var fsOutput = new MemoryStream())
            using (var output = new ExtendedBinaryWriter(fsOutput, FileEncoding))
            {
                var magic = input.ReadInt32();

                var count = 0x08;
                var searchPattern = new byte[]{ 0x00, 0x00, 0x00, 0x08 };
                if (magic == 0x00100187)
                {
                    count = 0x0E;
                    searchPattern = new byte[]{ 0x00, 0x00, 0x00, 0x0E };
                }

                var patternPos = new List<int>();
                var pos = input.FindPattern(searchPattern);
                while (pos != -1)
                {
                    patternPos.Add(pos);
                    pos = input.FindPattern(searchPattern);
                }

                if (patternPos.Count > 0)
                {
                    input.Seek(0, SeekOrigin.Begin);
                    output.Write(input.ReadBytes(patternPos[patternPos.Count - 1] + 4));

                    for (var i = 0; i < count; i++)
                    {
                        output.Write(input.ReadBytes(0x1C));
                        var txt = ReadSubtitle(input);

                        var sub = subtitles.FirstOrDefault(x => x.Offset == txt.Offset + offsetBase);
                        output.WriteString(sub != null ? sub.Translation : txt.Text);

                        txt = ReadSubtitle(input);
                        sub = subtitles.FirstOrDefault(x => x.Offset == txt.Offset + offsetBase);
                        output.WriteString(sub != null ? sub.Translation : txt.Text);
                    }

                    output.Write(input.ReadBytes(4));
                    var subtitle = ReadSubtitle(input);
                    var sub2 = subtitles.FirstOrDefault(x => x.Offset == subtitle.Offset + offsetBase);
                    output.WriteString(sub2 != null ? sub2.Translation : subtitle.Text);

                    subtitle = ReadSubtitle(input);
                    sub2 = subtitles.FirstOrDefault(x => x.Offset == subtitle.Offset + offsetBase);
                    output.WriteString(sub2 != null ? sub2.Translation : subtitle.Text);
                }

                return fsOutput.ToArray();
            }
        }
    }
}
