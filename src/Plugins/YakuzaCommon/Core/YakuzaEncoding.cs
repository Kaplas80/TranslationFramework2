using System;
using System.Text;

namespace YakuzaCommon.Core
{
    class YakuzaEncoding : Encoding
    {
        private readonly Encoding isoEncoding = GetEncoding("ISO-8859-1");
        private readonly Encoding utf8Encoding = GetEncoding("UTF-8", EncoderFallback.ExceptionFallback,
            DecoderFallback.ExceptionFallback);

        public override int GetByteCount(char[] chars, int index, int count)
        {
            int result;
            try
            {
                result = utf8Encoding.GetEncoder().GetByteCount(chars, index, count, true);
            }
            catch (EncoderFallbackException)
            {
                result = isoEncoding.GetEncoder().GetByteCount(chars, index, count, true);
            }

            return result;
        }

        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            int result;
            try
            {
                result = utf8Encoding.GetEncoder().GetBytes(chars, charIndex, charCount, bytes, byteIndex, true);
            }
            catch (EncoderFallbackException)
            {
                result = isoEncoding.GetEncoder().GetBytes(chars, charIndex, charCount, bytes, byteIndex, true);
            }

            return result;
        }

        public override byte[] GetBytes(string s)
        {
            var str = s.Replace("\\n", "\n").Replace("\\r", "\r");

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
            str = str.Replace("\n", "\\n").Replace("\r", "\\r");
            return str;
        }
    }
}
