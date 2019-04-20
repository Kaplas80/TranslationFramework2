using System;
using System.IO;
using System.Reflection;

namespace CRIWareGame.Files
{
    public class CpkFile
    {
        public static void Extract(string inputPath, string outputFolder)
        {
            RunCpkTool("e", inputPath, outputFolder, string.Empty);
        }

        public static void Repack(string inputFolder, string outputPath, bool useCompression)
        {
            RunCpkTool("p", inputFolder, outputPath, useCompression ? "1" : "0");
        }

        private static void RunCpkTool(string operation, string param1, string param2, string param3)
        {
            using (var process = new System.Diagnostics.Process())
            {
                process.StartInfo.FileName = $@"""{GetExecutingDirectoryName()}\plugins\CpkTool.exe""";
                process.StartInfo.Arguments = $"{operation} \"{param1}\" \"{param2}\" \"{param3}\"";
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
