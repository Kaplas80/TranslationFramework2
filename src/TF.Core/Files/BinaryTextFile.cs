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
using Yarhl.IO;
using Yarhl.Media.Text;

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

        public BinaryTextFile(string gameName, string path, string changesFolder, System.Text.Encoding encoding) : base(gameName, path, changesFolder, encoding)
        {
            Type = FileType.TextFile;
        }

        public override void Open(DockPanel panel)
        {
            _view = new GridView(this);

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
        
        public override bool Search(string searchString, string path = "")
        {
            if (string.IsNullOrEmpty(path))
            {
                path = Path;
            }
            var bytes = File.ReadAllBytes(path);

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
                var dictionary = new Dictionary<long, Subtitle>(subtitles.Count);
                foreach (Subtitle subtitle in subtitles)
                {
                    dictionary.Add(subtitle.Offset, subtitle);
                }

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

                        if (dictionary.TryGetValue(offset, out Subtitle subtitle))
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
            while (i >= 0 && i < _subtitles.Count)
            {
                var subtitle = _subtitles[i];
                var original = subtitle.Text;
                var translation = subtitle.Translation;

                if (!string.IsNullOrWhiteSpace(original))
                {
                    if (original.Contains(searchString) || (!string.IsNullOrEmpty(translation) && translation.Contains(searchString)))
                    {
                        result = rowIndex;
                        break;
                    }

                    rowIndex += step;
                }

                i += step;
            }

            var found = i >= 0 && i < _subtitles.Count;
            if (found)
            {
                _view.DisplaySubtitle(rowIndex);
            }

            return found;
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

        public override void ExportPo(string path)
        {
            var directory = System.IO.Path.GetDirectoryName(path);
            Directory.CreateDirectory(directory);
            
            var po = new Po()
            {
                Header = new PoHeader(GameName, "dummy@dummy.com", "es-ES")
            };

            var subtitles = GetSubtitles();
            foreach (var subtitle in subtitles)
            {
                var entry = new PoEntry();

                var original = subtitle.Text;
                var translation = subtitle.Translation;
                if (string.IsNullOrEmpty(original))
                {
                    original = "<!empty>";
                    translation = "<!empty>";
                }

                entry.Original = original.Replace(LineEnding.ShownLineEnding, LineEnding.PoLineEnding);
                entry.Context = GetContext(subtitle);

                if (original != translation)
                {
                    entry.Translated = translation.Replace(LineEnding.ShownLineEnding, LineEnding.PoLineEnding);
                }

                po.Add(entry);
            }

            var po2binary = new Yarhl.Media.Text.Po2Binary();
            var binary = po2binary.Convert(po);
            
            binary.Stream.WriteTo(path);
        }

        public override void ImportPo(string inputFile, bool save = true)
        {
            var dataStream = DataStreamFactory.FromFile(inputFile, FileOpenMode.Read);
            var binary = new BinaryFormat(dataStream);
            var binary2Po = new Yarhl.Media.Text.Po2Binary();
            var po = binary2Po.Convert(binary);

            LoadBeforeImport();
            foreach (var subtitle in _subtitles)
            {
                var original = subtitle.Text;
                if (string.IsNullOrEmpty(original))
                {
                    original = "<!empty>";
                }

                var entry = po.FindEntry(original.Replace(LineEnding.ShownLineEnding, LineEnding.PoLineEnding),
                    GetContext(subtitle));

                if (entry == null || entry.Text == "<!empty>" || original == "<!empty>")
                {
                    subtitle.Translation = subtitle.Text;
                }
                else
                {
                    var translation = entry.Translated;
                    subtitle.Translation = string.IsNullOrEmpty(translation)
                        ? subtitle.Text
                        : translation.Replace(LineEnding.PoLineEnding, LineEnding.ShownLineEnding);
                }
            }

            if (save && NeedSaving)
            {
                SaveChanges();
            }
        }

        protected override void LoadBeforeImport()
        {
            _subtitles = GetSubtitles();
        }

        protected override string GetContext(Subtitle subtitle)
        {
            return subtitle.Offset.ToString();
        }
    }
}
