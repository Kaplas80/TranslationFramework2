using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Cyotek.Drawing.BitmapFont;
using DirectXTexNet;
using McMaster.Extensions.CommandLineUtils;
using Image = System.Drawing.Image;

namespace YakuzaFontCreator
{
    class Program
    {
        public static void Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        [Argument(0, "input", "Fichero .fmt (Solo texto o XML)")]
        [Required]
        public string Input { get; }

        private void OnExecute()
        {
            if (string.IsNullOrEmpty(Input) || !File.Exists(Input))
            {
                Console.WriteLine($"ERROR: No existe el fichero \"{Input}\"");
                Exit();
            }

            BitmapFont font = null;
            try
            {
                font = BitmapFontLoader.LoadFontFromFile(Input);
            }
            catch (InvalidDataException)
            {
                Console.WriteLine($"ERROR: No se reconoce el formato. Asegurate de que es un fichero de texto o XML creado por BMFont");
                Exit();
            }

            var textures = font.Pages.ToDictionary(page => page.Id, page => Image.FromFile(page.FileName));

            var bitmap = new Bitmap(512, 1024);

            var heightFactor = 1.0f;
            /*foreach (var character in font.Characters)
            {
                if (character.Value.Bounds.Height >= 64)
                {
                    heightFactor = Math.Min(heightFactor, 64f / character.Value.Bounds.Height);
                }
            }*/

            var yOffsetCorrection = 0;

            foreach (var character in font.Characters)
            {
                if (character.Value.Offset.Y + character.Value.Bounds.Height > 64)
                {
                    yOffsetCorrection = Math.Min(yOffsetCorrection, 64 - character.Value.Offset.Y - character.Value.Bounds.Height);
                }
            }

            using (var g = Graphics.FromImage(bitmap))
            {
                var x = 0;
                var y = 0;
                for (var row = 0x2; row < 0x8; row++)
                {
                    x = 0;
                    y = (row - 0x2) * 64;
                    for (var col = 0x0; col < 0x10; col++)
                    {
                        var value = row * 0x10 + col;
                        if (value == 0x5C)
                        {
                            value = 0xA5;
                        }

                        if (value == 0x7F)
                        {
                            value = 0xAE;
                        }

                        var chr = Convert.ToChar(value);
                        if (font.Characters.ContainsKey(chr))
                        {
                            var data = font[chr];
                            DrawCharacter(g, data, textures[data.TexturePage], x, y, heightFactor, yOffsetCorrection);
                        }

                        x += 32;
                    }
                }

                for (var row = 0xA; row < 0x10; row++)
                {
                    x = 0;
                    y = (row - 0x2) * 64;
                    for (var col = 0x0; col < 0x10; col++)
                    {
                        var chr = Convert.ToChar(row * 0x10 + col);
                        if (font.Characters.ContainsKey(chr))
                        {
                            var data = font[chr];
                            DrawCharacter(g, data, textures[data.TexturePage], x, y, heightFactor, yOffsetCorrection);
                        }
                        x += 32;
                    }
                }
            }

            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);

                var bytes = ms.ToArray();
                var unmanagedPointer = Marshal.AllocHGlobal(bytes.Length);
                Marshal.Copy(bytes, 0, unmanagedPointer, bytes.Length);
                var inputDds = TexHelper.Instance.LoadFromWICMemory(unmanagedPointer, bytes.Length, WIC_FLAGS.NONE);
                Marshal.FreeHGlobal(unmanagedPointer);

                var dds = inputDds.Compress(DXGI_FORMAT.BC3_UNORM, TEX_COMPRESS_FLAGS.DEFAULT, 0.5f);

                var outputFile = Path.Combine(Path.GetDirectoryName(Input), "hd2_hankaku.dds");
                dds.SaveToDDSFile(DDS_FLAGS.NONE, outputFile);
            }

            Exit();
        }

        private static void DrawCharacter(Graphics g, Character character, Image texture, int x, int y, float heightFactor, int yOffsetCorrection)
        {
            var width = character.Bounds.Width;
            var height = character.Bounds.Height * heightFactor;

            if (width >= 30)
            {
                width = 30;
            }

            /*if (character.Offset.Y - yOffsetCorrection + height >= 62)
            {
                height = 62 - character.Offset.Y + yOffsetCorrection;
            }*/

            var dest = new RectangleF(x + 16 - width / 2, y + character.Offset.Y + yOffsetCorrection, width, height);
            g.DrawImage(texture, dest, character.Bounds, GraphicsUnit.Pixel);
        }

        private static void Exit()
        {
            Console.WriteLine();
            Console.WriteLine("Pulse una tecla para salir...");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}
