namespace TFGame.NightCry.Files.EventList
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using TF.Core.Files;
    using TF.Core.TranslationEntities;

    public class File : BinaryTextFileWithIds
    {
        public File(string gameName, string path, string changesFolder, System.Text.Encoding encoding) : base(gameName, path, changesFolder, encoding)
        {
        }

        protected override IList<Subtitle> GetSubtitles()
        {
            var result = new List<Subtitle>();

            var dictionary = new Dictionary<string, bool>();

            string text = System.IO.File.ReadAllText(Path, FileEncoding);
            text = text.Replace("\n", "\\n");

            string[] items = text.Split('■');
            for (int i = 0; i < items.Length - 1; i++)
            {
                var data = items[i].Split('◆');

                if (!dictionary.ContainsKey(data[0]))
                {
                    var subtitle = new SubtitleWithId
                    {
                        Id = data[0],
                        Offset = 0,
                        Text = data[1],
                        Loaded = data[1],
                        Translation = data[1]
                    };
                    subtitle.PropertyChanged += SubtitlePropertyChanged;
                    result.Add(subtitle);
                    dictionary.Add(data[0], true);
                }
            }

            LoadChanges(result);

            return result;
        }

        public override void Rebuild(string outputFolder)
        {
            string outputPath = System.IO.Path.Combine(outputFolder, RelativePath);
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath));

            IList<Subtitle> subtitles = GetSubtitles();
            List<SubtitleWithId> subs = subtitles.Select(subtitle => subtitle as SubtitleWithId).ToList();
            var dictionary = new Dictionary<string, SubtitleWithId>(subs.Count);
            foreach (SubtitleWithId subtitle in subs)
            {
                if (!dictionary.ContainsKey(subtitle.Id))
                {
                    dictionary.Add(subtitle.Id, subtitle);
                }
            }

            string inputText = System.IO.File.ReadAllText(Path, FileEncoding);
            inputText = inputText.Replace("\n", "\\n");
            string[] inputItems = inputText.Split('■');
            string[] outputItems = new string[inputItems.Length];

            for (int i = 0; i < inputItems.Length - 1; i++)
            {
                var data = inputItems[i].Split('◆');
                var subtitle = dictionary[data[0]];
                outputItems[i] = string.Concat(data[0], "◆", subtitle.Translation, "◆", data[2]);
            }

            outputItems[inputItems.Length - 1] = inputItems[inputItems.Length - 1];

            string outputText = string.Join("■", outputItems);
            outputText = outputText.Replace("\\n", "\n");
            System.IO.File.WriteAllText(outputPath, outputText, FileEncoding);
        }
    }
}
