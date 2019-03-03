using System.Collections.Generic;
using System.IO;
using System.Text;
using TF.Core.Exceptions;
using TF.IO;
using YakuzaCommon.Files.SimpleSubtitle;

namespace YakuzaCommon.Files.Mfpb
{
    public class File : SimpleSubtitle.File
    {
        private readonly int MAX_SIZE = 0xC0;

        public File(string path, string changesFolder, Encoding encoding) : base(path, changesFolder, encoding)
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
                input.Skip(0x24);
                var pointer = input.ReadInt32();
                if (pointer > -1)
                {
                    input.Seek(pointer, SeekOrigin.Begin);
                    var offset = input.ReadInt32();
                    if (offset > 0)
                    {
                        result.Add(ReadSubtitle(input, offset));
                    }
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
