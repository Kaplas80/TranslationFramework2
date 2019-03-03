using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using TF.Core.Entities;
using TF.Core.Exceptions;
using TF.IO;
using WeifenLuo.WinFormsUI.Docking;

namespace YakuzaCommon.Files.Msg
{
    public class File : SimpleSubtitle.File
    {
        public File(string path, string changesFolder, Encoding encoding) : base(path, changesFolder, encoding)
        {
        }

        public override int SubtitleCount
        {
            get
            {
                var subtitles = GetSubtitles();
                return subtitles.Count;
            }
        }
#if DEBUG
        protected new IList<Subtitle> _subtitles;
        private new View _view;
        public override void Open(DockPanel panel, ThemeBase theme)
        {
            _view = new View(theme);
            var subtitles = GetSubtitles();
            _subtitles = new List<Subtitle>(subtitles.Count);
            foreach (var subtitle in subtitles)
            {
                _subtitles.Add(subtitle as Subtitle);
            }

            _view.LoadSubtitles(_subtitles.Where(x => !string.IsNullOrEmpty(x.Text)).ToList());
            _view.Show(panel, DockState.Document);
        }
        
        protected override void SubtitlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NeedSaving = _subtitles.Any(subtitle => subtitle.Loaded != subtitle.Translation);
            OnFileChanged();
        }
#else
        public override void Open(DockPanel panel, ThemeBase theme)
        {
            _view = new SimpleSubtitle.View(theme);
            _subtitles = GetSubtitles();
            _view.LoadSubtitles(_subtitles.Where(x => !string.IsNullOrEmpty(x.Text)).ToList());
            _view.Show(panel, DockState.Document);
        }
#endif
        protected override IList<SimpleSubtitle.Subtitle> GetSubtitles()
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

            var result = new List<SimpleSubtitle.Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding, Endianness.BigEndian))
            {
                input.Skip(3);
                var count = input.ReadByte();
                input.Skip(4);
                var pointer1 = input.ReadInt32();
                var countPointer1 = input.ReadInt16();

                var numTalkers = input.ReadInt16();
                var pointerTalkers = input.ReadInt32();
                var pointerRemainder = input.ReadInt32();

                for (var i = 0; i < count; i++)
                {
                    input.Skip(4);
                    var groupOffset = input.ReadInt32();
                    input.Skip(1);
                    var stringCount = input.ReadByte();
                    input.Skip(6);

                    var subs = ReadSubtitles(input, groupOffset, stringCount);

                    if (subs.Count > 0)
                    {
                        result.AddRange(subs);
                    }
                }

                if (pointerTalkers > 0)
                {
                    input.Seek(pointerTalkers, SeekOrigin.Begin);
                    for (var i = 0; i < numTalkers; i++)
                    {
                        var offset = input.ReadInt32();
                        var sub = ReadSubtitle(input, offset);
                        if (sub != null)
                        {
                            result.Add(ReadSubtitle(input, offset));
                        }
                    }
                }
            }

            return result;
        }

        private IList<Subtitle> ReadSubtitles(ExtendedBinaryReader input, long groupOffset, int count)
        {
            var result = new List<Subtitle>();

            var returnPos = input.Position;

            input.Seek(groupOffset, SeekOrigin.Begin);

            for (var i = 0; i < count; i++)
            {
                var strLength = input.ReadInt16();
                var propCount = input.ReadByte();
                var zero = input.ReadByte();
                var strOffset = input.ReadInt32();
                var propOffset = input.ReadInt32();

                var subtitle = ReadSubtitle(input, strOffset);
                if (subtitle != null)
                {
                    var stringCharCount = subtitle.CleanText().Length;
                    subtitle.Properties = ReadProperties(input, propOffset, propCount);
                    foreach (var property in subtitle.Properties)
                    {
                        if (property.Position == stringCharCount)
                        {
                            property.IsEndProperty = true;
                        }
#if DEBUG
                        if (property.GetType().Name == nameof(MsgProperty))
                        {
                            if (!property.IsEndProperty && property.Position != 0)
                            {
                                //throw new UnknownPropertyException($"{Path}\t{subtitle.Text}");
                                var data = property.ToByteArray();

                                Debug.WriteLine($"{this.Name}\t{data[0]:X2}{data[1]:X2}\t{subtitle.Text}");
                            }
                        }
#endif
                    }
                    result.Add(subtitle);
                }
            }

            input.Seek(returnPos, SeekOrigin.Begin);

            return result;
        }

        private Subtitle ReadSubtitle(ExtendedBinaryReader input, int offset)
        {
            var result = new Subtitle { Offset = offset };
            var pos = input.Position;
            input.Seek(offset, SeekOrigin.Begin);
            result.Text = input.ReadString();
            result.Loaded = result.Text;
            result.Translation = result.Text;
            result.PropertyChanged += SubtitlePropertyChanged;
            input.Seek(pos, SeekOrigin.Begin);

            if (result.Offset == 0 || string.IsNullOrEmpty(result.Text))
            {
                result = null;
            }

            return result;
        }

        private MsgProperties ReadProperties(ExtendedBinaryReader input, int offset, int count)
        {
            var pos = input.Position;
            input.Seek(offset, SeekOrigin.Begin);
            var data = input.ReadBytes(16 * count);
            input.Seek(pos, SeekOrigin.Begin);

            var result = new MsgProperties(data);
            return result;
        }

        protected override IList<SimpleSubtitle.Subtitle> LoadChanges(string file)
        {
            using (var fs = new FileStream(file, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, System.Text.Encoding.Unicode))
            {
                var version = input.ReadInt32();

                if (version != ChangesFileVersion)
                {
                    throw new ChangesFileVersionMismatchException();
                }

                var result = new List<SimpleSubtitle.Subtitle>();
                var subtitleCount = input.ReadInt32();

                for (var i = 0; i < subtitleCount; i++)
                {
                    var subtitle = new Subtitle
                    {
                        Offset = input.ReadInt64(),
                        Text = input.ReadString(),
                        Translation = input.ReadString()
                    };

                    var propertiesSize = input.ReadInt32();
                    if (propertiesSize > 0)
                    {
                        var stringCharCount = subtitle.CleanText().Length;
                        var data = input.ReadBytes(propertiesSize);
                        subtitle.Properties = new MsgProperties(data);

                        foreach (var property in subtitle.Properties)
                        {
                            if (property.Position == stringCharCount)
                            {
                                property.IsEndProperty = true;
                            }
                        }
                    }

                    subtitle.Loaded = subtitle.Translation;

                    subtitle.PropertyChanged += SubtitlePropertyChanged;

                    result.Add(subtitle);
                }

                return result;
            }
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
                    output.Write(subtitle.Offset);
                    output.WriteString(subtitle.Text);
                    output.WriteString(subtitle.Translation);

                    if (subtitle.Properties == null)
                    {
                        output.Write(0);
                    }
                    else
                    {
                        var data = subtitle.Properties.ToByteArray();
                        output.Write(data.Length);
                        output.Write(data);
                    }

                    subtitle.Loaded = subtitle.Translation;
                }
            }

            NeedSaving = false;
            OnFileChanged();
        }

        public override void Rebuild(string outputFolder)
        {
            var outputPath = System.IO.Path.Combine(outputFolder, RelativePath);
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath));

            var subtitles = GetSubtitles();

            using (var fsInput = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fsInput, FileEncoding, Endianness.BigEndian))
            using (var fsOutput = new FileStream(outputPath, FileMode.Create))
            using (var output = new ExtendedBinaryWriter(fsOutput, FileEncoding, Endianness.BigEndian))
            {
                output.Write(input.ReadBytes(3));
                var count = input.ReadByte();
                output.Write(count);
                output.Write(input.ReadBytes(4));
                var inputPointer1 = input.ReadInt32();
                var inputCountPointer1 = input.ReadInt16();

                var dataPointer1 = new byte[inputCountPointer1 * 16];
                if (inputCountPointer1 > 0)
                {
                    input.Seek(inputPointer1, SeekOrigin.Begin);
                    dataPointer1 = input.ReadBytes(inputCountPointer1 * 16);
                    input.Seek(14, SeekOrigin.Begin);
                }

                output.Write(0); // Si no es 0, ya lo rellenaré luego
                output.Write(inputCountPointer1);
                
                var numTalkers = input.ReadInt16();
                output.Write(numTalkers);

                var inputPointerTalkers = input.ReadInt32();
                output.Write(0); // Si no es 0, ya lo rellenaré luego

                var inputPointerRemainder = input.ReadInt32();
                output.Write(0); // Si no es 0, ya lo rellenaré luego

                using (var propertiesMemoryStream = new MemoryStream())
                using (var outputProperties = new ExtendedBinaryWriter(propertiesMemoryStream, FileEncoding, Endianness.BigEndian))
                using (var stringsMemoryStream = new MemoryStream())
                using (var outputStrings = new ExtendedBinaryWriter(stringsMemoryStream, FileEncoding, Endianness.BigEndian))
                {
                    var stringLengths = new List<short>();
                    var stringOffsets = new List<int>();
                    var propertiesCount = new List<byte>();
                    var propertiesOffsets = new List<int>();

                    var strOffset = 0;
                    var propOffset = 0;
                    var totalStrCount = 0;
                    for (var i = 0; i < count; i++)
                    {
                        output.Write(input.ReadBytes(4));
                        var groupOffset = input.ReadInt32();
                        output.Write(groupOffset);
                        output.Write(input.ReadBytes(1));

                        var stringCount = input.ReadByte();
                        output.Write(stringCount);
                        totalStrCount += stringCount;
                        output.Write(input.ReadBytes(6));

                        var returnPos = input.Position;
                        input.Seek(groupOffset, SeekOrigin.Begin);

                        for (var j = 0; j < stringCount; j++)
                        {
                            var inputStringLength = input.ReadInt16();
                            var inputPropertiesCount = input.ReadByte();
                            var zero = input.ReadByte();
                            var inputStringOffset = input.ReadInt32();
                            var inputPropertiesOffset = input.ReadInt32();

                            if (inputStringLength == 0)
                            {
                                stringLengths.Add(0);
                                propertiesCount.Add(inputPropertiesCount);
                                stringOffsets.Add(strOffset);
                                propertiesOffsets.Add(propOffset);

                                outputStrings.WriteString(string.Empty);
                                strOffset = (int)outputStrings.Position;

                                var ret = input.Position;
                                input.Seek(inputPropertiesOffset, SeekOrigin.Begin);
                                var prop = input.ReadBytes(inputPropertiesCount * 16);
                                input.Seek(ret, SeekOrigin.Begin);

                                outputProperties.Write(prop);
                                propOffset = (int)outputProperties.Position;
                            }
                            else
                            {
                                var sub = (Subtitle)subtitles.First(x => x.Offset == inputStringOffset);

                                stringLengths.Add((short)FileEncoding.GetByteCount(sub.Translation));
                                propertiesCount.Add((byte)sub.TranslationProperties.Count);
                                stringOffsets.Add(strOffset);
                                propertiesOffsets.Add(propOffset);

                                outputStrings.WriteString(sub.Translation);
                                strOffset = (int)outputStrings.Position;
                                outputProperties.Write(sub.TranslationProperties.ToByteArray());
                                propOffset = (int)outputProperties.Position;
                            }
                            
                        }

                        input.Seek(returnPos, SeekOrigin.Begin);
                    }

                    var propertiesBytes = propertiesMemoryStream.ToArray();
                    var stringsBytes = stringsMemoryStream.ToArray();

                    var propBase = (int) output.Position + totalStrCount * 12;
                    var strBase = propBase + propertiesBytes.Length;

                    for (var i = 0; i < totalStrCount; i++)
                    {
                        output.Write(stringLengths[i]);
                        output.Write(propertiesCount[i]);
                        output.Write((byte) 0);
                        output.Write(stringOffsets[i] + strBase);
                        output.Write(propertiesOffsets[i] + propBase);
                    }

                    output.Write(propertiesBytes);
                    output.Write(stringsBytes);
                    output.WritePadding(4);
                }

                if (inputPointer1 > 0)
                {
                    var ouputPointer1 = (int)output.Position;
                    output.Seek(8, SeekOrigin.Begin);
                    output.Write(ouputPointer1);
                    output.Seek(ouputPointer1, SeekOrigin.Begin);
                    output.Write(dataPointer1);
                }
                
                if (inputPointerTalkers > 0)
                {
                    var outputPointerTalkers = (int) output.Position;
                    output.Seek(16, SeekOrigin.Begin);
                    output.Write(outputPointerTalkers);
                    output.Seek(outputPointerTalkers, SeekOrigin.Begin);

                    input.Seek(inputPointerTalkers, SeekOrigin.Begin);

                    using (var stringsMemoryStream = new MemoryStream())
                    using (var outputStrings =
                        new ExtendedBinaryWriter(stringsMemoryStream, FileEncoding, Endianness.BigEndian))
                    {
                        var strOffset = (int)output.Position + numTalkers * 4;
                        for (var i = 0; i < numTalkers; i++)
                        {
                            var offset = input.ReadInt32();
                            var sub = subtitles.FirstOrDefault(x => x.Offset == offset);
                            output.Write(strOffset);
                            if (sub != null)
                            {
                                outputStrings.WriteString(sub.Translation);
                            }
                            else
                            {
                                outputStrings.WriteString(string.Empty);
                            }
                            
                            
                            strOffset += (int)outputStrings.Position;
                        }

                        output.Write(stringsMemoryStream.ToArray());
                    }
                }

                if (inputPointerRemainder > 0)
                {
                    var outputPointerRemainder = (int)output.Position;
                    output.Seek(20, SeekOrigin.Begin);
                    output.Write(outputPointerRemainder);
                    output.Seek(outputPointerRemainder, SeekOrigin.Begin);

                    input.Seek(inputPointerRemainder, SeekOrigin.Begin);
                    var data = input.ReadBytes((int) (input.Length - inputPointerRemainder));
                    output.Write(data);
                }
            }
        }
    }
}
