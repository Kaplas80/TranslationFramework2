using System.Collections.Generic;
using System.IO;
using TF.Core.Files;
using TF.IO;

namespace YakuzaGame.Files.Scenario
{
    public class File : BinaryTextFile
    {
        public File(string gameName, string path, string changesFolder, System.Text.Encoding encoding) : base(gameName, path, changesFolder, encoding)
        {
        }

        protected override IList<TF.Core.TranslationEntities.Subtitle> GetSubtitles()
        {
            var result = new List<TF.Core.TranslationEntities.Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding, Endianness.BigEndian))
            {
                input.Seek(0x38, SeekOrigin.Begin);
                var offset = input.ReadInt32();

                input.Seek(offset, SeekOrigin.Begin);
                while (input.BaseStream.Position < input.BaseStream.Length)
                {
                    var subtitle = ReadSubtitle(input);
                    if (subtitle != null)
                    {
                        subtitle.PropertyChanged += SubtitlePropertyChanged;
                        result.Add(subtitle);
                    }
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
                input.Seek(0x38, SeekOrigin.Begin);
                var offset = input.ReadInt32();
                input.Seek(0, SeekOrigin.Begin);

                output.Write(input.ReadBytes(offset));

                long outputOffset = offset;
                while (input.BaseStream.Position < input.BaseStream.Length)
                {
                    var subtitle = ReadSubtitle(input);
                    outputOffset = WriteSubtitle(output, subtitles, subtitle.Offset, outputOffset);
                }
            }
        }
    }
}
