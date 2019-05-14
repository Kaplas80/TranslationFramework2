using System;
using System.Collections.Generic;

namespace TFGame.YakuzaIshin
{
    public class Encoding : System.Text.Encoding
    {
        private readonly System.Text.Encoding defaultEncoding = GetEncoding(932);

        private List<Tuple<string, string>> DecodingReplacements;
        private List<Tuple<string, string>> EncodingReplacements;

        public Encoding() : base()
        {
            DecodingReplacements = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("\r", "\\r"),
                new Tuple<string, string>("\n", "\\n"),
                
            };

            EncodingReplacements = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("\\r", "\r"),
                new Tuple<string, string>("\\n", "\n"),
            };

        }

        public override int GetByteCount(string str)
        {
            var bytes = GetBytes(str);
            return bytes.Length;
        }

        public override int GetByteCount(char[] chars, int index, int count)
        {
            return defaultEncoding.GetEncoder().GetByteCount(chars, index, count, true);
        }

        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            return defaultEncoding.GetEncoder().GetBytes(chars, charIndex, charCount, bytes, byteIndex, true); 
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
            return defaultEncoding.GetDecoder().GetCharCount(bytes, index, count, true);
        }

        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            return defaultEncoding.GetDecoder().GetChars(bytes, byteIndex, byteCount, chars, charIndex, true);
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

            foreach (var t in DecodingReplacements)
            {
                str = str.Replace(t.Item1, t.Item2);
            }

            return str;
        }
    }
}
