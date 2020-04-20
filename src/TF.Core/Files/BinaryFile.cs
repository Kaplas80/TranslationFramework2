using TF.Core.Entities;
using TF.Core.Views;
using WeifenLuo.WinFormsUI.Docking;

namespace TF.Core.Files
{
    public class BinaryFile : TranslationFile
    {
        protected BinaryView _view;

        protected virtual string Filter => "Todos los ficheros (*.*)|*.*";

        public BinaryFile(string gameName, string path, string changesFolder) : base(gameName, path, changesFolder, null)
        {
            Type = FileType.Unknown;
        }

        public override void Open(DockPanel panel)
        {
            _view = new BinaryView(System.IO.Path.GetFileName(Path));
            _view.ImportFile += FormOnImportFile;
            _view.ExportFile += FormOnExportFile;
            _view.SetFileFilter(Filter);

            UpdateForm();
            _view.Show(panel, DockState.Document);
        }

        protected virtual void FormOnImportFile(string filename)
        {
            System.IO.File.Copy(filename, ChangesFile, true);

            UpdateForm();
        }

        protected virtual void FormOnExportFile(string filename)
        {
            if (System.IO.File.Exists(ChangesFile))
            {
                System.IO.File.Copy(ChangesFile, filename, true);
            }
            else
            {
                System.IO.File.Copy(Path, filename, true);
            }
        }

        protected virtual void UpdateForm()
        {
            if (System.IO.File.Exists(ChangesFile))
            {
                _view.LoadData(ChangesFile);
            }
            else
            {
                _view.LoadData(Path);
            }
        }
    }
}
