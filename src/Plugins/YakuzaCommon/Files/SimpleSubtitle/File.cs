using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using TF.Core.Entities;
using TF.Core.Helpers;
using TF.IO;
using WeifenLuo.WinFormsUI.Docking;
using YakuzaCommon.Core;

namespace YakuzaCommon.Files.SimpleSubtitle
{
    public class File : TranslationFile
    {
        protected virtual int HEADER_SIZE => 0;

        protected readonly YakuzaEncoding Encoding = new YakuzaEncoding();

        protected IList<Subtitle> _subtitles;

        public File(string path, string changesFolder) : base(path, changesFolder)
        {
            this.Type = FileType.TextFile;
        }

        public override void Open(DockPanel panel, ThemeBase theme)
        {
            var view = new View(theme);

            _subtitles = GetSubtitles();
            view.LoadSubtitles(_subtitles.Where(x => !string.IsNullOrEmpty(x.Text)).ToList());
            view.Show(panel, DockState.Document);
        }

        protected virtual IList<Subtitle> GetSubtitles()
        {
            if (HasChanges)
            {
                return LoadChanges(ChangesFile);
            }

            var result = new List<Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, Encoding, Endianness.BigEndian))
            {
                input.Seek(HEADER_SIZE, SeekOrigin.Begin);

                var firstOffset = input.PeekInt32();

                while (input.Position < firstOffset)
                {
                    var offset = input.ReadInt32();
                    var subtitle = new Subtitle { Offset = offset };
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
        
        protected void SubtitlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NeedSaving = _subtitles.Any(subtitle => subtitle.Loaded != subtitle.Translation);
            OnFileChanged();
        }

        public override bool Search(string searchString)
        {
            var bytes = System.IO.File.ReadAllBytes(HasChanges ? ChangesFile : Path);

            var pattern = Encoding.GetBytes(searchString);

            var index = SearchHelper.SearchPattern(bytes, pattern, 0);

            return index != -1;
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

            NeedSaving = false;
            OnFileChanged();
        }

        protected IList<Subtitle> LoadChanges(string file)
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
                        Offset = input.ReadInt64(),
                        Text = input.ReadString(),
                        Translation = input.ReadString()
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

            byte[] header = null;
            if (HEADER_SIZE > 0)
            {
                using (var fs = new FileStream(Path, FileMode.Open))
                using (var input = new ExtendedBinaryReader(fs, Encoding))
                {
                    header = input.ReadBytes(HEADER_SIZE);
                }
            }

            var subtitles = GetSubtitles();

            using (var fs = new FileStream(outputPath, FileMode.Create))
            using (var output = new ExtendedBinaryWriter(fs, Encoding, Endianness.BigEndian))
            {
                if (HEADER_SIZE > 0)
                {
                    output.Write(header);
                }

                long currentOffset = HEADER_SIZE + 4 * subtitles.Count;

                for (var i = 0; i < subtitles.Count; i++)
                {
                    var subtitle = subtitles[i];

                    output.Seek(HEADER_SIZE + i * 4, SeekOrigin.Begin);

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
