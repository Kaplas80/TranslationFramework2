using System;
using System.Collections.Generic;
using System.Text;

namespace TFGame.TrailsSky
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
                //new Tuple<string, string>("\\n", "<LineBreak>"),
                //new Tuple<string, string>("\u0026", "É"), //&
                //new Tuple<string, string>("\u0027", "Í"), //'
                //new Tuple<string, string>("\u003B", "á"), //;
                //new Tuple<string, string>("\u005C", "é"), //\
                //new Tuple<string, string>("\u005E", "í"), //^
                //new Tuple<string, string>("\u005F", "ó"), //_
                //new Tuple<string, string>("\u0060", "ú"), //`
                //new Tuple<string, string>("\u007B", "ü"), //{
                //new Tuple<string, string>("\u007D", "ñ"), //}
                //new Tuple<string, string>("\u007E", "¡"), //~
                //new Tuple<string, string>("\u007F", "¿"), //DEL

                new Tuple<string, string>("\u0001", "\\n"),
                new Tuple<string, string>("\u0002", "<Enter>"),
                new Tuple<string, string>("\u0003", "<Clear>"),
                new Tuple<string, string>("\u0004", "<0x04>"),
                new Tuple<string, string>("\u0005", "<0x05>"),
                new Tuple<string, string>("\u0006", "<0x06>"),
                new Tuple<string, string>("\u0007", "<0x07>"),
                new Tuple<string, string>("\u0008", "<0x08>"),
                new Tuple<string, string>("\u0009", "<0x09>"),
                new Tuple<string, string>("\u000A", "<0x0A>"),
                new Tuple<string, string>("\u000B", "<0x0B>"),
                new Tuple<string, string>("\u000C", "<0x0C>"),
                new Tuple<string, string>("\u000D", "<0x0D>"),
                new Tuple<string, string>("\u000E", "<0x0E>"),
                new Tuple<string, string>("\u000F", "<0x0F>"),
                new Tuple<string, string>("\u0010", "<0x10>"),
                new Tuple<string, string>("\u0011", "<0x11>"),
                new Tuple<string, string>("\u0012", "<0x12>"),
                new Tuple<string, string>("\u0013", "<0x13>"),
                new Tuple<string, string>("\u0014", "<0x14>"),
                new Tuple<string, string>("\u0015", "<0x15>"),
                new Tuple<string, string>("\u0016", "<0x16>"),
                new Tuple<string, string>("\u0017", "<0x17>"),
                new Tuple<string, string>("\u0018", "<0x18>"),
                new Tuple<string, string>("\u0019", "<0x19>"),
                new Tuple<string, string>("\u001A", "<0x1A>"),
                new Tuple<string, string>("\u001B", "<0x1B>"),
                new Tuple<string, string>("\u001C", "<0x1C>"),
                new Tuple<string, string>("\u001D", "<0x1D>"),
                new Tuple<string, string>("\u001E", "<0x1E>"),
                new Tuple<string, string>("\u001F", "<0x1F>"),
            };

            EncodingReplacements = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("\\n", "\u0001"),
                new Tuple<string, string>("<Enter>", "\u0002"),
                new Tuple<string, string>("<Clear>", "\u0003"),
                new Tuple<string, string>("<0x04>", "\u0004"),
                new Tuple<string, string>("<0x05>", "\u0005"),
                new Tuple<string, string>("<0x06>", "\u0006"),
                new Tuple<string, string>("<0x07>", "\u0007"),
                new Tuple<string, string>("<0x08>", "\u0008"),
                new Tuple<string, string>("<0x09>", "\u0009"),
                new Tuple<string, string>("<0x0A>", "\u000A"),
                new Tuple<string, string>("<0x0B>", "\u000B"),
                new Tuple<string, string>("<0x0C>", "\u000C"),
                new Tuple<string, string>("<0x0D>", "\u000D"),
                new Tuple<string, string>("<0x0E>", "\u000E"),
                new Tuple<string, string>("<0x0F>", "\u000F"),
                new Tuple<string, string>("<0x10>", "\u0010"),
                new Tuple<string, string>("<0x11>", "\u0011"),
                new Tuple<string, string>("<0x12>", "\u0012"),
                new Tuple<string, string>("<0x13>", "\u0013"),
                new Tuple<string, string>("<0x14>", "\u0014"),
                new Tuple<string, string>("<0x15>", "\u0015"),
                new Tuple<string, string>("<0x16>", "\u0016"),
                new Tuple<string, string>("<0x17>", "\u0017"),
                new Tuple<string, string>("<0x18>", "\u0018"),
                new Tuple<string, string>("<0x19>", "\u0019"),
                new Tuple<string, string>("<0x1A>", "\u001A"),
                new Tuple<string, string>("<0x1B>", "\u001B"),
                new Tuple<string, string>("<0x1C>", "\u001C"),
                new Tuple<string, string>("<0x1D>", "\u001D"),
                new Tuple<string, string>("<0x1E>", "\u001E"),
                new Tuple<string, string>("<0x1F>", "\u001F"),

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
                //new Tuple<string, string>("<LineBreak>", "\\n"),
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
