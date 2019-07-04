using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TF.Core.Helpers;

namespace TFGame.Yakuza0
{
    public class Encoding : System.Text.Encoding
    {
        private readonly System.Text.Encoding isoEncoding = GetEncoding("ISO-8859-1", EncoderFallback.ExceptionFallback, DecoderFallback.ReplacementFallback);
        private readonly System.Text.Encoding utf8Encoding = GetEncoding("UTF-8", EncoderFallback.ReplacementFallback, DecoderFallback.ExceptionFallback);

        private List<Tuple<string, string>> DecodingReplacements;
        private List<Tuple<string, string>> EncodingReplacements;

        public Encoding() : base()
        {
            DecodingReplacements = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("⑮", "\u2665"),
                new Tuple<string, string>("\\", "¥"),
                new Tuple<string, string>("\u007F", "®"),
                new Tuple<string, string>("^", "%"),
                new Tuple<string, string>("~", "™"),
                new Tuple<string, string>("\r", "\\r"),
                new Tuple<string, string>("\n", "\\n"),
            };

            EncodingReplacements = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("\\n", "\n"),
                new Tuple<string, string>("\\r", "\r"),
                new Tuple<string, string>("™", "~"),
                new Tuple<string, string>("^", "%"),
                new Tuple<string, string>("®", "\u007F"),
                new Tuple<string, string>("¥", "\\"),
                new Tuple<string, string>("\u266A", "¤"), // Nota musical
                new Tuple<string, string>("\u2665", "§"), // Corazón
                new Tuple<string, string>("\u221E", "\u00B8"), // Infinito

                new Tuple<string, string>("\u25B3", "tf1"), // Triángulo
                new Tuple<string, string>("\u25CB", "tf2"), // Círculo
                new Tuple<string, string>("\u25A1", "tf3"), // Cuadrado
            };

        }

        public override int GetByteCount(string str)
        {
            var bytes = GetBytes(str);
            return bytes.Length;
        }

        public override int GetByteCount(char[] chars, int index, int count)
        {
            int result;
            try
            {
                result = isoEncoding.GetEncoder().GetByteCount(chars, index, count, true);
            }
            catch (EncoderFallbackException)
            {
                result = utf8Encoding.GetEncoder().GetByteCount(chars, index, count, true);
            }

            return result;
        }

        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            int result;
            try
            {
                result = isoEncoding.GetEncoder().GetBytes(chars, charIndex, charCount, bytes, byteIndex, true);
            }
            catch (EncoderFallbackException)
            {
                result = utf8Encoding.GetEncoder().GetBytes(chars, charIndex, charCount, bytes, byteIndex, true);
            }

            if (bytes.Length < 3)
            {
                return result;
            }

            // Hack para los símbolos cuadrado, triángulo y círculo
            // Triangulo = "tf1" 746631
            // Circulo = "tf2" 746632
            // Cuadrado = "tf3" 746633
            FixPlaceholder(bytes, new byte[] { 0x74, 0x66, 0x31 }, new byte[] { 0xE2, 0x96, 0xB3 });
            FixPlaceholder(bytes, new byte[] { 0x74, 0x66, 0x32 }, new byte[] { 0xE2, 0x97, 0x8B });
            FixPlaceholder(bytes, new byte[] { 0x74, 0x66, 0x33 }, new byte[] { 0xE2, 0x96, 0xA1 });

            return result;
        }

        private void FixPlaceholder(byte[] input, byte[] oldBytes, byte[] newBytes)
        {
            var placeholder = SearchHelper.SearchPattern(input, oldBytes);
            while (placeholder != -1)
            {
                input[placeholder] = newBytes[0];
                input[placeholder + 1] = newBytes[1];
                input[placeholder + 2] = newBytes[2];

                placeholder = SearchHelper.SearchPattern(input, oldBytes);
            }
        }

        public override byte[] GetBytes(string s)
        {
            var str = s;

            foreach (var t in EncodingReplacements)
            {
                str = str.Replace(t.Item1, t.Item2);
            }

            return GetBytes(str.ToCharArray(), 0, str.Length);
        }

        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            int result;
            try
            {
                result = utf8Encoding.GetDecoder().GetCharCount(bytes, index, count, true);
            }
            catch (DecoderFallbackException)
            {
                result = isoEncoding.GetDecoder().GetCharCount(bytes, index, count, true);
            }

            return result;
        }

        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            int result;
            try
            {
                result = utf8Encoding.GetDecoder().GetChars(bytes, byteIndex, byteCount, chars, charIndex, true);
            }
            catch (DecoderFallbackException)
            {
                result = isoEncoding.GetDecoder().GetChars(bytes, byteIndex, byteCount, chars, charIndex, true);
            }

            return result;
        }

        public override int GetMaxByteCount(int charCount)
        {
            return utf8Encoding.GetMaxByteCount(charCount);
        }

        public override int GetMaxCharCount(int byteCount)
        {
            return utf8Encoding.GetMaxCharCount(byteCount);
        }

        public override string GetString(byte[] bytes, int index, int count)
        {
            var str = new string(GetChars(bytes, index, count));

            foreach (var t in DecodingReplacements)
            {
                str = str.Replace(t.Item1, t.Item2);
            }

            return str;
        }
    }
}
