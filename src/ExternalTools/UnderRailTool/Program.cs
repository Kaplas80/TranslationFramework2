using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UnderRailTool
{
    using System.Reflection;

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

            if (args[0] == "d")
            {
                Dump(args[1], args[2]);
            }
        }

        private static void Initialize()
        {
            // Search for "Initializing assembly resolver" in underrail.exe
            var assemblyResolver = new AssemblyResolver();
            assemblyResolver.b(true);
            assemblyResolver.a(true);
            assemblyResolver.nz();
            assemblyResolver.e();

            // This is the assignment after "Creating data model element type map"
            FieldInfo fieldInfo = typeof(cp3).GetField("a");
            fieldInfo.SetValue(null, assemblyResolver);
        }

        private static void Read(string gameFile, string outputFile)
        {
            Initialize();

            switch (Path.GetExtension(gameFile))
            {
                case ".udlg":
                {
                    Dictionary<string, string> texts = Dialogs.GetSubtitles(gameFile);

                    WriteTexts(texts, outputFile);
                    break;
                }
                case ".k":
                {
                    Dictionary<string, string> texts = Knowledge.GetSubtitles(gameFile);

                    WriteTexts(texts, outputFile);
                    break;
                }
                case ".item":
                {
                    Dictionary<string, string> texts = Items.GetSubtitles(gameFile);

                    WriteTexts(texts, outputFile);
                    break;
                }
                case ".uz":
                {
                    Dictionary<string, string> texts = Zone.GetSubtitles(gameFile);

                    WriteTexts(texts, outputFile);
                    break;
                }
                case ".uzl":
                {
                    Dictionary<string, string> texts = ZoneLayer.GetSubtitles(gameFile);

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
                    Dictionary<string, string> texts = ReadTexts(textsFile);
                    aqq model = Dialogs.SetSubtitles(gameFile, texts);
                    FileManager.Save(model, outputFile, false);
                    break;
                }
                case ".k":
                {
                    Dictionary<string, string> texts = ReadTexts(textsFile);
                    aj8 model = Knowledge.SetSubtitles(gameFile, texts);
                    FileManager.Save(model, outputFile, true);
                    break;
                }
                case ".item":
                {
                    Dictionary<string, string> texts = ReadTexts(textsFile);
                    bqj model = Items.SetSubtitles(gameFile, texts);
                    FileManager.Save(model, outputFile, true);
                    break;
                }
                case ".uz":
                {
                    Dictionary<string, string> texts = ReadTexts(textsFile);
                    cfs model = Zone.SetSubtitles(gameFile, texts);
                    FileManager.Save(model, outputFile, true);
                    break;
                }
                case ".uzl":
                {
                    Dictionary<string, string> texts = ReadTexts(textsFile);
                    cy3 model = ZoneLayer.SetSubtitles(gameFile, texts);
                    FileManager.Save(model, outputFile, true);
                    break;
                }
            }
        }

        private static Dictionary<string, string> ReadTexts(string inputFile)
        {
            string[] lines = File.ReadAllLines(inputFile);

            var result = new Dictionary<string, string>(lines.Length);

            foreach (string line in lines)
            {
                string[] split = line.Split(new[] {"<Split>"}, StringSplitOptions.None);
                result.Add(split[0], split[1].Replace("\\r", "\r").Replace("\\n", "\n"));
            }

            return result;
        }

        private static void Dump(string gameFile, string outputFile)
        {
            SerializationManager.Dump(gameFile, outputFile, true);
        }
    }
}
