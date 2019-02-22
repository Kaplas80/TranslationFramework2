using System.Collections.Generic;
using System.IO;
using System.Linq;
using TF.IO;
using YakuzaCommon.Files.SimpleSubtitle;

namespace YakuzaCommon.Files.Bar
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
                input.Seek(6, SeekOrigin.Begin);
                var groupCount = input.ReadInt16();
                input.Seek(8, SeekOrigin.Current);

                var offset = 0;

                for (var i = 0; i < 43; i++)
                {
                    offset = input.ReadInt32();
                    var returnPos = input.Position;
                    result.Add(ReadSubtitle(input, offset));
                    input.Seek(returnPos, SeekOrigin.Begin);
                }
                
                // barkeeper
                input.Seek(0x108, SeekOrigin.Begin);
                offset = input.ReadInt32();
                var barkeeper = ReadSubtitle(input, offset);
                result.Add(barkeeper);

                input.Seek(0x110, SeekOrigin.Begin);
                for (var i = 0; i < groupCount; i++)
                {
                    var offsets = new int[22];
                    for (var j = 0; j < 22; j++)
                    {
                        offsets[j] = input.ReadInt32();
                    }

                    var returnPos = input.Position;

                    result.Add(ReadSubtitle(input, offsets[8]));
                    result.Add(ReadSubtitle(input, offsets[14]));
                    result.Add(ReadSubtitle(input, offsets[15]));
                    result.Add(ReadSubtitle(input, offsets[16]));
                    result.Add(ReadSubtitle(input, offsets[17]));
                    result.Add(ReadSubtitle(input, offsets[18]));
                    result.Add(ReadSubtitle(input, offsets[19]));
                    result.Add(ReadSubtitle(input, offsets[20]));
                    result.Add(ReadSubtitle(input, offsets[21]));

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

                output.Write(input.ReadBytes(0x108 - (int)input.Position));

                // barkeeper
                var barKeeperOffset = input.ReadInt32();
                outputOffset = WriteString(output, subtitles, barKeeperOffset, outputOffset);

                output.Write(input.ReadBytes(0x110 - (int)input.Position));
                for (var i = 0; i < groupCount; i++)
                {
                    var offsets = new int[22];
                    for (var j = 0; j < 22; j++)
                    {
                        offsets[j] = input.ReadInt32();
                    }

                    for (var j = 0; j < 8; j++)
                    {
                        output.Write(offsets[j]);
                    }

                    outputOffset = WriteString(output, subtitles, offsets[8], outputOffset);

                    for (var j = 9; j < 14; j++)
                    {
                        output.Write(offsets[j]);
                    }

                    for (var j = 14; j < 22; j++)
                    {
                        outputOffset = WriteString(output, subtitles, offsets[j], outputOffset);
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
