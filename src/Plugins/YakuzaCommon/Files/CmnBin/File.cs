using System.Collections.Generic;
using System.IO;
using TF.IO;
using YakuzaCommon.Files.SimpleSubtitle;

namespace YakuzaCommon.Files.CmnBin
{
    internal class File : SimpleSubtitle.File
    {
        private static readonly byte[] SearchPattern = { 0x83, 0x41, 0x83, 0x6A, 0x83, 0x81, 0x81, 0x5B, 0x83, 0x56, 0x83, 0x87, 0x83, 0x93 };

        public File(string path, string changesFolder) : base(path, changesFolder)
        {
        }

        protected override IList<SimpleSubtitle.Subtitle> GetSubtitles()
        {
            if (HasChanges)
            {
                return LoadChanges(ChangesFile);
            }

            var result = new List<SimpleSubtitle.Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, Encoding, Endianness.BigEndian))
            {
                var currentIndex = input.FindPattern(SearchPattern);

                while (currentIndex != -1)
                {
                    input.ReadBytes(16); //0x10

                    var type = input.ReadUInt64();

                    var subtitles = type == 0 ? ReadLongSubtitles(input) : ReadShortSubtitles(input);

                    result.AddRange(subtitles);

                    currentIndex = input.FindPattern(SearchPattern);
                }
            }

            return result;
        }

        private IList<Subtitle> ReadLongSubtitles(ExtendedBinaryReader input)
        {
            var result = new List<Subtitle>();

            input.ReadBytes(40); //0x28

            var numJapaneseSubs = input.ReadInt32();
            var numEnglishSubs = input.ReadInt32();
            var totalSubs = numJapaneseSubs + numEnglishSubs;

            input.ReadBytes(16); //0x10

            for (var i = 0; i < totalSubs; i++)
            {
                input.ReadBytes(16); //0x10

                var subtitle = ReadLongSubtitle(input);
                if (subtitle.Text.Length > 0)
                {
                    result.Add(subtitle);
                }
            }

            return result;
        }

        private IList<Subtitle> ReadShortSubtitles(ExtendedBinaryReader input)
        {
            var result = new List<Subtitle>();

            input.ReadBytes(266); //0x010A

            for (var subGroup = 0; subGroup < 2; subGroup++)
            {
                var numSubs = input.ReadInt32();
                input.ReadBytes(12); //0x0C
                input.ReadBytes(16); //0x10

                for (var i = 0; i < numSubs; i++)
                {
                    input.ReadBytes(16); //0x10

                    var subtitle = ReadShortSubtitle(input);
                    if (subtitle.Text.Length > 0)
                    {
                        result.Add(subtitle);
                    }
                }
            }

            return result;
        }

        private Subtitle ReadLongSubtitle(ExtendedBinaryReader input)
        {
            var subtitle = new LongSubtitle { Offset = input.Position };

            subtitle.Text = input.ReadString(subtitle.MaxLength, true);
            subtitle.Loaded = subtitle.Text;
            subtitle.Translation = subtitle.Text;

            subtitle.PropertyChanged += SubtitlePropertyChanged;
            input.Seek(subtitle.Offset + subtitle.MaxLength, SeekOrigin.Begin);

            return subtitle;
        }

        private Subtitle ReadShortSubtitle(ExtendedBinaryReader input)
        {
            var subtitle = new ShortSubtitle { Offset = input.Position };

            subtitle.Text = input.ReadString(subtitle.MaxLength, true);
            subtitle.Loaded = subtitle.Text;
            subtitle.Translation = subtitle.Text;
            subtitle.PropertyChanged += SubtitlePropertyChanged;
            input.Seek(subtitle.Offset + subtitle.MaxLength, SeekOrigin.Begin);

            return subtitle;
        }

        public override void SaveChanges()
        {
            using (var fs = new FileStream(ChangesFile, FileMode.Create))
            using (var output = new ExtendedBinaryWriter(fs, System.Text.Encoding.Unicode))
            {
                output.Write(_subtitles.Count);
                foreach (var subtitle in _subtitles)
                {
                    var type = subtitle.GetType().Name;
                    output.Write(type == "LongSubtitle" ? 1 : 0);
                    output.Write(subtitle.Offset);
                    output.WriteString(subtitle.Text);
                    output.WriteString(subtitle.Translation);

                    subtitle.Loaded = subtitle.Translation;
                }
            }

            NeedSaving = false;
            OnFileChanged();
        }

        private IList<SimpleSubtitle.Subtitle> LoadChanges(string file)
        {
            using (var fs = new FileStream(file, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, System.Text.Encoding.Unicode))
            {
                var result = new List<SimpleSubtitle.Subtitle>();
                var subtitleCount = input.ReadInt32();

                for (var i = 0; i < subtitleCount; i++)
                {
                    Subtitle sub;
                    var type = input.ReadInt32();
                    if (type == 0)
                    {
                        sub = new ShortSubtitle();
                    }
                    else
                    {
                        sub = new LongSubtitle();
                    }
                    sub.Offset = input.ReadInt64();
                    sub.Text = input.ReadString();
                    sub.Translation = input.ReadString();
                    sub.Loaded = sub.Translation;

                    sub.PropertyChanged += SubtitlePropertyChanged;

                    result.Add(sub);
                }

                return result;
            }
        }

        public override void Rebuild(string outputFolder)
        {
            var outputPath = System.IO.Path.Combine(outputFolder, RelativePath);
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath));
            System.IO.File.Copy(Path, outputPath);

            var subtitles = GetSubtitles();

            using (var fs = new FileStream(outputPath, FileMode.Open))
            using (var output = new ExtendedBinaryWriter(fs, Encoding))
            {
                foreach (var subtitle in subtitles)
                {
                    output.Seek(subtitle.Offset, SeekOrigin.Begin);

                    var sub = subtitle as Subtitle;
                    for (var i = 0; i < sub.MaxLength; i++)
                    {
                        output.Write((byte) 0);
                    }

                    output.Seek(subtitle.Offset, SeekOrigin.Begin);
                    output.WriteString(subtitle.Translation);
                }
            }
        }
    }
}
