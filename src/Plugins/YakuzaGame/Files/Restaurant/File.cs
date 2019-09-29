using System.Collections.Generic;
using System.IO;
using System.Text;
using TF.Core.Files;
using TF.Core.TranslationEntities;
using TF.IO;

namespace YakuzaGame.Files.Restaurant
{
    public class File : BinaryTextFileWithOffsetTable
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
                input.Skip(6);
                var groupCount = input.ReadInt16();
                input.Skip(8);

                Subtitle subtitle;
                for (var i = 0; i < 43; i++)
                {
                    subtitle = ReadSubtitle(input);
                    subtitle.PropertyChanged += SubtitlePropertyChanged;
                    result.Add(subtitle);
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

                    subtitle = ReadSubtitle(input, offsets[8], false);
                    subtitle.PropertyChanged += SubtitlePropertyChanged;
                    result.Add(subtitle);

                    input.Seek(returnPos, SeekOrigin.Begin);
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
                output.Write(input.ReadBytes(6));
                var groupCount = input.ReadInt16();
                output.Write(groupCount);
                output.Write(input.ReadBytes(8));

                long outputOffset = input.PeekInt32();
                var firstStringOffset = outputOffset;

                for (var i = 0; i < 43; i++)
                {
                    var offset = input.ReadInt32();
                    outputOffset = WriteSubtitle(output, subtitles, offset, outputOffset);
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

                    outputOffset = WriteSubtitle(output, subtitles, offsets[8], outputOffset);

                    for (var j = 9; j < 12; j++)
                    {
                        output.Write(offsets[j]);
                    }
                }

                output.Write(input.ReadBytes((int)(firstStringOffset - input.Position)));
            }
        }
    }
}
