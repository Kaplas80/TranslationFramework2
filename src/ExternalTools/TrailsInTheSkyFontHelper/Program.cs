using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace TrailsInTheSkyFontHelper
{
    class Program
    {
        public static void Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        [Argument(0, "input", "Directorio con las fuentes")]
        [Required]
        public string Input { get; }

        private void OnExecute()
        {
            if (string.IsNullOrEmpty(Input) || !Directory.Exists(Input))
            {
                Console.WriteLine($"ERROR: No existe el directorio\"{Input}\"");
                Exit();
            }

            var files = Directory.GetFiles(Input, "FONT*._DA");
            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file);
                var match = Regex.Match(fileName, @"FONT(?<height>[\d]+)");

                if (!match.Success)
                {
                    Console.WriteLine($"ERROR: No se puede sacar la altura a partir del nombre del fichero");
                    Exit();
                }

                var charHeight = int.Parse(match.Groups["height"].Value);

                var path = Path.Combine(Path.GetDirectoryName(file), "new");
                Directory.CreateDirectory(path);
                var outputFile = Path.Combine(path, $"{fileName}._DA");

                var chars = new Dictionary<int, byte[]>();

                using (var br = new BinaryReader(new FileStream(file, FileMode.Open)))
                {
                    // Lo primero es extraer todos los caracteres
                    var i = 0x20;
                    var currentPos = 0;
                    while (br.BaseStream.Position < br.BaseStream.Length)
                    {
                        var width = charHeight / 2;
                        if (i < 0xE0)
                        {
                            width = width / 2;
                        }

                        br.BaseStream.Seek(currentPos, SeekOrigin.Begin);
                        var chrBytes = br.ReadBytes(width * charHeight);
                        chars.Add(i, chrBytes);
                        currentPos += width * charHeight;
                        i++;
                    }
                }

                // Y ahora se genera el nuevo fichero
                using (var bw = new BinaryWriter(new FileStream(outputFile, FileMode.Create)))
                {
                    foreach (var c in chars)
                    {
                        var data = ProcessChar(c.Key, chars, charHeight);
                        bw.Write(data, 0, data.Length);
                    }
                }
            }

            Exit();
        }

        private static byte[] ProcessChar(int index, Dictionary<int, byte[]> chars, int height)
        {
            switch (index)
            {
                case 0xA4: // á
                {
                    var bytes1 = chars[0x61];
                    var bytes2 = chars[0xB4];
                    var data = new byte[bytes1.Length];

                    for (var i = 0; i < bytes1.Length; i++)
                    {
                        data[i] = (byte)(bytes1[i] | bytes2[i]);
                    }

                    return data;
                }
                case 0xA6: // é
                {
                    var bytes1 = chars[0x65];
                    var bytes2 = chars[0xB4];
                    var data = new byte[bytes1.Length];

                    for (var i = 0; i < bytes1.Length; i++)
                    {
                        data[i] = (byte) (bytes1[i] | bytes2[i]);
                    }

                    return data;
                }
                case 0xA7: // í
                {
                    var bytes1 = chars[0x69];
                    var bytes2 = chars[0xB4];
                    var data = new byte[bytes1.Length];

                    for (var i = 0; i < bytes1.Length; i++)
                    {
                        data[i] = (byte)(bytes1[i] | bytes2[i]);
                    }

                    return data;
                }
                case 0xA8: // ó
                {
                    var bytes1 = chars[0x6F];
                    var bytes2 = chars[0xB4];
                    var data = new byte[bytes1.Length];

                    for (var i = 0; i < bytes1.Length; i++)
                    {
                        data[i] = (byte)(bytes1[i] | bytes2[i]);
                    }

                    return data;
                }
                case 0xB5: // ú
                {
                    var bytes1 = chars[0x75];
                    var bytes2 = chars[0xB4];
                    var data = new byte[bytes1.Length];

                    for (var i = 0; i < bytes1.Length; i++)
                    {
                        data[i] = (byte)(bytes1[i] | bytes2[i]);
                    }

                    return data;
                }

                case 0xB6: // ü
                {
                    var bytes1 = chars[0x75];
                    var bytes2 = chars[0xA8];
                    var data = new byte[bytes1.Length];

                    for (var i = 0; i < bytes1.Length; i++)
                    {
                        data[i] = (byte)(bytes1[i] | bytes2[i]);
                    }

                    return data;
                }

                case 0xB8: // ñ
                {
                    var bytes1 = chars[0x6E];
                    var bytes2 = chars[0x7E];

                    var data = new byte[bytes1.Length];

                    var width = height / 4;

                    // Hay que dibujar 20 líneas más arriba 80 /4 = 20
                    var j = (height / 4) * width;

                    for (var i = 0; i < bytes1.Length; i++)
                    {
                        if (j >= bytes2.Length)
                        {
                            data[i] = bytes1[i];
                        }
                        else
                        {
                            data[i] = (byte)(bytes1[i] | bytes2[j]);
                        }

                        j++;
                    }

                    return data;
                    }
                default:
                {
                    return chars[index];
                }
            }
        }


        private static void Exit()
        {
            Console.WriteLine();
            Console.WriteLine("Pulse una tecla para salir...");
            Console.ReadKey();
            Environment.Exit(0);
        }

        private static bool NeedReplace(int value)
        {
            if (value == 0xA4)
            {
                return true;
            }

            return false;
        }

        private static Bitmap MergeBitmaps(Bitmap bmp1, Bitmap bmp2)
        {
            var bitmap = new Bitmap(bmp1.Width, bmp1.Height);
            
            using (var canvas = Graphics.FromImage(bitmap))
            {
                canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
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
                canvas.DrawImage(bmp2, 0, -20);
                canvas.Save();
            }

            return bitmap;
        }

        private static Bitmap GetBitmap(int width, int height, byte[] imageData)
        {
            var data = new byte[width * height * 4];

            var o = 0;

            for (var i = 0; i < width * height; i++)
            {
                var value = imageData[i];

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

        private static byte[] GetBytes(Bitmap bmp)
        {
            var rectangle = new Rectangle(0, 0, bmp.Width, bmp.Height);

            var bitmapData = bmp.LockBits(rectangle, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            var length = bitmapData.Stride * bitmapData.Height;

            var bytes = new byte[length];
            // Copy bitmap to byte[]
            Marshal.Copy(bitmapData.Scan0, bytes, 0, length);
            bmp.UnlockBits(bitmapData);

            var result = new byte[bmp.Width * bmp.Height];
            
            var o = 0;

            for (var i = 0; i < bytes.Length; i = i+4)
            {
                var value = bytes[i];

                result[o++] = value;
            }

            return result;
        }
    }
}
