using System;
using System.IO;
using System.Reflection;
using TFGame.PhoenixWrightTrilogy.Core;

namespace TFGame.PhoenixWrightTrilogy.Files
{
    public class Unity3DFile
    {
        public static void Extract(string inputPath, string outputFolder)
        {
            string fileName = Path.GetFileName(inputPath);
            string copyPath = Path.Combine(outputFolder, fileName);
            File.Copy(inputPath, copyPath);

            RunUnityEx("exportbundle", string.Empty, copyPath);
            RunUnityEx("export", string.Empty, copyPath);
        }

        public static void Repack(string inputFolder, string outputPath, bool useCompression)
        {
            string copyPath = Path.Combine(inputFolder, Path.GetFileName(outputPath));

            RunUnityEx("import", string.Empty, copyPath);

            string dir = Path.GetDirectoryName(outputPath);
            Directory.CreateDirectory(dir);

            File.Copy(copyPath, outputPath);
        }

        private static void RunUnityEx(string operation, string parameters, string unityFile)
        {
            string unityExPath = Path.Combine(GetExecutingDirectoryName(), "plugins", "UnityEX.exe");

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
