using System.Collections.Generic;
using System.IO;
using System.Text;
using TF.Core.Files;
using TF.Core.TranslationEntities;
using TF.IO;

namespace YakuzaGame.Files.Mfpb
{
    public class File : BinaryTextFileWithOffsetTable
    {
        private readonly int MAX_SIZE = 0xC0;

        public File(string path, string changesFolder, Encoding encoding) : base(path, changesFolder, encoding)
        {
        }

        protected override IList<Subtitle> GetSubtitles()
        {
            var result = new List<Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding, Endianness.BigEndian))
            {
                input.Seek(0x20, SeekOrigin.Begin);

                var pointer = input.ReadInt32();
                if (pointer > -1)
                {
                    input.Seek(pointer, SeekOrigin.Begin);

                    var subtitle = ReadSubtitle(input);
                    subtitle.PropertyChanged += SubtitlePropertyChanged;
                    result.Add(subtitle);
                }

                input.Seek(0x24, SeekOrigin.Begin);

                pointer = input.ReadInt32();
                if (pointer > -1)
                {
                    input.Seek(pointer, SeekOrigin.Begin);

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
            System.IO.File.Copy(Path, outputPath, true);

            var subtitles = GetSubtitles();

            using (var fs = new FileStream(outputPath, FileMode.Open))
            using (var output = new ExtendedBinaryWriter(fs, FileEncoding, Endianness.BigEndian))
            {
                foreach (var subtitle in subtitles)
                {
                    if (subtitle.Offset > 0)
                    {
                        output.Seek(subtitle.Offset, SeekOrigin.Begin);

                        var zeros = new byte[MAX_SIZE];
                        output.Write(zeros);

                        output.Seek(subtitle.Offset, SeekOrigin.Begin);
                        output.WriteString(subtitle.Translation);
                    }
                }
            }
        }
    }
}
