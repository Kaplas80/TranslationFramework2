using System;
using System.Drawing;
using System.Drawing.Imaging;
using TF.Core.Files;

namespace TFGame.TrailsSky.Files.Images
{
    public abstract class AbstractImageFile : ImageFile
    {
        protected byte[] _currentImage;
        protected abstract int ImageWidth { get; }
        protected abstract int BytesPerPixel { get; }
        protected abstract string ImageFormat { get; }

        protected override string Filter => "Ficheros de imágenes (*._CH)|*._CH";

        protected AbstractImageFile(string path, string changesFolder) : base(path, changesFolder)
        {
        }

        protected override void FormOnSaveImage(string filename)
        {
            System.IO.File.WriteAllBytes(filename, _currentImage);
        }

        protected override void FormOnNewImageLoaded(string filename)
        {
            System.IO.File.Copy(filename, ChangesFile, true);

            UpdateFormImage();
        }

        protected override Tuple<Image, object> GetImage()
        {
            var source = HasChanges ? ChangesFile : Path;
            _currentImage = System.IO.File.ReadAllBytes(source);

            var bmp = GetBitmap(ImageWidth, _currentImage);
            var properties = GetProperties(bmp.Width, bmp.Height, ImageFormat);
            return new Tuple<Image, object>(bmp, properties);
        }

        protected Bitmap GetBitmap(int imageWidth, byte[] imageData)
        {
            var imageHeight = GetImageHeight(imageData.Length, imageWidth);

            var data = new byte[imageWidth * imageHeight * 4];

            var tempValues = new byte[BytesPerPixel];

            var outputIndex = 0;
            for (var i = 0; i < imageWidth * imageHeight * BytesPerPixel; i = i + BytesPerPixel) 
            {
                for (var j = 0; j < BytesPerPixel; j++)
                {
                    tempValues[j] = imageData[i + j];
                }

                var resultPixels = ConvertPixel(tempValues);

                for (var j = 0; j < resultPixels.Length; j++)
                {
                    data[outputIndex + j] = resultPixels[j];
                }

                outputIndex += resultPixels.Length;
            }

            unsafe
            {
                fixed (byte* ptr = data)
                {
                    var image = new Bitmap(imageWidth, imageHeight, imageWidth * 4, PixelFormat.Format32bppArgb, new IntPtr(ptr));
                    return image;
                }
            }
        }

        private int GetImageHeight(int fileLength, int imageWidth)
        {
            return fileLength / (imageWidth * BytesPerPixel);
        }

        protected abstract byte[] ConvertPixel(byte[] inputValues);
    }
}
