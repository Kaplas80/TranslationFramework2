using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TF.Core.Files;
using TF.Core.TranslationEntities;
using TF.IO;

namespace YakuzaGame.Files.Mail
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
                var count = input.ReadInt32();
                input.Skip(8);

                for (var i = 0; i < count; i++)
                {
                    var subtitle = ReadSubtitle(input);
                    if (result.All(x => x.Offset != subtitle.Offset))
                    {
                        subtitle.PropertyChanged += SubtitlePropertyChanged;
                        result.Add(subtitle);
                    }

                    subtitle = ReadSubtitle(input);
                    if (result.All(x => x.Offset != subtitle.Offset))
                    {
                        subtitle.PropertyChanged += SubtitlePropertyChanged;
                        result.Add(subtitle);
                    }

                    var pointer = input.ReadInt32();
                    var numLines = input.ReadInt32();

                    subtitle = ReadSubtitle(input);
                    if (result.All(x => x.Offset != subtitle.Offset))
                    {
                        subtitle.PropertyChanged += SubtitlePropertyChanged;
                        result.Add(subtitle);
                    }

                    subtitle = ReadSubtitle(input);
                    if (result.All(x => x.Offset != subtitle.Offset))
                    {
                        subtitle.PropertyChanged += SubtitlePropertyChanged;
                        result.Add(subtitle);
                    }

                    input.Skip(4);

                    subtitle = ReadSubtitle(input);
                    if (result.All(x => x.Offset != subtitle.Offset))
                    {
                        subtitle.PropertyChanged += SubtitlePropertyChanged;
                        result.Add(subtitle);
                    }

                    var pos = input.Position;

                    input.Seek(pointer, SeekOrigin.Begin);

                    for (var j = 0; j < numLines; j++)
                    {
                        subtitle = ReadSubtitle(input);
                        if (result.All(x => x.Offset != subtitle.Offset))
                        {
                            subtitle.PropertyChanged += SubtitlePropertyChanged;
                            result.Add(subtitle);
                        }
                    }

                    input.Seek(pos + 36, SeekOrigin.Begin);
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

            var used = new Dictionary<long, long>();

            using (var fsInput = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fsInput, FileEncoding, Endianness.BigEndian))
            using (var fsOutput = new FileStream(outputPath, FileMode.Create))
            using (var output = new ExtendedBinaryWriter(fsOutput, FileEncoding, Endianness.BigEndian))
            {
                output.Write(input.ReadBytes(4));
                var count = input.ReadInt32();
                output.Write(count);
                output.Write(input.ReadBytes(8));

                long outputOffset = input.PeekInt32();

                for (var i = 0; i < count; i++)
                {
                    var offset = input.ReadInt32();
                    outputOffset = WriteString(output, subtitles, used, offset, outputOffset);

                    offset = input.ReadInt32();
                    outputOffset = WriteString(output, subtitles, used, offset, outputOffset);

                    var pointer = input.ReadInt32();
                    output.Write(pointer);
                    var numLines = input.ReadInt32();
                    output.Write(numLines);

                    offset = input.ReadInt32();
                    outputOffset = WriteString(output, subtitles, used, offset, outputOffset);

                    offset = input.ReadInt32();
                    outputOffset = WriteString(output, subtitles, used, offset, outputOffset);

                    output.Write(input.ReadBytes(4));

                    offset = input.ReadInt32();
                    outputOffset = WriteString(output, subtitles, used, offset, outputOffset);

                    var pos = input.Position;

                    input.Seek(pointer, SeekOrigin.Begin);
                    output.Seek(pointer, SeekOrigin.Begin);

                    for (var j = 0; j < numLines; j++)
                    {
                        offset = input.ReadInt32();
                        outputOffset = WriteString(output, subtitles, used, offset, outputOffset);
                    }

                    input.Seek(pos, SeekOrigin.Begin);
                    output.Seek(pos, SeekOrigin.Begin);
                    
                    output.Write(input.ReadBytes(36));
                }
            }
        }

        private long WriteString(ExtendedBinaryWriter output, IList<Subtitle> subtitles, IDictionary<long, long> used, long inputOffset, long outputOffset)
        {
            var result = outputOffset;

            if (inputOffset == 0)
            {
                output.Write(0);
            }
            else
            {
                if (!used.ContainsKey(inputOffset))
                {
                    used.Add(inputOffset, outputOffset);
                    var sub = subtitles.First(x => x.Offset == inputOffset);
                    result = WriteSubtitle(output, sub, outputOffset, true);
                }
                else
                {
                    output.Write((int)used[inputOffset]);
                }
            }

            return result;
        }
    }
}
