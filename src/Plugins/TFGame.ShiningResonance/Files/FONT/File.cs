using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using TF.Core.Files;

namespace TFGame.ShiningResonance.Files.FONT
{
    public class File : ImageFile
    {
        private byte[] _currentFont;

        protected override string Filter => "Ficheros de fuente (*.ufn)|*.ufn";

        public File(string path, string changesFolder) : base(path, changesFolder)
        {
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

            var bmp = GetBitmap(_currentFont);

            return new Tuple<Image, object>(bmp, null);
        }

        private static Bitmap GetBitmap(byte[] imageData)
        {
            var chars = UfnFileHelper.ReadGameFont(imageData);

            var charHeight = chars.Select(c => c.Value.Height).Max();
            var width = 16 * charHeight;
            var height = (int) Math.Ceiling(chars.Count / 16.0) * charHeight;

            var bmp = new Bitmap(width, height, PixelFormat.Format32bppRgb);
            using (var canvas = Graphics.FromImage(bmp))
            {
                var x = 0;
                var y = 0;
                foreach (var c in chars)
                {
                    var aux = c.Value.ToBitmap();
                    var xPos = (x * charHeight) + charHeight / 2 - c.Value.Width / 2;
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
