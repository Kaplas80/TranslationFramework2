using System.ComponentModel;
using System.IO;
using System.Linq;
using TF.Core.Entities;
using TF.IO;
using WeifenLuo.WinFormsUI.Docking;

namespace TFGame.TrailsSky.Files.Exe
{
    public class File : TranslationFile
    {
        private CharacterInfo[] _charWidths;
        private FontTableView _ftView;

        protected virtual long FontTableOffset => 0x137770;

        public File(string path, string changesFolder) : base(path, changesFolder)
        {
        }

        public override void Open(DockPanel panel, ThemeBase theme)
        {
            _ftView = new FontTableView(theme);
            _charWidths = GetFontTable();
            _ftView.LoadFontTable(_charWidths);
            _ftView.Show(panel, DockState.Document);
        }

        protected virtual CharacterInfo[] GetFontTable()
        {
            var result = new CharacterInfo[128];

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs))
            {
                input.Seek(FontTableOffset, SeekOrigin.Begin);

                for (var i = 0; i < 128; i++)
                {
                    var width = input.ReadInt32();
                    
                    result[i] = new CharacterInfo((byte) i)
                    {
                        OriginalWidth = width,
                        Width = width,
                    };
                }
            }

            LoadFontTableChanges(result);

            for (var i = 0; i < 128; i++)
            {
                result[i].SetLoadedWidth();
                result[i].PropertyChanged += WidthPropertyChanged;
            }

            return result;
        }

        public override void SaveChanges()
        {
            using (var fs = new FileStream(ChangesFile, FileMode.Create))
            using (var output = new ExtendedBinaryWriter(fs))
            {
                output.Write(ChangesFileVersion);

                for (var i = 0; i < 128; i++)
                {
                    output.Write(_charWidths[i].Width);
                    _charWidths[i].SetLoadedWidth();
                }
            }

            NeedSaving = false;
            OnFileChanged();
        }

        private void LoadFontTableChanges(CharacterInfo[] data)
        {
            if (HasChanges)
            {
                using (var fs = new FileStream(ChangesFile, FileMode.Open))
                using (var input = new ExtendedBinaryReader(fs))
                {
                    var version = input.ReadInt32();

                    if (version != ChangesFileVersion)
                    {
                        System.IO.File.Delete(ChangesFile);
                        return;
                    }

                    for (var i = 0; i < 128; i++)
                    {
                        data[i].Width = input.ReadInt32();
                    }
                }
            }
        }

        protected void WidthPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NeedSaving = _charWidths.Any(x => x.HasChanged);
            OnFileChanged();
        }

        public override void Rebuild(string outputFolder)
        {
            var outputPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(outputFolder, RelativePath));
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath));

            RebuildFontTable(outputPath);
        }

        private void RebuildFontTable(string outputFile)
        {
            if (!System.IO.File.Exists(outputFile))
            {
                System.IO.File.Copy(Path, outputFile);
            }

            var data = GetFontTable();

            using (var fs = new FileStream(outputFile, FileMode.Open))
            using (var output = new ExtendedBinaryWriter(fs))
            {
                output.Seek(FontTableOffset, SeekOrigin.Begin);

                for (var i = 0; i < 128; i++)
                {
                    output.Write(data[i].Width);
                }
            }
        }
    }
}
