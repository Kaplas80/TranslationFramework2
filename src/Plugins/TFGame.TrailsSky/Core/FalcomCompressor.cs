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

        private class Flag
        {
            private readonly ExtendedBinaryReader _input;
            private ushort _currentValue;
            private byte _remaining;

            public Flag(ExtendedBinaryReader input)
            {
                _input = input;

                _currentValue = (ushort)(input.ReadUInt16() >> 8);
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

                var flag = new Flag(input);

                var finished = false;

                while (!finished)
                {
                    if (input.Position >= startPos + chunkSize)
                    {
                        throw new Exception(); // El fichero está corrupto
                    }

                    var type = flag.GetFlagType();

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
                            var copyCount = flag.GetCopyCount();

                            CopyBytes(output, copyDistance, copyCount);
                        }
                        break;

                        case FlagType.LongJump:
                        {
                            var highDistance = flag.GetValue(5);
                            var lowDistance = input.ReadByte();

                            if (highDistance != 0 || lowDistance > 2)
                            {
                                var copyDistance = (highDistance << 8) | lowDistance;
                                var copyCount = flag.GetCopyCount();

                                CopyBytes(output, copyDistance, copyCount);
                            }
                            else if (lowDistance == 0)
                            {
                                finished = true;
                            }
                            else
                            {
                                var branch = flag.GetFlag();

                                var count = flag.GetValue(4);

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
            using (var ms = new MemoryStream())
            using (var output = new ExtendedBinaryWriter(ms, Encoding.GetEncoding(1252), Endianness.BigEndian))
            {
                return ms.ToArray();
            }
        }
    }
}
