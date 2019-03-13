using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TF.Core.Files;
using TF.Core.TranslationEntities;
using TF.IO;

namespace YakuzaCommon.Files.PresentSendThrow
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
                input.Skip(4);
                var tableEnd = input.ReadInt32();
                input.Skip(4);

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
                output.Write(input.ReadInt32());
                var tableEnd = input.ReadInt32();
                output.Write(tableEnd);
                output.Write(input.ReadInt32());

                long outputOffset = input.PeekInt32();
                var firstStringOffset = outputOffset;

                while (input.Position < tableEnd)
                {
                    var offset = input.ReadInt32();
                    outputOffset = WriteSubtitle(output, subtitles, offset, outputOffset);
                }

                output.Write(input.ReadBytes((int)(firstStringOffset - tableEnd)));
            }
        }
    }
}
