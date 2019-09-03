using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using TF.Core.Files;
using TF.IO;
using WeifenLuo.WinFormsUI.Docking;

namespace YakuzaGame.Files.Msg
{
    public class File : BinaryTextFile
    {
        public override string LineEnding => "\r\n";

        public File(string path, string changesFolder, Encoding encoding) : base(path, changesFolder, encoding)
        {
        }

#if DEBUG
        private View _msgView;
        public override void Open(DockPanel panel)
        {
            _msgView = new View();

            _subtitles = GetSubtitles();
            _msgView.LoadSubtitles(_subtitles.Where(x => !string.IsNullOrEmpty(x.Text)).ToList());
            _msgView.Show(panel, DockState.Document);
        }
#endif
        protected override IList<TF.Core.TranslationEntities.Subtitle> GetSubtitles()
        {
            var result = new List<TF.Core.TranslationEntities.Subtitle>();

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

                if (pointerTalkers > 0 && result.Count > 0) 
                {
                    input.Seek(pointerTalkers, SeekOrigin.Begin);
                    for (var i = 0; i < numTalkers; i++)
                    {
                        var offset = input.ReadInt32();
                        var subtitle = ReadSubtitle(input, offset, true);
                        if (subtitle != null)
                        {
                            subtitle.PropertyChanged += SubtitlePropertyChanged;
                            result.Add(subtitle);
                        }
                    }
                }
            }

            LoadChanges(result);

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

                var sub = ReadSubtitle(input, strOffset, true);
                if (sub != null)
                {
                    var subtitle = new Subtitle(sub);
                    subtitle.PropertyChanged += SubtitlePropertyChanged;

                    var stringCharCount = subtitle.CleanText().Length;
                    subtitle.Properties = ReadProperties(input, propOffset, propCount);
                    foreach (var property in subtitle.Properties)
                    {
                        if (property.Position == stringCharCount)
                        {
                            property.IsEndProperty = true;
                        }
                    }

                    result.Add(subtitle);
                }
            }

            input.Seek(returnPos, SeekOrigin.Begin);

            return result;
        }

        protected override TF.Core.TranslationEntities.Subtitle ReadSubtitle(ExtendedBinaryReader input, long offset, bool returnToPos)
        {
            var subtitle = base.ReadSubtitle(input, offset, returnToPos);

            if (subtitle.Offset == 0)
            {
                subtitle = null;
            }

            return subtitle;
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

                using (var unknownMemoryStream = new MemoryStream())
                using (var outputUnknown = new ExtendedBinaryWriter(unknownMemoryStream, FileEncoding, Endianness.BigEndian))
                using (var propertiesMemoryStream = new MemoryStream())
                using (var outputProperties = new ExtendedBinaryWriter(propertiesMemoryStream, FileEncoding, Endianness.BigEndian))
                using (var stringsMemoryStream = new MemoryStream())
                using (var outputStrings = new ExtendedBinaryWriter(stringsMemoryStream, FileEncoding, Endianness.BigEndian))
                {
                    var stringLengths = new List<short>();
                    var stringOffsets = new List<int>();
                    var propertiesCount = new List<byte>();
                    var propertiesOffsets = new List<int>();
                    var unknownOffsets = new List<int>();

                    var unknownSectionOffset = 0;
                    var strOffset = 0;
                    var propOffset = 0;
                    var totalStrCount = 0;
                    for (var i = 0; i < count; i++)
                    {
                        input.Seek(0x18 + i * 0x10, SeekOrigin.Begin);

                        var unknownOffset = input.ReadInt32();
                        output.Write(0); // Si no es 0, ya lo rellenaré luego

                        var groupOffset = input.ReadInt32();
                        output.Write(groupOffset);
                        var unknownCount = input.ReadByte();
                        output.Write(unknownCount);

                        var stringCount = input.ReadByte();
                        output.Write(stringCount);
                        totalStrCount += stringCount;
                        output.Write(input.ReadBytes(6));

                        if (unknownOffset != 0)
                        {
                            var returnPos = input.Position;
                            input.Seek(unknownOffset, SeekOrigin.Begin);
                            outputUnknown.Write(input.ReadBytes(unknownCount * 12));
                            unknownOffsets.Add(unknownSectionOffset);
                            unknownSectionOffset = (int) outputUnknown.Position;
                            input.Seek(returnPos, SeekOrigin.Begin);
                        }
                        else
                        {
                            unknownOffsets.Add(-1);
                        }

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
                    }

                    var unknownBytes = unknownMemoryStream.ToArray();
                    var propertiesBytes = propertiesMemoryStream.ToArray();
                    var stringsBytes = stringsMemoryStream.ToArray();

                    var propBase = (int) output.Position + totalStrCount * 12;
                    var unknownBase = propBase + propertiesBytes.Length;
                    var strBase = unknownBase + unknownBytes.Length;

                    var outputReturnPos = output.Position;
                    for (var i = 0; i < count; i++)
                    {
                        output.Seek(0x18 + i * 0x10, SeekOrigin.Begin);
                        if (unknownOffsets[i] >= 0)
                        {
                            output.Write(unknownOffsets[i] + unknownBase);
                        }
                    }

                    output.Seek(outputReturnPos, SeekOrigin.Begin);

                    for (var i = 0; i < totalStrCount; i++)
                    {
                        output.Write(stringLengths[i]);
                        output.Write(propertiesCount[i]);
                        output.Write((byte) 0);
                        output.Write(stringOffsets[i] + strBase);
                        output.Write(propertiesOffsets[i] + propBase);
                    }

                    output.Write(propertiesBytes);
                    if (unknownBytes.Length > 0)
                    {
                        output.Write(unknownBytes);
                    }
                    output.Write(stringsBytes);
                    output.WritePadding(4);
                }

                if (inputPointer1 > 0)
                {
                    var outputPointer1 = (int)output.Position;
                    output.Seek(8, SeekOrigin.Begin);
                    output.Write(outputPointer1);
                    output.Seek(outputPointer1, SeekOrigin.Begin);
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
                        var baseOffset = (int) output.Position + numTalkers * 4;
                        var strOffset = baseOffset;
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
                            
                            
                            strOffset = baseOffset + (int)outputStrings.Position;
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
