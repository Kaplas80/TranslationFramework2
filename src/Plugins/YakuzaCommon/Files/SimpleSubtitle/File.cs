using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TF.Core.Entities;
using TF.Core.Helpers;
using WeifenLuo.WinFormsUI.Docking;
using YakuzaCommon.Core;

namespace YakuzaCommon.Files.SimpleSubtitle
{
    internal abstract class File : TranslationFile
    {
        protected readonly YakuzaEncoding Encoding = new YakuzaEncoding();

        protected IList<Subtitle> _subtitles;

        protected File(string path, string changesFolder) : base(path, changesFolder)
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

        protected abstract IList<Subtitle> GetSubtitles();
        
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
    }
}
