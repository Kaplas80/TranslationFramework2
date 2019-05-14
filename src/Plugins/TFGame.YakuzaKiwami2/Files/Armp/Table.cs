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
    public class IdData
    {
        public string Name { get; set; }
        public bool Unknown1 { get; set; }
        public int Unknown4 { get; set; }
        public byte Unknown8 { get; set; }
    }

    public class Table
    {
        public IdData[] Id { get; private set; }
        public static Table ReadTable(ExtendedBinaryReader input, int tableOffset)
        {
            var result = new Table();

            input.Seek(tableOffset, SeekOrigin.Begin);

            var idCount = input.ReadInt32();
            var columnCount = input.ReadInt32();
            var itemCount3 = input.ReadInt32();

            var zero = input.ReadInt32();
            Debug.Assert(zero == 0);

            var ptrId = input.ReadInt32();
            Debug.Assert(ptrId > 0);
            var ids = ReadStrings(input, ptrId, idCount);
            var ptrIdBitMask = input.ReadInt32();
            var bitmask = ReadBitmask(input, ptrIdBitMask, idCount);

            return result;
        }

        private static string[] ReadStrings(ExtendedBinaryReader input, int tableOffset, int itemCount)
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
                result[i] = input.ReadString(System.Text.Encoding.ASCII);
            }

            input.Seek(returnOffset, SeekOrigin.Begin);
            return result;
        }

        private static bool[] ReadBitmask(ExtendedBinaryReader input, int tableOffset, int itemCount)
        {
            var returnOffset = input.Position;
            var result = new bool[itemCount];

            input.Seek(tableOffset, SeekOrigin.Begin);

            var numBytes = (int) Math.Ceiling(itemCount / 8.0);
            
            for (var i = 0; i < numBytes; i++)
            {
                
            }

            input.Seek(returnOffset, SeekOrigin.Begin);
            return result;
        }
    }
}
