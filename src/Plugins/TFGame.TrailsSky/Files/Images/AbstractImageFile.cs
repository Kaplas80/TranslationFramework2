using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using TF.Core.Files;

namespace TFGame.TrailsSky.Files.Images
{
    public abstract class AbstractImageFile : ImageFile
    {
        protected byte[] _currentImage;
        protected abstract int ImageWidth { get; }
        protected abstract int BytesPerPixel { get; }
        protected abstract string ImageFormat { get; }

        protected override string Filter => "Ficheros de imágenes (*.png)|*.png";

        protected AbstractImageFile(string gameName, string path, string changesFolder) : base(gameName, path, changesFolder)
        {
        }

        public override void ImportImage(string path)
        {
            using (var bmp = new Bitmap(path))
            {
                byte[] image = GetBitmapOriginalFormat(bmp);
                File.WriteAllBytes(ChangesFile, image);
            }
        }

        protected override Image GetDrawingImage()
        {
            string source = HasChanges ? ChangesFile : Path;
            byte[] imageData = System.IO.File.ReadAllBytes(source);

            Bitmap bmp = GetBitmap32BppArgb(ImageWidth, imageData);
            return bmp;
        }

        protected Bitmap GetBitmap32BppArgb(int imageWidth, byte[] imageData)
        {
            int imageHeight = GetImageHeight(imageData.Length, imageWidth);

            var data = new byte[imageWidth * imageHeight * 4];

            var tempValues = new byte[BytesPerPixel];

            var outputIndex = 0;
            for (var i = 0; i < imageWidth * imageHeight * BytesPerPixel; i += BytesPerPixel) 
            {
                for (var j = 0; j < BytesPerPixel; j++)
                {
                    tempValues[j] = imageData[i + j];
                }

                byte[] resultPixels = ConvertPixelToBGRA8888(tempValues);

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

        protected byte[] GetBitmapOriginalFormat(Bitmap bmp)
        {
            var data = new byte[bmp.Width * bmp.Height * BytesPerPixel];

            var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
                        
            var tempValues = new byte[4];

            var outputIndex = 0;
            unsafe
            {
                var imageData = (byte *)bmpData.Scan0.ToPointer();

                for (var i = 0; i < bmp.Width * bmp.Height * 4; i += 4)
                {
                    for (var j = 0; j < 4; j++)
                    {
                        tempValues[j] = imageData[i + j];
                    }

                    byte[] resultPixels = ConvertPixelToOriginalFormat(tempValues);

                    for (var j = 0; j < resultPixels.Length; j++)
                    {
                        data[outputIndex + j] = resultPixels[j];
                    }

                    outputIndex += resultPixels.Length;
                }
            }

            bmp.UnlockBits(bmpData);
            return data;
        }

        private int GetImageHeight(int fileLength, int imageWidth)
        {
            return fileLength / (imageWidth * BytesPerPixel);
        }

        protected abstract byte[] ConvertPixelToBGRA8888(byte[] inputValues);
        protected abstract byte[] ConvertPixelToOriginalFormat(byte[] inputValues);
    }
}
