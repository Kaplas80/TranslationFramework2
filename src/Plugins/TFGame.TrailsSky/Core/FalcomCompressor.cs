using System;
using System.IO;
using System.Text;
using TF.IO;

namespace TFGame.TrailsSky
{
    class FalcomCompressor
    {
        private class Status
        {
            public ushort Flag;
            public byte FlagPos;
        }

        private static bool GetFlag(ExtendedBinaryReader input, Status status)
        {
            if (status.FlagPos == 0x00)
            {
                var data = input.ReadUInt16();
                status.Flag = data;
                status.FlagPos = 0x10;
            }

            var isFlag = (status.Flag & 0x01) == 0x01;

            status.Flag >>= 0x01;
            status.FlagPos--;

            return isFlag;
        }

        private static void SetupRun(int prevBufferPos, ExtendedBinaryReader input, Stream output, Status status)
        {
            var run = 0x02;

            if (!GetFlag(input, status))
            {
                run++;

                if (!GetFlag(input, status))
                {
                    run++;

                    if (!GetFlag(input, status))
                    {
                        run++;

                        if (!GetFlag(input, status))
                        {
                            if (!GetFlag(input, status))
                            {
                                run = input.ReadByte();
                                run = run + 0x0E;
                            }
                            else
                            {
                                run = 0;
                                for (var i = 0; i < 3; i++)
                                {
                                    run <<= 1;

                                    if (GetFlag(input, status))
                                    {
                                        run |= 1;
                                    }
                                }

                                run += 0x06;
                            }
                        }
                    }
                }
            }

            for (var i = 0; i < run; i++)
            {
                output.Seek(-prevBufferPos, SeekOrigin.End);
                var data = (byte)output.ReadByte();
                output.Seek(0, SeekOrigin.End);

                output.WriteByte(data);
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

                } while (endByte != 0 && input.Position < input.Length);

                return output.ToArray();
            }
        }

        private static byte[] DecompressChunk(ExtendedBinaryReader input, ushort chunkSize)
        { 
            using (var output = new MemoryStream())
            {
                var flag = (ushort)(input.ReadUInt16() >> 8);
                var status = new Status { Flag = flag, FlagPos = 8 };

                var startPos = input.Position;

                while (true)
                {
                    if (input.Position >= startPos + chunkSize)
                    {
                        throw new Exception(); // El fichero está corrupto
                    }

                    if (GetFlag(input, status))
                    {
                        if (GetFlag(input, status))
                        {
                            var prevBufferPos = 0;
                            var run = 0;

                            for (var i = 0; i < 5; i++)
                            {
                                run <<= 0x01;
                                if (GetFlag(input, status))
                                {
                                    run |= 0x01;
                                }
                            }

                            prevBufferPos = input.ReadByte();

                            if (run != 0x00)
                            {
                                prevBufferPos |= (run << 8);
                                SetupRun(prevBufferPos, input, output, status);
                            }
                            else if (prevBufferPos > 2)
                            {
                                SetupRun(prevBufferPos, input, output, status);
                            }
                            else if (prevBufferPos == 0)
                            {
                                break;
                            }
                            else
                            {
                                var branch = GetFlag(input, status);

                                for (var i = 0; i < 4; i++)
                                {
                                    run <<= 0x01;
                                    if (GetFlag(input, status))
                                    {
                                        run |= 0x01;
                                    }
                                }

                                if (branch)
                                {
                                    run <<= 0x08;
                                    run |= input.ReadByte();
                                }

                                run += 0x0E;

                                var b = input.ReadByte();

                                for (var i = 0; i < run; i++)
                                {
                                    output.WriteByte(b);
                                }
                            }
                        }
                        else
                        {
                            var prevBufferPos = input.ReadByte();
                            SetupRun(prevBufferPos, input, output, status);
                        }
                    }
                    else
                    {
                        output.WriteByte(input.ReadByte());
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
                return ms.ToArray();
            }
        }
    }
}
