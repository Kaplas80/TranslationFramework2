using System.Collections.Generic;
using System.IO;
using TF.Core.Files;
using TF.Core.TranslationEntities;
using TF.IO;

namespace TFGame.Yakuza0.Files.Snitch
{
    public class File : BinaryTextFileWithOffsetTable
    {
        public File(string gameName, string path, string changesFolder, System.Text.Encoding encoding) : base(gameName, path, changesFolder, encoding)
        {
        }

        protected override IList<Subtitle> GetSubtitles()
        {
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

                    var subtitle = ReadSubtitle(input);
                    subtitle.PropertyChanged += SubtitlePropertyChanged;
                    result.Add(subtitle);

                    input.Skip(0x4);
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
                output.Write(input.ReadBytes(0x08));

                var count1 = input.ReadInt32();
                var start1 = input.ReadInt32();
                var count2 = input.ReadInt32();
                var start2 = input.ReadInt32();

                output.Write(count1);
                output.Write(start1);
                output.Write(count2);
                output.Write(start2);

                long outputOffset = 0x18 + 0x10 * (count1 + count2);

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

                    var inputSubtitle = ReadSubtitle(input);
                    outputOffset = WriteSubtitle(output, subtitles, inputSubtitle.Offset, outputOffset);

                    var inputOffset = input.ReadInt32();
                    output.Write((int)outputOffset);
                    
                    input.Seek(inputOffset, SeekOrigin.Begin);
                    output.Seek(outputOffset, SeekOrigin.Begin);

                    output.Write(input.ReadBytes(inputUnknownSizes[i]));
                    outputOffset += inputUnknownSizes[i];
                }
            }
        }
    }
}
