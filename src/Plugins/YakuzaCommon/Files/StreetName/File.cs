using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TF.IO;
using YakuzaCommon.Files.SimpleSubtitle;

namespace YakuzaCommon.Files.StreetName
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
            using (var input = new ExtendedBinaryReader(fs, FileEncoding, Endianness.LittleEndian))
            {
                while (input.Position < input.Length)
                {
                    var nextOffset = input.ReadInt16();
                    var sectionName = input.ReadString(Encoding.UTF8);

                    var aux = input.PeekInt16();

                    while (aux != -1)
                    {
                        input.Skip(2);
                        result.Add(ReadSubtitle(input));
                        aux = input.PeekInt16();
                    }

                    input.Skip(2);
                }
            }

            return result;
        }

        private Subtitle ReadSubtitle(ExtendedBinaryReader input)
        {
            var offset = (int) input.Position;

            var result = new Subtitle {Offset = offset};
            result.Text = input.ReadString();
            result.Loaded = result.Text;
            result.Translation = result.Text;
            result.PropertyChanged += SubtitlePropertyChanged;

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
                            WriteString(temp, subtitles, offset);

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

        private void WriteString(ExtendedBinaryWriter output, IList<Subtitle> subtitles, int inputOffset)
        {
            var str = subtitles.First(x => x.Offset == inputOffset);
            output.Write((byte)FileEncoding.GetByteCount(str.Translation));
            output.WriteString(str.Translation);
        }
    }
}
