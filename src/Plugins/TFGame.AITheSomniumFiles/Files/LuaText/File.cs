using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            var tempFile = System.IO.Path.GetTempFileName();
            
            LuaTool.Decompile(Path, tempFile);
            
            var result = new List<Subtitle>();

            if (System.IO.File.Exists(tempFile))
            {
                var lines = System.IO.File.ReadAllLines(tempFile);
                System.IO.File.Delete(tempFile);

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

        public override void SaveChanges()
        {
            using (var fs = new FileStream(ChangesFile, FileMode.Create))
            using (var output = new ExtendedBinaryWriter(fs, System.Text.Encoding.Unicode))
            {
                output.Write(ChangesFileVersion);
                output.Write(_subtitles.Count);
                foreach (var subtitle in _subtitles)
                {
                    var sub = subtitle as LuaSubtitle;
                    output.WriteString(sub.Id);
                    output.WriteString(subtitle.Translation);

                    subtitle.Loaded = subtitle.Translation;
                }
            }

            NeedSaving = false;
            OnFileChanged();
        }

        protected override void LoadChanges(IList<Subtitle> subtitles)
        {
            if (HasChanges)
            {
                var subs = subtitles.Select(subtitle => subtitle as LuaSubtitle).ToList();
                using (var fs = new FileStream(ChangesFile, FileMode.Open))
                using (var input = new ExtendedBinaryReader(fs, System.Text.Encoding.Unicode))
                {
                    var version = input.ReadInt32();

                    if (version != ChangesFileVersion)
                    {
                        //File.Delete(ChangesFile);
                        return;
                    }

                    var subtitleCount = input.ReadInt32();

                    for (var i = 0; i < subtitleCount; i++)
                    {
                        var id = input.ReadString();
                        var text = input.ReadString();

                        var subtitle = subs.FirstOrDefault(x => x.Id == id);
                        if (subtitle != null)
                        {
                            subtitle.PropertyChanged -= SubtitlePropertyChanged;
                            subtitle.Translation = text;
                            subtitle.Loaded = subtitle.Translation;
                            subtitle.PropertyChanged += SubtitlePropertyChanged;
                        }
                    }
                }
            }
        }

        public override void Rebuild(string outputFolder)
        {
            var outputPath = System.IO.Path.Combine(outputFolder, RelativePath);
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath));

            var subtitles = GetSubtitles();

            var inputTempFile = System.IO.Path.GetTempFileName();
            
            LuaTool.Decompile(Path, inputTempFile);
            
            var lines = System.IO.File.ReadAllLines(inputTempFile);
            System.IO.File.Delete(inputTempFile);

            var output = new List<string>(lines.Length);

            foreach (var line in lines)
            {
                if (!line.StartsWith("text = {"))
                {
                    output.Add(line);
                    continue;
                }

                var strings = new List<string>();

                var regex = new Regex(
                    @"\[\""(?<Tag>[^""]+)\""\]\s=\s\""(?<Text>[^""]*)\""");
                var match = regex.Match(line);

                while (match.Success)
                {
                    var tag = match.Groups["Tag"].Value;
                    
                    var sub = subtitles.First(x => (x as LuaSubtitle)?.Id == tag);

                    strings.Add($"[\"{tag}\"] = \"{EncodeText(sub.Translation)}\"");
                    match = match.NextMatch();
                }

                output.Add($"text = {{{string.Join(", ", strings)}}}");
            }

            var outputTempFile = System.IO.Path.GetTempFileName();
            System.IO.File.WriteAllLines(outputTempFile, output);

            LuaTool.Compile(outputTempFile, outputPath);
            System.IO.File.Delete(outputTempFile);
        }

        public override bool Search(string searchString, string path = "")
        {
            var tempFile = System.IO.Path.GetTempFileName();

            LuaTool.Decompile(Path, tempFile);

            var result = base.Search(EncodeText(searchString), tempFile);

            System.IO.File.Delete(tempFile);

            return result;
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
