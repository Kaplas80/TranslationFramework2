using System;
using System.IO;
using System.Reflection;

namespace TFGame.AITheSomniumFiles.Files
{
    public static class LuaTool
    {
        public static void Decompile(string inputFile, string outputFile)
        {
            var tempFile = Path.GetTempFileName();
            
            Xor(inputFile, tempFile);

            try
            {
                var exePath = Path.Combine(GetExecutingDirectoryName(), "plugins", "luadec.exe");

                using (var process = new System.Diagnostics.Process())
                {
                    process.StartInfo.FileName = exePath;
                    process.StartInfo.WorkingDirectory = Path.GetDirectoryName(exePath);
                    process.StartInfo.Arguments = $"-se UTF8 -o \"{outputFile}\" \"{tempFile}\"";
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
            finally
            {
                File.Delete(tempFile);
            }
        }

        public static void Compile(string inputFile, string outputFile)
        {
            var tempFile = Path.GetTempFileName();
            
            try
            {
                var exePath = Path.Combine(GetExecutingDirectoryName(), "plugins", "luac.exe");

                using (var process = new System.Diagnostics.Process())
                {
                    process.StartInfo.FileName = exePath;
                    process.StartInfo.WorkingDirectory = Path.GetDirectoryName(exePath);
                    process.StartInfo.Arguments = $"-o \"{tempFile}\" \"{inputFile}\"";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    process.StartInfo.CreateNoWindow = true; //not diplay a windows

                    process.Start();
                    process.WaitForExit();
                }

                Xor(tempFile, outputFile);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                File.Delete(tempFile);
            }
        }

        private static string GetExecutingDirectoryName()
        {
            var location = new Uri(Assembly.GetEntryAssembly().GetName().CodeBase);
            return Path.GetDirectoryName(location.LocalPath);
        }

        public static void Xor(string inputFile, string outputFile)
        {
            var data = File.ReadAllBytes(inputFile);

            for (var i = 4; i < data.Length; i++)
            {
                data[i] ^= (byte) i;
            }

            File.WriteAllBytes(outputFile, data);
        }
    }
}
