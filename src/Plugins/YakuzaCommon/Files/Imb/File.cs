using System.Collections.Generic;
using System.IO;
using System.Text;
using TF.Core.Files;
using TF.Core.TranslationEntities;
using TF.IO;

namespace YakuzaCommon.Files.Imb
{
    public class File : BinaryTextFileWithOffsetTable
    {
        public File(string path, string changesFolder, Encoding encoding) : base(path, changesFolder, encoding)
        {
        }

        protected override IList<Subtitle> GetSubtitles()
        {
            var result = new List<Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding, Endianness.BigEndian))
            {
                input.Skip(32);

                var subtitle = ReadSubtitle(input);
                subtitle.PropertyChanged += SubtitlePropertyChanged;
                result.Add(subtitle);

                subtitle = ReadSubtitle(input);
                subtitle.PropertyChanged += SubtitlePropertyChanged;
                result.Add(subtitle);
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
            using (var input = new ExtendedBinaryReader(fsInput, FileEncoding, Endianness.BigEndian))
            using (var fsOutput = new FileStream(outputPath, FileMode.Create))
            using (var output = new ExtendedBinaryWriter(fsOutput, FileEncoding, Endianness.BigEndian))
            {
                output.Write(input.ReadBytes(32));

                var firstString = input.PeekInt32();
                long outputOffset = firstString;

                var subtitle = ReadSubtitle(input);
                outputOffset = WriteSubtitle(output, subtitles, subtitle.Offset, outputOffset);

                subtitle = ReadSubtitle(input);
                outputOffset = WriteSubtitle(output, subtitles, subtitle.Offset, outputOffset);

                output.Write(input.ReadBytes(24));

                var dds = ReadSubtitle(input);
                WriteString(output, dds, outputOffset);

                output.Write(input.ReadBytes(firstString - (int)input.Position));

                output.WritePadding(16);
            }
        }

        private void WriteString(ExtendedBinaryWriter output, Subtitle dds, long offset)
        {
            var pos = output.Position;

            if (dds == null || dds.Offset == 0)
            {
                output.Write(0);
            }
            else
            {
                output.Write((int)offset);
                base.WriteSubtitle(output, dds, offset, false);
            }

            output.Seek(pos + 4, SeekOrigin.Begin);
        }
    }
}
