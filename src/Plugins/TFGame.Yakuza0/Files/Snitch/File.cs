using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TF.Core.Exceptions;
using TF.IO;
using YakuzaCommon.Files.SimpleSubtitle;

namespace TFGame.Yakuza0.Files.Snitch
{
    public class File : YakuzaCommon.Files.SimpleSubtitle.File
    {
        public File(string path, string changesFolder, System.Text.Encoding encoding) : base(path, changesFolder, encoding)
        {
        }

        protected override IList<Subtitle> GetSubtitles()
        {
            if (HasChanges)
            {
                try
                {
                    var loadedSubs = LoadChanges(ChangesFile);
                    return loadedSubs;
                }
                catch (ChangesFileVersionMismatchException e)
                {
                    System.IO.File.Delete(ChangesFile);
                }
            }

            var result = new List<Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding, Endianness.BigEndian))
            {
                input.Skip(0x08);
                var count1 = input.ReadInt32();
                var start1 = input.ReadInt32();
                var count2 = input.ReadInt32();
                var start2 = input.ReadInt32();

                for (var i = 0; i < count1 + count2; i++)
                {
                    input.Skip(0x8);
                    var offset = input.ReadInt32();
                    if (offset > 0)
                    {
                        result.Add(ReadSubtitle(input, offset));
                    }
                    input.Skip(0x4);
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
                output.Write(input.ReadBytes(0x08));

                var count1 = input.ReadInt32();
                var start1 = input.ReadInt32();
                var count2 = input.ReadInt32();
                var start2 = input.ReadInt32();

                output.Write(count1);
                output.Write(start1);
                output.Write(count2);
                output.Write(start2);

                var outputOffset = 0x18 + 0x10 * (count1 + count2);

                var inputStringOffsets = new int[count1 + count2];
                var inputUnknownOffsets = new int[count1 + count2];
                var inputUnknownSizes = new int[count1 + count2];
                for (var i = 0; i < count1 + count2; i++)
                {
                    input.Seek(0x18 + 0x10 * i, SeekOrigin.Begin);
                    input.Skip(8);
                    inputStringOffsets[i] = input.ReadInt32();
                    inputUnknownOffsets[i] = input.ReadInt32();
                }

                for (var i = 0; i < count1 + count2 - 1; i++)
                {
                    inputUnknownSizes[i] = (inputStringOffsets[i + 1] - inputUnknownOffsets[i]);
                }

                inputUnknownSizes[count1 + count2 - 1] = (int) (input.Length - inputUnknownOffsets[count1 + count2 - 1]);

                for (var i = 0; i < count1 + count2; i++)
                {
                    input.Seek(0x18 + 0x10 * i, SeekOrigin.Begin);
                    output.Seek(0x18 + 0x10 * i, SeekOrigin.Begin);

                    output.Write(input.ReadBytes(8));

                    var inputOffset = input.ReadInt32();
                    outputOffset = WriteString(output, subtitles, inputOffset, outputOffset);
                    inputOffset = input.ReadInt32();
                    output.Write(outputOffset);
                    
                    input.Seek(inputOffset, SeekOrigin.Begin);
                    output.Seek(outputOffset, SeekOrigin.Begin);
                    output.Write(input.ReadBytes(inputUnknownSizes[i]));
                    outputOffset += inputUnknownSizes[i];
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
