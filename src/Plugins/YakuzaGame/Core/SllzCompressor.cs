using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TF.IO;

namespace YakuzaGame.Core
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
            private enum WorkMode
            {
                Read,
                Write
            }

            private readonly ExtendedBinaryReader _input;
            private readonly ExtendedBinaryWriter _output;
            private byte _currentValue;
            private byte _bitCount;

            private WorkMode _mode;

            public bool IsByteChange { get; private set; }

            public BitManager(ExtendedBinaryReader input)
            {
                _input = input;

                _currentValue = input.ReadByte();
                _bitCount = 0x08;

                IsByteChange = false;

                _mode = WorkMode.Read;
            }

            public BitManager(ExtendedBinaryWriter output)
            {
                _output = output;

                _currentValue = 0x0;
                _bitCount = 0x00;

                _mode = WorkMode.Write;
            }

            public void Flush()
            {
                if (_mode == WorkMode.Read)
                {
                    throw new NotImplementedException("Esta función no es válida para descomprimir");
                }

                _output.Write(_currentValue);
                _currentValue = 0x00;
                _bitCount = 0x00;
                IsByteChange = true;
            }

            public FlagType GetFlagType()
            {
                if (_mode == WorkMode.Write)
                {
                    throw new NotImplementedException("Esta función no es válida para comprimir");
                }

                var val = GetFlag();

                return val == 0 ? FlagType.Literal : FlagType.Copy;
            }

            public void SetFlagType(FlagType value)
            {
                if (_mode == WorkMode.Read)
                {
                    throw new NotImplementedException("Esta función no es válida para descomprimir");
                }

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

            public Tuple<int, int> GetCopyInfo()
            {
                if (_mode == WorkMode.Write)
                {
                    throw new NotImplementedException("Esta función no es válida para comprimir");
                }

                var copyFlags = (ushort)(_input.ReadByte() | _input.ReadByte() << 8);

                var copyDistance = 1 + (copyFlags >> 4);
                var copyCount = 3 + (copyFlags & 0xF);

                return new Tuple<int, int>(copyDistance, copyCount);
            }

            private int GetFlag()
            {
                if (_mode == WorkMode.Write)
                {
                    throw new NotImplementedException("Esta función no es válida para comprimir");
                }

                IsByteChange = false;

                var result = (_currentValue & 0x80);

                _bitCount--;
                _currentValue <<= 0x01;

                if (_bitCount == 0x00)
                {
                    var data = _input.ReadByte();
                    _currentValue = data;
                    _bitCount = 0x08;
                    IsByteChange = true;
                }

                return result;
            }

            private void SetFlag(byte value)
            {
                if (_mode == WorkMode.Read)
                {
                    throw new NotImplementedException("Esta función no es válida para descomprimir");
                }

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
            using (var output = new MemoryStream())
            {
                var magic = input.ReadUInt32();
                var endianness = input.ReadByte();

                input.Endianness = endianness == 0 ? Endianness.LittleEndian : Endianness.BigEndian;

                var version = input.ReadByte();
                var headerSize = input.ReadUInt16();

                var uncompressedSize = input.ReadUInt32();
                var compressedSize = input.ReadUInt32();

                var block = new byte[18];

                var manager = new BitManager(input);

                while (input.Position < input.Length)
                {
                    var type = manager.GetFlagType();

                    switch (type)
                    {
                        case FlagType.Literal:
                            output.WriteByte(input.ReadByte());

                            break;

                        case FlagType.Copy:
                            var (copyPosition, copyCount) = manager.GetCopyInfo();

                            output.Seek(-copyPosition, SeekOrigin.End);
                            output.Read(block, 0, copyCount);
                            output.Seek(0, SeekOrigin.End);
                            output.Write(block, 0, copyCount);

                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
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
                
                if (queue.Count > 0)
                {
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
