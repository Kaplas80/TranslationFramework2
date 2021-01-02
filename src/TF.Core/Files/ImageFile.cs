using System;
using System.Drawing;
using System.IO;
using TF.Core.Entities;
using TF.Core.Views;
using WeifenLuo.WinFormsUI.Docking;

namespace TF.Core.Files
{
    public class ImageProperties
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Format { get; set; }
    }

    public class ImageFile : TranslationFile
    {
        protected ImageView _view;
        private Image _currentImage;

        protected virtual string Filter => "Todos los ficheros (*.*)|*.*";

        public ImageFile(string gameName, string path, string changesFolder) : base(gameName, path, changesFolder, null)
        {
            Type = FileType.ImageFile;
        }

        public override void Open(DockPanel panel)
        {
            _view = new ImageView(System.IO.Path.GetFileName(Path));
            _view.ImportImage += FormOnImportImage;
            _view.ExportImage += FormOnExportImage;
            _view.SetFileFilter(Filter);

            UpdateFormImage();
            _view.Show(panel, DockState.Document);
        }

        protected virtual void FormOnImportImage(string filename)
        {
            ImportImage(filename);

            UpdateFormImage();
        }

        protected virtual void FormOnExportImage(string filename)
        {
            ExportImage(filename);
        }

        protected virtual void UpdateFormImage()
        {
            _currentImage = GetDrawingImage();
            object properties = GetImageProperties(_currentImage);
            _view.LoadImage(_currentImage, properties);
        }

        protected virtual Image GetDrawingImage()
        {
            string source = HasChanges ? ChangesFile : Path;
            return Image.FromFile(source);
        }

        protected virtual object GetImageProperties(object genericImage)
        {
            return genericImage is Image image ? GetProperties(image.Width, image.Height, image.PixelFormat.ToString()) : null;
        }

        protected virtual ImageProperties GetProperties(int width, int height, string format)
        {
            var result = new ImageProperties
            {
                Width = width,
                Height = height,
                Format = format,
            };

            return result;
        }

        public override void ExportImage(string path)
        {
            if (_currentImage == null)
            {
                _currentImage = GetDrawingImage();
            }
            
            string directory = System.IO.Path.GetDirectoryName(path);
            Directory.CreateDirectory(directory);

            _currentImage.Save(path);
        }

        public override void ImportImage(string path)
        {
            File.Copy(path, ChangesFile, true);
        }
    }
}
