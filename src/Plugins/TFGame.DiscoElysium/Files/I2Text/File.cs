namespace TFGame.DiscoElysium.Files.I2Text
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using TF.Core.Files;
    using TF.Core.TranslationEntities;
    using TF.IO;
    using TFGame.DiscoElysium.Files.Common;
    using WeifenLuo.WinFormsUI.Docking;

    public class File : BinaryTextFile
    {
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
            var result = new List<Subtitle>();
            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding))
            {
                input.Skip(0x0C);

                var termCount = input.ReadInt32();

                for (var termIndex = 0; termIndex < termCount; termIndex++)
                {
                    var term = input.ReadStringSerialized(0x04);
                    var termType = input.ReadInt32();

                    var description = input.ReadStringSerialized(0x04);

                    var languageCount = input.ReadInt32();

                    for (var i = 0; i < languageCount; i++)
                    {
                        var sub = input.ReadStringSerialized(0x04);

                        if (i == 0 && !string.IsNullOrEmpty(sub)) // Inglés
                        {
                            var subtitle = new DiscoElysiumSubtitle
                            {
                                Id = term,
                                Offset = 0,
                                Text = sub,
                                Loaded = sub,
                                Translation = sub
                            };

                            subtitle.PropertyChanged += SubtitlePropertyChanged;
                            result.Add(subtitle);
                        }
                    }

                    var flagCount = input.ReadInt32();
                    input.Skip(flagCount);
                    input.SkipPadding(0x04);

                    var languageTouchCount = input.ReadInt32();
                    for (var i = 0; i < languageTouchCount; i++)
                    {
                        input.ReadStringSerialized(0x04);
                    }
                }
            }

            LoadChanges(result);

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
                    var sub = subtitle as DiscoElysiumSubtitle;
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
                var subs = subtitles.Select(subtitle => subtitle as DiscoElysiumSubtitle).ToList();
                var dictionary = new Dictionary<string, DiscoElysiumSubtitle>(subs.Count);
                foreach (DiscoElysiumSubtitle subtitle in subs)
                {
                    dictionary.Add(subtitle.Id, subtitle);
                }

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

                        if (dictionary.TryGetValue(id, out DiscoElysiumSubtitle subtitle))
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
            var subs = subtitles.Select(subtitle => subtitle as DiscoElysiumSubtitle).ToList();
            var dictionary = new Dictionary<string, DiscoElysiumSubtitle>(subs.Count);
            foreach (DiscoElysiumSubtitle subtitle in subs)
            {
                dictionary.Add(subtitle.Id, subtitle);
            }

            using (var fsInput = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fsInput, FileEncoding))
            using (var fsOutput = new FileStream(outputPath, FileMode.Create))
            using (var output = new ExtendedBinaryWriter(fsOutput, FileEncoding))
            {
                output.Write(input.ReadBytes(0x0C));

                var termCount = input.ReadInt32();
                output.Write(termCount);
                for (var termIndex = 0; termIndex < termCount; termIndex++)
                {
                    var term = input.ReadStringSerialized(0x04);
                    output.WriteStringSerialized(term, 0x04);

                    var termType = input.ReadInt32();
                    output.Write(termType);

                    var description = input.ReadStringSerialized(0x04);
                    output.WriteStringSerialized(description, 0x04);

                    var languageCount = input.ReadInt32();
                    output.Write(languageCount);

                    for (var i = 0; i < languageCount; i++)
                    {
                        var sub = input.ReadStringSerialized(0x04);
                        if (i != 0)
                        {
                            output.WriteStringSerialized(sub, 0x04);
                        }
                        else
                        {
                            if (dictionary.TryGetValue(term, out DiscoElysiumSubtitle subtitle))
                            {
                                output.WriteStringSerialized(subtitle.Translation, 0x04);
                            }
                            else
                            {
                                output.WriteStringSerialized(sub, 0x04);
                            }
                        }
                    }

                    var flagCount = input.ReadInt32();
                    output.Write(flagCount);
                    output.Write(input.ReadBytes(flagCount));
                    input.SkipPadding(0x04);
                    output.WritePadding(0x04);

                    var languageTouchCount = input.ReadInt32();
                    output.Write(languageTouchCount);
                    for (var i = 0; i < languageTouchCount; i++)
                    {
                        var sub = input.ReadStringSerialized(0x04);
                        output.WriteStringSerialized(sub, 0x04);
                    }
                }

                var remainderLength = (int)(input.Length - input.Position);
                var remainder = input.ReadBytes(remainderLength);
                output.Write(remainder);
            }
        }

        protected override string GetContext(Subtitle subtitle)
        {
            return (subtitle as DiscoElysiumSubtitle).Id.Replace(LineEnding.ShownLineEnding, LineEnding.PoLineEnding);
        }
    }
}
