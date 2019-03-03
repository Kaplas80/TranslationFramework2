using System;
using System.Collections.Generic;
using System.IO;
using TF.Core.Exceptions;
using TF.IO;

namespace TFGame.YakuzaKiwami.Files.PocketCircuit
{
    public class File : YakuzaCommon.Files.SimpleSubtitle.File
    {
        public File(string path, string changesFolder, System.Text.Encoding encoding) : base(path, changesFolder, encoding)
        {
        }

        protected override IList<YakuzaCommon.Files.SimpleSubtitle.Subtitle> GetSubtitles()
        {
            if (HasChanges)
            {
                try
                {
                    var loadedSubs = LoadChanges(ChangesFile);
                    return loadedSubs;
                }
                catch (ChangesFileVersionMismatchException e)
                {
                    System.IO.File.Delete(ChangesFile);
                }
            }

            if (Name.StartsWith("pokecir_car.bin"))
            {
                return GetSubtitlesPOCB();
            }

            if (Name.StartsWith("pokecir_chara_set.bin"))
            {
                return GetSubtitlesPCSB();
            }

            if (Name.StartsWith("pokecir_chara.bin"))
            {
                return GetSubtitlesPOCB1();
            }

            if (Name.StartsWith("pokecir_competition.bin"))
            {
                return GetSubtitlesPOCB2();
            }

            if (Name.StartsWith("pokecir_course.bin"))
            {
                return GetSubtitlesPOCB3();
            }

            if (Name.StartsWith("pokecir_parts_type.bin"))
            {
                return GetSubtitlesPPTB();
            }

            if (Name.StartsWith("pokecir_parts.bin"))
            {
                return GetSubtitlesPOPB();
            }

            if (Name.StartsWith("pokecir_unit_set.bin"))
            {
                return GetSubtitlesPUSB();
            }

            return new List<YakuzaCommon.Files.SimpleSubtitle.Subtitle>();
        }

        private IList<YakuzaCommon.Files.SimpleSubtitle.Subtitle> GetSubtitlesPOCB()
        {
            var temp = new List<YakuzaCommon.Files.SimpleSubtitle.Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding, Endianness.BigEndian))
            {
                input.Skip(16);
                var numItems = input.ReadInt32();

                for (var i = 0; i < numItems; i++)
                {
                    input.Skip(32); // id
                    input.Skip(20); // unknown

                    var offset = input.Position;
                    var str = input.ReadString();
                    var subtitle = new Subtitle {Offset = offset, Text = str, Translation = str, Loaded = str, MaxLength = 64 };
                    
                    if (!string.IsNullOrEmpty(str))
                    {
                        subtitle.PropertyChanged += SubtitlePropertyChanged;
                        temp.Add(subtitle);
                    }

                    input.Seek(offset + 64, SeekOrigin.Begin);
                    input.Skip(4); // unknown
                }
            }

            return temp;
        }

        private IList<YakuzaCommon.Files.SimpleSubtitle.Subtitle> GetSubtitlesPCSB()
        {
            var temp = new List<YakuzaCommon.Files.SimpleSubtitle.Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding, Endianness.BigEndian))
            {
                input.Skip(16);
                var numItems = input.ReadInt32();

                for (var i = 0; i < numItems; i++)
                {
                    input.Skip(32); // id
                    input.Skip(17); // unknown

                    var offset = input.Position;
                    var str = input.ReadString();
                    var subtitle = new Subtitle { Offset = offset, Text = str, Translation = str, Loaded = str, MaxLength = 64 };

                    if (!string.IsNullOrEmpty(str))
                    {
                        subtitle.PropertyChanged += SubtitlePropertyChanged;
                        temp.Add(subtitle);
                    }
                    input.Seek(offset + 64, SeekOrigin.Begin);

                    offset = input.Position;
                    str = input.ReadString();
                    subtitle = new Subtitle { Offset = offset, Text = str, Translation = str, Loaded = str, MaxLength = 192 };

                    if (!string.IsNullOrEmpty(str))
                    {
                        subtitle.PropertyChanged += SubtitlePropertyChanged;
                        temp.Add(subtitle);
                    }
                    input.Seek(offset + 192, SeekOrigin.Begin);

                    input.SkipPadding(4);
                }
            }

            return temp;
        }

        private IList<YakuzaCommon.Files.SimpleSubtitle.Subtitle> GetSubtitlesPOCB1()
        {
            var temp = new List<YakuzaCommon.Files.SimpleSubtitle.Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding, Endianness.BigEndian))
            {
                input.Skip(16);
                var numItems = input.ReadInt32();

                for (var i = 0; i < numItems; i++)
                {
                    input.Skip(32); // id
                    input.Skip(2); // unknown

                    var offset = input.Position;
                    var str = input.ReadString();
                    var subtitle = new Subtitle { Offset = offset, Text = str, Translation = str, Loaded = str, MaxLength = 64 };

                    if (!string.IsNullOrEmpty(str))
                    {
                        subtitle.PropertyChanged += SubtitlePropertyChanged;
                        temp.Add(subtitle);
                    }
                    input.Seek(offset + 64, SeekOrigin.Begin);

                    input.Skip(42); // unknown
                }
            }

            return temp;
        }

        private IList<YakuzaCommon.Files.SimpleSubtitle.Subtitle> GetSubtitlesPOCB2()
        {
            var temp = new List<YakuzaCommon.Files.SimpleSubtitle.Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding, Endianness.BigEndian))
            {
                input.Skip(16);
                var numItems = input.ReadInt32();

                for (var i = 0; i < numItems; i++)
                {
                    input.Skip(32); // id
                    input.Skip(2); // unknown

                    var offset = input.Position;
                    var str = input.ReadString();
                    var subtitle = new Subtitle { Offset = offset, Text = str, Translation = str, Loaded = str, MaxLength = 64 };

                    if (!string.IsNullOrEmpty(str))
                    {
                        subtitle.PropertyChanged += SubtitlePropertyChanged;
                        temp.Add(subtitle);
                    }

                    input.Seek(offset + 64, SeekOrigin.Begin);

                    offset = input.Position;
                    str = input.ReadString();
                    subtitle = new Subtitle { Offset = offset, Text = str, Translation = str, Loaded = str, MaxLength = 192 };

                    if (!string.IsNullOrEmpty(str))
                    {
                        subtitle.PropertyChanged += SubtitlePropertyChanged;
                        temp.Add(subtitle);
                    }

                    input.Seek(offset + 192, SeekOrigin.Begin);

                    input.SkipPadding(4);
                }
            }

            return temp;
        }

        private IList<YakuzaCommon.Files.SimpleSubtitle.Subtitle> GetSubtitlesPOCB3()
        {
            var temp = new List<YakuzaCommon.Files.SimpleSubtitle.Subtitle>();

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

                    var offset = input.Position;
                    var str = input.ReadString();

                    var subtitle = new Subtitle { Offset = offset, Text = str, Translation = str, Loaded = str, MaxLength = 64 };
                    if (!string.IsNullOrEmpty(str))
                    {
                        subtitle.PropertyChanged += SubtitlePropertyChanged;
                        temp.Add(subtitle);
                    }
                    input.Seek(offset + 64, SeekOrigin.Begin);

                    offset = input.Position;
                    str = input.ReadString();
                    subtitle = new Subtitle { Offset = offset, Text = str, Translation = str, Loaded = str, MaxLength = 192 };

                    if (!string.IsNullOrEmpty(str))
                    {
                        subtitle.PropertyChanged += SubtitlePropertyChanged;
                        temp.Add(subtitle);
                    }
                    input.Seek(offset + 192, SeekOrigin.Begin);

                    input.SkipPadding(4);
                }
            }

            return temp;
        }

        private IList<YakuzaCommon.Files.SimpleSubtitle.Subtitle> GetSubtitlesPPTB()
        {
            var temp = new List<YakuzaCommon.Files.SimpleSubtitle.Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding, Endianness.BigEndian))
            {
                input.Skip(16);
                var numItems = input.ReadInt32();

                for (var i = 0; i < numItems; i++)
                {
                    input.Skip(32); // id
                    input.Skip(1); // unknown

                    var offset = input.Position;
                    var str = input.ReadString();

                    var subtitle = new Subtitle { Offset = offset, Text = str, Translation = str, Loaded = str, MaxLength = 64 };
                    if (!string.IsNullOrEmpty(str))
                    {
                        subtitle.PropertyChanged += SubtitlePropertyChanged;
                        temp.Add(subtitle);
                    }
                    input.Seek(offset + 64, SeekOrigin.Begin);

                    input.SkipPadding(4);
                }
            }

            return temp;
        }

        private IList<YakuzaCommon.Files.SimpleSubtitle.Subtitle> GetSubtitlesPOPB()
        {
            var temp = new List<YakuzaCommon.Files.SimpleSubtitle.Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding, Endianness.BigEndian))
            {
                input.Skip(16);
                var numItems = input.ReadInt32();

                for (var i = 0; i < numItems; i++)
                {
                    input.Skip(32); // id
                    input.Skip(140); // unknown

                    var offset = input.Position;
                    var str = input.ReadString();

                    var subtitle = new Subtitle { Offset = offset, Text = str, Translation = str, Loaded = str, MaxLength = 64 };
                    if (!string.IsNullOrEmpty(str))
                    {
                        subtitle.PropertyChanged += SubtitlePropertyChanged;
                        temp.Add(subtitle);
                    }

                    input.Seek(offset + 64, SeekOrigin.Begin);

                    offset = input.Position;
                    str = input.ReadString();
                    subtitle = new Subtitle { Offset = offset, Text = str, Translation = str, Loaded = str, MaxLength = 192 };

                    if (!string.IsNullOrEmpty(str))
                    {
                        subtitle.PropertyChanged += SubtitlePropertyChanged;
                        temp.Add(subtitle);
                    }

                    input.Seek(offset + 192, SeekOrigin.Begin);

                    input.Skip(4);
                }
            }

            return temp;
        }

        private IList<YakuzaCommon.Files.SimpleSubtitle.Subtitle> GetSubtitlesPUSB()
        {
            var temp = new List<YakuzaCommon.Files.SimpleSubtitle.Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding, Endianness.BigEndian))
            {
                input.Skip(16);
                var numItems = input.ReadInt32();

                for (var i = 0; i < numItems; i++)
                {
                    input.Skip(32); // id
                    input.Skip(11); // unknown

                    var offset = input.Position;
                    var str = input.ReadString();

                    var subtitle = new Subtitle { Offset = offset, Text = str, Translation = str, Loaded = str, MaxLength = 64 };
                    if (!string.IsNullOrEmpty(str))
                    {
                        subtitle.PropertyChanged += SubtitlePropertyChanged;
                        temp.Add(subtitle);
                    }

                    input.Seek(offset + 64, SeekOrigin.Begin);

                    input.Skip(193); // unknown
                }
            }

            return temp;
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
                    var sub = subtitle as Subtitle;
                    output.Write(sub.Offset);
                    output.Write(sub.MaxLength);
                    output.WriteString(sub.Text);
                    output.WriteString(sub.Translation);

                    sub.Loaded = sub.Translation;
                }
            }

            NeedSaving = false;
            OnFileChanged();
        }

        private new IList<YakuzaCommon.Files.SimpleSubtitle.Subtitle> LoadChanges(string file)
        {
            using (var fs = new FileStream(file, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, System.Text.Encoding.Unicode))
            {
                var version = input.ReadInt32();

                if (version != ChangesFileVersion)
                {
                    throw new ChangesFileVersionMismatchException();
                }

                var result = new List<YakuzaCommon.Files.SimpleSubtitle.Subtitle>();
                var subtitleCount = input.ReadInt32();

                for (var i = 0; i < subtitleCount; i++)
                {
                    var sub = new Subtitle
                    {
                        Offset = input.ReadInt64(),
                        MaxLength = input.ReadInt32(),
                        Text = input.ReadString(),
                        Translation = input.ReadString()
                    };

                    sub.Loaded = sub.Translation;

                    sub.PropertyChanged += SubtitlePropertyChanged;

                    result.Add(sub);
                }

                return result;
            }
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
                    output.Seek(sub.Offset, SeekOrigin.Begin);
                    output.WriteString(sub.Translation);
                }
            }
        }
    }
}
