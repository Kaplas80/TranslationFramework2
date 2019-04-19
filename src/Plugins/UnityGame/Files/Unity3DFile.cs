using System;
using System.IO;
using System.Reflection;

namespace UnityGame.Files
{
    public class Unity3DFile
    {
        public static void Extract(string inputPath, string outputFolder)
        {
            var fileName = Path.GetFileName(inputPath);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(inputPath);
            var copyPath = Path.Combine(outputFolder, fileName);
            var inputFolder = Path.GetDirectoryName(inputPath);
            var files = Directory.EnumerateFiles(inputFolder, $"{fileNameWithoutExtension}.*");
            foreach (var file in files)
            {
                var outputPath = Path.Combine(outputFolder, Path.GetFileName(file));
                File.Copy(file, outputPath);
            }

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
