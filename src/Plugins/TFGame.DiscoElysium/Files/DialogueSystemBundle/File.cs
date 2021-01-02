namespace TFGame.DiscoElysium.Files.DialogueSystemBundle
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using TF.Core.TranslationEntities;
    using TF.IO;

    public class File : DialogueSystem.File
    {
        public File(string gameName, string path, string changesFolder, System.Text.Encoding encoding) : base(gameName, path, changesFolder, encoding)
        {
        }

        protected override IList<Subtitle> GetSubtitles()
        {
            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding))
            {
                input.Skip(0x04);
                input.Skip(0x08);
                input.Skip(0x04);
                input.Skip(0x04);
                input.Skip(0x08);
                input.ReadStringSerialized(0x04); // m_name

                return GetSubtitles(input);
            }
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
                output.Write(input.ReadBytes(0x04));
                output.Write(input.ReadBytes(0x08));
                output.Write(input.ReadBytes(0x04));
                output.Write(input.ReadBytes(0x04));
                output.Write(input.ReadBytes(0x08));
                output.WriteStringSerialized(input.ReadStringSerialized(0x04), 0x04);
                
                Rebuild(input, output, dictionary);
            }
        }
    }
}
