using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TF.IO;

namespace TFGame.YakuzaKiwami2.Files.Armp
{
    public class Row
    {
        public string Id { get; set; }
        public bool Unknown1 { get; set; }
        public int Unknown4 { get; set; }
        public byte Unknown8 { get; set; }
        public string Translation { get; set; }

        public string[] Data { get; set; }
    }

    public class Column
    {
        public string Name { get; set; }
        public byte Type { get; set; }
        public int ContentOffset { get; set; }
        public int Unknown5 { get; set; }
        public bool Unknown6 { get; set; }
        public byte Unknown7 { get; set; }
    }

    public class Table
    {
        public Row[] Rows { get; private set; }
        public Column[] Columns { get; private set; }

        public static Table ReadTable(ExtendedBinaryReader input, int tableOffset)
        {
            var result = new Table();

            input.Seek(tableOffset, SeekOrigin.Begin);

            var rowCount = input.ReadInt32();
            var columnCount = input.ReadInt32();
            var textCount = input.ReadInt32();

            var zero = input.ReadInt32();
            Debug.Assert(zero == 0);

            var ptrId = input.ReadInt32();
            Debug.Assert(ptrId > 0);
            var ids = ReadStrings(input, ptrId, rowCount, System.Text.Encoding.ASCII);
            var ptrIdBitMask = input.ReadInt32();
            var bitmask = ReadBitmask(input, ptrIdBitMask, rowCount);

            var ptrByteList = input.ReadInt32();
            var byteList = ReadBytes(input, ptrByteList, columnCount); // ¿Tipo de datos de cada columna?

            var ptrIntList = input.ReadInt32();
            var intList = ReadInts(input, ptrIntList, columnCount); // ¿Offset del contenido de cada columna?

            zero = input.ReadInt32();
            Debug.Assert(zero == 0);

            var ptrTexts = input.ReadInt32();

            if (textCount > 0)
            {
                var texts = ReadStrings(input, ptrTexts, textCount);
            }

            var ptrHeaders = input.ReadInt32();
            var headers = ReadStrings(input, ptrHeaders, columnCount, System.Text.Encoding.ASCII);

            zero = input.ReadInt32();
            Debug.Assert(zero == 0);

            var ptrUnknown4 = input.ReadInt32();
            var unknown4 = ReadInts(input, ptrUnknown4, rowCount);

            var ptrUnknown5 = input.ReadInt32();
            var unknown5 = ReadInts(input, ptrUnknown5, columnCount); // ¿Indice de la columna?

            var ptrColumnBitMask = input.ReadInt32();
            var columnBitmask = ReadBitmask(input, ptrColumnBitMask, columnCount);

            zero = input.ReadInt32();
            Debug.Assert(zero == 0);
            zero = input.ReadInt32();
            Debug.Assert(zero == 0);
            zero = input.ReadInt32();
            Debug.Assert(zero == 0);

            var ptrByteList2 = input.ReadInt32();
            var byteList2 = ReadBytes(input, ptrByteList2, columnCount); // ¿?

            var ptrUnknown8 = input.ReadInt32();
            var unknown8 = ReadBytes(input, ptrUnknown8, rowCount);

            result.Columns = new Column[columnCount];
            for (var i = 0; i < columnCount; i++)
            {
                var col = new Column
                {
                    Name = headers[i],
                    Type = byteList[i],
                    ContentOffset = intList[i],
                    Unknown5 = unknown5[i],
                    Unknown6 = columnBitmask[i],
                    Unknown7 = byteList2[i]
                };

                result.Columns[i] = col;
            }

            result.Rows = new Row[rowCount];
            for (var i = 0; i < rowCount; i++)
            {
                var row = new Row
                {
                    Id = ids[i],
                    Unknown1 = bitmask[i],
                    Unknown4 = unknown4[i],
                    Unknown8 = unknown8[i],
                    Data = new string[columnCount]
                };

                result.Rows[i] = row;
            }

            ReadData(input, result);

            return result;
        }

        private static void ReadData(ExtendedBinaryReader input, Table table)
        {
            
        }

        private static string[] ReadStrings(ExtendedBinaryReader input, int tableOffset, int itemCount, System.Text.Encoding encoding = null)
        {
            var returnOffset = input.Position;
            var result = new string[itemCount];

            input.Seek(tableOffset, SeekOrigin.Begin);
            var offsets = new int[itemCount];

            for (var i = 0; i < itemCount; i++)
            {
                offsets[i] = input.ReadInt32();
            }

            for (var i = 0; i < itemCount; i++)
            {
                input.Seek(offsets[i], SeekOrigin.Begin);
                if (encoding == null)
                {
                    result[i] = input.ReadString();
                }
                else
                {
                    result[i] = input.ReadString(encoding);
                }
            }

            input.Seek(returnOffset, SeekOrigin.Begin);
            return result;
        }

        private static bool[] ReadBitmask(ExtendedBinaryReader input, int tableOffset, int itemCount)
        {
            var returnOffset = input.Position;
            var result = new bool[itemCount];

            if (itemCount == 0)
            {
                return result;
            }
            input.Seek(tableOffset, SeekOrigin.Begin);

            
            var b = input.ReadByte();
            var bitsLeft = 8;
            var i = 0;
            while (i < itemCount)
            {
                result[i] = (b & 0x01) == 0x01;
                
                b >>= 1;
                bitsLeft--;

                if (bitsLeft == 0)
                {
                    b = input.ReadByte();
                    bitsLeft = 8;
                }
                i++;
            }

            input.Seek(returnOffset, SeekOrigin.Begin);
            return result;
        }

        private static byte[] ReadBytes(ExtendedBinaryReader input, int tableOffset, int itemCount)
        {
            var returnOffset = input.Position;
            var result = new byte[itemCount];

            if (itemCount == 0)
            {
                return result;
            }
            input.Seek(tableOffset, SeekOrigin.Begin);


            for (var i = 0; i < itemCount; i++)
            {
                result[i] = input.ReadByte();
            }

            input.Seek(returnOffset, SeekOrigin.Begin);
            return result;
        }

        private static int[] ReadInts(ExtendedBinaryReader input, int tableOffset, int itemCount)
        {
            var returnOffset = input.Position;
            var result = new int[itemCount];

            if (itemCount == 0)
            {
                return result;
            }
            input.Seek(tableOffset, SeekOrigin.Begin);


            for (var i = 0; i < itemCount; i++)
            {
                result[i] = input.ReadByte();
            }

            input.Seek(returnOffset, SeekOrigin.Begin);
            return result;
        }
    }
}
