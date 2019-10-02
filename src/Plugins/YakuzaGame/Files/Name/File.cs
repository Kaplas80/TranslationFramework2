using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TF.Core.Files;
using TF.Core.TranslationEntities;
using TF.IO;

namespace YakuzaGame.Files.Name
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
                input.Skip(0x38);
                var pointer = input.ReadInt32();
                var count = input.ReadInt32();
                
                input.Seek(pointer, SeekOrigin.Begin);
                for (var i = 0; i < count; i++)
                {
                    var subtitle = ReadSubtitle(input);
                    subtitle.PropertyChanged += SubtitlePropertyChanged;
                    result.Add(subtitle);
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
            using (var tempMemoryStream = new MemoryStream())
            {
                var offsets = new List<int>();

                input.Skip(0x24);
                var firstStringOffset = input.ReadInt32() + 4;
                input.Skip(0x4);
                var inputLastPointer = input.ReadInt32();

                input.Seek(0, SeekOrigin.Begin);

                output.Write(input.ReadBytes(firstStringOffset)); // Copia de la cabecera

                input.Seek(0x38, SeekOrigin.Begin);
                var pointer = input.ReadInt32();
                var count = input.ReadInt32();

                input.Seek(pointer, SeekOrigin.Begin);
                using (var temp = new ExtendedBinaryWriter(tempMemoryStream, FileEncoding, Endianness.BigEndian))
                {
                    for (var i = 0; i < count; i++)
                    {
                        var offset = input.ReadInt32();
                        if (offset == 0)
                        {
                            offsets.Add(0);
                        }
                        else
                        {
                            var outputOffset = WriteString(temp, subtitles, offset);
                            offsets.Add(outputOffset);
                        }
                    }
                }

                var strings = tempMemoryStream.ToArray();

                output.Write(strings);

                output.WritePadding(16);

                var outputOffsetsPointer = output.Position;

                foreach (var offset in offsets)
                {
                    output.Write(offset + firstStringOffset);
                }

                var outputRemainderPointer = output.Position;

                output.Write(input.ReadBytes((int)(inputLastPointer - input.Position)));
                var outputLastPointer = output.Position;
                output.Write((int)(outputRemainderPointer + 8));

                output.WritePadding(16);

                var outputTotalLength = output.Position;

                output.Seek(0x0C, SeekOrigin.Begin);
                output.Write((int)outputTotalLength);

                output.Seek(0x28, SeekOrigin.Begin);
                output.Write((int)outputRemainderPointer);
                output.Write((int)outputLastPointer);

                output.Seek(0x34, SeekOrigin.Begin);
                output.Write((int)(outputRemainderPointer + 4));
                output.Write((int)outputOffsetsPointer);
            }
        }

        private int WriteString(ExtendedBinaryWriter output, IList<Subtitle> subtitles, int inputOffset)
        {
            var result = output.Position;

            var str = subtitles.First(x => x.Offset == inputOffset);
            output.WriteString(str.Translation);

            return (int)result;
        }
    }
}
