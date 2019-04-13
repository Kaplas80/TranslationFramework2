using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using TF.Core.Files;
using TF.Core.Helpers;

namespace TFGame.PhoenixWrightTrilogy.Files
{
    public class EncryptedFile : BinaryTextFile
    {
        private const string Password = "u8DurGE2";
        private const string Salt = "6BBGizHE";

        public EncryptedFile(string path, string changesFolder, System.Text.Encoding encoding) : base(path, changesFolder, encoding)
        {
        }

        public override bool Search(string searchString)
        {
            var encryptedBytes = File.ReadAllBytes(Path);
            var bytes = DecryptData(encryptedBytes);

            var pattern = FileEncoding.GetBytes(searchString);

            var index1 = SearchHelper.SearchPattern(bytes, pattern, 0);

            var index2 = -1;
            if (HasChanges)
            {
                bytes = File.ReadAllBytes(ChangesFile);
                pattern = System.Text.Encoding.Unicode.GetBytes(searchString);
                index2 = SearchHelper.SearchPattern(bytes, pattern, 0);
            }

            return index1 != -1 || index2 != -1;
        }

        protected static byte[] DecryptData(byte[] data)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(Salt);
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(Password, bytes) { IterationCount = 1000 };

            var rijndaelManaged = new RijndaelManaged { KeySize = 128, BlockSize = 128 };
            rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged.KeySize / 8);
            rijndaelManaged.IV = rfc2898DeriveBytes.GetBytes(rijndaelManaged.BlockSize / 8);

            var cryptoTransform = rijndaelManaged.CreateDecryptor();

            var result = cryptoTransform.TransformFinalBlock(data, 0, data.Length);
            cryptoTransform.Dispose();

            return result;
        }

        protected static byte[] EncryptData(byte[] data)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(Salt);
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(Password, bytes) { IterationCount = 1000 };

            var rijndaelManaged = new RijndaelManaged { KeySize = 128, BlockSize = 128 };
            rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged.KeySize / 8);
            rijndaelManaged.IV = rfc2898DeriveBytes.GetBytes(rijndaelManaged.BlockSize / 8);

            var cryptoTransform = rijndaelManaged.CreateEncryptor();

            var result = cryptoTransform.TransformFinalBlock(data, 0, data.Length);
            cryptoTransform.Dispose();

            return result;
        }

        protected static string ToHalfWidthChars(string input)
        {
            var dictionary = new Dictionary<string, string>
            {
                {"１", "1"},
                {"２", "2"},
                {"３", "3"},
                {"４", "4"},
                {"５", "5"},
                {"６", "6"},
                {"７", "7"},
                {"８", "8"},
                {"９", "9"},
                {"０", "0"},
                {"Ａ", "A"},
                {"Ｂ", "B"},
                {"Ｃ", "C"},
                {"Ｄ", "D"},
                {"Ｅ", "E"},
                {"Ｆ", "F"},
                {"Ｇ", "G"},
                {"Ｈ", "H"},
                {"Ｉ", "I"},
                {"Ｊ", "J"},
                {"Ｋ", "K"},
                {"Ｌ", "L"},
                {"Ｍ", "M"},
                {"Ｎ", "N"},
                {"Ｏ", "O"},
                {"Ｐ", "P"},
                {"Ｑ", "Q"},
                {"Ｒ", "R"},
                {"Ｓ", "S"},
                {"Ｔ", "T"},
                {"Ｕ", "U"},
                {"Ｖ", "V"},
                {"Ｗ", "W"},
                {"Ｘ", "X"},
                {"Ｙ", "Y"},
                {"Ｚ", "Z"},
                {"ａ", "a"},
                {"ｂ", "b"},
                {"ｃ", "c"},
                {"ｄ", "d"},
                {"ｅ", "e"},
                {"ｆ", "f"},
                {"ｇ", "g"},
                {"ｈ", "h"},
                {"ｉ", "i"},
                {"ｊ", "j"},
                {"ｋ", "k"},
                {"ｌ", "l"},
                {"ｍ", "m"},
                {"ｎ", "n"},
                {"ｏ", "o"},
                {"ｐ", "p"},
                {"ｑ", "q"},
                {"ｒ", "r"},
                {"ｓ", "s"},
                {"ｔ", "t"},
                {"ｕ", "u"},
                {"ｖ", "v"},
                {"ｗ", "w"},
                {"ｘ", "x"},
                {"ｙ", "y"},
                {"ｚ", "z"},
                {"\u3000", " "},
                {"．", "."},
                {"，", ","},
                {"＇", "'"},
                {"！", "!"},
                {"（", "("},
                {"）", ")"},
                {"－", "-"},
                {"／", "/"},
                {"？", "?"},
                {"∠", "_"},
                {"［", "["},
                {"］", "]"},
                {"“", "\""},
                {"”", "\""},
                {"＂", "\""},
                {"―", "-"},
                {"‘", "'"},
                {"’", "'"},
                {"：", ":"},
                {"＊", "*"},
                {"；", ";"},
                {"＄", "$"},
                {"Ы", "©"},
                {"∋", "è"},
                {"∈", "é"},
                {"∀", "á"},
                {"∧", "à"},
                {"⊆", "ç"},
                {"⊂", "Ç"},
                {"Ц", "û"},
                {"↑", "î"},
                {"α", "â"},
                {"л", "ñ"},
                {"↓", "ï"},
                {"ε", "ê"}
            };
            var sb = new StringBuilder();

            foreach (var chr in input)
            {
                sb.Append(dictionary.ContainsKey(chr.ToString()) ? dictionary[chr.ToString()] : chr.ToString());
            }

            return sb.ToString();
        }

        protected static string ToFullWidthChars(string input)
        {
            var dictionary = new Dictionary<string, string>
            {
                {"1", "１"},
                {"2", "２"},
                {"3", "３"},
                {"4", "４"},
                {"5", "５"},
                {"6", "６"},
                {"7", "７"},
                {"8", "８"},
                {"9", "９"},
                {"0", "０"},
                {"A", "Ａ"},
                {"B", "Ｂ"},
                {"C", "Ｃ"},
                {"D", "Ｄ"},
                {"E", "Ｅ"},
                {"F", "Ｆ"},
                {"G", "Ｇ"},
                {"H", "Ｈ"},
                {"I", "Ｉ"},
                {"J", "Ｊ"},
                {"K", "Ｋ"},
                {"L", "Ｌ"},
                {"M", "Ｍ"},
                {"N", "Ｎ"},
                {"O", "Ｏ"},
                {"P", "Ｐ"},
                {"Q", "Ｑ"},
                {"R", "Ｒ"},
                {"S", "Ｓ"},
                {"T", "Ｔ"},
                {"U", "Ｕ"},
                {"V", "Ｖ"},
                {"W", "Ｗ"},
                {"X", "Ｘ"},
                {"Y", "Ｙ"},
                {"Z", "Ｚ"},
                {"a", "ａ"},
                {"b", "ｂ"},
                {"c", "ｃ"},
                {"d", "ｄ"},
                {"e", "ｅ"},
                {"f", "ｆ"},
                {"g", "ｇ"},
                {"h", "ｈ"},
                {"i", "ｉ"},
                {"j", "ｊ"},
                {"k", "ｋ"},
                {"l", "ｌ"},
                {"m", "ｍ"},
                {"n", "ｎ"},
                {"o", "ｏ"},
                {"p", "ｐ"},
                {"q", "ｑ"},
                {"r", "ｒ"},
                {"s", "ｓ"},
                {"t", "ｔ"},
                {"u", "ｕ"},
                {"v", "ｖ"},
                {"w", "ｗ"},
                {"x", "ｘ"},
                {"y", "ｙ"},
                {"z", "ｚ"},
                {" ", "\u3000"},
                {".", "．"},
                {",", "，"},
                {"'", "＇"},
                {"!", "！"},
                {"(", "（"},
                {")", "）"},
                {"-", "－"},
                {"/", "／"},
                {"?", "？"},
                {"_", "∠"},
                {"[", "［"},
                {"]", "］"},
                {"\"", "＂"},
                {":", "："},
                {"*", "＊"},
                {";", "；"},
                {"$", "＄"},
                {"©", "Ы"},
                {"è", "∋"},
                {"é", "∈"},
                {"á", "∀"},
                {"à", "∧"},
                {"ç", "⊆"},
                {"Ç", "⊂"},
                {"û", "Ц"},
                {"î", "↑"},
                {"â", "α"},
                {"ñ", "л"},
                {"ï", "↓"},
                {"ê", "ε"},
            };

            var sb = new StringBuilder();

            foreach (var chr in input)
            {
                sb.Append(dictionary.ContainsKey(chr.ToString()) ? dictionary[chr.ToString()] : chr.ToString());
            }

            return sb.ToString();
        }
    }
}
