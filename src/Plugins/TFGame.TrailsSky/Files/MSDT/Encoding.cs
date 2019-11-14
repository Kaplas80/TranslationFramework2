using System;
using System.Collections.Generic;
using System.Text;

namespace TFGame.TrailsSky.Files.MSDT
{
    public class Encoding : System.Text.Encoding
    {
        private readonly System.Text.Encoding isoEncoding = GetEncoding("ISO-8859-1", EncoderFallback.ExceptionFallback, DecoderFallback.ReplacementFallback);
        private readonly System.Text.Encoding defaultEncoding = GetEncoding(932, EncoderFallback.ReplacementFallback, DecoderFallback.ExceptionFallback);

        public List<Tuple<string, string>> DecodingReplacements;
        public List<Tuple<string, string>> EncodingReplacements;

        public Encoding() : base()
        {
            DecodingReplacements = new List<Tuple<string, string>>
            {
            };

            EncodingReplacements = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("\\n", "\n"),

                new Tuple<string, string>("É", "\u0026"), //&
                new Tuple<string, string>("Í", "\u0027"), //'
                new Tuple<string, string>("á", "\u003B"), //;
                new Tuple<string, string>("é", "\u005C"), //\
                new Tuple<string, string>("í", "\u005E"), //^
                new Tuple<string, string>("ó", "\u005F"), //_
                new Tuple<string, string>("ú", "\u0060"), //`
                new Tuple<string, string>("ü", "\u007B"), //{
                new Tuple<string, string>("ñ", "\u007D"), //}
                new Tuple<string, string>("¡", "\u007E"), //~
                new Tuple<string, string>("¿", "\u007F"), //DEL

                new Tuple<string, string>("\u8140", "\u0020"),
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
