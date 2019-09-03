using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using TF.Core.Entities;
using TF.Core.Helpers;
using TF.Core.TranslationEntities;
using TF.Core.Views;
using TF.IO;
using WeifenLuo.WinFormsUI.Docking;

namespace TF.Core.Files
{
    public class BinaryTextFile : TranslationFile
    {
        protected virtual int StartOffset => 0;

        protected IList<Subtitle> _subtitles;

        protected GridView _view;

        public override int SubtitleCount
        {
            get
            {
                var subtitles = GetSubtitles();
                return subtitles.Count(x => (x != null) && (!string.IsNullOrEmpty(x.Text)));
            }
        }

        public BinaryTextFile(string path, string changesFolder, Encoding encoding) : base(path, changesFolder, encoding)
        {
            Type = FileType.TextFile;
        }

        public override void Open(DockPanel panel, ThemeBase theme)
        {
            _view = new GridView(theme, LineEnding);

            _subtitles = GetSubtitles();
            _view.LoadData(_subtitles.Where(x => !string.IsNullOrEmpty(x.Text)).ToList());
            _view.Show(panel, DockState.Document);
        }

        protected virtual IList<Subtitle> GetSubtitles()
        {
            var result = new List<Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding))
            {
                input.Skip(StartOffset);

                while (input.Position < input.Length)
                {
                    var subtitle = ReadSubtitle(input);
                    subtitle.PropertyChanged += SubtitlePropertyChanged;
                    result.Add(subtitle);
                }
            }

            LoadChanges(result);
            
            return result;
        }
        
        public override bool Search(string searchString)
        {
            var bytes = File.ReadAllBytes(Path);

            var pattern = FileEncoding.GetBytes(searchString);

            var searchHelper = new SearchHelper(pattern);
            var index1 = searchHelper.Search(bytes);

            var index2 = -1;
            if (HasChanges)
            {
                bytes = File.ReadAllBytes(ChangesFile);
                pattern = Encoding.Unicode.GetBytes(searchString);
                searchHelper = new SearchHelper(pattern);
                index2 = searchHelper.Search(bytes);
            }

            return index1 != -1 || index2 != -1;
        }

        public override void SaveChanges()
        {
            using (var fs = new FileStream(ChangesFile, FileMode.Create))
            using (var output = new ExtendedBinaryWriter(fs, Encoding.Unicode))
            {
                output.Write(ChangesFileVersion);
                output.Write(_subtitles.Count);
                foreach (var subtitle in _subtitles)
                {
                    output.Write(subtitle.Offset);
                    output.WriteString(subtitle.Translation);

                    subtitle.Loaded = subtitle.Translation;
                }
            }

            NeedSaving = false;
            OnFileChanged();
        }

        protected virtual void LoadChanges(IList<Subtitle> subtitles)
        {
            if (HasChanges)
            {
                using (var fs = new FileStream(ChangesFile, FileMode.Open))
                using (var input = new ExtendedBinaryReader(fs, Encoding.Unicode))
                {
                    var version = input.ReadInt32();

                    if (version != ChangesFileVersion)
                    {
                        //File.Delete(ChangesFile);
                        return;
                    }

                    var subtitleCount = input.ReadInt32();

                    for (var i = 0; i < subtitleCount; i++)
                    {
                        var offset = input.ReadInt64();
                        var text = input.ReadString();

                        var subtitle = subtitles.FirstOrDefault(x => x.Offset == offset);
                        if (subtitle != null)
                        {
                            subtitle.PropertyChanged -= SubtitlePropertyChanged;
                            subtitle.Translation = text;
                            subtitle.Loaded = subtitle.Translation;
                            subtitle.PropertyChanged += SubtitlePropertyChanged;
                        }
                    }
                }
            }
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
                if (StartOffset > 0)
                {
                    output.Write(input.ReadBytes(StartOffset));
                }

                long outputOffset = StartOffset;

                while (input.Position < input.Length)
                {
                    var inputSubtitle = ReadSubtitle(input);
                    outputOffset = WriteSubtitle(output, subtitles, inputSubtitle.Offset, outputOffset);
                }
            }
        }

        public override bool SearchText(string searchString, int direction)
        {
            if (_subtitles == null || _subtitles.Count == 0 || _view == null)
            {
                return false;
            }

            int i;
            int rowIndex;
            if (direction == 0)
            {
                i = 1;
                rowIndex = 1;
            }
            else
            {
                var (item1, item2) = _view.GetSelectedSubtitle();
                i = _subtitles.IndexOf(item2) + direction;
                rowIndex = item1 + direction;
            }

            var step = direction < 0 ? -1 : 1;

            var result = -1;
            while (i >= 0  && i < _subtitles.Count)
            {
                var subtitle = _subtitles[i];
                var original = subtitle.Text;
                var translation = subtitle.Translation;

                if (!string.IsNullOrEmpty(original))
                {
                    if (original.Contains(searchString) || (!string.IsNullOrEmpty(translation) && translation.Contains(searchString)))
                    {
                        result = rowIndex;
                        break;
                    }

                    rowIndex += step;
                }

                i+=step;
            }

            if (result != -1)
            {
                _view.DisplaySubtitle(rowIndex);
            }

            return result != -1;
        }

        protected virtual void SubtitlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NeedSaving = _subtitles.Any(subtitle => subtitle.HasChanges);
            OnFileChanged();
        }

        protected virtual Subtitle ReadSubtitle(ExtendedBinaryReader input)
        {
            var offset = input.Position;
            var subtitle = ReadSubtitle(input, offset, false);
            return subtitle;
        }

        protected virtual Subtitle ReadSubtitle(ExtendedBinaryReader input, long offset, bool returnToPos)
        {
            var pos = input.Position;

            input.Seek(offset, SeekOrigin.Begin);

            var text = input.ReadString();

            var subtitle = new Subtitle
            {
                Offset = offset, Text = text, Translation = text, Loaded = text,
            };

            if (returnToPos)
            {
                input.Seek(pos, SeekOrigin.Begin);
            }

            return subtitle;
        }

        protected virtual long WriteSubtitle(ExtendedBinaryWriter output, IList<Subtitle> subtitles, long inputOffset, long outputOffset)
        {
            var sub = subtitles.First(x => x.Offset == inputOffset);
            var result = WriteSubtitle(output, sub, outputOffset, false);

            return result;
        }

        protected virtual long WriteSubtitle(ExtendedBinaryWriter output, Subtitle subtitle, long offset, bool returnToPos)
        {
            var pos = output.Position;
            output.Seek(offset, SeekOrigin.Begin);
            output.WriteString(subtitle.Translation);

            var result = output.Position;

            if (returnToPos)
            {
                output.Seek(pos, SeekOrigin.Begin);
            }

            return result;
        }
    }
}
