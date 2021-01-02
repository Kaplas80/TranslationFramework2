﻿using System;
using System.IO;
using System.Reflection;

namespace TFGame.DiscoElysium.Files
{
    public class Unity3DFile
    {
        public static void Extract(string inputPath, string outputFolder)
        {
            var fileName = Path.GetFileName(inputPath);
            var copyPath = Path.Combine(outputFolder, fileName);
            // Tipos:
            // -2 -> I2Loc LanguageSourceAsset
            // -63 -> DialogueDatabase_00001
            // -28 -> UnityUIText
            // -175 -> level1 Text
            RunUnityEx("export", "-mb_new -t -2,-63,-28,-175,dds", copyPath);
        }

        public static void Repack(string inputFolder, string outputPath, bool useCompression)
        {
            var copyPath = Path.Combine(inputFolder, Path.GetFileName(outputPath));

            RunUnityEx("import", "-mb_new", copyPath);

            var dir = Path.GetDirectoryName(outputPath);
            Directory.CreateDirectory(dir);

            File.Copy(copyPath, outputPath);
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
