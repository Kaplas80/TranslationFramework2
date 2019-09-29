using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TF.Core.TranslationEntities;
using TF.IO;

namespace TF.Core.Files
{
    public class BinaryTextFileWithOffsetTable : BinaryTextFile
    {
        public BinaryTextFileWithOffsetTable(string path, string changesFolder, System.Text.Encoding encoding) : base(path, changesFolder, encoding)
        {
        }

        protected override IList<Subtitle> GetSubtitles()
        {
            var result = new List<Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding, Endianness.BigEndian))
            {
                input.Skip(StartOffset);

                var tableEnd = input.PeekInt32();
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
            using (var input = new ExtendedBinaryReader(fsInput, FileEncoding, Endianness.BigEndian))
            using (var fsOutput = new FileStream(outputPath, FileMode.Create))
            using (var output = new ExtendedBinaryWriter(fsOutput, FileEncoding, Endianness.BigEndian))
            {
                if (StartOffset > 0)
                {
                    output.Write(input.ReadBytes(StartOffset));
                }

                long outputOffset = StartOffset + subtitles.Count * 4;

                var tableEnd = input.PeekInt32();

                while (input.Position < tableEnd)
                {
                    var inputSubtitle = ReadSubtitle(input);
                    outputOffset = WriteSubtitle(output, subtitles, inputSubtitle.Offset, outputOffset);
                }
            }
        }

        protected override Subtitle ReadSubtitle(ExtendedBinaryReader input)
        {
            var offset = input.ReadInt32();
            var subtitle = ReadSubtitle(input, offset, true);

            return subtitle;
        }

        protected override Subtitle ReadSubtitle(ExtendedBinaryReader input, long offset, bool returnToPos)
        {
            if (offset > 0)
            {
                var subtitle = base.ReadSubtitle(input, offset, returnToPos);
                return subtitle;
            }
            else
            {
                var subtitle = new Subtitle
                {
                    Offset = 0,
                };
                return subtitle;
            }
        }

        protected override long WriteSubtitle(ExtendedBinaryWriter output, IList<Subtitle> subtitles, long inputOffset, long outputOffset)
        {
            long result;

            if (inputOffset == 0)
            {
                result = WriteSubtitle(output, null, outputOffset, true);
            }
            else
            {
                var subtitle = subtitles.First(x => x.Offset == inputOffset);
                result = WriteSubtitle(output, subtitle, outputOffset, true);
            }

            return result;
        }

        protected override long WriteSubtitle(ExtendedBinaryWriter output, Subtitle subtitle, long offset, bool returnToPos)
        {
            var result = offset;
            var pos = output.Position;

            if (subtitle == null || offset == 0)
            {
                output.Write(0);
            }
            else
            {
                output.Write((int)offset);
                result = base.WriteSubtitle(output, subtitle, offset, false);
            }

            if (returnToPos)
            {
                output.Seek(pos + 4, SeekOrigin.Begin); // Le añado 4 para que vuelva después de escribir el offset
            }

            return result;
        }
    }
}
