using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TF.Core.Files;
using TF.IO;

namespace YakuzaCommon.Files.CmnBin
{
    public class File : BinaryTextFile
    {
        private static readonly byte[] SearchPattern = { 0x8E, 0x9A, 0x96, 0x8B };

        public File(string path, string changesFolder, Encoding encoding) : base(path, changesFolder, encoding)
        {
        }

        protected override IList<TF.Core.TranslationEntities.Subtitle> GetSubtitles()
        {
            var temp = new List<Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding, Endianness.BigEndian))
            {
                var currentIndex = input.FindPattern(SearchPattern);

                while (currentIndex != -1)
                {
                    input.ReadBytes(12); //0x0C

                    var type = input.ReadUInt64();

                    var subtitles = type == 0 ? ReadLongSubtitles(input) : ReadShortSubtitles(input);

                    temp.AddRange(subtitles);

                    currentIndex = input.FindPattern(SearchPattern);
                }
            }

            var englishSubtitles = new List<Subtitle>();
            var japaneseSubtitles = new List<Subtitle>();
            foreach (var subtitle in temp)
            {
                switch (subtitle.Language)
                {
                    case SubtitleLanguage.English:
                        englishSubtitles.Add(subtitle);
                        break;
                    case SubtitleLanguage.Japanese:
                        japaneseSubtitles.Add(subtitle);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            var result = new List<TF.Core.TranslationEntities.Subtitle>();
            result.AddRange(englishSubtitles);
            result.AddRange(japaneseSubtitles);

            LoadChanges(result);

            return result;
        }

        private IList<Subtitle> ReadLongSubtitles(ExtendedBinaryReader input)
        {
            var result = new List<Subtitle>();

            input.Skip(40); //0x28

            var numJapaneseSubs = input.ReadInt32();
            var numEnglishSubs = input.ReadInt32();

            input.Skip(16); //0x10

            for (var i = 0; i < numJapaneseSubs; i++)
            {
                input.Skip(16); //0x10

                var subtitle = ReadSubtitle(input, 256);
                subtitle.Language = SubtitleLanguage.Japanese;
                if (subtitle.Text.Length > 0)
                {
                    result.Add(subtitle);
                }
            }

            for (var i = 0; i < numEnglishSubs; i++)
            {
                input.Skip(16); //0x10

                var subtitle = ReadSubtitle(input, 256);
                subtitle.Language = SubtitleLanguage.English;
                if (subtitle.Text.Length > 0)
                {
                    result.Add(subtitle);
                }
            }

            return result;
        }

        private IList<Subtitle> ReadShortSubtitles(ExtendedBinaryReader input)
        {
            var result = new List<Subtitle>();

            input.Skip(266); //0x010A

            var numSubs = input.ReadInt32();
            input.Skip(28); 

            for (var i = 0; i < numSubs; i++)
            {
                input.Skip(16); //0x10

                var subtitle = ReadSubtitle(input, 128);
                subtitle.Language = SubtitleLanguage.Japanese;
                if (subtitle.Text.Length > 0)
                {
                    result.Add(subtitle);
                }
            }

            numSubs = input.ReadInt32();
            input.Skip(28); //0x0C

            for (var i = 0; i < numSubs; i++)
            {
                input.Skip(16); //0x10

                var subtitle = ReadSubtitle(input, 128);
                subtitle.Language = SubtitleLanguage.English;
                if (subtitle.Text.Length > 0)
                {
                    result.Add(subtitle);
                }
            }

            return result;
        }

        private Subtitle ReadSubtitle(ExtendedBinaryReader input, int subtitleLength)
        {
            var sub = ReadSubtitle(input);

            var subtitle = new Subtitle(sub, subtitleLength);
            subtitle.PropertyChanged += SubtitlePropertyChanged;

            input.Seek(subtitle.Offset + subtitle.MaxLength, SeekOrigin.Begin);

            return subtitle;
        }

        public override void Rebuild(string outputFolder)
        {
            var outputPath = System.IO.Path.Combine(outputFolder, RelativePath);
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath));
            System.IO.File.Copy(Path, outputPath, true);

            var subtitles = GetSubtitles();

            using (var fs = new FileStream(outputPath, FileMode.Open))
            using (var output = new ExtendedBinaryWriter(fs, FileEncoding, Endianness.BigEndian))
            {
                foreach (var subtitle in subtitles)
                {
                    output.Seek(subtitle.Offset, SeekOrigin.Begin);

                    var sub = subtitle as Subtitle;
                    var zeros = new byte[sub.MaxLength];
                    output.Write(zeros);

                    WriteSubtitle(output, subtitle, (int)subtitle.Offset, false);
                }
            }
        }
    }
}
