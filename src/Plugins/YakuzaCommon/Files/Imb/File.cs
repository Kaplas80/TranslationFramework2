using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TF.IO;
using YakuzaCommon.Files.SimpleSubtitle;

namespace YakuzaCommon.Files.Imb
{
    public class File : SimpleSubtitle.File
    {
        public File(string path, string changesFolder, Encoding encoding) : base(path, changesFolder, encoding)
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
            using (var input = new ExtendedBinaryReader(fs, FileEncoding, Endianness.BigEndian))
            {
                input.Skip(32);
                var titlePointer = input.ReadInt32();
                var descriptionPointer = input.ReadInt32();

                if (titlePointer > 0)
                {
                    result.Add(ReadSubtitle(input, titlePointer));
                }

                if (descriptionPointer > 0)
                {
                    result.Add(ReadSubtitle(input, descriptionPointer));
                }
            }

            return result;
        }

        private Subtitle ReadSubtitle(ExtendedBinaryReader input, int offset)
        {
            var result = new Subtitle { Offset = offset };
            var pos = input.Position;
            input.Seek(offset, SeekOrigin.Begin);
            result.Text = input.ReadString();
            result.Loaded = result.Text;
            result.Translation = result.Text;
            result.PropertyChanged += SubtitlePropertyChanged;
            input.Seek(pos, SeekOrigin.Begin);

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
                var titlePointer = input.ReadInt32();
                var descriptionPointer = input.ReadInt32();

                var outputOffset = titlePointer;
                outputOffset = WriteString(output, subtitles, titlePointer, outputOffset);
                outputOffset = WriteString(output, subtitles, descriptionPointer, outputOffset);

                output.Write(input.ReadBytes(24));
                var ddsPointer = input.ReadInt32();
                var dds = ReadSubtitle(input, ddsPointer);
                outputOffset = WriteString(output, dds.Text, outputOffset);

                output.Write(input.ReadBytes(titlePointer - (int)input.Position));

                while (output.Position % 16 != 0)
                {
                    output.Write((byte)0);
                }
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
                result = WriteString(output, str.Translation, outputOffset);
            }

            return result;
        }

        private int WriteString(ExtendedBinaryWriter output, string text, int outputOffset)
        {
            output.Write(outputOffset);
            var retPos = output.Position;
            output.Seek(outputOffset, SeekOrigin.Begin);
            output.WriteString(text);

            while (output.Position % 2 != 0)
            {
                output.Write((byte)0);
            }

            var result = (int)output.Position;
            output.Seek(retPos, SeekOrigin.Begin);

            return result;
        }
    }
}
