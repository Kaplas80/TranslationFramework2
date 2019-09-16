using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UnderRailTool
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args[0] == "r")
            {
                Read(args[1], args[2]);
            }

            if (args[0] == "w")
            {
                Write(args[1], args[2], args[3]);
            }
        }

        private static void Initialize()
        {
            var assemblyResolver = new AssemblyResolver();
            assemblyResolver.b(true);
            assemblyResolver.a(true);
            assemblyResolver.nt();
            assemblyResolver.e();

            var fieldInfo = typeof(co0).GetField("a");
            fieldInfo.SetValue(null, assemblyResolver);
        }

        private static void Read(string gameFile, string outputFile)
        {
            Initialize();

            switch (Path.GetExtension(gameFile))
            {
                case ".udlg":
                {
                    var texts = Dialogs.GetSubtitles(gameFile);

                    WriteTexts(texts, outputFile);
                    break;
                }
                case ".k":
                {
                    var texts = Knowledge.GetSubtitles(gameFile);

                    WriteTexts(texts, outputFile);
                    break;
                }
                case ".item":
                {
                    var texts = Items.GetSubtitles(gameFile);

                    WriteTexts(texts, outputFile);
                    break;
                }
            }
        }

        private static void WriteTexts(Dictionary<string, string> texts, string outputFile)
        {

            if (texts.Count > 0)
            {
                var lines = new List<string>(texts.Count);

                lines.AddRange(texts.Select(text => $"{text.Key}<Split>{text.Value.Replace("\r", "\\r").Replace("\n", "\\n")}"));

                File.WriteAllLines(outputFile, lines);
            }
        }

        private static void Write(string gameFile, string textsFile, string outputFile)
        {
            Initialize();

            switch (Path.GetExtension(gameFile))
            {
                case ".udlg":
                {
                    var texts = ReadTexts(textsFile);
                    var model = Dialogs.SetSubtitles(gameFile, texts);
                    FileManager.Save(model, outputFile, false);
                    break;
                }
                case ".k":
                {
                    var texts = ReadTexts(textsFile);
                    var model = Knowledge.SetSubtitles(gameFile, texts);
                    FileManager.Save(model, outputFile, true);
                    break;
                }
                case ".item":
                {
                    var texts = ReadTexts(textsFile);
                    var model = Items.SetSubtitles(gameFile, texts);
                    FileManager.Save(model, outputFile, true);
                    break;
                }
            }
        }

        private static Dictionary<string, string> ReadTexts(string inputFile)
        {
            var lines = File.ReadAllLines(inputFile);

            var result = new Dictionary<string, string>(lines.Length);

            foreach (var line in lines)
            {
                var split = line.Split(new[] {"<Split>"}, StringSplitOptions.None);
                result.Add(split[0], split[1].Replace("\\r", "\r").Replace("\\n", "\n"));
            }

            return result;
        }
    }
}
