using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFGame.ShiningResonance.Files.FONT
{
    public class FileHeader
    {
        public int Magic { get; set; }
        public int DataSize { get; set; }
        public int HeaderSize { get; set; }
        public int Unknown1 { get; set; } // 0x10000000
        public int Unknown2 { get; set; } // 0x00000000
        public int DataSize2 { get; set; } // en UFNF es 0 en el resto es el tamaño de la primera estructura de datos
        public int Unknown3 { get; set; } // 0x00000000
        public int Unknown4 { get; set; } // 0x00000000
    }

    public class CharacterTableInfo
    {
        public int StartCharacter { get; set; }
        public short CharacterCount { get; set; }
        public short CharacterHeight { get; set; }
        public long WidthTableOffset { get; set; }
        public long PointerTableOffset { get; set; }
        public long DataOffset { get; set; }
    }

    public class Character
    {
        public int Index { get; set; }
        public byte[] Data { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Bitmap ToBitmap()
        {
            var expandedSize = Width * Height;
            var expandedData = new byte[expandedSize];
            for (var i = 0; i < Data.Length; i++)
            {
                var pixelColor = (byte)((Data[i] & 0x0F) << 4);
                expandedData[2 * i] = pixelColor;

                if (2 * i + 1 < expandedSize)
                {
                    pixelColor = (byte)(Data[i] & 0xF0);
                    expandedData[2 * i + 1] = pixelColor;
                }
            }

            var bmp = new Bitmap(Width, Height, PixelFormat.Format32bppRgb);
            var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);

            var data = new byte[expandedSize * 4];

            var ptr = bmpData.Scan0;

            var x = 0;
            var y = 0;

            foreach (var pixelColor in expandedData)
            {
                data[y * bmpData.Stride + x] = pixelColor;
                data[y * bmpData.Stride + x + 1] = pixelColor;
                data[y * bmpData.Stride + x + 2] = pixelColor;
                data[y * bmpData.Stride + x + 3] = 0;

                x += 4;

                if (x >= bmpData.Stride)
                {
                    y++;
                    x = 0;
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(data, 0, ptr, data.Length);
            bmp.UnlockBits(bmpData);
            return bmp;
        }
    }

    public class UfnFileHelper
    {
        public static IDictionary<int, Character> ReadGameFont(byte[] data)
        {
            var chars = new Dictionary<int, Character>();

            using (var ms = new MemoryStream(data))
            using (var br = new BinaryReader(ms))
            {
                // Cabecera UFNF
                var ufnfHeader = ReadHeader(br);
                var utf8Header = ReadHeader(br);
                var mfntHeader = ReadHeader(br);

                var baseOffset = br.BaseStream.Position;

                var unknown1 = br.ReadInt32(); // Es 0x00000003
                var width = br.ReadInt16();
                var height = br.ReadInt16();
                var unknown2 = br.ReadInt64(); // Es 0x00000003
                br.BaseStream.Seek(0x10, SeekOrigin.Current);

                var characterTableOffset = br.ReadInt64();
                var characterTableCount = br.ReadInt32();
                var characterCount = br.ReadInt32(); // Número total de caracteres

                br.BaseStream.Seek(baseOffset + characterTableOffset, SeekOrigin.Begin);
                var characterTables = new CharacterTableInfo[characterTableCount];
                for (var i = 0; i < characterTableCount; i++)
                {
                    characterTables[i] = ReadCharacterTableInfo(br);
                    if (characterTables[i].CharacterHeight == 0)
                    {
                        characterTables[i].CharacterHeight = height;
                    }
                }

                foreach (var characterTable in characterTables)
                {
                    var currentChars = ReadChars(br, characterTable, baseOffset);

                    foreach (var chr in currentChars)
                    {
                        chars.Add(chr.Index, chr);
                    }
                }
            }

            return chars;
        }

        private static FileHeader ReadHeader(BinaryReader br)
        {
            var header = new FileHeader
            {
                Magic = br.ReadInt32(),
                DataSize = br.ReadInt32(),
                HeaderSize = br.ReadInt32(),
                Unknown1 = br.ReadInt32(),
                Unknown2 = br.ReadInt32(),
                DataSize2 = br.ReadInt32(),
                Unknown3 = br.ReadInt32(),
                Unknown4 = br.ReadInt32()
            };
            return header;
        }

        private static CharacterTableInfo ReadCharacterTableInfo(BinaryReader br)
        {
            var chrTableInfo = new CharacterTableInfo
            {
                StartCharacter = br.ReadInt32(),
                CharacterCount = br.ReadInt16(),
                CharacterHeight = br.ReadInt16(),
                WidthTableOffset = br.ReadInt64(),
                PointerTableOffset = br.ReadInt64(),
                DataOffset = br.ReadInt64()
            };
            return chrTableInfo;
        }

        private static IList<Character> ReadChars(BinaryReader br, CharacterTableInfo characterTable, long baseOffset)
        {
            var result = new List<Character>();

            br.BaseStream.Seek(baseOffset + characterTable.WidthTableOffset, SeekOrigin.Begin);

            var widths = new byte[characterTable.CharacterCount];
            for (var i = 0; i < characterTable.CharacterCount; i++)
            {
                widths[i] = br.ReadByte();
            }

            br.BaseStream.Seek(baseOffset + characterTable.PointerTableOffset, SeekOrigin.Begin);
            var pointers = new int[characterTable.CharacterCount];
            var sizes = new int[characterTable.CharacterCount];
            for (var i = 0; i < characterTable.CharacterCount; i++)
            {
                pointers[i] = br.ReadInt32();
            }

            for (var i = 0; i < characterTable.CharacterCount; i++)
            {
                sizes[i] = (int)Math.Ceiling(widths[i] / 2.0 * characterTable.CharacterHeight);
            }

            //br.BaseStream.Seek(baseOffset + characterTable.DataOffset, SeekOrigin.Begin);
            for (var index = 0; index < pointers.Length; index++)
            {
                br.BaseStream.Seek(baseOffset + characterTable.DataOffset + pointers[index], SeekOrigin.Begin);
                var chr = new Character
                {
                    Index = characterTable.StartCharacter + index,
                    Width = widths[index],
                    Height = characterTable.CharacterHeight,
                    Data = br.ReadBytes(sizes[index])
                };

                result.Add(chr);
            }

            return result;
        }
    }
}
