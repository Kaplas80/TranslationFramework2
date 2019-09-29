using System.Collections.Generic;
using System.IO;
using TF.Core.Files;
using TF.Core.TranslationEntities;
using TF.IO;

namespace TFGame.YakuzaIshin.Files.Restaurant
{
    public class File : BinaryTextFileWithOffsetTable
    {

        public File(string path, string changesFolder, System.Text.Encoding encoding) : base(path, changesFolder, encoding)
        {
        }

        private int _groupSize => 8;

        protected override IList<Subtitle> GetSubtitles()
        {
            var result = new List<Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding, Endianness.BigEndian))
            {
                input.Skip(6);
                var groupCount = input.ReadInt16();
                input.Skip(4);
                var groupsOffset = input.ReadInt32();

                Subtitle subtitle;

                while (input.Position < groupsOffset)
                {
                    var subtitleOffset = input.ReadInt32();

                    if (subtitleOffset > 0 && subtitleOffset < input.Length)
                    {
                        input.Seek(-4, SeekOrigin.Current);
                        subtitle = ReadSubtitle(input);
                        subtitle.PropertyChanged += SubtitlePropertyChanged;
                        result.Add(subtitle);
                    }
                }

                input.Seek(groupsOffset, SeekOrigin.Begin);

                for (var i = 0; i < groupCount; i++)
                {
                    var offsets = new int[_groupSize];
                    for (var j = 0; j < _groupSize; j++)
                    {
                        offsets[j] = input.ReadInt32();
                    }

                    var returnPos = input.Position;

                    subtitle = ReadSubtitle(input, offsets[6], false);
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
                output.Write(input.ReadBytes(4));
                var groupsOffset = input.ReadInt32();
                output.Write(groupsOffset);

                long outputOffset = input.PeekInt32();
                var firstStringOffset = outputOffset;

                while (input.Position < groupsOffset)
                {
                    var offset = input.ReadInt32();
                    if (offset > 0 && offset < input.Length)
                    {
                        outputOffset = WriteSubtitle(output, subtitles, offset, outputOffset);
                    }
                    else
                    {
                        output.Write(offset);
                    }
                }

                output.Write(input.ReadBytes((int) (groupsOffset - (int)input.Position)));

                for (var i = 0; i < groupCount; i++)
                {
                    var offsets = new int[_groupSize];
                    for (var j = 0; j < _groupSize; j++)
                    {
                        offsets[j] = input.ReadInt32();
                    }

                    for (var j = 0; j < 6; j++)
                    {
                        output.Write(offsets[j]);
                    }

                    outputOffset = WriteSubtitle(output, subtitles, offsets[6], outputOffset);

                    output.Write(offsets[7]);
                }

                output.Write(input.ReadBytes((int)(firstStringOffset - input.Position)));
            }
        }
    }
}
