using System.Collections.Generic;
using System.IO;
using System.Linq;
using TF.Core.Files;
using TF.Core.TranslationEntities;
using TF.IO;
using TFGame.TrailsSky.Views;
using WeifenLuo.WinFormsUI.Docking;

namespace TFGame.TrailsSky.Files.DT
{
    public abstract class GenericDTFile : BinaryTextFileWithOffsetTable
    {
        protected abstract int NumStringsPerItem { get; }
        protected abstract int UnknownSize { get; }

        protected GenericDTFile(string path, string changesFolder, System.Text.Encoding encoding) : base(path, changesFolder, encoding)
        {

        }

        public override void Open(DockPanel panel, ThemeBase theme)
        {
            _view = new View(theme);

            _subtitles = GetSubtitles();
            _view.LoadData(_subtitles.Where(x => !string.IsNullOrWhiteSpace(x.Text)).ToList());
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
                    var item = ReadItem(input);

                    foreach (var subtitle in item.Strings)
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
                var tableEnd = input.PeekUInt16();

                long outputOffset = tableEnd;

                while (input.Position < tableEnd)
                {
                    var item = ReadItem(input);

                    outputOffset = WriteItem(output, subtitles, item, outputOffset);
                }
            }
        }

        private DTElement ReadItem(ExtendedBinaryReader input)
        {
            var offset = input.ReadUInt16();
            var returnPos = input.Position;

            input.Seek(offset, SeekOrigin.Begin);
            var id = input.ReadUInt16();
            var unknown = input.ReadBytes(UnknownSize);

            var item = new DTElement(NumStringsPerItem) { Id = id, Unknown = unknown };
            for (var i = 0; i < NumStringsPerItem; i++)
            {
                var str = ReadSubtitle(input);
                item.Strings.Add(str);
            }

            input.Seek(returnPos, SeekOrigin.Begin);
            return item;
        }

        protected override Subtitle ReadSubtitle(ExtendedBinaryReader input)
        {
            var offset = input.ReadUInt16();
            var subtitle = ReadSubtitle(input, offset, true);

            return subtitle;
        }

        private long WriteItem(ExtendedBinaryWriter output, IList<Subtitle> subtitles, DTElement item, long outputOffset)
        {
            output.Write((ushort)outputOffset);
            var returnPos = output.Position;

            output.Seek(outputOffset, SeekOrigin.Begin);
            output.Write(item.Id);
            output.Write(item.Unknown);

            var currentOffset = outputOffset + 2 + UnknownSize + 2 * NumStringsPerItem;

            foreach (var str in item.Strings)
            {
                var subtitle = subtitles.First(x => x.Offset == str.Offset);
                currentOffset = WriteSubtitle(output, subtitle, currentOffset, true);
            }

            output.Seek(returnPos, SeekOrigin.Begin);

            return currentOffset;
        }

        protected override long WriteSubtitle(ExtendedBinaryWriter output, Subtitle subtitle, long offset, bool returnToPos)
        {
            var result = offset;
            var pos = output.Position;

            if (subtitle == null || offset == 0)
            {
                output.Write((ushort)0);
            }
            else
            {
                output.Write((ushort)offset);

                output.Seek(offset, SeekOrigin.Begin);
                output.WriteString(subtitle.Translation);

                result = output.Position;
            }

            if (returnToPos)
            {
                output.Seek(pos + 2, SeekOrigin.Begin);
            }

            return result;
        }
    }
}
