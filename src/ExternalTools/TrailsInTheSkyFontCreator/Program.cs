using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Cyotek.Drawing.BitmapFont;
using McMaster.Extensions.CommandLineUtils;
using Image = System.Drawing.Image;

namespace TrailsInTheSkyFontCreator
{
    class Program
    {
        public static void Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        [Argument(0, "bmfc", "Fichero .bmfc")]
        [Required]
        public string ConfigFile { get; }

        [Argument(1, "input", "Carpeta con los ficheros originales")]
        [Required]
        public string InputPath { get; }

        [Argument(2, "output", "Carpeta de salida")]
        [Required]
        public string OutputPath { get; }

        private int[] FontSizes = {8, 12, 16, 18, 20, 24, 26, 30, 32, 36, 40, 44, 48, 50, 54, 60, 64, 72, 80, 96, 128, 144, 160, 192};

        private static string GenerateConfigFile(string originalConfig, int fontSize)
        {
            var resultPath = Path.GetTempFileName();
            var lines = File.ReadAllLines(originalConfig);
            for (var i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("fontSize"))
                {
                    lines[i] = $"fontSize={fontSize}";
                }
            }
            File.WriteAllLines(resultPath, lines);

            return resultPath;
        }

        private static void GenerateFont(string configFile, string outputFile)
        {
            using (var process = new System.Diagnostics.Process())
            {
                process.StartInfo.FileName = @"bmfont64.exe";
                process.StartInfo.Arguments = $"-c \"{configFile}\" -o \"{outputFile}\"";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                process.StartInfo.CreateNoWindow = true; //not diplay a windows
                process.Start();
                process.WaitForExit();
            }
        }

        private static IList<byte[]> ReadGameFont(string inputPath, int size)
        {
            var fileName = $"FONT{size}";
            var limit = fileName.Length;
            for (var i = 0; i < 8 - limit; i++)
            {
                fileName = string.Concat(fileName, " ");
            }
            fileName = string.Concat(fileName, "._DA");
            var file = Path.Combine(inputPath, fileName);

            var chars = new List<byte[]>();

            using (var br = new BinaryReader(new FileStream(file, FileMode.Open)))
            {
                var i = 0x20;
                var currentPos = 0;
                while (br.BaseStream.Position < br.BaseStream.Length)
                {
                    var width = (int)Math.Ceiling(size / 2.0d);
                    if (i < 0xE0)
                    {
                        width = (int)Math.Ceiling(size / 4.0d);
                    }

                    br.BaseStream.Seek(currentPos, SeekOrigin.Begin);
                    var chrBytes = br.ReadBytes(width * size);
                    chars.Add(chrBytes);
                    currentPos += width * size;
                    i++;
                }
            }

            return chars;
        }
        private static void Clean(string configFile, string outputPath)
        {
            var files = Directory.GetFiles(outputPath, "FuenteTrails*.*");
            foreach (var file in files)
            {
                File.Delete(file);
            }
            File.Delete(configFile);
        }

        private void OnExecute()
        {
            foreach (var size in FontSizes)
            {
                var originalChars = ReadGameFont(InputPath, size);
                var newChars = new List<byte[]>();
                var newChars2 = new List<byte[]>();

                var configFilePath = GenerateConfigFile(ConfigFile, size);
                var fontFilePath = Path.Combine(OutputPath, "FuenteTrails.fnt");

                GenerateFont(configFilePath, fontFilePath);

                var font = BitmapFontLoader.LoadFontFromFile(fontFilePath);
                var textures = font.Pages.ToDictionary(page => page.Id, page => Image.FromFile(page.FileName));

                var width = (int) Math.Ceiling(size / 2.0d);

                for (var i = 0x21; i < 0x7F; i++)
                {
                    var chr = Convert.ToChar(i);
                    if (font.Characters.ContainsKey(chr))
                    {
                        newChars.Add(DrawCharacter(font[chr], textures[font[chr].TexturePage], size, width));
                    }
                }

                for (var i = 0xA1; i < 0x100; i++)
                {
                    var chr = Convert.ToChar(i);
                    if (font.Characters.ContainsKey(chr))
                    {
                        newChars2.Add(DrawCharacter(font[chr], textures[font[chr].TexturePage], size, width));
                    }
                }

                var fileName = $"FONT{size}";
                var limit = fileName.Length;
                for (var i = 0; i < 8 - limit; i++)
                {
                    fileName = string.Concat(fileName, " ");
                }
                fileName = string.Concat(fileName, "._DA");
                var file = Path.Combine(OutputPath, fileName);

                using (var bw = new BinaryWriter(new FileStream(file, FileMode.Create)))
                {
                    bw.Write(originalChars[0]); // El espacio

                    for (var i = 0x21; i < 0x7F; i++)
                    {
                        bw.Write(newChars[i - 0x21]);
                    }

                    for (var i = 0x7F; i < 0xA1; i++)
                    {
                        bw.Write(originalChars[i - 0x20]);
                    }

                    for (var i = 0xA1; i < 0x100; i++)
                    {
                        bw.Write(newChars2[i - 0xA1]);
                    }

                    for (var i = 0x100; i < originalChars.Count + 0x20; i++)
                    {
                        bw.Write(originalChars[i - 0x20]);
                    }
                }

                foreach (var texture in textures)
                {
                    texture.Value.Dispose();
                }
                Clean(configFilePath, OutputPath);
            }

            Exit();
        }

        private static byte[] DrawCharacter(Character character, Image texture, int outputHeight, int outputWidth)
        {
            var width = (int)Math.Ceiling(character.Bounds.Width / 1.66);
            var height = character.Bounds.Height;

            var dest = new RectangleF(character.Offset.X, character.Offset.Y, width, height);

            var bmp = new Bitmap(outputWidth, outputHeight, PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(bmp))
            {
                g.DrawImage(texture, dest, character.Bounds, GraphicsUnit.Pixel);
            }
            
            var grayScale = To4BppArray(bmp);
            
            return grayScale;
        }

        private static byte[] To4BppArray(Bitmap bmp)
        {
            var outputHeight = bmp.Height;
            var outputWidth = (int) Math.Ceiling(bmp.Width / 2.0);

            var bytes = new byte[outputWidth * outputHeight];

            for (var y = 0; y < bmp.Height; y++)
            {
                var col = 0;
                for (var x = 0; x < bmp.Width; x = x + 2)
                {
                    var pixel1 = bmp.GetPixel(x, y);
                    var value1 = (byte)((pixel1.R + pixel1.G + pixel1.B) / 3);
                    value1 = (byte) (value1 >> 4);

                    byte value2;
                    if (x + 1 < bmp.Width)
                    {
                        var pixel2 = bmp.GetPixel(x + 1, y);
                        value2 = (byte) ((pixel2.R + pixel2.G + pixel2.B) / 3);
                        value2 = (byte) (value2 >> 4);
                    }
                    else
                    {
                        value2 = 0;
                    }

                    bytes[y * outputWidth + col] = (byte) ((value1 << 4) | value2);
                    col++;
                }
            }

            return bytes;
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
