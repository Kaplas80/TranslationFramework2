using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;

namespace TFGame.PhoenixWrightTrilogy.Files
{
    public class EncryptedUnity3DFile
    {
        private const string Password = "u8DurGE2";
        private const string Salt = "6BBGizHE";

        public static void Extract(string inputPath, string outputFolder)
        {
            var data = File.ReadAllBytes(inputPath);
            var decryptedData = DecryptData(data);

            var copyPath = Path.Combine(outputFolder, Path.GetFileName(inputPath));
            File.WriteAllBytes(copyPath, decryptedData);

            RunUnityEx("exportbundle", copyPath);
            RunUnityEx("export", copyPath);
        }

        public static void Repack(string inputFolder, string outputPath, bool useCompression)
        {
            var copyPath = Path.Combine(inputFolder, Path.GetFileName(outputPath));

            RunUnityEx("import", copyPath);

            var decryptedData = File.ReadAllBytes(copyPath);
            var data = EncryptData(decryptedData);

            var dir = Path.GetDirectoryName(outputPath);
            Directory.CreateDirectory(dir);

            File.WriteAllBytes(outputPath, data);
            File.WriteAllBytes($"{outputPath}.decrypt", decryptedData);
        }

        private static void RunUnityEx(string operation, string unityFile)
        {
            using (var process = new System.Diagnostics.Process())
            {
                process.StartInfo.FileName = $@"{GetExecutingDirectoryName()}\plugins\UnityEX.exe";
                process.StartInfo.Arguments = $"{operation} \"{unityFile}\"";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                process.StartInfo.CreateNoWindow = true; //not diplay a windows
                process.Start();
                process.WaitForExit();
            }
        }

        private static string GetExecutingDirectoryName()
        {
            var location = new Uri(Assembly.GetEntryAssembly().GetName().CodeBase);
            return new FileInfo(location.AbsolutePath).Directory.FullName;
        }

        private static byte[] DecryptData(byte[] data)
        {
            try
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
            catch (Exception)
            {
                return data;
            }
        }

        private static byte[] EncryptData(byte[] data)
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
