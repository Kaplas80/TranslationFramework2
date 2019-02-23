using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TF.IO;
using YakuzaCommon.Files.SimpleSubtitle;

namespace YakuzaCommon.Files.Mail
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

            var used = new Dictionary<int, bool>();
            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding, Endianness.BigEndian))
            {
                input.Skip(4);
                var count = input.ReadInt32();
                input.Skip(8);

                for (var i = 0; i < count; i++)
                {
                    var offset = input.ReadInt32();
                    if (!used.ContainsKey(offset))
                    {
                        result.Add(ReadSubtitle(input, offset));
                        used.Add(offset, true);
                    }

                    offset = input.ReadInt32();
                    if (!used.ContainsKey(offset))
                    {
                        result.Add(ReadSubtitle(input, offset));
                        used.Add(offset, true);
                    }

                    var pointer = input.ReadInt32();
                    var numLines = input.ReadInt32();

                    offset = input.ReadInt32();
                    if (!used.ContainsKey(offset))
                    {
                        result.Add(ReadSubtitle(input, offset));
                        used.Add(offset, true);
                    }

                    offset = input.ReadInt32();
                    if (!used.ContainsKey(offset))
                    {
                        result.Add(ReadSubtitle(input, offset));
                        used.Add(offset, true);
                    }

                    input.Skip(4);

                    offset = input.ReadInt32();
                    if (!used.ContainsKey(offset))
                    {
                        result.Add(ReadSubtitle(input, offset));
                        used.Add(offset, true);
                    }

                    var pos = input.Position;

                    input.Seek(pointer, SeekOrigin.Begin);

                    for (var j = 0; j < numLines; j++)
                    {
                        offset = input.ReadInt32();
                        if (!used.ContainsKey(offset))
                        {
                            result.Add(ReadSubtitle(input, offset));
                            used.Add(offset, true);
                        }
                    }

                    input.Seek(pos + 36, SeekOrigin.Begin);
                }
            }

            return result;
        }

        private Subtitle ReadSubtitle(ExtendedBinaryReader input, int offset)
        {
            var result = new Subtitle { Offset = offset };
            if (offset > 0)
            {
                var pos = input.Position;
                input.Seek(offset, SeekOrigin.Begin);
                result.Text = input.ReadString();
                result.Loaded = result.Text;
                result.Translation = result.Text;
                result.PropertyChanged += SubtitlePropertyChanged;
                input.Seek(pos, SeekOrigin.Begin);
            }

            return result;
        }

        public override void Rebuild(string outputFolder)
        {
            var outputPath = System.IO.Path.Combine(outputFolder, RelativePath);
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath));

            var subtitles = GetSubtitles();

            var used = new Dictionary<int, int>();

            using (var fsInput = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fsInput, FileEncoding, Endianness.BigEndian))
            using (var fsOutput = new FileStream(outputPath, FileMode.Create))
            using (var output = new ExtendedBinaryWriter(fsOutput, FileEncoding, Endianness.BigEndian))
            {
                output.Write(input.ReadBytes(4));
                var count = input.ReadInt32();
                output.Write(count);
                output.Write(input.ReadBytes(8));

                var outputOffset = input.PeekInt32();

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

        private int WriteString(ExtendedBinaryWriter output, IList<Subtitle> subtitles, Dictionary<int, int> used, int inputOffset, int outputOffset)
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
                    var str = subtitles.First(x => x.Offset == inputOffset);
                    result = WriteString(output, str.Translation, outputOffset);
                }
                else
                {
                    output.Write(used[inputOffset]);
                }
            }

            return result;
        }

        private int WriteString(ExtendedBinaryWriter output, string text, int outputOffset)
        {
            output.Write(outputOffset);
            var retPos = output.Position;
            output.Seek(outputOffset, SeekOrigin.Begin);
            output.WriteString(text);

            var result = (int)output.Position;
            output.Seek(retPos, SeekOrigin.Begin);

            return result;
        }
    }
}
