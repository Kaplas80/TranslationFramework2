using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TF.Core.Files;
using TF.Core.POCO;
using TF.Core.TranslationEntities;

namespace TFGame.UnderRail.Files.Common
{
    public class File : BinaryTextFileWithIds
    {
        public override LineEnding LineEnding => new LineEnding
        {
            RealLineEnding = "\r\n",
            ShownLineEnding = "\\r\\n",
            PoLineEnding = "\n",
            ScintillaLineEnding = ScintillaLineEndings.CrLf,
        };

        public File(string gameName, string path, string changesFolder, System.Text.Encoding encoding) : base(gameName, path, changesFolder, encoding)
        {
        }

        protected override IList<Subtitle> GetSubtitles()
        {
            var tempFile = System.IO.Path.GetTempFileName();

            UnderRailTool.Run("r", Path, tempFile, string.Empty);

            var result = new List<Subtitle>();

            if (System.IO.File.Exists(tempFile))
            {
                var lines = System.IO.File.ReadAllLines(tempFile);
                System.IO.File.Delete(tempFile);

                if (lines.Length > 0)
                {
                    var dictionary = lines.Select(line => line.Split(new[] {"<Split>"}, StringSplitOptions.None))
                        .ToDictionary(split => split[0], split => split[1]);

                    foreach (var pair in dictionary)
                    {
                        var sub = new SubtitleWithId
                        {
                            Id = pair.Key,
                            Text = pair.Value,
                            Loaded = pair.Value,
                            Translation = pair.Value,
                            Offset = 0,
                        };
                        sub.PropertyChanged += SubtitlePropertyChanged;

                        result.Add(sub);
                    }

                    LoadChanges(result);
                }
            }

            return result;
        }

        public override void Rebuild(string outputFolder)
        {
            var outputPath = System.IO.Path.Combine(outputFolder, RelativePath);
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath));

            var subtitles = GetSubtitles();

            var dictionary = subtitles.Select(subtitle => subtitle as SubtitleWithId).ToDictionary(udlgSubtitle => udlgSubtitle.Id, udlgSubtitle => udlgSubtitle.Translation);

            var tempFile = System.IO.Path.GetTempFileName();
            var lines = new List<string>(dictionary.Count);
            lines.AddRange(dictionary.Select(text => $"{text.Key}<Split>{text.Value}"));

            System.IO.File.WriteAllLines(tempFile, lines);

            UnderRailTool.Run("w", Path, tempFile, outputPath);

            System.IO.File.Delete(tempFile);
        }

        public override bool Search(string searchString, string path = "")
        {
            if (Path.EndsWith(".udlg"))
            {
                return base.Search(searchString);
            }

            var tempFile = System.IO.Path.GetTempFileName();

            UnderRailTool.Run("d", Path, tempFile, string.Empty);

            var result = base.Search(searchString, tempFile);

            System.IO.File.Delete(tempFile);

            return result;
        }
    }
}
