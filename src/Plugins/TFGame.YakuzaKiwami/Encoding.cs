using System.Text;

namespace TFGame.YakuzaKiwami
{
    public class Encoding : UTF8Encoding
    {
        public override byte[] GetBytes(string s)
        {
            var str = s.Replace("\\n", "\n").Replace("\\r", "\r");

            return GetBytes(str.ToCharArray(), 0, str.Length);
        }

        public override string GetString(byte[] bytes, int index, int count)
        {
            var str = new string(GetChars(bytes, index, count));
            str = str.Replace("\n", "\\n").Replace("\r", "\\r");
            return str;
        }
    }
}
