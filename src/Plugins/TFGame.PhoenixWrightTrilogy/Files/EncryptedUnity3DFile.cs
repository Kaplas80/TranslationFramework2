using System;
using System.IO;
using System.Reflection;
using TFGame.PhoenixWrightTrilogy.Core;

namespace TFGame.PhoenixWrightTrilogy.Files
{
    public class EncryptedUnity3DFile
    {
        public static void Extract(string inputPath, string outputFolder)
        {
            var data = File.ReadAllBytes(inputPath);
            var decryptedData = EncryptionManager.DecryptData(data);

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
            var data = EncryptionManager.EncryptData(decryptedData);

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
    }
}
