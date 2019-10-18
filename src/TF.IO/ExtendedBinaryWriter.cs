using System;
using System.IO;
using System.Text;

namespace TF.IO
{
    public class ExtendedBinaryWriter : BinaryWriter
    {
        public Endianness Endianness { get; set; }
        public Encoding Encoding { get; }

        private readonly byte[] _buffer;

        #region Constructors

        public ExtendedBinaryWriter(Stream input, Endianness endianness = Endianness.LittleEndian) : base(input)
        {
            Endianness = endianness;
            Encoding = Encoding.UTF8;
            _buffer = new byte[16];
        }

        public ExtendedBinaryWriter(Stream input, Encoding encoding, Endianness endianness = Endianness.LittleEndian) : base(input, encoding)
        {
            Endianness = endianness;
            Encoding = encoding;
            _buffer = new byte[16];
        }

        public ExtendedBinaryWriter(Stream input, Encoding encoding, bool leaveOpen, Endianness endianness = Endianness.LittleEndian) : base(input, encoding, leaveOpen)
        {
            Endianness = endianness;
            Encoding = encoding;
            _buffer = new byte[16];
        }

        #endregion

        #region Stream Functions

        public long Position
        {
            get => BaseStream.Position;
            set => BaseStream.Position = value;
        }

        public long Seek(long offset, SeekOrigin origin)
        {
            return BaseStream.Seek(offset, origin);
        }

        public void Skip(int byteCount)
        {
            Seek(byteCount, SeekOrigin.Current);
        }

        public long Length => BaseStream.Length;

        #endregion

        #region Write Methods
        public override void Write(short value)
        {
            if (Endianness == Endianness.LittleEndian)
            {
                _buffer[0] = (byte) value;
                _buffer[1] = (byte) ((uint) value >> 8);
            }
            else
            {
                _buffer[0] = (byte) ((uint) value >> 8);
                _buffer[1] = (byte) value;
            }
            BaseStream.Write(_buffer, 0, 2);
        }

        public override void Write(int value)
        {
            if (Endianness == Endianness.LittleEndian)
            {
                _buffer[0] = (byte) value;
                _buffer[1] = (byte) (value >> 8);
                _buffer[2] = (byte) (value >> 16);
                _buffer[3] = (byte) (value >> 24);
            }
            else
            {
                _buffer[0] = (byte) (value >> 24);
                _buffer[1] = (byte) (value >> 16);
                _buffer[2] = (byte) (value >> 8);
                _buffer[3] = (byte) value;
            }
            BaseStream.Write(_buffer, 0, 4);
        }

        public override void Write(long value)
        {
            if (Endianness == Endianness.LittleEndian)
            {
                _buffer[0] = (byte) value;
                _buffer[1] = (byte) (value >> 8);
                _buffer[2] = (byte) (value >> 16);
                _buffer[3] = (byte) (value >> 24);
                _buffer[4] = (byte) (value >> 32);
                _buffer[5] = (byte) (value >> 40);
                _buffer[6] = (byte) (value >> 48);
                _buffer[7] = (byte) (value >> 56);
            }
            else
            {
                _buffer[0] = (byte) (value >> 56);
                _buffer[1] = (byte) (value >> 48);
                _buffer[2] = (byte) (value >> 40);
                _buffer[3] = (byte) (value >> 32);
                _buffer[4] = (byte) (value >> 24);
                _buffer[5] = (byte) (value >> 16);
                _buffer[6] = (byte) (value >> 8);
                _buffer[7] = (byte) value;
            }
            BaseStream.Write(_buffer, 0, 8);
        }

        public override void Write(ushort value)
        {
            if (Endianness == Endianness.LittleEndian)
            {
                _buffer[0] = (byte) value;
                _buffer[1] = (byte) ((uint) value >> 8);
            }
            else
            {
                _buffer[0] = (byte) ((uint) value >> 8);
                _buffer[1] = (byte) value;
            }
            BaseStream.Write(_buffer, 0, 2);
        }

        public override void Write(uint value)
        {
            if (Endianness == Endianness.LittleEndian)
            {
                _buffer[0] = (byte) value;
                _buffer[1] = (byte) (value >> 8);
                _buffer[2] = (byte) (value >> 16);
                _buffer[3] = (byte) (value >> 24);
            }
            else
            {
                _buffer[0] = (byte) (value >> 24);
                _buffer[1] = (byte) (value >> 16);
                _buffer[2] = (byte) (value >> 8);
                _buffer[3] = (byte) value;
            }
            BaseStream.Write(_buffer, 0, 4);
        }

        public override void Write(ulong value)
        {
            if (Endianness == Endianness.LittleEndian)
            {
                _buffer[0] = (byte) value;
                _buffer[1] = (byte) (value >> 8);
                _buffer[2] = (byte) (value >> 16);
                _buffer[3] = (byte) (value >> 24);
                _buffer[4] = (byte) (value >> 32);
                _buffer[5] = (byte) (value >> 40);
                _buffer[6] = (byte) (value >> 48);
                _buffer[7] = (byte) (value >> 56);
            }
            else
            {
                _buffer[0] = (byte) (value >> 56);
                _buffer[1] = (byte) (value >> 48);
                _buffer[2] = (byte) (value >> 40);
                _buffer[3] = (byte) (value >> 32);
                _buffer[4] = (byte) (value >> 24);
                _buffer[5] = (byte) (value >> 16);
                _buffer[6] = (byte) (value >> 8);
                _buffer[7] = (byte) value;
            }
            BaseStream.Write(_buffer, 0, 8);
        }
        
        public override unsafe void Write(float value)
        {
            var num = *(uint*) &value;
            if (Endianness == Endianness.LittleEndian)
            {
                _buffer[0] = (byte) num;
                _buffer[1] = (byte) (num >> 8);
                _buffer[2] = (byte) (num >> 16);
                _buffer[3] = (byte) (num >> 24);
            }
            else
            {
                _buffer[0] = (byte) (num >> 24);
                _buffer[1] = (byte) (num >> 16);
                _buffer[2] = (byte) (num >> 8);
                _buffer[3] = (byte) num;
            }
            BaseStream.Write(_buffer, 0, 4);
        }

        public void WriteString(string value, Encoding encoding, bool trailingNull = true, char trailingChar = '\0')
        {
            var input = value;
            if (trailingNull)
            {
                input = string.Concat(input, trailingChar);
            }
            
            var data = encoding.GetBytes(input);
            BaseStream.Write(data, 0, data.Length);
        }

        public void WriteString(string value, bool trailingNull = true, char trailingChar = '\0')
        {
            WriteString(value, Encoding, trailingNull, trailingChar);
        }

        public void WriteString(string value, int minSize, bool trailingNull = true, char trailingChar = '\0')
        {
            var currentPos = Position;
            WriteString(value, Encoding, trailingNull, trailingChar);

            while (Position < currentPos + minSize)
            {
                Write(trailingChar);
            }
        }

        public void WritePadding(int align)
        {
            var value = Position % align;

            if (value != 0)
            {
                value = align + (-Position % align);
                var bytes = new byte[value];
                Write(bytes);
            }
        }

        public void Write(byte[] data, int padding)
        {
            Write(data);
            WritePadding(padding);
        }

        public void WriteStringSerialized(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                Write(0);
            }
            else
            {
                byte[] data = Encoding.GetBytes(value);
                Write(data.Length);
                Write(data);
            }
        }

        public void WriteStringSerialized(string value, int padding)
        {
            WriteStringSerialized(value);
            if (!string.IsNullOrEmpty(value))
            {
                WritePadding(padding);
            }
        }
        #endregion
    }
}