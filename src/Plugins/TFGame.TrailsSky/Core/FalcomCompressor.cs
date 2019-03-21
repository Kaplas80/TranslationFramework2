using System;
using System.IO;
using System.Linq;
using System.Text;
using TF.IO;

namespace TFGame.TrailsSky
{
    // Código adaptado de https://heroesoflegend.org/forums/viewtopic.php?f=38&t=289

    class FalcomCompressor
    {
        private enum FlagType
        {
            Literal,
            ShortJump,
            LongJump
        }

        private class BitManager
        {
            private readonly ExtendedBinaryReader _input;
            private readonly ExtendedBinaryWriter _output;
            private MemoryStream _outputBuffer;
            private ushort _currentValue;
            private byte _remaining;

            public BitManager(ExtendedBinaryReader input)
            {
                _input = input;

                _currentValue = (ushort)(input.ReadUInt16() >> 8);
                _remaining = 0x08;
            }

            public BitManager(ExtendedBinaryWriter output)
            {
                _output = output;
                _outputBuffer = new MemoryStream();

                _currentValue = 0;
                _remaining = 0x08;
            }

            public FlagType GetFlagType()
            {
                var val = GetFlag();

                if (val == 0)
                {
                    return FlagType.Literal;
                }

                val = GetFlag();
                return val == 0 ? FlagType.ShortJump : FlagType.LongJump;
            }


            public int GetFlag()
            {
                if (_remaining == 0x00)
                {
                    var data = _input.ReadUInt16();
                    _currentValue = data;
                    _remaining = 0x10;
                }

                var result = (_currentValue & 0x01);

                _currentValue >>= 0x01;
                _remaining--;

                return result;
            }

            public int GetValue(int count)
            {
                var value = 0;

                for (var i = 0; i < count; i++)
                {
                    value <<= 0x01;
                    value |= GetFlag();
                }

                return value;
            }

            public int GetCopyCount()
            {
                if (GetFlag() == 1)
                {
                    return 0x02;
                }

                if (GetFlag() == 1)
                {
                    return 0x03;
                }

                if (GetFlag() == 1)
                {
                    return 0x04;
                }

                if (GetFlag() == 1)
                {
                    return 0x05;
                }

                if (GetFlag() == 1)
                {
                    return GetValue(3) + 0x06;
                }

                return _input.ReadByte() + 0x0E;
            }

            public void SetFlag(int value)
            {
                if (value == 1)
                {
                    _currentValue |= 0x8000;
                }

                _remaining--;

                if (_remaining == 0)
                {
                    _output.Write(_currentValue);
                    _output.Write(_outputBuffer.ToArray());
                    _outputBuffer.Close();
                    _outputBuffer.Dispose();
                    _outputBuffer = new MemoryStream();

                    _remaining = 0x10;
                    _currentValue = 0x0000;
                }
                else
                {
                    _currentValue >>= 0x01;
                }
            }

            public void Append(byte b)
            {
                _outputBuffer.WriteByte(b);
            }

            public void Flush()
            {
                if (_remaining != 0x10)
                {
                    var x = _remaining;
                    for (var i = 0; i < x; i++)
                    {
                        SetFlag(0);
                    }
                }

                _outputBuffer.Close();
                _outputBuffer.Dispose();
            }
        }

        public static byte[] Decompress(byte[] compressedData)
        {
            using (var input = new ExtendedBinaryReader(new MemoryStream(compressedData)))
            using (var output = new MemoryStream())
            {
                byte endByte;
                do
                {
                    var chunkSize = input.ReadUInt16();
                    var data = DecompressChunk(input, chunkSize);
                    output.Write(data, 0, data.Length);

                    endByte = input.ReadByte();

                } while (endByte != 0);

                return output.ToArray();
            }
        }

        private static byte[] DecompressChunk(ExtendedBinaryReader input, ushort chunkSize)
        { 
            using (var output = new MemoryStream())
            {
                var startPos = input.Position;

                var manager = new BitManager(input);

                var finished = false;

                while (!finished)
                {
                    if (input.Position >= startPos + chunkSize)
                    {
                        throw new Exception(); // El fichero está corrupto
                    }

                    var type = manager.GetFlagType();

                    switch (type)
                    {
                        case FlagType.Literal:
                        {
                            output.WriteByte(input.ReadByte());
                        }
                        break;

                        case FlagType.ShortJump:
                        {
                            var copyDistance = (int) input.ReadByte();
                            var copyCount = manager.GetCopyCount();

                            CopyBytes(output, copyDistance, copyCount);
                        }
                        break;

                        case FlagType.LongJump:
                        {
                            var highDistance = manager.GetValue(5);
                            var lowDistance = input.ReadByte();

                            if (highDistance != 0 || lowDistance > 2)
                            {
                                var copyDistance = (highDistance << 8) | lowDistance;
                                var copyCount = manager.GetCopyCount();

                                CopyBytes(output, copyDistance, copyCount);
                            }
                            else if (lowDistance == 0)
                            {
                                finished = true;
                            }
                            else
                            {
                                var branch = manager.GetFlag();

                                var count = manager.GetValue(4);

                                if (branch == 1)
                                {
                                    count <<= 8;
                                    count |= input.ReadByte();
                                }

                                count += 0x0E;

                                var b = input.ReadByte();
                                var data = Enumerable.Repeat(b, count).ToArray();
                                output.Write(data, 0, count);
                            }
                        }
                        break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                return output.ToArray();
            }
        }

        private static void CopyBytes(Stream s, int distance, int count)
        {
            for (var i = 0; i < count; i++)
            {
                s.Seek(-distance, SeekOrigin.End);
                var data = (byte)s.ReadByte();
                s.Seek(0, SeekOrigin.End);

                s.WriteByte(data);
            }
        }

        public static byte[] Compress(byte[] uncompressedData)
        {
            using (var outputMemoryStream = new MemoryStream())
            using (var output = new ExtendedBinaryWriter(outputMemoryStream))
            {
                var pos = 0;

                while (pos < uncompressedData.Length)
                {
                    var data = CompressChunk(uncompressedData, ref pos);
                    output.Write((ushort) data.Length);
                    output.Write(data);
                    if (pos == uncompressedData.Length)
                    {
                        output.Write((byte) 0);
                    }
                    else
                    {
                        output.Write((byte) 1);
                    }

                }
                return outputMemoryStream.ToArray();
            }
        }

        private static byte[] CompressChunk(byte[] data, ref int pos)
        {
            var CHUNK_SIZE_CMP_MAX = 0x7FC0;
            var CHUNK_SIZE_UNC_MAX = 0x3DFF0;

            using (var outputMemoryStream = new MemoryStream())
            using (var output = new ExtendedBinaryWriter(outputMemoryStream))
            {
                var manager = new BitManager(output);

                while (pos < data.Length)
                {
                    if (output.Length >= CHUNK_SIZE_CMP_MAX || pos >= CHUNK_SIZE_UNC_MAX)
                    {
                        break;
                    }

                    var match = FindMatch(data, pos);
                    var repeat = FindRepeat(data, pos);

                    if (repeat.Length > match.Length)
                    {
                        EncodeRepeat(manager, data[pos], repeat.Length);
                        pos += repeat.Length;
                    }
                    else if (match.Length > 0)
                    {
                        EncodeMatch(manager, match.Length, match.Position);
                        pos += match.Length;
                    }
                    else
                    {
                        manager.Append(data[pos]);
                        pos++;
                        manager.SetFlag(0);
                    }

                }

                manager.SetFlag(1);
                manager.SetFlag(1);

                manager.SetFlag(0);
                manager.SetFlag(0);
                manager.SetFlag(0);
                manager.SetFlag(0);
                manager.Append(0);
                manager.SetFlag(0);

                manager.Flush();

                return outputMemoryStream.ToArray();
            }
        }

        private class MatchInfo
        {
            public int Length;
            public int Position;
        }

        private class RepeatInfo
        {
            public int Length;
        }

        private static MatchInfo FindMatch(byte[] data, int pos)
        {
            var result = new MatchInfo {Position = -1, Length = 0};

            var WINDOW_SIZE = 0x1FFF;
            var MIN_MATCH = 2;
            var maxMatch = 0xFF + 0x0E;

            var bestPos = 0;
            var bestLength = 1;

            var current = pos - 1;

            var startPos = Math.Max(pos - WINDOW_SIZE, 0);

            while (current >= startPos)
            {
                if (data[current] == data[pos])
                {
                    var maxLength = Math.Min(data.Length - pos, maxMatch);
                    maxLength = Math.Min(maxLength, pos - current);
                    var length = DataCompare(data, current + 1, pos + 1, maxLength);
                    if (length > bestLength)
                    {
                        bestLength = length;
                        bestPos = current;
                    }
                }

                current--;
            }

            if (bestLength < MIN_MATCH)
            {
                // No se ha encontrado coincidencia o la longitud es menor que el mínimo
                return result;
            }

            result.Position = pos - bestPos;
            result.Length = bestLength;

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

        private static RepeatInfo FindRepeat(byte[] data, int pos)
        {
            var maxMatch = 0x0FFF + 0x0E;
            var startPos = pos;
            var endPos = Math.Min(data.Length, pos + maxMatch);

            var bestLength = 0;

            var i = startPos;
            while (i < endPos && data[pos] == data[i])
            {
                bestLength++;
                i++;
            }

            var result = new RepeatInfo {Length = bestLength >= 3 ? bestLength : 0};

            return result;
        }

        private static void EncodeRepeat(BitManager manager, byte repeatByte, int size)
        {
            if (size < 0x0E)
            {
                manager.Append(repeatByte);
                manager.SetFlag(0);
                EncodeMatch(manager, size - 1, 1);
            }
            else
            {
                size -= 0x0E;

                manager.SetFlag(1);
                manager.SetFlag(1);
                manager.SetFlag(0);
                manager.SetFlag(0);
                manager.SetFlag(0);
                manager.SetFlag(0);

                manager.Append((byte) 0x01);
                manager.SetFlag(0);

                if (size < 0x10)
                {
                    manager.SetFlag(0);
                    for (var i = 3; i >= 0; i--)
                    {
                        manager.SetFlag((size >> i) & 1);
                        if (i == 1)
                        {
                            manager.Append(repeatByte);
                        }
                    }
                }
                else
                {
                    var high = size >> 8;
                    var low = size & 0xFF;
                    manager.SetFlag(1);

                    for (var i = 3; i >= 0; i--)
                    {
                        manager.SetFlag((high >> i) & 1);
                        if (i == 1)
                        {
                            manager.Append((byte) low);
                            manager.Append(repeatByte);
                        }
                    }
                }
            }
        }

        private static void EncodeMatch(BitManager manager, int matchSize, int matchPos)
        {
            if (matchPos < 0x100)
            {
                manager.SetFlag(1);
                manager.Append((byte) matchPos);
                manager.SetFlag(0);
            }
            else
            {
                var high = matchPos >> 8;
                var low = matchPos & 0xFF;

                manager.SetFlag(1);
                manager.SetFlag(1);
                manager.SetFlag((high >> 4) & 0x01);
                manager.SetFlag((high >> 3) & 0x01);
                manager.SetFlag((high >> 2) & 0x01);
                manager.SetFlag((high >> 1) & 0x01);
                manager.Append((byte)low);
                manager.SetFlag((high >> 0) & 0x01);
            }

            for (var i = 2; i < 5; i++)
            {
                if (i >= matchSize)
                {
                    break;
                }
                manager.SetFlag(0);
            }

            if (matchSize >= 0x06)
            {
                manager.SetFlag(0);
                if (matchSize >= 0x0E)
                {
                    matchSize -= 0x0E;
                    manager.Append((byte) matchSize);
                    manager.SetFlag(0);
                }
                else
                {
                    manager.SetFlag(1);
                    matchSize -= 0x06;
                    manager.SetFlag((matchSize >> 2) & 0x01);
                    manager.SetFlag((matchSize >> 1) & 0x01);
                    manager.SetFlag((matchSize >> 0) & 0x01);
                }
            }
            else
            {
                manager.SetFlag(1);
            }
        }
    }
}
