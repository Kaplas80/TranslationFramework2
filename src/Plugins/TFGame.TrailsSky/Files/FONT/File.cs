using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using TF.Core.Files;

namespace TFGame.TrailsSky.Files.FONT
{
    public class File : ImageFile
    {
        private byte[] _currentFont;
        private int _charHeight;

        protected override string Filter => "Ficheros de fuente (FONT*._DA)|FONT*._DA";

        public File(string path, string changesFolder) : base(path, changesFolder)
        {
            var fileName = System.IO.Path.GetFileNameWithoutExtension(path);
            var match = Regex.Match(fileName, @"FONT(?<height>[\d]+)");

            if (!match.Success)
            {
                throw new FormatException("El nombre del fichero debe incluir el tamaño de la fuente");
            }

            _charHeight = int.Parse(match.Groups["height"].Value);
        }

        protected override void FormOnSaveImage(string filename)
        {
            System.IO.File.WriteAllBytes(filename, _currentFont);
        }

        protected override void FormOnNewImageLoaded(string filename)
        {
            System.IO.File.Copy(filename, ChangesFile, true);

            UpdateFormImage();
        }

        protected override Tuple<Image, object> GetImage()
        {
            var source = HasChanges ? ChangesFile : Path;
            _currentFont = System.IO.File.ReadAllBytes(source);

            var halfSizeWidth = (int)Math.Ceiling(_charHeight / 4.0d);
            var halfSizeHeight = _charHeight * 0xC0;

            var bmpHalfSize = GetBitmap(halfSizeWidth, halfSizeHeight, _currentFont, 0);

            var fullSizeWidth = (int)Math.Ceiling(_charHeight / 2.0d);
            var fullSizeHeight = (_currentFont.Length - (halfSizeWidth * halfSizeHeight)) / fullSizeWidth;

            var bmpFullSize = GetBitmap(fullSizeWidth, fullSizeHeight, _currentFont, halfSizeWidth * halfSizeHeight);

            var fullWidth = (int)Math.Ceiling(_charHeight / 2.0d);
            var fullHeight = halfSizeHeight + fullSizeHeight;
            var bitmap = MergeBitmaps(bmpHalfSize, bmpFullSize, fullWidth, fullHeight);

            bmpHalfSize.Dispose();
            bmpFullSize.Dispose();

            return new Tuple<Image, object>(bitmap, null);
        }

        private static Bitmap GetBitmap(int width, int height, byte[] imageData, int startOffset)
        {
            var data = new byte[width * height * 4];

            var o = 0;

            for (var i = 0; i < width * height; i++)
            {
                var value = imageData[startOffset + i];

                data[o++] = 255;
                data[o++] = value;
                data[o++] = value;
                data[o++] = value;
            }

            unsafe
            {
                fixed (byte* ptr = data)
                {
                    var image = new Bitmap(width, height, width * 4, PixelFormat.Format32bppArgb, new IntPtr(ptr));
                    return image;
                }
            }
        }

        private static Bitmap MergeBitmaps(Bitmap bmp1, Bitmap bmp2, int width, int height)
        {
            var bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            using (var canvas = Graphics.FromImage(bitmap))
            {
                canvas.InterpolationMode = InterpolationMode.Default;
                canvas.DrawImage(bmp1,
                    new Rectangle(0,
                        0,
                        bmp1.Width,
                        bmp1.Height),
                    new Rectangle(0,
                        0,
                        bmp1.Width,
                        bmp1.Height),
                    GraphicsUnit.Pixel);
                canvas.DrawImage(bmp2,
                    new Rectangle(0,
                        bmp1.Height,
                        bmp2.Width,
                        bmp2.Height),
                    new Rectangle(0,
                        0,
                        bmp2.Width,
                        bmp2.Height),
                    GraphicsUnit.Pixel);
                canvas.Save();
            }

            return bitmap;
        }
    }
}
