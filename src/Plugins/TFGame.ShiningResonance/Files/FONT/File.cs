using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using TF.Core.Files;

namespace TFGame.ShiningResonance.Files.FONT
{
    using System.Collections.Generic;

    public class File : ImageFile
    {
        private byte[] _currentFont;

        protected override string Filter => "Ficheros de fuente (*.ufn)|*.ufn";

        public File(string gameName, string path, string changesFolder) : base(gameName, path, changesFolder)
        {
        }

        protected override void FormOnExportImage(string filename)
        {
            System.IO.File.WriteAllBytes(filename, _currentFont);
        }

        protected override void FormOnImportImage(string filename)
        {
            System.IO.File.Copy(filename, ChangesFile, true);

            UpdateFormImage();
        }

        protected override Image GetDrawingImage()
        {
            string source = HasChanges ? ChangesFile : Path;
            _currentFont = System.IO.File.ReadAllBytes(source);

            Bitmap bmp = GetBitmap(_currentFont);

            return bmp;
        }

        private static Bitmap GetBitmap(byte[] imageData)
        {
            IDictionary<int, Character> chars = UfnFileHelper.ReadGameFont(imageData);

            int charHeight = chars.Select(c => c.Value.Height).Max();
            int width = 16 * charHeight;
            int height = (int) Math.Ceiling(chars.Count / 16.0) * charHeight;

            var bmp = new Bitmap(width, height, PixelFormat.Format32bppRgb);
            using (Graphics canvas = Graphics.FromImage(bmp))
            {
                var x = 0;
                var y = 0;
                foreach (KeyValuePair<int, Character> c in chars)
                {
                    Bitmap aux = c.Value.ToBitmap();
                    int xPos = (x * charHeight) + charHeight / 2 - c.Value.Width / 2;
                    canvas.DrawImage(aux,
                        new Rectangle(xPos,
                            y * charHeight,
                            c.Value.Width,
                            c.Value.Height),
                        new Rectangle(0,
                            0,
                            c.Value.Width,
                            c.Value.Height),
                        GraphicsUnit.Pixel);

                    x++;

                    if (x == 16)
                    {
                        x = 0;
                        y++;
                    }
                }

                canvas.Save();
            }

            
            return bmp;
        }
    }
}
