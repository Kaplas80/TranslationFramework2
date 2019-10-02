using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TF.Core.Files;
using TF.Core.POCO;
using TF.Core.TranslationEntities;
using TF.IO;
using TFGame.UnderRail.Files.Common;
using WeifenLuo.WinFormsUI.Docking;

namespace TFGame.UnderRail.Files.Common
{
    public class File : BinaryTextFile
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

        public override void Open(DockPanel panel)
        {
            _view = new GridView(this);

            _subtitles = GetSubtitles();
            _view.LoadData(_subtitles.Where(x => !string.IsNullOrEmpty(x.Text)).ToList());
            _view.Show(panel, DockState.Document);
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
                        var sub = new UnderRailSubtitle
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

        public override void SaveChanges()
        {
            using (var fs = new FileStream(ChangesFile, FileMode.Create))
            using (var output = new ExtendedBinaryWriter(fs, System.Text.Encoding.Unicode))
            {
                output.Write(ChangesFileVersion);
                output.Write(_subtitles.Count);
                foreach (var subtitle in _subtitles)
                {
                    var sub = subtitle as UnderRailSubtitle;
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
                var subs = subtitles.Select(subtitle => subtitle as UnderRailSubtitle).ToList();
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

            var dictionary = subtitles.Select(subtitle => subtitle as UnderRailSubtitle).ToDictionary(udlgSubtitle => udlgSubtitle.Id, udlgSubtitle => udlgSubtitle.Translation);

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

        protected override string GetContext(Subtitle subtitle)
        {
            return (subtitle as UnderRailSubtitle).Id.Replace(LineEnding.ShownLineEnding, LineEnding.PoLineEnding);;
        }
    }
}
