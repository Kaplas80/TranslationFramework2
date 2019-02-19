using System.Collections.Generic;
using System.IO;
using TF.IO;
using YakuzaCommon.Files.SimpleSubtitle;

namespace YakuzaCommon.Files.AiPopup
{
    internal class File : SimpleSubtitleFile
    {
        private int FIRST_OFFSET = 0x1AC;

        public File(string path, string changesFolder) : base(path, changesFolder)
        {
        }

        protected override IList<Subtitle> GetSubtitles()
        {
            if (System.IO.File.Exists(ChangesFile))
            {
                return LoadChanges(ChangesFile);
            }

            var result = new List<Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, Encoding, Endianness.BigEndian))
            {
                input.Seek(FIRST_OFFSET, SeekOrigin.Begin);
                var firstOffset = input.PeekInt32();
                
                while (input.Position < firstOffset)
                {
                    var offset = input.ReadInt32();
                    var subtitle = new Subtitle {Offset = offset};
                    result.Add(subtitle);

                }

                foreach (var subtitle in result)
                {
                    if (subtitle.Offset > 0)
                    {
                        input.Seek(subtitle.Offset, SeekOrigin.Begin);
                        subtitle.Text = input.ReadString();
                        subtitle.Loaded = subtitle.Text;
                        subtitle.Translation = subtitle.Text;

                        subtitle.PropertyChanged += SubtitlePropertyChanged;
                    }
                }
            }

            return result;
        }

        public override void SaveChanges()
        {
            using (var fs = new FileStream(ChangesFile, FileMode.Create))
            using (var output = new ExtendedBinaryWriter(fs, System.Text.Encoding.Unicode))
            {
                output.Write(_subtitles.Count);
                foreach (var subtitle in _subtitles)
                {
                    output.Write(subtitle.Offset);
                    output.WriteString(subtitle.Text);
                    output.WriteString(subtitle.Translation);

                    subtitle.Loaded = subtitle.Translation;
                }
            }

            HasChanges = false;
            OnFileChanged();
        }

        private IList<Subtitle> LoadChanges(string file)
        {
            using (var fs = new FileStream(file, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, System.Text.Encoding.Unicode))
            {
                var result = new List<Subtitle>();
                var subtitleCount = input.ReadInt32();

                for (var i = 0; i < subtitleCount; i++)
                {
                    var subtitle = new Subtitle
                    {
                        Offset = input.ReadInt64(), Text = input.ReadString(), Translation = input.ReadString()
                    };
                    subtitle.Loaded = subtitle.Translation;

                    subtitle.PropertyChanged += SubtitlePropertyChanged;

                    result.Add(subtitle);
                }

                return result;
            }
        }

        public override void Rebuild(string outputFolder)
        {
            var outputPath = System.IO.Path.Combine(outputFolder, RelativePath);
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath));

            byte[] header;
            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, Encoding))
            {
                header = input.ReadBytes(FIRST_OFFSET);
            }

            var subtitles = GetSubtitles();

            using (var fs = new FileStream(outputPath, FileMode.Create))
            using (var output = new ExtendedBinaryWriter(fs, Encoding, Endianness.BigEndian))
            {
                output.Write(header);
                long currentOffset = FIRST_OFFSET + 4 * subtitles.Count;

                for (var i = 0; i < subtitles.Count; i++)
                {
                    var subtitle = subtitles[i];

                    output.Seek(FIRST_OFFSET + i * 4, SeekOrigin.Begin);

                    if (subtitle.Offset > 0)
                    {
                        output.Write((int)currentOffset);
                        output.Seek(currentOffset, SeekOrigin.Begin);
                        output.WriteString(subtitle.Translation);
                        currentOffset = output.Position;
                    }
                    else
                    {
                        output.Write((int)0);
                    }
                }
            }
        }
    }
}
