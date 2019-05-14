using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ComponentAce.Compression.Libs.zlib;
using TF.IO;

namespace ParTool
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

        private enum FlagType
        {
            Literal,
            Copy,
        }

        private class BitManager
        {
            private readonly ExtendedBinaryReader _input;
            private readonly ExtendedBinaryWriter _output;
            private byte _currentValue;
            private byte _bitCount;

            public bool IsByteChange { get; private set; }

            public BitManager(ExtendedBinaryWriter output)
            {
                _output = output;

                _currentValue = 0x0;
                _bitCount = 0x00;
            }

            public void Flush()
            {
                _output.Write(_currentValue);
                _currentValue = 0x00;
                _bitCount = 0x00;
                IsByteChange = true;
            }

            public void SetFlagType(FlagType value)
            {
                switch (value)
                {
                    case FlagType.Literal:
                        SetFlag(0);
                        break;
                    case FlagType.Copy:
                        SetFlag(1);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }

            private void SetFlag(byte value)
            {
                IsByteChange = false;

                if (value == 0x01)
                {
                    _currentValue |= (byte)(1 << (7 - _bitCount));
                }

                _bitCount++;

                if (_bitCount == 0x08)
                {
                    Flush();
                }
            }
        }

        public static byte[] Decompress(byte[] compressedData)
        {
            using (var input = new ExtendedBinaryReader(new MemoryStream(compressedData), Encoding.GetEncoding(1252), Endianness.BigEndian))
            {
                var magic = input.ReadUInt32(); // 53 4C 4C 5A
                var endianness = input.ReadByte();

                input.Endianness = endianness == 0 ? Endianness.LittleEndian : Endianness.BigEndian;

                var version = input.ReadByte();
                var headerSize = input.ReadUInt16();

                var uncompressedSize = input.ReadUInt32();
                var compressedSize = input.ReadUInt32();

                if (version == 0x01)
                {
                    return DecompressV1(compressedData, uncompressedSize);
                }

                if (version == 0x02)
                {
                    return DecompressV2(compressedData, uncompressedSize);
                }

                return new byte[0];
            }
        }

        private static byte[] DecompressV1(byte[] data, uint uncompressedSize)
        {
            if (uncompressedSize == 0)
            {
                return new byte[0];
            }

            var output = new byte[uncompressedSize];

            var processedBytes = 0;
            var inputPosition = 0x10; // Después de la cabecera del SLLZ
            var outputPosition = 0;

            var flag = data[inputPosition];
            inputPosition++;
            var flagCount = 8;
            do
            {
                if ((flag & 0x80) == 0x80)
                {
                    flag = (byte) (flag << 1);
                    flagCount--;
                    if (flagCount == 0)
                    {
                        flag = data[inputPosition];
                        inputPosition++;
                        flagCount = 8;
                    }

                    var copyFlags = (ushort)(data[inputPosition] | data[inputPosition + 1] << 8);
                    inputPosition += 2;

                    var copyDistance = 1 + (copyFlags >> 4);
                    var copyCount = 3 + (copyFlags & 0xF);

                    var i = 0;
                    do
                    {
                        output[outputPosition] = output[outputPosition - copyDistance];
                        outputPosition++;
                        i++;
                    } while (i < copyCount);

                    processedBytes += copyCount;
                }
                else
                {
                    flag = (byte)(flag << 1);
                    flagCount--;
                    if (flagCount == 0)
                    {
                        flag = data[inputPosition];
                        inputPosition++;
                        flagCount = 8;
                    }

                    output[outputPosition] = data[inputPosition];
                    inputPosition++;
                    outputPosition++;
                    processedBytes++;
                }
            } while (processedBytes < uncompressedSize);
            
            return output;
        }

        private static byte[] DecompressV2(byte[] data, uint uncompressedSize)
        {
            if (uncompressedSize == 0)
            {
                return new byte[0];
            }

            var output = new byte[uncompressedSize];

            var inputPosition = 0x10; // Después de la cabecera del SLLZ
            var outputPosition = 0;


            while (outputPosition < uncompressedSize)
            {
                var compressedChunkSize = ((data[inputPosition] << 16) | (data[inputPosition + 1] << 8) | (data[inputPosition + 2]));
                var uncompressedChunkSize = ((data[inputPosition + 3] << 8) | (data[inputPosition + 4])) + 1;

                var flag = (data[inputPosition] & 0x80) == 0x80;

                if (!flag)
                {
                    var decompressedData = DecompressZlib(data, inputPosition + 5, compressedChunkSize - 5);
                    Array.Copy(decompressedData, 0, output, outputPosition, decompressedData.Length);
                    inputPosition += compressedChunkSize;
                }
                else
                {
                    throw new NotImplementedException("Modo de compresión no soportado");
                    inputPosition += (compressedChunkSize - 0x800000);
                }

                outputPosition += uncompressedChunkSize;
            }
            return output;
        }

        private static byte[] DecompressZlib(byte[] input, int position, int size)
        {
            using (var outputMemoryStream = new MemoryStream())
            using (var zlibStream = new ZOutputStream(outputMemoryStream))
            using (var inputMemoryStream = new MemoryStream(input, position, size))

            {
                inputMemoryStream.CopyTo(zlibStream);
                return outputMemoryStream.ToArray();
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

                var manager = new BitManager(output);

                var queue = new Queue<SllzItem>();
                var itemsToWrite = 7; // La primera vez se escriben 7 elementos, el resto de las veces, 8

                while (currentPosition < uncompressedSize)
                {
                    var match = FindMatch(uncompressedData, currentPosition);

                    if (!match.Found)
                    {
                        var item = new SllzItem
                        {
                            IsLiteral = true,
                            Literal = uncompressedData[currentPosition],
                            CopyFlags = 0
                        };

                        queue.Enqueue(item);

                        manager.SetFlagType(FlagType.Literal);
                    }
                    else
                    {
                        manager.SetFlagType(FlagType.Copy);

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

                    if (manager.IsByteChange)
                    {
                        for (var i = 0; i < itemsToWrite; i++)
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

                        itemsToWrite = 8;
                    }
                }

                manager.Flush();
                
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

                var length = output.Length;
                output.Seek(0x0C, SeekOrigin.Begin);
                output.Write((uint)length);
                return ms.ToArray();
            }
        }

        private static MatchResult FindMatch(byte[] input, int readPos)
        {
            var bestPos = 0;
            var bestLength = 1;

            var current = readPos - 1;

            var startPos = Math.Max(readPos - SEARCH_SIZE, 0);

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
