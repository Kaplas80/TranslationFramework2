using System.Collections.Generic;
using System.IO;
using System.Linq;
using TF.IO;
using YakuzaCommon.Files.SimpleSubtitle;

namespace YakuzaCommon.Files.PresentSendThrow
{
    internal class File : SimpleSubtitle.File
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
                input.Seek(4, SeekOrigin.Current);
                var limit = input.ReadInt32();
                input.Seek(4, SeekOrigin.Current);

                while (input.Position < limit)
                {
                    var offset = input.ReadInt32();
                    var returnPos = input.Position;
                    result.Add(ReadSubtitle(input, offset));
                    input.Seek(returnPos, SeekOrigin.Begin);
                }
            }

            return result;
        }

        private Subtitle ReadSubtitle(ExtendedBinaryReader input, int offset)
        {
            var result = new Subtitle {Offset = offset};
            if (offset > 0)
            {
                input.Seek(offset, SeekOrigin.Begin);
                result.Text = input.ReadString();
                result.Loaded = result.Text;
                result.Translation = result.Text;
                result.PropertyChanged += SubtitlePropertyChanged;
            }

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
                output.Write(input.ReadInt32());
                var limit = input.ReadInt32();
                output.Write(limit);
                output.Write(input.ReadInt32());

                var outputOffset = input.PeekInt32();
                var firstStringOffset = outputOffset;

                while (input.Position < limit)
                {
                    var offset = input.ReadInt32();
                    outputOffset = WriteString(output, subtitles, offset, outputOffset);
                }
                
                output.Write(input.ReadBytes(firstStringOffset - limit));
            }
        }

        private int WriteString(ExtendedBinaryWriter output, IList<Subtitle> subtitles, int inputOffset, int outputOffset)
        {
            var result = outputOffset;

            if (inputOffset == 0)
            {
                output.Write(0);
            }
            else
            {
                var str = subtitles.First(x => x.Offset == inputOffset);
                output.Write(outputOffset);
                var retPos = output.Position;
                output.Seek(outputOffset, SeekOrigin.Begin);
                output.WriteString(str.Translation);
                result = (int)output.Position;
                output.Seek(retPos, SeekOrigin.Begin);
            }

            return result;
        }
    }
}
