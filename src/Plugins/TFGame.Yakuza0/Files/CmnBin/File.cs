using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using TF.Core.Entities;
using TF.IO;
using TFGame.Yakuza0.Core;
using TFGame.Yakuza0.Files.CmnBin;
using TFGame.Yakuza0.Views;
using WeifenLuo.WinFormsUI.Docking;

namespace TFGame.Yakuza0.Files
{
    class CmnBinFile : TranslationFile
    {
        private static readonly byte[] SearchPattern = { 0x8E, 0x9A, 0x96, 0x8B, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

        private readonly YakuzaEncoding Encoding = new YakuzaEncoding();

        private IList<Subtitle> _subtitles;

        public CmnBinFile(string path, string changesFolder) : base(path, changesFolder)
        {
            this.Type = FileType.TextFile;
        }

        public override void Open(DockPanel panel, ThemeBase theme)
        {
            var view = new CmnBinView(theme);

            _subtitles = GetSubtitles();
            view.LoadSubtitles(_subtitles);
            view.Show(panel, DockState.Document);
        }

        private IList<Subtitle> GetSubtitles()
        {
            if (File.Exists(ChangesFile))
            {
                return LoadChanges(ChangesFile);
            }

            var result = new List<Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, Encoding, Endianness.BigEndian))
            {
                var currentIndex = input.FindPattern(SearchPattern);

                while (currentIndex != -1)
                {
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

            input.ReadBytes(40);

            var numJapaneseSubs = input.ReadInt32();
            var numEnglishSubs = input.ReadInt32();
            var totalSubs = numJapaneseSubs + numEnglishSubs;

            input.ReadBytes(16);

            for (var i = 0; i < totalSubs; i++)
            {
                input.ReadBytes(16);

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

            input.ReadBytes(266);

            for (var subGroup = 0; subGroup < 2; subGroup++)
            {
                var numSubs = input.ReadInt32();
                input.ReadBytes(12);
                input.ReadBytes(16);

                for (var i = 0; i < numSubs; i++)
                {
                    input.ReadBytes(16);

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

        private void SubtitlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            HasChanges = _subtitles.Any(subtitle => subtitle.Loaded != subtitle.Translation);
            OnFileChanged();
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
            File.Copy(Path, outputPath);

            var subtitles = GetSubtitles();

            using (var fs = new FileStream(outputPath, FileMode.Open))
            using (var output = new ExtendedBinaryWriter(fs, Encoding))
            {
                foreach (var subtitle in subtitles)
                {
                    output.Seek(subtitle.Offset, SeekOrigin.Begin);
                    for (var i = 0; i < subtitle.MaxLength; i++)
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
