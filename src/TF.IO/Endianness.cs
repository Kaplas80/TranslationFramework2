using System;

namespace TF.IO
{
    public enum Endianness
    {
        LittleEndian,
        BigEndian
    }

    public static class EndiannessHelper
    {
        public static Endianness SystemEndianness => BitConverter.IsLittleEndian ? Endianness.LittleEndian : Endianness.BigEndian;
    }
}