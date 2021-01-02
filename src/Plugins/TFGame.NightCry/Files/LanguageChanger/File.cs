namespace TFGame.NightCry.Files.LanguageChanger
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
                var count = input.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    // UILabel
                    input.Skip(0x04);
                    input.Skip(0x08);
                    var strJap = input.ReadStringSerialized(0x04);
                    var strEng = input.ReadStringSerialized(0x04);

                    var subtitle = new SubtitleWithId
                    {
                        Id = (i + 1).ToString(),
                        Offset = 0,
                        Text = strEng,
                        Loaded = strEng,
                        Translation = strEng
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
                var count = input.ReadInt32();
                output.Write(count);
                for (int i = 0; i < count; i++)
                {
                    // UILabel
                    output.Write(input.ReadBytes(0x04));
                    output.Write(input.ReadBytes(0x08));

                    var strJap = input.ReadStringSerialized(0x04);
                    var strEng = input.ReadStringSerialized(0x04);

                    output.WriteStringSerialized(strJap, 0x04);
                    output.WriteStringSerialized(dictionary.TryGetValue((i + 1).ToString(), out SubtitleWithId subtitle) ? subtitle.Translation : strEng, 0x04);
                }
            }
        }
    }
}
