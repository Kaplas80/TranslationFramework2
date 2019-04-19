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
    public class ShopFile : BinaryTextFileWithOffsetTable
    {
        public ShopFile(string path, string changesFolder, System.Text.Encoding encoding) : base(path, changesFolder, encoding)
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

                var itemOffsets = new List<ushort>(tableEnd / 2);
                var itemLengths = new int[tableEnd / 2];

                for (var i = 0; i < tableEnd / 2; i++)
                {
                    var offset = input.ReadUInt16();
                    itemOffsets.Add(offset);
                }

                itemOffsets.Sort();

                for (var i = 0; i < itemOffsets.Count - 1; i++)
                {
                    itemLengths[i] = itemOffsets[i + 1] - itemOffsets[i];
                }

                itemLengths[itemLengths.Length - 1] = (int) (input.Length - itemOffsets[itemOffsets.Count - 1]);

                input.Seek(0, SeekOrigin.Begin);
                for (var i = 0; i < itemOffsets.Count; i++)
                {
                    var item = ReadItem(input, itemOffsets[i], itemLengths[i]);

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
                var tableEnd = input.PeekUInt16();
                long outputOffset = tableEnd;

                var itemOffsets = new List<ushort>(tableEnd / 2);

                for (var i = 0; i < tableEnd / 2; i++)
                {
                    var offset = input.ReadUInt16();
                    itemOffsets.Add(offset);
                }

                itemOffsets.Sort();

                var dictionary = new Dictionary<ushort, int>();

                for (var i = 0; i < itemOffsets.Count - 1; i++)
                {
                    dictionary.Add(itemOffsets[i], itemOffsets[i + 1] - itemOffsets[i]);
                }

                dictionary.Add(itemOffsets[itemOffsets.Count - 1], (int)(input.Length - itemOffsets[itemOffsets.Count - 1]));

                input.Seek(0, SeekOrigin.Begin);
                for (var i = 0; i < itemOffsets.Count; i++)
                {
                    var offset = input.ReadUInt16();
                    var size = dictionary[offset];

                    var item = ReadItem(input, offset, size);
                    outputOffset = WriteItem(output, subtitles, item, outputOffset);
                }
            }
        }

        private ShopElement ReadItem(ExtendedBinaryReader input, int offset, int itemLength)
        {
            var returnPos = input.Position;

            input.Seek(offset, SeekOrigin.Begin);
            var id = input.ReadUInt16();
            var unknown = input.ReadBytes(0x0C);
            var unknown2Offset = input.ReadUInt16();
            var title = ReadSubtitle(input);

            var unknown2 = input.ReadBytes(itemLength - (unknown2Offset - offset));

            var item = new ShopElement { Id = id, Unknown1 = unknown, Title = title, Unknown2 = unknown2 };

            input.Seek(returnPos, SeekOrigin.Begin);
            return item;
        }

        protected override Subtitle ReadSubtitle(ExtendedBinaryReader input)
        {
            var offset = input.ReadUInt16();
            var subtitle = ReadSubtitle(input, offset, false);

            return subtitle;
        }

        private long WriteItem(ExtendedBinaryWriter output, IList<Subtitle> subtitles, ShopElement item, long outputOffset)
        {
            output.Write((ushort)outputOffset);
            var returnPos = output.Position;

            output.Seek(outputOffset, SeekOrigin.Begin);
            output.Write(item.Id);
            output.Write(item.Unknown1);
            output.Write((ushort) 0x0000);

            var subtitle = subtitles.First(x => x.Offset == item.Title.Offset);

            var currentOffset = outputOffset + 2 + 0x0C + 4;
            currentOffset = WriteSubtitle(output, subtitle, currentOffset, true);
            output.Seek(outputOffset + 2 + 0x0C, SeekOrigin.Begin);
            output.Write((ushort)currentOffset);
            output.Seek(currentOffset, SeekOrigin.Begin);
            output.Write(item.Unknown2);
            currentOffset = output.Position;
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
