using System.Collections.Generic;
using System.IO;
using System.Linq;
using TF.IO;
using YakuzaCommon.Files.SimpleSubtitle;

namespace YakuzaCommon.Files.Epmb
{
    public class File : SimpleSubtitle.File
    {
        public File(string path, string changesFolder) : base(path, changesFolder)
        {
        }

        protected override IList<Subtitle> GetSubtitles()
        {
            if (HasChanges)
            {
                return LoadChanges(ChangesFile);
            }

            var result = new List<Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, Encoding, Endianness.BigEndian))
            {
                input.Seek(16, SeekOrigin.Current);
                var count = input.ReadInt32();

                for (var i = 0; i < count; i++)
                {
                    input.ReadInt32();
                    var returnPos = input.Position;
                    result.Add(ReadSubtitle(input));
                    input.Seek(returnPos + 64, SeekOrigin.Begin);
                }
            }

            return result;
        }

        private Subtitle ReadSubtitle(ExtendedBinaryReader input)
        {
            var result = new Subtitle {Offset = input.Position, Text = input.ReadString(64)};
            result.Loaded = result.Text;
            result.Translation = result.Text;
            result.PropertyChanged += SubtitlePropertyChanged;

            return result;
        }

        public override void Rebuild(string outputFolder)
        {
            var outputPath = System.IO.Path.Combine(outputFolder, RelativePath);
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath));

            var subtitles = GetSubtitles();

            using (var fsInput = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fsInput, Encoding, Endianness.BigEndian))
            using (var fsOutput = new FileStream(outputPath, FileMode.Create))
            using (var output = new ExtendedBinaryWriter(fsOutput, Encoding, Endianness.BigEndian))
            {
                output.Write(input.ReadBytes(16));
                var count = input.ReadInt32();
                output.Write(count);

                for (var i = 0; i < count; i++)
                {
                    output.Write(input.ReadInt32());
                    var offset = (int)input.Position;
                    input.Seek(64, SeekOrigin.Current);
                    WriteString(output, subtitles, offset);
                }
            }
        }

        private void WriteString(ExtendedBinaryWriter output, IList<Subtitle> subtitles, int inputOffset)
        {
            var str = subtitles.First(x => x.Offset == inputOffset);
            var startPos = output.Position;
            var zeros = new byte[64];
            output.Write(zeros);

            output.Seek(-64, SeekOrigin.Current);
            output.WriteString(str.Translation);

            output.Seek(startPos + 64, SeekOrigin.Begin);
        }
    }
}
