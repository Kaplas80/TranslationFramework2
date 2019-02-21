using System;
using System.IO;
using WeifenLuo.WinFormsUI.Docking;

namespace TF.Core.Entities
{
    public enum FileType
    {
        Unknown = 1,
        TextFile,
        ImageFile,
    }

    public class TranslationFile
    {
        private string _id;
        private readonly string _changesFolder;

        public string Id {
            get => _id;
            set
            {
                _id = value;
                ChangesFile = System.IO.Path.Combine(_changesFolder, $"{value}.tf");
            }
        }
        public string Path { get; set; }
        public string RelativePath { get; set; }
        public string Name { get; set; }
        public FileType Type { get; set; }
        public bool NeedSaving { get; set; }

        protected string ChangesFile { get; private set; }
        public bool HasChanges => File.Exists(ChangesFile);

        public TranslationFile(string path, string changesFolder)
        {
            _changesFolder = changesFolder;
            Id = Guid.NewGuid().ToString();
            Path = path;
            Name = System.IO.Path.GetFileName(path);
            Type = FileType.Unknown;

            NeedSaving = false;
        }

        public virtual void Open(DockPanel panel, ThemeBase theme)
        {
        }

        public virtual void SaveChanges()
        {
        }

        public virtual void Rebuild(string outputFolder)
        {
            var outputFile = System.IO.Path.Combine(outputFolder, RelativePath);
            var dir = System.IO.Path.GetDirectoryName(outputFile);
            Directory.CreateDirectory(dir);

            File.Copy(HasChanges ? ChangesFile : Path, outputFile);
        }

        public virtual void Restore()
        {
            if (HasChanges)
            {
                File.Delete(ChangesFile);
            }

            NeedSaving = false;
            OnFileChanged();
        }

        public event FileChangedEventHandler FileChanged;
        public delegate void FileChangedEventHandler();

        protected virtual void OnFileChanged()
        {
            FileChanged?.Invoke();
        }

        public virtual bool Search(string searchString)
        {
            return false;
        }
    }
}
