using System.IO;
using TF.Core.Entities;
using TF.Core.Views;
using WeifenLuo.WinFormsUI.Docking;

namespace TF.Core.Files
{
    public class TrueTypeFontFile : TranslationFile
    {
        private FontView _view;
        private byte[] _currentFont;
        
        protected virtual string Filter => "Fuentes (*.ttf)|*.ttf";

        public TrueTypeFontFile(string path, string changesFolder) : base(path, changesFolder, null)
        {
        }

        public override void Open(DockPanel panel, ThemeBase theme)
        {
            _view = new FontView(theme);
            _view.NewFontLoaded += FormOnNewFontLoaded;
            _view.SaveFont += FormOnSaveFont;
            _view.SetFileFilter(Filter);

            UpdateFormLabel();
            _view.Show(panel, DockState.Document);
        }

        protected virtual void FormOnNewFontLoaded(string filename)
        {
            File.Copy(filename, ChangesFile, true);

            UpdateFormLabel();
        }

        protected virtual void FormOnSaveFont(string filename)
        {
            File.WriteAllBytes(filename, _currentFont);
        }

        protected virtual void UpdateFormLabel()
        {
            _currentFont = GetFont();
            _view.LoadFont(_currentFont);
        }

        protected virtual byte[] GetFont()
        {
            var source = HasChanges ? ChangesFile : Path;
            return File.ReadAllBytes(source);
        }
    }
}
