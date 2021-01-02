namespace TF.Core.Files
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using TF.Core.TranslationEntities;
    using TF.Core.Views;
    using TF.IO;
    using WeifenLuo.WinFormsUI.Docking;

    public class BinaryTextFileWithIds : BinaryTextFile
    {
        public BinaryTextFileWithIds(string gameName, string path, string changesFolder, System.Text.Encoding encoding) : base(gameName, path, changesFolder, encoding)
        {
        }

        public override void Open(DockPanel panel)
        {
            _view = new GridViewWithId(this);

            _subtitles = GetSubtitles();
            _view.LoadData(_subtitles.Where(x => !string.IsNullOrEmpty(x.Text)).ToList());
            _view.Show(panel, DockState.Document);
        }

        public override void SaveChanges()
        {
            using (var fs = new FileStream(ChangesFile, FileMode.Create))
            using (var output = new ExtendedBinaryWriter(fs, System.Text.Encoding.Unicode))
            {
                output.Write(ChangesFileVersion);
                output.Write(_subtitles.Count);
                foreach (Subtitle subtitle in _subtitles)
                {
                    var sub = subtitle as SubtitleWithId;
                    output.WriteString(sub.Id);
                    output.WriteString(subtitle.Translation);

                    subtitle.Loaded = subtitle.Translation;
                }
            }

            NeedSaving = false;
            OnFileChanged();
        }

        protected override void LoadChanges(IList<Subtitle> subtitles)
        {
            if (!HasChanges)
            {
                return;
            }

            List<SubtitleWithId> subs = subtitles.Select(subtitle => subtitle as SubtitleWithId).ToList();
            var dictionary = new Dictionary<string, SubtitleWithId>(subs.Count);
            foreach (SubtitleWithId subtitle in subs)
            {
                if (!dictionary.ContainsKey(subtitle.Id))
                {
                    dictionary.Add(subtitle.Id, subtitle);
                }
            }

            using (var fs = new FileStream(ChangesFile, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, System.Text.Encoding.Unicode))
            {
                int version = input.ReadInt32();

                if (version != ChangesFileVersion)
                {
                    //File.Delete(ChangesFile);
                    return;
                }

                int subtitleCount = input.ReadInt32();

                for (int i = 0; i < subtitleCount; i++)
                {
                    string id = input.ReadString();
                    string text = input.ReadString();

                    if (dictionary.TryGetValue(id, out SubtitleWithId subtitle))
                    {
                        subtitle.PropertyChanged -= SubtitlePropertyChanged;
                        subtitle.Translation = text;
                        subtitle.Loaded = subtitle.Translation;
                        subtitle.PropertyChanged += SubtitlePropertyChanged;
                    }
                }
            }
        }

        protected override string GetContext(Subtitle subtitle)
        {
            return (subtitle as SubtitleWithId).Id.Replace(LineEnding.ShownLineEnding, LineEnding.PoLineEnding);
        }
    }
}
