namespace TFGame.LoveEsquire.Files.FocusObj
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using TF.Core.Files;
    using TF.Core.TranslationEntities;
    using TF.IO;

    public class File : BinaryTextFileWithIds
    {
        public File(string gameName, string path, string changesFolder, System.Text.Encoding encoding) : base(gameName, path, changesFolder, encoding)
        {
        }

        protected override IList<Subtitle> GetSubtitles()
        {
            var result = new List<Subtitle>();
            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding))
            {
                int arraySize = input.ReadInt32();
                for (int i = 0; i < arraySize; i++)
                {
                    string focusObj = input.ReadStringSerialized(0x04);

                    string text = input.ReadStringSerialized(0x04);

                    var subtitle = new SubtitleWithId
                    {
                        Id = focusObj,
                        Offset = 0,
                        Text = text,
                        Loaded = text,
                        Translation = text
                    };

                    subtitle.PropertyChanged += SubtitlePropertyChanged;
                    result.Add(subtitle);
                }
            }

            LoadChanges(result);

            return result;
        }

        public override void Rebuild(string outputFolder)
        {
            var outputPath = System.IO.Path.Combine(outputFolder, RelativePath);
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath));

            var subtitles = GetSubtitles();
            var subs = subtitles.Select(subtitle => subtitle as SubtitleWithId).ToList();
            var dictionary = new Dictionary<string, SubtitleWithId>(subs.Count);
            foreach (SubtitleWithId subtitle in subs)
            {
                dictionary.Add(subtitle.Id, subtitle);
            }

            using (var fsInput = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fsInput, FileEncoding))
            using (var fsOutput = new FileStream(outputPath, FileMode.Create))
            using (var output = new ExtendedBinaryWriter(fsOutput, FileEncoding))
            {
                int arraySize = input.ReadInt32();
                output.Write(arraySize);

                for (int i = 0; i < arraySize; i++)
                {
                    string focusObj = input.ReadStringSerialized(0x04);
                    string text = input.ReadStringSerialized(0x04);

                    output.WriteStringSerialized(focusObj, 0x04);
                    output.WriteStringSerialized(dictionary.TryGetValue(focusObj, out SubtitleWithId subtitle) ? subtitle.Translation : text, 0x04);
                }
            }
        }
    }
}
