﻿namespace TFGame.LoveEsquire.Files.UnityUIText
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
                // Material
                input.Skip(0x04);
                input.Skip(0x08);

                // ColorRGBA
                input.Skip(4 * 0x04);

                // RaycastTarget
                input.Skip(0x04);

                // CullStateChangedEvent
                int persistentCallCount = input.ReadInt32();
                for (int i = 0; i < persistentCallCount; i++)
                {
                    // PersistentCall
                }

                // FontData
                input.Skip(0x04);
                input.Skip(0x08);
                input.Skip(0x04); // Font size
                input.Skip(0x04); // Font style
                input.Skip(0x04); // Best Fit
                input.Skip(0x04); // Min size
                input.Skip(0x04); // Max size
                input.Skip(0x04); // Alignment
                input.Skip(0x04); // Align by geometry
                input.Skip(0x04); // Rich Text
                input.Skip(0x04); // Horizontal Overflow
                input.Skip(0x04); // Vertical Overflow
                input.Skip(0x04); // Line spacing

                string sub = input.ReadStringSerialized(0x04);

                var subtitle = new SubtitleWithId
                {
                    Id = sub,
                    Offset = 0,
                    Text = sub,
                    Loaded = sub,
                    Translation = sub
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
                output.Write(input.ReadBytes(0x04)); // Best Fit
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
