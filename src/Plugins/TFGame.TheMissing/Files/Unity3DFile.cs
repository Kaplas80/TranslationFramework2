using System;
using System.IO;
using System.Reflection;

namespace TFGame.TheMissing.Files
{
    public class Unity3DFile
    {
        public static void Extract(string inputPath, string outputFolder)
        {
            var fileName = Path.GetFileName(inputPath);
            var copyPath = Path.Combine(outputFolder, fileName);

            RunUnityEx("export", copyPath);
        }

        public static void Repack(string inputFolder, string outputPath, bool useCompression)
        {
            var copyPath = Path.Combine(inputFolder, Path.GetFileName(outputPath));

            RunUnityEx("import", copyPath);

            var dir = Path.GetDirectoryName(outputPath);
            Directory.CreateDirectory(dir);

            File.Copy(copyPath, outputPath);
        }

        private static void RunUnityEx(string operation, string unityFile)
        {
            using (var process = new System.Diagnostics.Process())
            {
                process.StartInfo.FileName = $@"""{GetExecutingDirectoryName()}\plugins\UnityEX.exe""";
                process.StartInfo.Arguments = $"{operation} \"{unityFile}\" -t -13";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                process.StartInfo.CreateNoWindow = true; //not diplay a windows
                process.Start();
                process.WaitForExit();
            }

            using (var process = new System.Diagnostics.Process())
            {
                process.StartInfo.FileName = $@"""{GetExecutingDirectoryName()}\plugins\UnityEX.exe""";
                process.StartInfo.Arguments = $"{operation} \"{unityFile}\" -t dds";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                process.StartInfo.CreateNoWindow = true; //not diplay a windows
                process.Start();
                process.WaitForExit();
            }

            using (var process = new System.Diagnostics.Process())
            {
                process.StartInfo.FileName = $@"""{GetExecutingDirectoryName()}\plugins\UnityEX.exe""";
                process.StartInfo.Arguments = $"{operation} \"{unityFile}\" -t txt";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                process.StartInfo.CreateNoWindow = true; //not diplay a windows
                process.Start();
                process.WaitForExit();
            }

            using (var process = new System.Diagnostics.Process())
            {
                process.StartInfo.FileName = $@"""{GetExecutingDirectoryName()}\plugins\UnityEX.exe""";
                process.StartInfo.Arguments = $"{operation} \"{unityFile}\" -t ttf";
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
