using System;
using System.Drawing;
using System.Drawing.Imaging;
using TF.Core.Files;

namespace TFGame.TrailsSky.Files.Images
{
    public enum ImageFormat
    {
        Format4444,
        Format1555
    }

    public abstract class TrailsInTheSkyImage : ImageFile
    {
        private byte[] _currentImage;
        protected abstract int ImageWidth { get; }
        protected abstract ImageFormat ImagePixelFormat { get; }

        protected override string Filter => "Ficheros de imágenes (*._CH)|*._CH";

        protected TrailsInTheSkyImage(string path, string changesFolder) : base(path, changesFolder)
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

            var bmp = GetBitmap(ImageWidth, ImagePixelFormat,_currentImage);

            return new Tuple<Image, object>(bmp, null);
        }

        private static Bitmap GetBitmap(int imageWidth, ImageFormat imagePixelFormat, byte[] imageData)
        {
            var imageHeight = GetImageHeight(imageData.Length, imageWidth, imagePixelFormat);
            var bpp = GetBpp(imagePixelFormat);

            var data = new byte[imageWidth * imageHeight * 4];

            var tempValues = new byte[bpp];

            var outputIndex = 0;
            for (var i = 0; i < imageWidth * imageHeight * bpp; i = i + bpp) 
            {
                for (var j = 0; j < bpp; j++)
                {
                    tempValues[j] = imageData[i + j];
                }

                var resultPixels = ConvertPixel(tempValues, imagePixelFormat);

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

        private static int GetImageHeight(int fileLength, int imageWidth, ImageFormat format)
        {
            var bpp = GetBpp(format);
            return fileLength / (imageWidth * bpp);
        }

        private static int GetBpp(ImageFormat format)
        {
            switch (format)
            {
                case ImageFormat.Format4444:
                case ImageFormat.Format1555:
                    return 2;
                default:
                    throw new ArgumentOutOfRangeException(nameof(format), format, null);
            }
        }

        private static byte[] ConvertPixel(byte[] inputValues, ImageFormat format)
        {
            var result = new byte[4];

            switch (format)
            {
                case ImageFormat.Format4444:
                {
                    result[0] = (byte) ((byte)(inputValues[0] & 0x0F) * 0x11);
                    result[1] = (byte) ((byte)((inputValues[0] >> 4) & 0x0F) * 0x11);
                    result[2] = (byte) ((byte)(inputValues[1] & 0x0F) * 0x11);
                    result[3] = (byte) ((byte)((inputValues[1] >> 4) & 0x0F) * 0x11);

                    return result;
                }
                case ImageFormat.Format1555:
                {
                    var b = inputValues[0] & 0b00011111;
                    var g = ((inputValues[0] >> 5) & 0b00000111) | ((inputValues[1] & 0b00000011) << 3);
                    var r = (inputValues[1] >> 2) & 0b00011111;
                    var a = ((inputValues[1] >> 7) & 0b00000001) * 0xFF;
                    result[0] = (byte) ((b << 3) | (b >> 2));
                    result[1] = (byte) ((g << 3) | (g >> 2));
                    result[2] = (byte) ((r << 3) | (r >> 2));
                    result[3] = (byte) a;

                    return result;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(format), format, null);
            }
        }
    }
}
