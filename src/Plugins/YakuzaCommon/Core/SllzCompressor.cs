using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TF.IO;

namespace YakuzaCommon.Core
{
    class SllzCompressor
    {
        private const int SEARCH_SIZE = 4096;
        private const int MAX_LENGTH = 18;

        private class SllzItem
        {
            public bool IsLiteral;
            public byte Literal;
            public short CopyFlags;
        }

        private class MatchResult
        {
            public bool Found;
            public int Distance;
            public int Length;
        }

        public static byte[] Decompress(byte[] compressedData)
        {
            using (var input = new ExtendedBinaryReader(new MemoryStream(compressedData), Encoding.GetEncoding(1252), Endianness.BigEndian))
            using (var output = new MemoryStream())
            {
                var magic = input.ReadUInt32();
                var endianness = input.ReadByte();

                input.Endianness = endianness == 0 ? Endianness.LittleEndian : Endianness.BigEndian;

                var version = input.ReadByte();
                var headerSize = input.ReadUInt16();

                var uncompressedSize = input.ReadUInt32();
                var compressedSize = input.ReadUInt32();

                compressedSize -= 16; // compressed size includes SLLZ header

                var block = new byte[18];
                long compressedCount = 0;
                long uncompressedCount = 0;

                var opFlags = input.ReadByte();
                compressedCount++;
                var opBits = 8;

                var literalCount = 0;
                while (compressedCount < compressedSize)
                {
                    var isCopy = (opFlags & 0x80) != 0;
                    opFlags <<= 1;
                    opBits--;

                    if (opBits == 0)
                    {
                        if (literalCount > 0)
                        {
                            input.Read(block, 0, literalCount);
                            output.Write(block, 0, literalCount);
                            uncompressedCount += literalCount;
                            literalCount = 0;
                        }

                        opFlags = input.ReadByte();
                        compressedCount++;
                        opBits = 8;
                    }

                    if (isCopy == false)
                    {
                        literalCount++;
                        compressedCount++;
                        continue;
                    }

                    if (literalCount > 0)
                    {
                        input.Read(block, 0, literalCount);
                        output.Write(block, 0, literalCount);
                        uncompressedCount += literalCount;
                        literalCount = 0;
                    }

                    var copyFlags = (ushort) (input.ReadByte() | input.ReadByte() << 8);
                    compressedCount += 2;

                    var copyDistance = 1 + (copyFlags >> 4);
                    var copyCount = 3 + (copyFlags & 0xF);

                    var originalPosition = output.Position;
                    output.Position = output.Length - copyDistance;
                    output.Read(block, 0, copyCount);
                    output.Position = originalPosition;
                    output.Write(block, 0, copyCount);
                    uncompressedCount += copyCount;
                }

                if (literalCount > 0)
                {
                    input.Read(block, 0, literalCount);
                    output.Write(block, 0, literalCount);
                    uncompressedCount += literalCount;
                }

                return output.ToArray();
            }
        }

        public static byte[] Compress(byte[] uncompressedData)
        {
            using (var ms = new MemoryStream())
            using (var output = new ExtendedBinaryWriter(ms, Encoding.GetEncoding(1252), Endianness.BigEndian))
            {
                output.Write((uint)0x534c4c5a);
                output.Write((byte)0x00);
                output.Write((byte)0x01);

                output.Endianness = Endianness.LittleEndian;

                output.Write((ushort)0x0010);
                output.Write((uint)uncompressedData.Length);
                output.Write((uint)0);

                var uncompressedSize = uncompressedData.Length;
                var currentPosition = 0;

                byte flag = 0;
                var flagCount = 0;

                var queue = new Queue<SllzItem>();
                var first = true;

                while (currentPosition < uncompressedSize)
                {
                    var scanPos = Math.Max(currentPosition - SEARCH_SIZE, 0);
                    var match = FindMatch(uncompressedData, scanPos, currentPosition);

                    if (!match.Found)
                    {
                        var item = new SllzItem
                        {
                            IsLiteral = true,
                            Literal = uncompressedData[currentPosition],
                            CopyFlags = 0
                        };

                        queue.Enqueue(item);
                    }
                    else
                    {
                        flag |= (byte)(1 << (7 - flagCount));

                        var copyCount = (short)((match.Length - 3) & 0x0F);
                        var copyDistance = (short)((match.Distance - 1) << 4);
                        var tuple = (short)(copyDistance | copyCount);

                        var item = new SllzItem
                        {
                            IsLiteral = false,
                            Literal = 0,
                            CopyFlags = tuple
                        };

                        queue.Enqueue(item);
                    }

                    currentPosition += match.Length;
                    flagCount++;

                    if (flagCount == 8)
                    {
                        // Escribir flag
                        output.Write(flag);

                        // Escribir contenido acumulado
                        var max = 8;

                        if (first)
                        {
                            max = 7;
                            first = false;
                        }

                        for (var i = 0; i < max; i++)
                        {
                            var item = queue.Dequeue();
                            if (item.IsLiteral)
                            {
                                output.Write(item.Literal);
                            }
                            else
                            {
                                output.Write(item.CopyFlags);
                            }
                        }

                        flag = 0;
                        flagCount = 0;
                    }
                }

                if (queue.Count > 0)
                {
                    output.Write(flag);
                    while (queue.Count > 0)
                    {
                        var item = queue.Dequeue();
                        if (item.IsLiteral)
                        {
                            output.Write(item.Literal);
                        }
                        else
                        {
                            output.Write(item.CopyFlags);
                        }
                    }
                }

                var length = output.Length;
                output.Seek(0x0C, SeekOrigin.Begin);
                output.Write((uint)length);
                return ms.ToArray();
            }
        }

        private static MatchResult FindMatch(byte[] input, int startPos, int readPos)
        {
            var bestPos = 0;
            var bestLength = 1;

            var current = readPos - 1;
            while (current >= startPos)
            {
                if (input[current] == input[readPos])
                {
                    var maxLength = Math.Min(input.Length - readPos, MAX_LENGTH);
                    maxLength = Math.Min(maxLength, readPos - current);
                    var length = DataCompare(input, current + 1, readPos + 1, maxLength);
                    if (length > bestLength)
                    {
                        bestLength = length;
                        bestPos = current;
                    }
                }

                current--;
            }

            var result = new MatchResult();

            if (bestLength >= 3)
            {
                result.Found = true;
                result.Distance = readPos - bestPos;
                result.Length = bestLength;
            }
            else
            {
                result.Found = false;
                result.Distance = 0;
                result.Length = 1;
            }

            return result;
        }

        private static int DataCompare(byte[] input, int pos1, int pos2, int maxLength)
        {
            var length = 1;

            while (length < maxLength && input[pos1] == input[pos2])
            {
                pos1++;
                pos2++;
                length++;
            }

            return length;
        }
    }
}
