using System.Collections.Generic;
using System.IO;
using System.Linq;
using TF.Core.Files;
using TF.Core.TranslationEntities;

using ShenmueDKSharp.Files.Subtitles;

namespace TFGame.Shenmue.Files.Sub
{
    public class File : BinaryTextFile
    {

        public File(string path, string changesFolder, System.Text.Encoding encoding) : base(path, changesFolder, encoding)
        {
        }

        protected override IList<Subtitle> GetSubtitles()
        {
            var result = new List<Subtitle>();

            var subFile = new SUB(Path);

            foreach (var entry in subFile.Entries)
            {
                var txt = entry.GetText(FileEncoding);
                var subtitle = new Subtitle
                {
                    Offset = entry.Offset,
                    Text = txt,
                    Loaded = txt,
                    Translation = txt
                };
                
                subtitle.PropertyChanged += SubtitlePropertyChanged;

                result.Add(subtitle);
            }

            LoadChanges(result);

            return result;
        }

        public override void Rebuild(string outputFolder)
        {
            var outputPath = System.IO.Path.Combine(outputFolder, RelativePath);
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath));

            var subtitles = GetSubtitles();

            var subFile = new SUB(Path);

            foreach (var entry in subFile.Entries)
            {
                var subtitle = subtitles.FirstOrDefault(x => x.Offset == entry.Offset);

                if (subtitle != null && subtitle.Text != subtitle.Translation)
                {
                    entry.SetText(FileEncoding, subtitle.Translation);
                }
            }

            subFile.Write(outputPath);
        }
    }
}
