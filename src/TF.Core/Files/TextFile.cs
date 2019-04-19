using System.ComponentModel;
using System.IO;
using System.Text;
using TF.Core.Entities;
using TF.Core.Helpers;
using TF.Core.TranslationEntities;
using TF.Core.Views;
using TF.IO;
using WeifenLuo.WinFormsUI.Docking;

namespace TF.Core.Files
{
    public class TextFile : TranslationFile
    {
        protected PlainText _text;
        protected TextView _view;

        public override int SubtitleCount
        {
            get
            {
                var subtitles = GetText();
                return subtitles.Text.Length;
            }
        }

        public TextFile(string path, string changesFolder, Encoding encoding) : base(path, changesFolder, encoding)
        {
            Type = FileType.TextFile;
        }

        public override void Open(DockPanel panel, ThemeBase theme)
        {
            _view = new TextView(theme);

            _text = GetText();
            _view.LoadData(_text);
            _view.Show(panel, DockState.Document);
        }

        protected virtual PlainText GetText()
        {
            var result = new PlainText();

            var lines = File.ReadAllText(Path, FileEncoding);
            result.Text = lines;
            result.Translation = lines;
            result.Loaded = lines;
            result.PropertyChanged += SubtitlePropertyChanged;
            
            LoadChanges(result);
            
            return result;
        }
        
        protected virtual void SubtitlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NeedSaving = _text.HasChanges;
            OnFileChanged();
        }

        public override bool Search(string searchString)
        {
            var bytes = File.ReadAllBytes(Path);

            var pattern = FileEncoding.GetBytes(searchString);

            var index1 = SearchHelper.SearchPattern(bytes, pattern, 0);

            var index2 = -1;
            if (HasChanges)
            {
                bytes = File.ReadAllBytes(ChangesFile);
                pattern = Encoding.Unicode.GetBytes(searchString);
                index2 = SearchHelper.SearchPattern(bytes, pattern, 0);
            }

            return index1 != -1 || index2 != -1;
        }

        public override void SaveChanges()
        {
            using (var fs = new FileStream(ChangesFile, FileMode.Create))
            using (var output = new ExtendedBinaryWriter(fs, Encoding.Unicode))
            {
                output.Write(ChangesFileVersion);
                output.WriteString(_text.Translation);
                _text.Loaded = _text.Translation;
            }

            NeedSaving = false;
            OnFileChanged();
        }

        protected virtual void LoadChanges(PlainText text)
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

                    var t = input.ReadString();
                    text.PropertyChanged -= SubtitlePropertyChanged;
                    text.Translation = t;
                    text.Loaded = t;
                    text.PropertyChanged += SubtitlePropertyChanged;
                }
            }
        }

        public override void Rebuild(string outputFolder)
        {
            var outputPath = System.IO.Path.Combine(outputFolder, RelativePath);
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath));

            var text = GetText();

            using (var fs = new FileStream(outputPath, FileMode.Create))
            using (var output = new StreamWriter(fs, FileEncoding))
            {
                output.Write(text.Translation);
            }
        }

        public override bool SearchText(string searchString, int direction)
        {
            if (_text == null || string.IsNullOrEmpty(searchString))
            {
                return false;
            }

            return _view.SearchText(searchString, direction);
        }
    }
}
