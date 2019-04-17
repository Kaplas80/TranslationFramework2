using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TF.Core.Files;
using TF.Core.TranslationEntities;
using TF.Core.Views;
using TF.IO;
using WeifenLuo.WinFormsUI.Docking;

namespace TFGame.TheMissing.Files.Txt
{
    public class File : BinaryTextFile
    {
        public File(string path, string changesFolder, Encoding encoding) : base(path, changesFolder, encoding)
        {
        }

        public override void Open(DockPanel panel, ThemeBase theme)
        {
            _view = new GridView(theme);

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
                input.Seek(0x10, SeekOrigin.Begin);
                var stringOffsetTable = input.ReadInt32();
                var stringBase = input.ReadInt32();

                input.Seek(0x30, SeekOrigin.Begin);
                var stringCount = input.ReadInt32();

                for (var i = 0; i < stringCount; i++)
                {
                    input.Seek(stringOffsetTable + 4 * i, SeekOrigin.Begin);
                    var offset = input.ReadInt32();
                    input.Seek(stringBase + offset, SeekOrigin.Begin);
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
                input.Seek(0x0C, SeekOrigin.Begin);
                var lengthTableOffset = input.ReadInt32();
                var stringOffsetTable = input.ReadInt32();
                var stringBase = input.ReadInt32();
                var stringOffsetTable2 = input.ReadInt32();
                var stringBase2 = input.ReadInt32();

                input.Seek(0x2C, SeekOrigin.Begin);
                var lengthCount = input.ReadInt32();
                var stringCount = input.ReadInt32();
                var stringCount2 = input.ReadInt32();

                input.Seek(0, SeekOrigin.Begin);
                output.Write(input.ReadBytes(stringOffsetTable));

                long outputOffset = stringBase;
                var lengths = new Dictionary<int, short>();

                for (var i = 0; i < stringCount; i++)
                {
                    input.Seek(stringOffsetTable + 4 * i, SeekOrigin.Begin);
                    var offset = input.ReadInt32();

                    output.Seek(stringOffsetTable + 4 * i, SeekOrigin.Begin);
                    output.Write((int)(outputOffset - stringBase));

                    output.Seek(outputOffset, SeekOrigin.Begin);
                    var newOutputOffset = WriteSubtitle(output, subtitles, stringBase + offset, outputOffset);
                    var length = newOutputOffset - outputOffset - 1;
                    lengths.Add(i, (short)length);
                    outputOffset = newOutputOffset;
                }

                output.WritePadding(0x10);

                var newStringBase2 = output.Position;

                input.Seek(stringOffsetTable2, SeekOrigin.Begin);
                output.Seek(stringOffsetTable2, SeekOrigin.Begin);
                output.Write(input.ReadBytes(stringCount2 * 4));

                input.Seek(stringBase2, SeekOrigin.Begin);
                output.Seek(newStringBase2, SeekOrigin.Begin);
                output.Write(input.ReadBytes((int) (input.Length - stringBase2)));

                output.Seek(0x1C, SeekOrigin.Begin);
                output.Write((int)newStringBase2);

                for (var i = 0; i < lengthCount; i++)
                {
                    input.Seek(lengthTableOffset + i * 16, SeekOrigin.Begin);
                    var id = input.ReadInt32();
                    output.Seek(lengthTableOffset + i * 16 + 4, SeekOrigin.Begin);
                    output.Write(lengths[id]);
                }
            }
        }
    }
}
