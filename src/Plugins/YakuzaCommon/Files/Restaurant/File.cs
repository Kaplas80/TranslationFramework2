using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TF.IO;
using YakuzaCommon.Files.SimpleSubtitle;

namespace YakuzaCommon.Files.Restaurant
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
                input.Skip(6);
                var groupCount = input.ReadInt16();
                input.Skip(8);

                for (var i = 0; i < 43; i++)
                {
                    var offset = input.ReadInt32();
                    var returnPos = input.Position;
                    result.Add(ReadSubtitle(input, offset));
                    input.Seek(returnPos, SeekOrigin.Begin);
                }
                
                input.Seek(0x110, SeekOrigin.Begin);
                for (var i = 0; i < groupCount; i++)
                {
                    var offsets = new int[12];
                    for (var j = 0; j < 12; j++)
                    {
                        offsets[j] = input.ReadInt32();
                    }

                    var returnPos = input.Position;

                    result.Add(ReadSubtitle(input, offsets[8]));

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
            using (var input = new ExtendedBinaryReader(fsInput, FileEncoding, Endianness.BigEndian))
            using (var fsOutput = new FileStream(outputPath, FileMode.Create))
            using (var output = new ExtendedBinaryWriter(fsOutput, FileEncoding, Endianness.BigEndian))
            {
                output.Write(input.ReadBytes(6));
                var groupCount = input.ReadInt16();
                output.Write(groupCount);
                output.Write(input.ReadBytes(8));

                var outputOffset = input.PeekInt32();
                var firstStringOffset = outputOffset;

                for (var i = 0; i < 43; i++)
                {
                    var offset = input.ReadInt32();
                    outputOffset = WriteString(output, subtitles, offset, outputOffset);
                }

                output.Write(input.ReadBytes(0x110 - (int)input.Position));
                for (var i = 0; i < groupCount; i++)
                {
                    var offsets = new int[12];
                    for (var j = 0; j < 12; j++)
                    {
                        offsets[j] = input.ReadInt32();
                    }

                    for (var j = 0; j < 8; j++)
                    {
                        output.Write(offsets[j]);
                    }

                    outputOffset = WriteString(output, subtitles, offsets[8], outputOffset);

                    for (var j = 9; j < 12; j++)
                    {
                        output.Write(offsets[j]);
                    }
                }

                output.Write(input.ReadBytes(firstStringOffset - (int)input.Position));
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
