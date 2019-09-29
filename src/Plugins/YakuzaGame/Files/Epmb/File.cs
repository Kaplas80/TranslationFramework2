using System.Collections.Generic;
using System.IO;
using System.Text;
using TF.Core.Files;
using TF.Core.TranslationEntities;
using TF.IO;

namespace YakuzaGame.Files.Epmb
{
    public class File : BinaryTextFile
    {
        public File(string path, string changesFolder, System.Text.Encoding encoding) : base(path, changesFolder, encoding)
        {
        }

        protected override IList<Subtitle> GetSubtitles()
        {
            var result = new List<Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding, Endianness.BigEndian))
            {
                input.Skip(16);
                var count = input.ReadInt32();

                for (var i = 0; i < count; i++)
                {
                    input.ReadInt32();

                    var subtitle = ReadSubtitle(input);
                    subtitle.PropertyChanged += SubtitlePropertyChanged;
                    result.Add(subtitle);

                    input.Seek(subtitle.Offset + 64, SeekOrigin.Begin);
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
                output.Write(input.ReadBytes(16));
                var count = input.ReadInt32();
                output.Write(count);

                for (var i = 0; i < count; i++)
                {
                    output.Write(input.ReadInt32());

                    var offset = (int)input.Position;
                    WriteSubtitle(output, subtitles, offset);

                    input.Seek(offset + 64, SeekOrigin.Begin);
                }
            }
        }

        private void WriteSubtitle(ExtendedBinaryWriter output, IList<Subtitle> subtitles, int offset)
        {
            output.Seek(offset, SeekOrigin.Begin);
            var zeros = new byte[64];
            output.Write(zeros);

            WriteSubtitle(output, subtitles, offset, offset);

            output.Seek(offset + 64, SeekOrigin.Begin);
        }
    }
}
