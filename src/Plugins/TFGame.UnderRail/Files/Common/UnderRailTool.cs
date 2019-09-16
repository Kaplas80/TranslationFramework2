using System;
using System.IO;
using System.Reflection;

namespace TFGame.UnderRail.Files.Common
{
    public static class UnderRailTool
    {
        public static void Run(string operation, string param1, string param2, string param3)
        {
            try
            {
                var exePath = Path.Combine(GetExecutingDirectoryName(), "plugins", "UnderRailTool.exe");

                using (var process = new System.Diagnostics.Process())
                {
                    process.StartInfo.FileName = exePath;
                    process.StartInfo.WorkingDirectory = Path.GetDirectoryName(exePath);
                    process.StartInfo.Arguments = $"{operation} \"{param1}\" \"{param2}\" \"{param3}\"";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    process.StartInfo.CreateNoWindow = true; //not diplay a windows
                    process.Start();
                    process.WaitForExit();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static string GetExecutingDirectoryName()
        {
            var location = new Uri(Assembly.GetEntryAssembly().GetName().CodeBase);
            return Path.GetDirectoryName(location.LocalPath);
        }
    }
}
