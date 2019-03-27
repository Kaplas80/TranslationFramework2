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
                        var width = (int)Math.Ceiling(charHeight / 2.0d);
                        if (i < 0xE0)
                        {
                            width = (int)Math.Ceiling(charHeight / 4.0d);
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
                case 0x26: // É
                {
                    return chars[0xC9];
                }
                case 0x27: // Í
                {
                    return chars[0xCD];
                }
                case 0x3B: // á
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
                case 0x5C: // é
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
                case 0x5E: // í
                {
                    var bytes1 = chars[0x69];
                    var bytes2 = chars[0xB4];
                    var data = new byte[bytes1.Length];

                    var width = height / 4;
                    var j = (height / 4) * width;

                    for (var i = 0; i < bytes1.Length; i++)
                    {
                        if (i < j)
                        {
                            data[i] = bytes2[i];
                        }
                        else
                        {
                            data[i] = bytes1[i];
                        }
                    }

                    return data;
                }
                case 0x5F: // ó
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
                case 0x60: // ú
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

                case 0x7B: // ü
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

                case 0x7D: // ñ
                {
                    var bytes1 = chars[0x6E];
                    var bytes2 = chars[0x7E];

                    var data = new byte[bytes1.Length];

                    var width = height / 4;

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

                case 0x7E: // ¡
                {
                    return chars[0xA1];
                }
                case 0x7F: // ¿
                {
                    return chars[0xBF];
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
    }
}
