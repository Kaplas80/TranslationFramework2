﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using TF.Core.Files;

namespace TFGame.TrailsSky.Files.FONT
{
    using System.IO;

    public class File : ImageFile
    {
        private byte[] _currentFont;
        private int _charHeight;

        protected override string Filter => "Ficheros de fuente (FONT*._DA)|FONT*._DA";

        public File(string gameName, string path, string changesFolder) : base(gameName, path, changesFolder)
        {
            string fileName = System.IO.Path.GetFileNameWithoutExtension(path);
            Match match = Regex.Match(fileName, @"FONT(?<height>[\d]+)");

            if (!match.Success)
            {
                throw new FormatException("El nombre del fichero debe incluir el tamaño de la fuente");
            }

            _charHeight = int.Parse(match.Groups["height"].Value);
        }

        public override void ExportImage(string path)
        {
            string directory = System.IO.Path.GetDirectoryName(path);
            Directory.CreateDirectory(directory);

            System.IO.File.WriteAllBytes(path, _currentFont);
        }

        protected override Image GetDrawingImage()
        {
            string source = HasChanges ? ChangesFile : Path;
            _currentFont = System.IO.File.ReadAllBytes(source);

            var halfSizeWidth = (int)Math.Ceiling(_charHeight / 2.0d);
            int halfSizeHeight = _charHeight * 0xE0;

            Bitmap bmpHalfSize = GetBitmap(halfSizeWidth, halfSizeHeight, _currentFont, 0);

            return bmpHalfSize;
        }

        private static Bitmap GetBitmap(int width, int height, byte[] imageData, int startOffset)
        {
            if (width % 2 == 1)
            {
                width++;
            }
            var bmp = new Bitmap(width, height, PixelFormat.Format32bppRgb);
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);

            int size = Math.Abs(bmpData.Stride) * bmp.Height;
            var data = new byte[size];

            IntPtr ptr = bmpData.Scan0;
            var index = 0;
            var pixel = 0;
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width * 4; x = x + 4)
                {
                    if (pixel == 0)
                    {
                        var pixelColor = (byte)(imageData[startOffset + index] & 0xF0);
                        data[y * bmpData.Stride + x] = pixelColor;
                        data[y * bmpData.Stride + x + 1] = pixelColor;
                        data[y * bmpData.Stride + x + 2] = pixelColor;
                        data[y * bmpData.Stride + x + 3] = 0;
                        pixel = 1;
                    }
                    else
                    {
                        var pixelColor = (byte)((imageData[startOffset + index] & 0x0F) << 4);
                        data[y * bmpData.Stride + x] = pixelColor;
                        data[y * bmpData.Stride + x + 1] = pixelColor;
                        data[y * bmpData.Stride + x + 2] = pixelColor;
                        data[y * bmpData.Stride + x + 3] = 0;
                        pixel = 0;
                        index++;
                    }
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(data, 0, ptr, data.Length);
            bmp.UnlockBits(bmpData);
            return bmp;
        }
    }
}
