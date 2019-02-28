using System;
using System.Collections.Generic;
using System.Text;

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
                new Tuple<string, string>("\\", "¥"),
                new Tuple<string, string>("^", "%"),
                new Tuple<string, string>("~", "™"),
                new Tuple<string, string>("\u0008", "®"),
                new Tuple<string, string>("\u00A0", "\u2022"),
                new Tuple<string, string>("\u00A1", "\uFF0E"),
                new Tuple<string, string>("\u00A2", "\u00A1"),
                new Tuple<string, string>("\r", "\\r"),
                new Tuple<string, string>("\n", "\\n"),
            };

            EncodingReplacements = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("\\r", "\r"),
                new Tuple<string, string>("\\n", "\n"),
                new Tuple<string, string>("\u00A1", "\u00A2"),
                new Tuple<string, string>("\uFF0E", "\u00A1"),
                new Tuple<string, string>("\u2022", "\u00A0"),
                new Tuple<string, string>("®", "\u0008"),
                new Tuple<string, string>("™", "~"),
                new Tuple<string, string>("¥", "\\"),
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

            return result;
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
