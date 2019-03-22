using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TF.Core.Files;
using TF.Core.TranslationEntities;
using TF.IO;
using TFGame.TrailsSky.Views;
using WeifenLuo.WinFormsUI.Docking;

namespace TFGame.TrailsSky.Files.DT
{
    public class BookFile : BinaryTextFileWithOffsetTable
    {
        public BookFile(string path, string changesFolder, System.Text.Encoding encoding) : base(path, changesFolder, encoding)
        {

        }

        public override void Open(DockPanel panel, ThemeBase theme)
        {
            _view = new View(theme);

            _subtitles = GetSubtitles();
            _view.LoadData(_subtitles.Where(x => !string.IsNullOrEmpty(x.Text)).ToList());
            _view.Show(panel, DockState.Document);
        }

        protected override IList<Subtitle> GetSubtitles()
        {
            var result = new List<Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding))
            {
                var tableEnd = input.PeekUInt16();
                while (input.Position < tableEnd)
                {
                    var subtitle = ReadSubtitle(input);
                    subtitle.PropertyChanged += SubtitlePropertyChanged;
                    result.Add(subtitle);
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
                var tableEnd = input.PeekUInt16();
                long outputOffset = tableEnd;

                while (input.Position < tableEnd)
                {
                    var inputSubtitle = ReadSubtitle(input);
                    outputOffset = WriteSubtitle(output, subtitles, inputSubtitle.Offset, outputOffset);
                }
            }
        }

        protected override Subtitle ReadSubtitle(ExtendedBinaryReader input)
        {
            var offset = input.ReadUInt16();
            var returnPos = input.Position;

            input.Seek(offset, SeekOrigin.Begin);

            using (var ms = new MemoryStream())
            {
                var subtitle = new Subtitle { Offset = offset };

                var b = input.ReadByte();

                while (b != 0x00)
                {
                    switch (b)
                    {
                        case 0x07: // Color
                        {
                            var b2 = input.ReadByte();
                            var buffer = this.FileEncoding.GetBytes($"<Color: {b2:X2}>");
                            ms.Write(buffer, 0, buffer.Length);
                            break;
                        }
                        case 0x1F: // Item
                        {
                            var id = input.ReadUInt16();
                            var buffer = this.FileEncoding.GetBytes($"<Item: {id:X4}>");
                            ms.Write(buffer, 0, buffer.Length);
                            break;
                        }
                        default:
                        {
                            ms.WriteByte(b); // Es el encoding quien se encarga de sustituirlo si es necesario
                            break;
                        }
                    }

                    b = input.Position == input.Length ? (byte) 0x00 : input.ReadByte();
                }

                var text = FileEncoding.GetString(ms.ToArray());
                subtitle.Text = text;
                subtitle.Translation = text;
                subtitle.Loaded = text;

                input.Seek(returnPos, SeekOrigin.Begin);
                return subtitle;
            }
        }

        protected override long WriteSubtitle(ExtendedBinaryWriter output, IList<Subtitle> subtitles, long inputOffset, long outputOffset)
        {
            var sub = subtitles.First(x => x.Offset == inputOffset);

            output.Write((ushort) outputOffset);
            var returnPos = output.Position;

            var evaluator = new MatchEvaluator(SubtitleMatchEvaluator);

            var newSub = Regex.Replace(sub.Translation, @"<Color: (?<colorValue>[0-9A-Fa-f]{2})>", evaluator);
            newSub = Regex.Replace(newSub, @"<Item: (?<itemValue>[0-9A-Fa-f]{4})>", evaluator);

            output.Seek(outputOffset, SeekOrigin.Begin);
            output.WriteString(newSub);

            var result = output.Position;

            output.Seek(returnPos, SeekOrigin.Begin);
            return result;
        }

        private static string SubtitleMatchEvaluator(Match match)
        {
            if (match.Groups[1].Name == "colorValue")
            {
                var value = new byte[]
                {
                    0x07,
                    byte.Parse(match.Groups[1].Value, NumberStyles.AllowHexSpecifier),
                };
                return System.Text.Encoding.ASCII.GetString(value);
            }

            if (match.Groups[1].Name == "itemValue")
            {
                var value = new byte[]
                {
                    0x1F,
                    byte.Parse(match.Groups[1].Value.Substring(0, 2), NumberStyles.AllowHexSpecifier),
                    byte.Parse(match.Groups[1].Value.Substring(2, 2), NumberStyles.AllowHexSpecifier),
                };
                return System.Text.Encoding.ASCII.GetString(value);
            }

            return match.Value;
        }
    }
}
