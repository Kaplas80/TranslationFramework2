using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using AsmResolver;
using TF.Core.Entities;
using TF.Core.Files;
using TF.Core.TranslationEntities;
using TF.IO;
using WeifenLuo.WinFormsUI.Docking;

namespace TFGame.TrailsSky.Files.Exe
{
    public class File : BinaryTextFile
    {
        private CharacterInfo[] _charWidths;
        private FontTableView _ftView;

        protected override int ChangesFileVersion => 2;
        
        protected virtual long FontTableOffset => 0x15F140;
        protected virtual string PointerSectionName => ".data\0\0\0";
        protected virtual string StringsSectionName => ".rdata\0\0";

        protected virtual List<Tuple<long, long>> AllowedStringOffsets => new List<Tuple<long, long>>()
        {
            new Tuple<long, long>(0x15E13C, 0x162AFC),
            new Tuple<long, long>(0x1654B0, 0x165870),
        };

        public File(string path, string changesFolder, Encoding encoding) : base(path, changesFolder, encoding)
        {
        }

        public override void Open(DockPanel panel, ThemeBase theme)
        {
            _ftView = new FontTableView(theme);
            _charWidths = GetFontTable();
            _ftView.LoadFontTable(_charWidths);
            _ftView.Show(panel, DockState.Document);

            base.Open(panel, theme);
        }

        protected virtual CharacterInfo[] GetFontTable()
        {
            var result = new CharacterInfo[128];

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs))
            {
                input.Seek(FontTableOffset, SeekOrigin.Begin);

                for (var i = 0; i < 128; i++)
                {
                    var width = input.ReadInt32();

                    result[i] = new CharacterInfo((byte) i)
                    {
                        OriginalWidth = width,
                        Width = width,
                    };
                }
            }

            LoadFontTableChanges(result);

            for (var i = 0; i < 128; i++)
            {
                result[i].SetLoadedWidth();
                result[i].PropertyChanged += SubtitlePropertyChanged;
            }

            return result;
        }

        protected override IList<Subtitle> GetSubtitles()
        {
            var result = new List<Subtitle>();

            var peInfo = WindowsAssembly.FromFile(Path);

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding, Endianness.LittleEndian))
            {
                var pointerSection = peInfo.GetSectionByName(PointerSectionName);
                var pointerSectionStart = pointerSection.Header.PointerToRawData;
                var pointerCount = pointerSection.GetPhysicalLength() / 4;

                var stringsSection = peInfo.GetSectionByName(StringsSectionName);
                var stringsSectionBase = (long) (peInfo.NtHeaders.OptionalHeader.ImageBase +
                                                 (stringsSection.Header.VirtualAddress - stringsSection.Header.PointerToRawData));
                for (var i = 0; i < pointerCount; i++)
                {
                    input.Seek(pointerSectionStart + i * 4, SeekOrigin.Begin);
                    var value = input.ReadInt32();
                    if (value != 0)
                    {
                        var possibleStringOffset = value - stringsSectionBase;

                        bool allowed;

                        if (AllowedStringOffsets != null)
                        {
                            allowed = AllowedStringOffsets.Any(x =>
                                x.Item1 <= possibleStringOffset && x.Item2 >= possibleStringOffset);
                        }
                        else
                        {
                            allowed = (stringsSection.Header.PointerToRawData <= possibleStringOffset) &&
                                      (possibleStringOffset < (stringsSection.Header.PointerToRawData + stringsSection.Header.SizeOfRawData));
                        }

                        if (allowed)
                        {
                            var exists = result.Any(x => x.Offset == possibleStringOffset);

                            if (!exists)
                            {
                                var sub = ReadSubtitle(input, possibleStringOffset, false);
                                sub.PropertyChanged += SubtitlePropertyChanged;

                                result.Add(sub);
                            }
                        }
                    }
                }

                result.Sort();
            }

            LoadChanges(result);

            return result;
        }

        public override void SaveChanges()
        {
            using (var fs = new FileStream(ChangesFile, FileMode.Create))
            using (var output = new ExtendedBinaryWriter(fs, System.Text.Encoding.Unicode))
            {
                output.Write(ChangesFileVersion);

                for (var i = 0; i < 128; i++)
                {
                    output.Write(_charWidths[i].Width);
                    _charWidths[i].SetLoadedWidth();
                }

                output.Write(_subtitles.Count);
                foreach (var subtitle in _subtitles)
                {
                    output.Write(subtitle.Offset);
                    output.WriteString(subtitle.Translation);

                    subtitle.Loaded = subtitle.Translation;
                }
            }

            NeedSaving = false;
            OnFileChanged();
        }

        protected override void LoadChanges(IList<Subtitle> subtitles)
        {
            if (HasChanges)
            {
                using (var fs = new FileStream(ChangesFile, FileMode.Open))
                using (var input = new ExtendedBinaryReader(fs, System.Text.Encoding.Unicode))
                {
                    var version = input.ReadInt32();

                    if (version != ChangesFileVersion)
                    {
                        //System.IO.File.Delete(ChangesFile);
                        return;
                    }

                    input.Skip(128 * 4);

                    var subtitleCount = input.ReadInt32();

                    for (var i = 0; i < subtitleCount; i++)
                    {
                        var offset = input.ReadInt64();
                        var text = input.ReadString();

                        var subtitle = subtitles.FirstOrDefault(x => x.Offset == offset);
                        if (subtitle != null)
                        {
                            subtitle.PropertyChanged -= SubtitlePropertyChanged;
                            subtitle.Translation = text;
                            subtitle.Loaded = subtitle.Translation;
                            subtitle.PropertyChanged += SubtitlePropertyChanged;
                        }
                    }
                }
            }
        }

        private void LoadFontTableChanges(CharacterInfo[] data)
        {
            if (HasChanges)
            {
                using (var fs = new FileStream(ChangesFile, FileMode.Open))
                using (var input = new ExtendedBinaryReader(fs))
                {
                    var version = input.ReadInt32();

                    if (version != ChangesFileVersion)
                    {
                        //System.IO.File.Delete(ChangesFile);
                        return;
                    }

                    for (var i = 0; i < 128; i++)
                    {
                        data[i].Width = input.ReadInt32();
                    }
                }
            }
        }

        protected override void SubtitlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NeedSaving = _subtitles.Any(subtitle => subtitle.HasChanges) || _charWidths.Any(x => x.HasChanged);
            OnFileChanged();
        }

        public override void Rebuild(string outputFolder)
        {
            var outputPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(outputFolder, RelativePath));
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath));

            RebuildSubtitles(outputPath);
            RebuildFontTable(outputPath);
        }

        private void RebuildSubtitles(string outputFile)
        {
            var data = GetSubtitles();

            if (data.Any(x => x.Text != x.Translation))
            {
                CreateExeFile(outputFile);

                var peInfo = WindowsAssembly.FromFile(outputFile);

                using (var inputFs = new FileStream(Path, FileMode.Open))
                using (var input = new ExtendedBinaryReader(inputFs, FileEncoding))
                using (var outputFs = new FileStream(outputFile, FileMode.Open))
                using (var output = new ExtendedBinaryWriter(outputFs, FileEncoding))
                {
                    var pointerSection = peInfo.GetSectionByName(PointerSectionName);
                    var pointerSectionStart = pointerSection.Header.PointerToRawData;
                    var pointerCount = pointerSection.GetPhysicalLength() / 4;

                    var stringsSection = peInfo.GetSectionByName(StringsSectionName);
                    var stringsSectionBase = (long) (peInfo.NtHeaders.OptionalHeader.ImageBase +
                                                     (stringsSection.Header.VirtualAddress -
                                                      stringsSection.Header.PointerToRawData));

                    var translationSection = peInfo.GetSectionByName(".trad\0\0\0");
                    var translationSectionBase = (long) (peInfo.NtHeaders.OptionalHeader.ImageBase +
                                                         (translationSection.Header.VirtualAddress -
                                                          translationSection.Header.PointerToRawData));

                    var used = new Dictionary<long, long>();

                    var outputOffset = (long) translationSection.Header.PointerToRawData;

                    for (var i = 0; i < pointerCount; i++)
                    {
                        input.Seek(pointerSectionStart + i * 4, SeekOrigin.Begin);
                        output.Seek(pointerSectionStart + i * 4, SeekOrigin.Begin);

                        var value = input.ReadInt32();
                        if (value != 0)
                        {
                            var possibleStringOffset = value - stringsSectionBase;

                            bool allowed;
                            if (AllowedStringOffsets != null)
                            {
                                allowed = AllowedStringOffsets.Any(x =>
                                    x.Item1 <= possibleStringOffset && x.Item2 >= possibleStringOffset);
                            }
                            else
                            {
                                allowed = (stringsSection.Header.PointerToRawData <= possibleStringOffset) &&
                                          (possibleStringOffset < (stringsSection.Header.PointerToRawData + stringsSection.Header.SizeOfRawData));
                            }

                            if (allowed)
                            {
                                var exists = used.ContainsKey(possibleStringOffset);

                                if (exists)
                                {
                                    output.Write((int) used[possibleStringOffset]);
                                }
                                else
                                {
                                    var newOffset = outputOffset + translationSectionBase;
                                    output.Write((int) newOffset);
                                    used[possibleStringOffset] = newOffset;

                                    outputOffset = WriteSubtitle(output, data, possibleStringOffset, outputOffset);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void CreateExeFile(string outputFile)
        {
            var peInfo = WindowsAssembly.FromFile(Path);

            using (var inputFs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(inputFs, FileEncoding, Endianness.LittleEndian))
            using (var outputFs = new FileStream(outputFile, FileMode.Create))
            {
                var output = new BinaryStreamWriter(outputFs);
                var writingContext = new WritingContext(peInfo, new BinaryStreamWriter(outputFs));

                var dosHeader = input.ReadBytes((int) peInfo.NtHeaders.StartOffset);
                output.WriteBytes(dosHeader);

                var ntHeader = peInfo.NtHeaders;
                ntHeader.FileHeader.NumberOfSections++;
                ntHeader.OptionalHeader.SizeOfImage += 0x00010000;

                ntHeader.Write(writingContext);

                var newSection = CreateTFSection(peInfo.SectionHeaders[peInfo.SectionHeaders.Count - 1], ntHeader.OptionalHeader.FileAlignment,
                    ntHeader.OptionalHeader.SectionAlignment);
                peInfo.SectionHeaders.Add(newSection);

                foreach (var section in peInfo.SectionHeaders)
                {
                    section.Write(writingContext);
                }

                foreach (var section in peInfo.SectionHeaders)
                {
                    input.Seek(section.PointerToRawData, SeekOrigin.Begin);
                    outputFs.Seek(section.PointerToRawData, SeekOrigin.Begin);

                    var data = input.ReadBytes((int) section.SizeOfRawData);
                    output.WriteBytes(data);
                }

                var bytes = new byte[0x00010000];
                output.WriteBytes(bytes);
            }
        }

        private ImageSectionHeader CreateTFSection(ImageSectionHeader previous, uint fileAlignment, uint sectionAlignment)
        {
            var realAddress = previous.PointerToRawData + previous.SizeOfRawData;
            realAddress = Align(realAddress, fileAlignment);

            var virtualAddress = previous.VirtualAddress + previous.VirtualSize;
            virtualAddress = Align(virtualAddress, sectionAlignment);

            var sectionHeader = new ImageSectionHeader
            {
                Name = ".trad",
                Attributes = ImageSectionAttributes.MemoryRead |
                             ImageSectionAttributes.ContentInitializedData,
                PointerToRawData = realAddress,
                SizeOfRawData = 0x00010000,
                VirtualAddress = virtualAddress,
                VirtualSize = 0x00010000,
            };

            return sectionHeader;
        }

        private static uint Align(uint value, uint align)
        {
            align--;
            return (value + align) & ~align;
        }

        private void RebuildFontTable(string outputFile)
        {
            if (!System.IO.File.Exists(outputFile))
            {
                System.IO.File.Copy(Path, outputFile);
            }

            var data = GetFontTable();

            using (var fs = new FileStream(outputFile, FileMode.Open))
            using (var output = new ExtendedBinaryWriter(fs))
            {
                output.Seek(FontTableOffset, SeekOrigin.Begin);

                for (var i = 0; i < 128; i++)
                {
                    output.Write(data[i].Width);
                }
            }
        }
    }
}
