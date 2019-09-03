using System;
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
    public class TownFile : BinaryTextFileWithOffsetTable
    {
        public TownFile(string path, string changesFolder, System.Text.Encoding encoding) : base(path, changesFolder, encoding)
        {

        }

        public override void Open(DockPanel panel)
        {
            _view = new View(LineEnding);

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
                var count = input.ReadUInt16();
                for (var i = 0; i < count; i++)
                {
                    var item = ReadItem(input);

                    item.Title.PropertyChanged += SubtitlePropertyChanged;
                    
                    result.Add(item.Title);
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
                var count = input.ReadUInt16();
                output.Write(count);

                long outputOffset = 2 + 2 * count;

                for (var i = 0; i < count; i++)
                {
                    var item = ReadItem(input);
                    outputOffset = WriteItem(output, subtitles, item, outputOffset);
                }
            }
        }

        private TownElement ReadItem(ExtendedBinaryReader input)
        {
            var returnPos = input.Position + 2;

            var title = ReadSubtitle(input);
            if (string.IsNullOrEmpty(title.Text))
            {
                input.Seek(-1, SeekOrigin.Current);
            }
            var unknown = input.ReadByte();
            var item = new TownElement { Title = title, Unknown = unknown };

            input.Seek(returnPos, SeekOrigin.Begin);
            return item;
        }

        protected override Subtitle ReadSubtitle(ExtendedBinaryReader input)
        {
            var offset = input.ReadUInt16();
            var subtitle = ReadSubtitle(input, offset, false);

            return subtitle;
        }

        private long WriteItem(ExtendedBinaryWriter output, IList<Subtitle> subtitles, TownElement item, long outputOffset)
        {
            var returnPos = output.Position + 2;

            var subtitle = subtitles.First(x => x.Offset == item.Title.Offset);
            WriteSubtitle(output, subtitle, outputOffset, false);
            output.Write(item.Unknown);

            var currentOffset = output.Position;

            output.Seek(returnPos, SeekOrigin.Begin);

            return currentOffset;
        }

        protected override long WriteSubtitle(ExtendedBinaryWriter output, Subtitle subtitle, long offset, bool returnToPos)
        {
            var result = offset;
            var pos = output.Position;

            output.Write((ushort)offset);
            output.Seek(offset, SeekOrigin.Begin);

            if (!string.IsNullOrEmpty(subtitle.Text))
            {
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
