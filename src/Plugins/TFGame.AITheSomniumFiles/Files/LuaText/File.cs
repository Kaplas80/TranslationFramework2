using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TF.Core.Files;
using TF.Core.TranslationEntities;
using TF.IO;
using WeifenLuo.WinFormsUI.Docking;

namespace TFGame.AITheSomniumFiles.Files.LuaText
{
    public class File : BinaryTextFile
    {
        public override string LineEnding => "\n";

        public File(string path, string changesFolder, System.Text.Encoding encoding) : base(path, changesFolder, encoding)
        {
        }

        public override void Open(DockPanel panel)
        {
            _view = new GridView(LineEnding);

            _subtitles = GetSubtitles();
            _view.LoadData(_subtitles.Where(x => !string.IsNullOrEmpty(x.Text)).ToList());
            _view.Show(panel, DockState.Document);
        }

        protected override IList<Subtitle> GetSubtitles()
        {
            var tempFile1 = System.IO.Path.GetTempFileName();
            var tempFile2 = System.IO.Path.GetTempFileName();
            Decrypt(Path, tempFile1);

            Debug.WriteLine($"Decompiling: {Path}");
            LuaTool.Decompile(tempFile1, tempFile2);
            Debug.WriteLine($"Decompiled");

            System.IO.File.Delete(tempFile1);
            var result = new List<Subtitle>();

            if (System.IO.File.Exists(tempFile2))
            {
                var lines = System.IO.File.ReadAllLines(tempFile2);
                System.IO.File.Delete(tempFile2);

                foreach (var line in lines)
                {
                    if (!line.StartsWith("text = {"))
                    {
                        continue;
                    }

                    var regex = new Regex(
                        @"\[\""(?<Tag>[^""]+)\""\]\s=\s\""(?<Text>[^""]*)\""");
                    var match = regex.Match(line);

                    while (match.Success)
                    {
                        var tag = match.Groups["Tag"].Value;
                        var text = DecodeText(match.Groups["Text"].Value);
                        
                        var sub = new LuaSubtitle
                        {
                            Id = tag,
                            Text = text,
                            Loaded = text,
                            Translation = text,
                            Offset = 0,
                        };
                        sub.PropertyChanged += SubtitlePropertyChanged;

                        result.Add(sub);

                        match = match.NextMatch();
                    }
                }
                
                LoadChanges(result);
            }

            return result;
        }

        //public override void SaveChanges()
        //{
        //    using (var fs = new FileStream(ChangesFile, FileMode.Create))
        //    using (var output = new ExtendedBinaryWriter(fs, System.Text.Encoding.Unicode))
        //    {
        //        output.Write(ChangesFileVersion);
        //        output.Write(_subtitles.Count);
        //        foreach (var subtitle in _subtitles)
        //        {
        //            var sub = subtitle as UnderRailSubtitle;
        //            output.WriteString(sub.Id);
        //            output.WriteString(subtitle.Translation);

        //            subtitle.Loaded = subtitle.Translation;
        //        }
        //    }

        //    NeedSaving = false;
        //    OnFileChanged();
        //}

        //protected override void LoadChanges(IList<Subtitle> subtitles)
        //{
        //    if (HasChanges)
        //    {
        //        var subs = subtitles.Select(subtitle => subtitle as UnderRailSubtitle).ToList();
        //        using (var fs = new FileStream(ChangesFile, FileMode.Open))
        //        using (var input = new ExtendedBinaryReader(fs, System.Text.Encoding.Unicode))
        //        {
        //            var version = input.ReadInt32();

        //            if (version != ChangesFileVersion)
        //            {
        //                //File.Delete(ChangesFile);
        //                return;
        //            }

        //            var subtitleCount = input.ReadInt32();

        //            for (var i = 0; i < subtitleCount; i++)
        //            {
        //                var id = input.ReadString();
        //                var text = input.ReadString();

        //                var subtitle = subs.FirstOrDefault(x => x.Id == id);
        //                if (subtitle != null)
        //                {
        //                    subtitle.PropertyChanged -= SubtitlePropertyChanged;
        //                    subtitle.Translation = text;
        //                    subtitle.Loaded = subtitle.Translation;
        //                    subtitle.PropertyChanged += SubtitlePropertyChanged;
        //                }
        //            }
        //        }
        //    }
        //}

        //public override void Rebuild(string outputFolder)
        //{
        //    var outputPath = System.IO.Path.Combine(outputFolder, RelativePath);
        //    Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath));

        //    var subtitles = GetSubtitles();

        //    var dictionary = subtitles.Select(subtitle => subtitle as UnderRailSubtitle).ToDictionary(udlgSubtitle => udlgSubtitle.Id, udlgSubtitle => udlgSubtitle.Translation);

        //    var tempFile = System.IO.Path.GetTempFileName();
        //    var lines = new List<string>(dictionary.Count);
        //    lines.AddRange(dictionary.Select(text => $"{text.Key}<Split>{text.Value}"));

        //    System.IO.File.WriteAllLines(tempFile, lines);

        //    UnderRailTool.Run("w", Path, tempFile, outputPath);

        //    System.IO.File.Delete(tempFile);
        //}

        //public override bool Search(string searchString, string path = "")
        //{
        //    if (Path.EndsWith(".udlg"))
        //    {
        //        return base.Search(searchString);
        //    }

        //    var tempFile = System.IO.Path.GetTempFileName();

        //    UnderRailTool.Run("d", Path, tempFile, string.Empty);

        //    var result = base.Search(searchString, tempFile);

        //    System.IO.File.Delete(tempFile);

        //    return result;
        //}

        private static void Decrypt(string inputFile, string outputFile)
        {
            var data = System.IO.File.ReadAllBytes(inputFile);

            for (var i = 4; i < data.Length; i++)
            {
                data[i] ^= (byte) i;
            }

            System.IO.File.WriteAllBytes(outputFile, data);
        }

        private static readonly Tuple<string, string>[] Replacements = new Tuple<string, string>[]
        {
            new Tuple<string, string>("\\'", "'"),
            new Tuple<string, string>("&quot;", "\""),
        };
        private static string DecodeText(string input)
        {
            var result = input;
            foreach (var (encodedString, decodedString) in Replacements)
            {
                result = result.Replace(encodedString, decodedString);
            }

            return result;
        }

        private static string EncodeText(string input)
        {
            var result = input;
            foreach (var (encodedString, decodedString) in Replacements)
            {
                result = result.Replace(decodedString, encodedString);
            }

            return result;
        }
    }
}
