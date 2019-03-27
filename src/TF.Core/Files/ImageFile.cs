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
        private ImageView _view;
        private Image _currentImage;

        protected virtual string Filter => "Todos los ficheros (*.*)|*.*";

        public ImageFile(string path, string changesFolder) : base(path, changesFolder, null)
        {
            Type = FileType.ImageFile;
        }

        public override void Open(DockPanel panel, ThemeBase theme)
        {
            _view = new ImageView(theme);
            _view.NewImageLoaded += FormOnNewImageLoaded;
            _view.SaveImage += FormOnSaveImage;
            _view.SetFileFilter(Filter);

            UpdateFormImage();
            _view.Show(panel, DockState.Document);
        }

        protected virtual void FormOnNewImageLoaded(string filename)
        {
            File.Copy(filename, ChangesFile, true);

            UpdateFormImage();
        }

        protected virtual void FormOnSaveImage(string filename)
        {
            _currentImage.Save(filename);
        }

        protected virtual void UpdateFormImage()
        {
            var image = GetImage();
            _currentImage = image.Item1;
            _view.LoadImage(_currentImage, image.Item2);
        }

        protected virtual Tuple<Image, object> GetImage()
        {
            var source = HasChanges ? ChangesFile : Path;
            var image = Image.FromFile(source);
            var properties = GetProperties(image.Width, image.Height, image.PixelFormat.ToString());
            return new Tuple<Image, object>(image, properties);
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
    }
}
