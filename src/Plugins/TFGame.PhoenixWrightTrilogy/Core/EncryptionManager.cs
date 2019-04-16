using System.Security.Cryptography;

namespace TFGame.PhoenixWrightTrilogy.Core
{
    public static class EncryptionManager
    {
        private const string Password = "u8DurGE2";
        private const string Salt = "6BBGizHE";

        public static byte[] DecryptData(byte[] data)
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

        public static byte[] EncryptData(byte[] data)
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
    }
}
