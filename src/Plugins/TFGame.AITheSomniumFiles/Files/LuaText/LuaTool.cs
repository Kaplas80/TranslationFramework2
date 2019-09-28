using System;
using System.IO;
using System.Reflection;

namespace TFGame.AITheSomniumFiles.Files
{
    public static class LuaTool
    {
        public static void Decompile(string inputFile, string outputFile)
        {
            try
            {
                var exePath = Path.Combine(GetExecutingDirectoryName(), "plugins", "luadec.exe");

                using (var process = new System.Diagnostics.Process())
                {
                    process.StartInfo.FileName = exePath;
                    process.StartInfo.WorkingDirectory = Path.GetDirectoryName(exePath);
                    process.StartInfo.Arguments = $"\"{inputFile}\"";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    process.StartInfo.CreateNoWindow = true; //not diplay a windows
                    
                    process.Start();
                    var output = process.StandardOutput.ReadToEnd();
                    File.WriteAllText(outputFile, output);
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
