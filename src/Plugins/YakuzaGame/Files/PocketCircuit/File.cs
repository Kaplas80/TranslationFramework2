using System.Collections.Generic;
using System.IO;
using System.Text;
using TF.Core.Files;
using TF.Core.TranslationEntities;
using TF.IO;

namespace YakuzaGame.Files.PocketCircuit
{
    public class File : BinaryTextFile
    {
        public File(string path, string changesFolder, System.Text.Encoding encoding) : base(path, changesFolder, encoding)
        {
        }

        protected override IList<Subtitle> GetSubtitles()
        {
            IList<Subtitle> result;

            if (Name.StartsWith("pokecir_car.bin"))
            {
                result = GetSubtitlesPOCB();
            }
            else if (Name.StartsWith("pokecir_chara_set.bin"))
            {
                result = GetSubtitlesPCSB();
            }
            else if (Name.StartsWith("pokecir_chara.bin"))
            {
                result = GetSubtitlesPOCB1();
            }
            else if (Name.StartsWith("pokecir_competition.bin"))
            {
                result = GetSubtitlesPOCB2();
            }
            else if (Name.StartsWith("pokecir_course.bin"))
            {
                result = GetSubtitlesPOCB3();
            }
            else if (Name.StartsWith("pokecir_parts_type.bin"))
            {
                result = GetSubtitlesPPTB();
            }
            else if (Name.StartsWith("pokecir_parts.bin"))
            {
                result = GetSubtitlesPOPB();
            }
            else if (Name.StartsWith("pokecir_unit_set.bin"))
            {
                result = GetSubtitlesPUSB();
            }
            else
            {
                result = new List<Subtitle>();
            }

            LoadChanges(result);

            return result;
        }

        private IList<Subtitle> GetSubtitlesPOCB()
        {
            var temp = new List<Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding, Endianness.BigEndian))
            {
                input.Skip(16);
                var numItems = input.ReadInt32();

                for (var i = 0; i < numItems; i++)
                {
                    input.Skip(32); // id
                    input.Skip(20); // unknown

                    var subtitle = ReadSubtitle(input, 64);
                    if (!string.IsNullOrEmpty(subtitle.Text))
                    {
                        temp.Add(subtitle);
                    }

                    input.Skip(4); // unknown
                }
            }

            return temp;
        }

        private IList<Subtitle> GetSubtitlesPCSB()
        {
            var temp = new List<Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding, Endianness.BigEndian))
            {
                input.Skip(16);
                var numItems = input.ReadInt32();

                for (var i = 0; i < numItems; i++)
                {
                    input.Skip(32); // id
                    input.Skip(17); // unknown

                    var subtitle = ReadSubtitle(input, 64);
                    if (!string.IsNullOrEmpty(subtitle.Text))
                    {
                        temp.Add(subtitle);
                    }

                    subtitle = ReadSubtitle(input, 192);
                    if (!string.IsNullOrEmpty(subtitle.Text))
                    {
                        temp.Add(subtitle);
                    }

                    input.SkipPadding(4);
                }
            }

            return temp;
        }

        private IList<Subtitle> GetSubtitlesPOCB1()
        {
            var temp = new List<Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding, Endianness.BigEndian))
            {
                input.Skip(16);
                var numItems = input.ReadInt32();

                for (var i = 0; i < numItems; i++)
                {
                    input.Skip(32); // id
                    input.Skip(2); // unknown

                    var subtitle = ReadSubtitle(input, 64);
                    if (!string.IsNullOrEmpty(subtitle.Text))
                    {
                        temp.Add(subtitle);
                    }

                    input.Skip(42); // unknown
                }
            }

            return temp;
        }

        private IList<Subtitle> GetSubtitlesPOCB2()
        {
            var temp = new List<Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding, Endianness.BigEndian))
            {
                input.Skip(16);
                var numItems = input.ReadInt32();

                for (var i = 0; i < numItems; i++)
                {
                    input.Skip(32); // id
                    input.Skip(2); // unknown

                    var subtitle = ReadSubtitle(input, 64);
                    if (!string.IsNullOrEmpty(subtitle.Text))
                    {
                        temp.Add(subtitle);
                    }

                    subtitle = ReadSubtitle(input, 192);
                    if (!string.IsNullOrEmpty(subtitle.Text))
                    {
                        temp.Add(subtitle);
                    }

                    input.SkipPadding(4);
                }
            }

            return temp;
        }

        private IList<Subtitle> GetSubtitlesPOCB3()
        {
            var temp = new List<Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding, Endianness.BigEndian))
            {
                input.Skip(16);
                var numItems = input.ReadInt32();

                for (var i = 0; i < numItems; i++)
                {
                    input.Skip(32); // id
                    input.Skip(32); // bin
                    input.Skip(3); // unknown

                    var subtitle = ReadSubtitle(input, 64);
                    if (!string.IsNullOrEmpty(subtitle.Text))
                    {
                        temp.Add(subtitle);
                    }

                    subtitle = ReadSubtitle(input, 192);
                    if (!string.IsNullOrEmpty(subtitle.Text))
                    {
                        temp.Add(subtitle);
                    }

                    input.SkipPadding(4);
                }
            }

            return temp;
        }

        private IList<Subtitle> GetSubtitlesPPTB()
        {
            var temp = new List<Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding, Endianness.BigEndian))
            {
                input.Skip(16);
                var numItems = input.ReadInt32();

                for (var i = 0; i < numItems; i++)
                {
                    input.Skip(32); // id
                    input.Skip(1); // unknown

                    var subtitle = ReadSubtitle(input, 64);
                    if (!string.IsNullOrEmpty(subtitle.Text))
                    {
                        temp.Add(subtitle);
                    }

                    input.SkipPadding(4);
                }
            }

            return temp;
        }

        private IList<Subtitle> GetSubtitlesPOPB()
        {
            var temp = new List<Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding, Endianness.BigEndian))
            {
                input.Skip(16);
                var numItems = input.ReadInt32();

                for (var i = 0; i < numItems; i++)
                {
                    input.Skip(32); // id
                    input.Skip(140); // unknown

                    var subtitle = ReadSubtitle(input, 64);
                    if (!string.IsNullOrEmpty(subtitle.Text))
                    {
                        temp.Add(subtitle);
                    }

                    subtitle = ReadSubtitle(input, 192);
                    if (!string.IsNullOrEmpty(subtitle.Text))
                    {
                        temp.Add(subtitle);
                    }

                    input.Skip(4);
                }
            }

            return temp;
        }

        private IList<Subtitle> GetSubtitlesPUSB()
        {
            var temp = new List<Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding, Endianness.BigEndian))
            {
                input.Skip(16);
                var numItems = input.ReadInt32();

                for (var i = 0; i < numItems; i++)
                {
                    input.Skip(32); // id
                    input.Skip(11); // unknown

                    var subtitle = ReadSubtitle(input, 64);
                    if (!string.IsNullOrEmpty(subtitle.Text))
                    {
                        temp.Add(subtitle);
                    }

                    input.Skip(193); // unknown
                }
            }

            return temp;
        }

        protected Subtitle ReadSubtitle(ExtendedBinaryReader input, int subtitleLength)
        {
            var sub = ReadSubtitle(input);

            var subtitle = new FixedLengthSubtitle(sub, subtitleLength);
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

                    var sub = subtitle as FixedLengthSubtitle;
                    var zeros = new byte[sub.MaxLength];
                    output.Write(zeros);
                    output.Seek(sub.Offset, SeekOrigin.Begin);
                    output.WriteString(sub.Translation);
                }
            }
        }
    }
}
