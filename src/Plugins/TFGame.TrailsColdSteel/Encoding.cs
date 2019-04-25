using System;
using System.Collections.Generic;
using System.Text;

namespace TFGame.TrailsColdSteel
{
    public class Encoding : System.Text.Encoding
    {
        private readonly System.Text.Encoding isoEncoding = GetEncoding("ISO-8859-1", EncoderFallback.ExceptionFallback, DecoderFallback.ReplacementFallback);
        private readonly System.Text.Encoding defaultEncoding = GetEncoding("UTF-8", EncoderFallback.ReplacementFallback, DecoderFallback.ExceptionFallback);

        public List<Tuple<string, string>> DecodingReplacements;
        public List<Tuple<string, string>> EncodingReplacements;

        public Encoding() : base()
        {
            DecodingReplacements = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("\\n", "<LineBreak>"),

                new Tuple<string, string>("\u0001", "\\n"),
                new Tuple<string, string>("\u0002", "<Enter>"),
            };

            EncodingReplacements = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("\\n", "\u0001"),
                new Tuple<string, string>("<Enter>", "\u0002"),
                new Tuple<string, string>("<LineBreak>", "\\n"),
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
                result = defaultEncoding.GetEncoder().GetByteCount(chars, index, count, true);
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
                result = defaultEncoding.GetEncoder().GetBytes(chars, charIndex, charCount, bytes, byteIndex, true);
            }

            return result;
        }

        public override byte[] GetBytes(string s)
        {
            var str = s;

            foreach (var (item1, item2) in EncodingReplacements)
            {
                str = str.Replace(item1, item2);
            }

            return GetBytes(str.ToCharArray(), 0, str.Length);
        }

        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            int result;
            try
            {
                result = defaultEncoding.GetDecoder().GetCharCount(bytes, index, count, true);
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
                result = defaultEncoding.GetDecoder().GetChars(bytes, byteIndex, byteCount, chars, charIndex, true);
            }
            catch (DecoderFallbackException)
            {
                result = isoEncoding.GetDecoder().GetChars(bytes, byteIndex, byteCount, chars, charIndex, true);
            }

            return result;
        }

        public override int GetMaxByteCount(int charCount)
        {
            return defaultEncoding.GetMaxByteCount(charCount);
        }

        public override int GetMaxCharCount(int byteCount)
        {
            return defaultEncoding.GetMaxCharCount(byteCount);
        }

        public override string GetString(byte[] bytes, int index, int count)
        {
            var str = new string(GetChars(bytes, index, count));

            foreach (var (item1, item2) in DecodingReplacements)
            {
                str = str.Replace(item1, item2);
            }

            return str;
        }
    }
}
