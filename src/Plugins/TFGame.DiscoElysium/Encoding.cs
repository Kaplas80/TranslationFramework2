using System;
using System.Collections.Generic;

namespace TFGame.DiscoElysium
{
    public class Encoding : System.Text.Encoding
    {
        private readonly System.Text.Encoding utf8Encoding = GetEncoding("UTF-8");

        private List<Tuple<string, string>> DecodingReplacements;
        private List<Tuple<string, string>> EncodingReplacements;

        public Encoding() : base()
        {
            DecodingReplacements = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("\\n", "<NewLine>"),
                new Tuple<string, string>("\n", "\\n"),
            };

            EncodingReplacements = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("\\n", "\n"),
                new Tuple<string, string>("<NewLine>", "\\n"),
            };

        }

        public override int GetByteCount(string str)
        {
            var bytes = GetBytes(str);
            return bytes.Length;
        }

        public override int GetByteCount(char[] chars, int index, int count)
        {
            return utf8Encoding.GetEncoder().GetByteCount(chars, index, count, true);
        }

        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            return utf8Encoding.GetEncoder().GetBytes(chars, charIndex, charCount, bytes, byteIndex, true); 
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
            return utf8Encoding.GetDecoder().GetCharCount(bytes, index, count, true);
        }

        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            return utf8Encoding.GetDecoder().GetChars(bytes, byteIndex, byteCount, chars, charIndex, true);
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
