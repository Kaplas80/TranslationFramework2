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

            RunUnityEx("exportbundle", string.Empty, copyPath);
            RunUnityEx("export", string.Empty, copyPath);
        }

        public static void Repack(string inputFolder, string outputPath, bool useCompression)
        {
            var copyPath = Path.Combine(inputFolder, Path.GetFileName(outputPath));

            RunUnityEx("import", string.Empty, copyPath);

            var decryptedData = File.ReadAllBytes(copyPath);
            var data = EncryptionManager.EncryptData(decryptedData);

            var dir = Path.GetDirectoryName(outputPath);
            Directory.CreateDirectory(dir);

            File.WriteAllBytes(outputPath, data);
            File.WriteAllBytes($"{outputPath}.decrypt", decryptedData);
        }

        private static void RunUnityEx(string operation, string parameters, string unityFile)
        {
            var unityExPath = Path.Combine(GetExecutingDirectoryName(), "plugins", "UnityEX.exe");

            using (var process = new System.Diagnostics.Process())
            {
                process.StartInfo.FileName = unityExPath;
                process.StartInfo.WorkingDirectory = Path.GetDirectoryName(unityExPath);
                process.StartInfo.Arguments = $"{operation} \"{unityFile}\" {parameters}";
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
            return Path.GetDirectoryName(location.LocalPath);
        }
    }
}
