namespace UnityGame.Files.UnityUIText2019
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using TF.Core.TranslationEntities;
    using TF.IO;

    public class FileWithBestFit : File
    {
        public FileWithBestFit(string gameName, string path, string changesFolder, System.Text.Encoding encoding) : base(gameName, path, changesFolder, encoding)
        {
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
                // Material
                output.Write(input.ReadBytes(0x04));
                output.Write(input.ReadBytes(0x08));

                // ColorRGBA
                output.Write(input.ReadBytes(4 * 0x04));

                // RaycastTarget
                output.Write(input.ReadBytes(0x04));

                // CullStateChangedEvent
                int persistentCallCount = input.ReadInt32();
                output.Write(persistentCallCount);
                for (int i = 0; i < persistentCallCount; i++)
                {
                    // PersistentCall
                }

                // FontData
                output.Write(input.ReadBytes(0x04));
                output.Write(input.ReadBytes(0x08));
                output.Write(input.ReadBytes(0x04)); // FOnt size
                output.Write(input.ReadBytes(0x04)); // Font style
                input.ReadBytes(0x04);
                output.Write(0x00000001); // Best Fit
                output.Write(input.ReadBytes(0x04)); // Min size
                output.Write(input.ReadBytes(0x04)); // Max size
                output.Write(input.ReadBytes(0x04)); // Alignment
                output.Write(input.ReadBytes(0x04)); // Align by geometry
                output.Write(input.ReadBytes(0x04)); // Rich Text
                output.Write(input.ReadBytes(0x04)); // Horizontal Overflow
                output.Write(input.ReadBytes(0x04)); // Vertical Overflow
                output.Write(input.ReadBytes(0x04)); // Line spacing

                string sub = input.ReadStringSerialized(0x04);

                output.WriteStringSerialized(
                    dictionary.TryGetValue(sub, out SubtitleWithId subtitle) ? subtitle.Translation : sub, 0x04);
            }
        }
    }
}
