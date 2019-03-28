using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TF.Core.Files;
using TF.Core.TranslationEntities;
using TF.IO;

namespace YakuzaGame.Files.StreetName
{
    public class File : BinaryTextFile
    {
        public File(string path, string changesFolder, Encoding encoding) : base(path, changesFolder, encoding)
        {
        }

        protected override IList<Subtitle> GetSubtitles()
        {
            var result = new List<Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding))
            {
                while (input.Position < input.Length)
                {
                    var nextOffset = input.ReadInt16();
                    var sectionName = input.ReadString(Encoding.UTF8);

                    var aux = input.PeekInt16();

                    while (aux != -1)
                    {
                        input.Skip(2);

                        var subtitle = ReadSubtitle(input);
                        subtitle.PropertyChanged += SubtitlePropertyChanged;
                        result.Add(subtitle);

                        aux = input.PeekInt16();
                    }

                    input.Skip(2);
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
            using (var input = new ExtendedBinaryReader(fsInput, FileEncoding, Endianness.LittleEndian))
            using (var fsOutput = new FileStream(outputPath, FileMode.Create))
            using (var output = new ExtendedBinaryWriter(fsOutput, FileEncoding, Endianness.LittleEndian))
            {
                var outputOffset = 0;

                while (input.Position < input.Length)
                {
                    var inputNextOffset = input.ReadInt16();
                    var sectionName = input.ReadString(Encoding.UTF8);
                    
                    using (var tempMemoryStream = new MemoryStream())
                    using (var temp = new ExtendedBinaryWriter(tempMemoryStream, FileEncoding, Endianness.LittleEndian))
                    {
                        temp.WriteString(sectionName);

                        var aux = input.PeekInt16();
                        while (aux != -1)
                        {
                            var index = input.ReadByte();
                            temp.Write(index);

                            var size = input.ReadByte();

                            var offset = (int)input.Position;
                            WriteSubtitle(temp, subtitles, offset);

                            input.ReadString();
                            aux = input.PeekInt16();
                        }

                        input.Skip(2);
                        temp.Write((short)-1);

                        var bytes = tempMemoryStream.ToArray();
                        output.Write((short)(outputOffset + 2 + bytes.Length));
                        output.Write(bytes);

                        outputOffset = outputOffset + 2 + bytes.Length;
                    }
                }
            }
        }

        private void WriteSubtitle(ExtendedBinaryWriter output, IList<Subtitle> subtitles, long inputOffset)
        {
            var sub = subtitles.First(x => x.Offset == inputOffset);
            output.Write((byte)FileEncoding.GetByteCount(sub.Translation));
            output.WriteString(sub.Translation);
        }
    }
}
