using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TF.Core.Files;
using TF.Core.TranslationEntities;
using TF.IO;
using TFGame.TrailsSky.Views;
using WeifenLuo.WinFormsUI.Docking;

namespace TFGame.TrailsSky.Files.SN
{
    // https://github.com/ZhenjianYang/EDDecompiler

    public class File : BinaryTextFile
    {
        private const int NUMBER_OF_INCLUDE_FILE = 8;
        private const int SCN_INFO_MAXIMUM = 6;

        private class ScenarioEntry
        {
            public ushort Offset { get; private set; }
            public ushort Size { get; private set; }

            public ScenarioEntry(ushort offset, ushort size)
            {
                Offset = offset;
                Size = size;
            }
        }

        public File(string path, string changesFolder, System.Text.Encoding encoding) : base(path, changesFolder, encoding)
        {
        }

        public override void Open(DockPanel panel)
        {
            _view = new View(LineEnding);

            _subtitles = GetSubtitles();
            _view.LoadData(_subtitles.Where(x => !string.IsNullOrEmpty(x.Text)).ToList());
            _view.Show(panel, DockState.Document);
        }

        protected override IList<Subtitle> GetSubtitles()
        {
            var result = new List<Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding))
            {
                var mapName = input.ReadString();
                input.Seek(0x0A, SeekOrigin.Begin);

                var location = input.ReadString();
                input.Seek(0x18, SeekOrigin.Begin);

                var mapIndex = input.ReadUInt16();
                var mapDefaultBGM = input.ReadUInt16();
                var flags = input.ReadUInt16();
                var entryFunctionIndex = input.ReadUInt16();
                var includedScenario = input.ReadBytes(NUMBER_OF_INCLUDE_FILE * 4);
                var reserved = input.ReadUInt16();
                var scnInfoOffset = input.ReadBytes(SCN_INFO_MAXIMUM * 4);

                var stringTableOffset = input.ReadUInt16();
                var headerEndOffset = input.ReadUInt32();
                var functionTable = new ScenarioEntry(input.ReadUInt16(), input.ReadUInt16());

                var functionCount = functionTable.Size / 2;
                var functionOffsets = new uint[functionCount];
                var functionSizes = new uint[functionCount];

                if (functionCount > 0)
                {
                    input.Seek(functionTable.Offset, SeekOrigin.Begin);
                    for (var i = 0; i < functionCount; i++)
                    {
                        functionOffsets[i] = input.ReadUInt16();
                    }

                    for (var i = 0; i < functionCount - 1; i++)
                    {
                        functionSizes[i] = functionOffsets[i + 1] - functionOffsets[i];
                    }

                    functionSizes[functionCount - 1] = functionTable.Offset - functionOffsets[functionCount - 1];

                    for (var i = 0; i < functionCount; i++)
                    {
                        result.AddRange(ExtractFunctionSubtitles(input, functionOffsets[i], functionSizes[i]));
                    }
                }

                input.Seek(stringTableOffset, SeekOrigin.Begin);
                var filename = ReadSubtitle(input); //@filename

                while (input.Position < input.Length)
                {
                    var subtitle = ReadSubtitle(input);
                    subtitle.PropertyChanged += SubtitlePropertyChanged;
                    result.Add(subtitle);
                }
            }

            LoadChanges(result);

            return result;
        }

        private IEnumerable<Subtitle> ExtractFunctionSubtitles(ExtendedBinaryReader input, uint startOffset, uint size)
        {
            var result = new List<Subtitle>();

            input.Seek(startOffset, SeekOrigin.Begin);

            while (input.Position < startOffset + size)
            {
                int op = input.ReadByte();

                result.AddRange(ExtractOperationSubtitles(input, op));
            }

            return result;
        }

        private IEnumerable<Subtitle> ExtractOperationSubtitles(ExtendedBinaryReader input, int op)
        {
            var result = new List<Subtitle>();
            switch (op)
            {
                case 0x00: // ExitThread
                case 0x01: // Return
                {
                    break;
                }
                case 0x02: // Jc
                {
                    result.AddRange(ExtractScpExpressionSubtitles(input));
                    input.Skip(2);
                    break;
                }
                case 0x03: // Jump
                {
                    input.Skip(2);
                    break;
                }
                case 0x04: // Switch
                {
                    result.AddRange(ExtractScpExpressionSubtitles(input));
                    var count = input.ReadUInt16();
                    input.Skip(4 * count);
                    input.Skip(2);
                    break;
                }
                case 0x05: // Call
                {
                    input.Skip(3);
                    break;
                }
                case 0x06: // NewScene
                {
                    input.Skip(7);
                    break;
                }
                case 0x07: // IdleLoop
                {
                    break;
                }
                case 0x08: // Sleep
                {
                    input.Skip(4);
                    break;
                }
                case 0x09: // SetMapFlags
                case 0x0A: // ClearMapFlags
                {
                    input.Skip(4);
                    break;
                }
                case 0x0B: // FadeToDark
                {
                    input.Skip(9);
                    break;
                }
                case 0x0C: // FadeToBright
                {
                    input.Skip(8);
                    break;
                }
                case 0x0D:
                {
                    break;
                }
                case 0x0E: // Fade
                {
                    input.Skip(4);
                    break;
                }
                case 0x0F: // Battle
                {
                    input.Skip(12);
                    break;
                }
                case 0x10:
                {
                    input.Skip(2);
                    break;
                }
                case 0x11:
                {
                    input.Skip(15);
                    break;
                }
                case 0x12: // StopSound
                {
                    input.Skip(12);
                    break;
                }
                case 0x13: // SetPlaceName
                {
                    input.Skip(2);
                    break;
                }
                case 0x14: // BlurSwitch
                case 0x15:
                {
                    break;
                }
                case 0x16:
                {
                    var aux = input.ReadByte();
                    if (aux == 2)
                    {
                        input.Skip(16);
                    }

                    break;
                }
                case 0x17: // ShowSaveMenu
                case 0x18:
                {
                    break;
                }
                case 0x19: // EventBegin
                case 0x1A: // EventEnd
                {
                    input.Skip(1);
                    break;
                }
                case 0x1B:
                case 0x1C:
                {
                    input.Skip(4);
                    break;
                }
                case 0x1D:
                {
                    input.Skip(1);
                    break;
                }
                case 0x1E:
                {
                    break;
                }
                case 0x1F:
                {
                    input.Skip(5);
                    break;
                }
                case 0x20:
                {
                    input.Skip(4);
                    break;
                }
                case 0x21:
                {
                    break;
                }
                case 0x22:
                {
                    input.Skip(4);
                    break;
                }
                case 0x23:
                {
                    input.Skip(2);
                    break;
                }
                case 0x24:
                {
                    input.Skip(3);
                    break;
                }
                case 0x25: // SoundDistance
                {
                    input.Skip(27);
                    break;
                }
                case 0x26: // SoundLoad
                {
                    input.Skip(2);
                    break;
                }
                case 0x27: // Yield
                {
                    break;
                }
                case 0x28:
                {
                    input.Skip(2);
                    var aux = input.ReadByte();
                    if (aux == 1 || aux == 2)
                    {
                        input.Skip(2);
                    }
                    else if (aux == 3 || aux == 4)
                    {
                        input.Skip(1);
                    }

                    break;
                }
                case 0x29:
                {
                    input.Skip(2);
                    var aux = input.ReadByte();
                    input.Skip(aux == 1 ? 2 : 1);
                    break;
                }
                case 0x2A:
                {
                    var aux = input.ReadUInt16();
                    while (aux != ushort.MaxValue)
                    {
                        aux = input.ReadUInt16();
                    }

                    break;
                }
                case 0x2B:
                case 0x2C:
                {
                    input.Skip(4);
                    break;
                }
                case 0x2D: // AddParty
                case 0x2E: // RemoveParty
                {
                    input.Skip(2);
                    break;
                }
                case 0x2F: // ClearParty
                {
                    break;
                }
                case 0x30:
                {
                    input.Skip(1);
                    break;
                }
                case 0x31:
                {
                    input.Skip(4);
                    break;
                }
                case 0x32:
                case 0x33:
                case 0x34:
                case 0x35:
                case 0x36:
                case 0x37:
                case 0x38: // AddSephith
                case 0x39: // SubSephith
                {
                    input.Skip(3);
                    break;
                }
                case 0x3A: // AddMira
                case 0x3B: // SubMira
                case 0x3C:
                case 0x3D:
                {
                    input.Skip(2);
                    break;
                }
                case 0x3E:
                case 0x3F:
                {
                    input.Skip(4);
                    break;
                }
                case 0x40:
                {
                    input.Skip(2);
                    break;
                }
                case 0x41:
                {
                    input.Skip(1);
                    var aux = input.ReadUInt16();
                    if ((aux >= 0x258) && (aux <= 0x276) ||
                        (aux >= 0x27D) && (aux <= 0x287) ||
                        (aux >= 0x28A) && (aux <= 0x28B) ||
                        (aux >= 0x28E) && (aux <= 0x28F) ||
                        (aux == 0x291) ||
                        (aux >= 0x2C1) && (aux <= 0x2C3) ||
                        (aux >= 0x2C6) && (aux <= 0x2CA) ||
                        (aux >= 0x2D0) && (aux <= 0x2D4) ||
                        (aux >= 0x315) && (aux <= 0x31F))
                    {
                        input.Skip(1);
                    }

                    break;
                }
                case 0x42:
                {
                    input.Skip(1);
                    break;
                }
                case 0x43:
                {
                    input.Skip(6);
                    break;
                }
                case 0x44:
                {
                    input.Skip(3);
                    break;
                }
                case 0x45: // QueueWorkItem
                {
                    input.Skip(5);
                    // Aquí hay una subrutina
                    break;
                }
                case 0x46: // QueueWorkItem2
                {
                    input.Skip(5);
                    // Aquí hay varias subrutinas
                    break;
                }
                case 0x47: // WaitChrThread
                {
                    input.Skip(4);
                    break;
                }
                case 0x48:
                {
                    break;
                }
                case 0x49: // Event
                {
                    input.Skip(3);
                    break;
                }
                case 0x4A:
                case 0x4B:
                {
                    input.Skip(3);
                    break;
                }
                case 0x4C:
                {
                    break;
                }
                case 0x4D: // RunExpression
                {
                    input.Skip(2);
                    result.AddRange(ExtractScpExpressionSubtitles(input));
                    break;
                }
                case 0x4E:
                {
                    break;
                }
                case 0x4F:
                {
                    input.Skip(1);
                    result.AddRange(ExtractScpExpressionSubtitles(input));
                    break;
                }
                case 0x50:
                {
                    break;
                }
                case 0x51:
                {
                    input.Skip(3);
                    result.AddRange(ExtractScpExpressionSubtitles(input));
                    break;
                }
                case 0x52: // TalkBegin
                case 0x53: // TalkEnd
                {
                    input.Skip(2);
                    break;
                }
                case 0x54: // AnonymousTalk
                {
                    var subtitle = ReadSubtitle(input);
                    subtitle.PropertyChanged += SubtitlePropertyChanged;
                    result.Add(subtitle);
                    break;
                }
                case 0x55:
                {
                    break;
                }
                case 0x56:
                {
                    input.Skip(1);
                    break;
                }
                case 0x57:
                case 0x58: // CloseMessageWindow
                case 0x59:
                {
                    break;
                }
                case 0x5A: // SetMessageWindowPos
                {
                    input.Skip(8);
                    break;
                }
                case 0x5B: // ChrTalk
                {
                    input.Skip(2);
                    var subtitle = ReadSubtitle(input);
                    subtitle.PropertyChanged += SubtitlePropertyChanged;
                    result.Add(subtitle);
                    break;
                }
                case 0x5C: // NpcTalk
                {
                    input.Skip(2);
                    var subtitle = ReadSubtitle(input);
                    subtitle.PropertyChanged += SubtitlePropertyChanged;
                    result.Add(subtitle);
                    subtitle = ReadSubtitle(input);
                    subtitle.PropertyChanged += SubtitlePropertyChanged;
                    result.Add(subtitle);
                    break;
                }
                case 0x5D: // Menu
                {
                    input.Skip(7);
                    var subtitle = ReadSubtitle(input);
                    subtitle.PropertyChanged += SubtitlePropertyChanged;
                    result.Add(subtitle);
                    break;
                }
                case 0x5E: // MenuEnd
                case 0x5F:
                {
                    input.Skip(2);
                    break;
                }
                case 0x60: // SetChrName
                {
                    var subtitle = ReadSubtitle(input);
                    subtitle.PropertyChanged += SubtitlePropertyChanged;
                    result.Add(subtitle);
                    break;
                }
                case 0x61:
                {
                    input.Skip(2);
                    break;
                }
                case 0x62:
                {
                    input.Skip(17);
                    break;
                }
                case 0x63:
                {
                    input.Skip(2);
                    break;
                }
                case 0x64:
                case 0x65:
                {
                    input.Skip(3);
                    break;
                }
                case 0x66:
                {
                    input.Skip(2);
                    break;
                }
                case 0x67:
                {
                    input.Skip(16);
                    break;
                }
                case 0x68:
                {
                    input.Skip(2);
                    break;
                }
                case 0x69:
                {
                    input.Skip(6);
                    break;
                }
                case 0x6A:
                {
                    input.Skip(2);
                    break;
                }
                case 0x6B:
                case 0x6C:
                {
                    input.Skip(8);
                    break;
                }
                case 0x6D:
                {
                    input.Skip(16);
                    break;
                }
                case 0x6E:
                {
                    input.Skip(8);
                    break;
                }
                case 0x6F:
                case 0x70:
                {
                    input.Skip(6);
                    break;
                }
                case 0x71:
                case 0x72:
                {
                    input.Skip(4);
                    break;
                }
                case 0x73:
                {
                    input.Skip(2);
                    break;
                }
                case 0x74:
                {
                    input.Skip(8);
                    break;
                }
                case 0x75:
                {
                    input.Skip(6);
                    break;
                }
                case 0x76:
                {
                    input.Skip(22);
                    break;
                }
                case 0x77:
                {
                    input.Skip(8);
                    break;
                }
                case 0x78:
                case 0x79:
                case 0x7A:
                {
                    input.Skip(3);
                    break;
                }
                case 0x7B:
                {
                    break;
                }
                case 0x7C:
                {
                    input.Skip(16);
                    break;
                }
                case 0x7D:
                {
                    input.Skip(1);
                    break;
                }
                case 0x7E:
                {
                    input.Skip(11);
                    break;
                }
                case 0x7F: // LoadEffect
                {
                    input.Skip(1);
                    var subtitle = ReadSubtitle(input);
                    //subtitle.PropertyChanged += SubtitlePropertyChanged;
                    //result.Add(subtitle);
                    break;
                }
                case 0x80: // PlayEffect
                {
                    input.Skip(52);
                    break;
                }
                case 0x81: // Play3DEffect
                {
                    input.Skip(3);
                    var subtitle = ReadSubtitle(input);
                    input.Skip(34);
                    break;
                }
                case 0x82:
                case 0x83:
                {
                    input.Skip(2);
                    break;
                }
                case 0x84:
                {
                    input.Skip(1);
                    break;
                }
                case 0x85:
                {
                    input.Skip(2);
                    break;
                }
                case 0x86: // SetChrChipByIndex
                case 0x87: // SetChrSubChip
                {
                    input.Skip(4);
                    break;
                }
                case 0x88: // SetChrPos
                case 0x89:
                {
                    input.Skip(16);
                    break;
                }
                case 0x8A: // TurnDirection
                {
                    input.Skip(6);
                    break;
                }
                case 0x8B:
                {
                    input.Skip(12);
                    break;
                }
                case 0x8C:
                {
                    input.Skip(6);
                    break;
                }
                case 0x8D:
                {
                    input.Skip(22);
                    break;
                }
                case 0x8E:
                case 0x8F:
                case 0x90:
                case 0x91:
                {
                    input.Skip(19);
                    break;
                }
                case 0x92:
                case 0x93:
                {
                    input.Skip(13);
                    break;
                }
                case 0x94:
                {
                    input.Skip(14);
                    break;
                }
                case 0x95:
                case 0x96:
                {
                    input.Skip(22);
                    break;
                }
                case 0x97:
                {
                    input.Skip(20);
                    break;
                }
                case 0x98:
                {
                    input.Skip(18);
                    break;
                }
                case 0x99:
                {
                    input.Skip(8);
                    break;
                }
                case 0x9A: // SetChrFlags
                case 0x9B: // ClearChrFlags
                case 0x9C: // SetChrBattleFlags
                case 0x9D: // ClearChrBattleFlags
                {
                    input.Skip(4);
                    break;
                }
                case 0x9E:
                {
                    input.Skip(18);
                    break;
                }
                case 0x9F:
                {
                    input.Skip(10);
                    break;
                }
                case 0xA0:
                {
                    input.Skip(11);
                    break;
                }
                case 0xA1:
                {
                    input.Skip(4);
                    break;
                }
                case 0xA2:
                case 0xA3:
                case 0xA4:
                case 0xA5:
                case 0xA6:
                {
                    input.Skip(2);
                    break;
                }
                case 0xA7:
                {
                    input.Skip(4);
                    break;
                }
                case 0xA8:
                {
                    input.Skip(5);
                    break;
                }
                case 0xA9:
                {
                    input.Skip(1);
                    break;
                }
                case 0xAA:
                case 0xAB:
                {
                    break;
                }
                case 0xAC:
                {
                    input.Skip(2);
                    break;
                }
                case 0xAD:
                {
                    input.Skip(10);
                    break;
                }
                case 0xAE:
                {
                    input.Skip(4);
                    break;
                }
                case 0xAF:
                case 0xB0:
                {
                    input.Skip(3);
                    break;
                }
                case 0xB1:
                {
                    var subtitle = ReadSubtitle(input);
                    break;
                }
                case 0xB2:
                {
                    input.Skip(4);
                    break;
                }
                case 0xB3: // PlayMovie
                {
                    input.Skip(1);
                    var subtitle = ReadSubtitle(input);
                    break;
                }
                case 0xB4:
                {
                    input.Skip(1);
                    break;
                }
                case 0xB5:
                {
                    input.Skip(3);
                    break;
                }
                case 0xB6:
                {
                    input.Skip(1);
                    break;
                }
                case 0xB7:
                {
                    input.Skip(3);
                    break;
                }
                case 0xB8:
                {
                    input.Skip(1);
                    break;
                }
                case 0xB9:
                {
                    input.Skip(4);
                    break;
                }
                case 0xBA:
                {
                    input.Skip(3);
                    break;
                }
                case 0xBB:
                {
                    input.Skip(2);
                    break;
                }
                case 0xDE: // SaveClearData
                {
                    break;
                }

                default:
                {
                    break;
                }
            }

            return result;
        }

        private IEnumerable<Subtitle> ExtractScpExpressionSubtitles(ExtendedBinaryReader input)
        {
            var result = new List<Subtitle>();
            var finish = false;
            while (!finish)
            {
                var op = input.ReadByte();
                switch (op)
                {
                    case 0x00: // Push Long
                        input.Skip(4);
                        break;
                    case 0x01:
                        finish = true;
                        break;
                    case 0x1C:
                    {
                        var b = input.ReadByte();
                        result.AddRange(ExtractOperationSubtitles(input, b));
                        break;
                    }
                    case 0x1E:
                    case 0x1F:
                        input.Skip(2);
                        break;
                    case 0x20:
                        input.Skip(1);
                        break;
                    case 0x21:
                        input.Skip(3);
                        break;
                    case 0x23:
                        input.Skip(1);
                        break;
                    default:
                        break;
                }
            }

            return result;
        }

        protected override Subtitle ReadSubtitle(ExtendedBinaryReader input)
        {
            using (var ms = new MemoryStream())
            {
                var subtitle = new Subtitle {Offset = input.Position};

                var b = input.ReadByte();

                while (b != 0x00)
                {
                    switch (b)
                    {
                        case 0x07: // Color
                        {
                            var b2 = input.ReadByte();
                            var buffer = this.FileEncoding.GetBytes($"<Color: {b2:X2}>");
                            ms.Write(buffer, 0, buffer.Length);
                            break;
                        }
                        case 0x1F: // Item
                        {
                            var id = input.ReadUInt16();
                            var buffer = this.FileEncoding.GetBytes($"<Item: {id:X4}>");
                            ms.Write(buffer, 0, buffer.Length);
                            break;
                        }
                        default:
                        {
                            ms.WriteByte(b); // Es el encoding quien se encarga de sustituirlo si es necesario
                            break;
                        }
                    }

                    b = input.ReadByte();
                }

                var text = FileEncoding.GetString(ms.ToArray());
                subtitle.Text = text;
                subtitle.Translation = text;
                subtitle.Loaded = text;

                return subtitle;
            }
        }

        public override void Rebuild(string outputFolder)
        {
            var outputPath = System.IO.Path.Combine(outputFolder, RelativePath);
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath));

            var subtitles = GetSubtitles();

            using (var fsInput = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fsInput, FileEncoding))
            using (var fsOutput = new FileStream(outputPath, FileMode.Create))
            using (var output = new ExtendedBinaryWriter(fsOutput, FileEncoding))
            {
                var mapName = input.ReadString();
                input.Seek(0x0A, SeekOrigin.Begin);
                output.WriteString(mapName, 0x0A);

                var location = input.ReadString();
                input.Seek(0x18, SeekOrigin.Begin);
                output.WriteString(location, 0x0E);

                var mapIndex = input.ReadUInt16();
                output.Write(mapIndex);

                var mapDefaultBGM = input.ReadUInt16();
                output.Write(mapDefaultBGM);

                var flags = input.ReadUInt16();
                output.Write(flags);

                var entryFunctionIndex = input.ReadUInt16();
                output.Write(entryFunctionIndex);

                var includedScenario = input.ReadBytes(NUMBER_OF_INCLUDE_FILE * 4);
                output.Write(includedScenario);

                var reserved = input.ReadUInt16();
                output.Write(reserved);

                var scnInfoOffset = input.ReadBytes(SCN_INFO_MAXIMUM * 4);
                output.Write(scnInfoOffset);

                var stringTableOffset = input.ReadUInt16();
                var returnPos1 = output.Position;
                output.Write((ushort) 0x0000); // En este momento no lo se, lo relleno luego

                var headerEndOffset = input.ReadUInt32();
                output.Write(headerEndOffset);

                var functionTable = new ScenarioEntry(input.ReadUInt16(), input.ReadUInt16());
                var returnPos2 = output.Position;
                output.Write((ushort) 0x0000); // En este momento no lo se, lo relleno luego
                output.Write(functionTable.Size);

                var functionCount = functionTable.Size / 2;
                var functionOffsets = new uint[functionCount];
                var newFunctionOffsets = new ushort[functionCount];
                var functionSizes = new uint[functionCount];

                var headerRemainder = input.ReadBytes((int) (headerEndOffset - input.Position));
                output.Write(headerRemainder);

                var jumpOffsets = new List<Tuple<long, ushort>>(); // Item1 = Offset; Item2 = Destino del salto
                var offsetVariations = new List<Tuple<long, int>>(); // Item1 = Offset original; Item2 = variacion (nuevo - original)

                if (functionCount > 0)
                {
                    input.Seek(functionTable.Offset, SeekOrigin.Begin);
                    for (var i = 0; i < functionCount; i++)
                    {
                        functionOffsets[i] = input.ReadUInt16();
                    }

                    for (var i = 0; i < functionCount - 1; i++)
                    {
                        functionSizes[i] = functionOffsets[i + 1] - functionOffsets[i];
                    }

                    functionSizes[functionCount - 1] = functionTable.Offset - functionOffsets[functionCount - 1];

                    for (var i = 0; i < functionCount; i++)
                    {
                        newFunctionOffsets[i] = (ushort) output.Position;
                        var newFunction = GenerateFunction(input, functionOffsets[i], functionSizes[i], newFunctionOffsets[i], subtitles, jumpOffsets,
                            offsetVariations);
                        output.Write(newFunction);
                    }
                }

                var newFunctionTableOffset = (ushort) output.Position;
                for (var i = 0; i < functionCount; i++)
                {
                    output.Write(newFunctionOffsets[i]);
                }

                var newStringTableOffset = (ushort) output.Position;

                input.Seek(stringTableOffset, SeekOrigin.Begin);

                var filename = ReadSubtitle(input); //@filename
                output.WriteString(filename.Text);

                while (input.Position < input.Length)
                {
                    var subtitle = ReadSubtitle(input);
                    WriteSubtitle(output, subtitles, subtitle.Offset, output.Position);
                }

                output.Seek(returnPos1, SeekOrigin.Begin);
                output.Write(newStringTableOffset);

                output.Seek(returnPos2, SeekOrigin.Begin);
                output.Write(newFunctionTableOffset);

                foreach (var jump in jumpOffsets)
                {
                    output.Seek(jump.Item1, SeekOrigin.Begin);
                    for (var i = offsetVariations.Count - 1; i >= 0; i--)
                    {
                        var variation = offsetVariations[i];
                        if (jump.Item2 >= variation.Item1)
                        {
                            output.Write((ushort) (jump.Item2 + variation.Item2));
                            break;
                        }
                    }
                }
            }
        }

        private byte[] GenerateFunction(ExtendedBinaryReader input, uint startOffset, uint size, ushort outputOffset, IList<Subtitle> subtitles,
            IList<Tuple<long, ushort>> jumps, IList<Tuple<long, int>> variations)
        {
            using (var output = new MemoryStream())
            {
                input.Seek(startOffset, SeekOrigin.Begin);

                while (input.Position < startOffset + size)
                {
                    int op = input.ReadByte();
                    output.WriteByte((byte) op);

                    var newFunction = GenerateOperation(input, (ushort) (outputOffset + output.Position), op, subtitles, jumps, variations);
                    output.Write(newFunction, 0, newFunction.Length);
                }

                return output.ToArray();
            }
        }

        private byte[] GenerateOperation(ExtendedBinaryReader input, ushort outputOffset, int operation, IList<Subtitle> subtitles,
            IList<Tuple<long, ushort>> jumps, IList<Tuple<long, int>> variations)
        {
            using (var ms = new MemoryStream())
            using (var output = new ExtendedBinaryWriter(ms, FileEncoding))
            {
                switch (operation)
                {
                    case 0x00: // ExitThread
                    case 0x01: // Return
                    {
                        break;
                    }
                    case 0x02: // Jc
                    {
                        output.Write(GenerateScpExpression(input, (ushort) (outputOffset + output.Position), subtitles, jumps, variations));

                        var jumpDest = input.ReadUInt16();
                        var jump = new Tuple<long, ushort>(outputOffset + output.Position, jumpDest);
                        jumps.Add(jump);
                        output.Write(jumpDest);
                        break;
                    }
                    case 0x03: // Jump
                    {
                        var jumpDest = input.ReadUInt16();
                        var jump = new Tuple<long, ushort>(outputOffset + output.Position, jumpDest);
                        jumps.Add(jump);
                        output.Write(jumpDest);
                        break;
                    }
                    case 0x04: // Switch
                    {
                        output.Write(GenerateScpExpression(input, (ushort) (outputOffset + output.Position), subtitles, jumps, variations));

                        var count = input.ReadUInt16();
                        output.Write(count);

                        for (var i = 0; i < count; i++)
                        {
                            var id = input.ReadUInt16();
                            output.Write(id);

                            var dest = input.ReadUInt16();
                            var j = new Tuple<long, ushort>(outputOffset + output.Position, dest);
                            jumps.Add(j);
                            output.Write(dest);
                        }

                        var jumpDest = input.ReadUInt16();
                        var jump = new Tuple<long, ushort>(outputOffset + output.Position, jumpDest);
                        jumps.Add(jump);
                        output.Write(jumpDest);

                        break;
                    }
                    case 0x05: // Call
                    {
                        output.Write(input.ReadBytes(3));
                        break;
                    }
                    case 0x06: // NewScene
                    {
                        output.Write(input.ReadBytes(7));
                        break;
                    }
                    case 0x07: // IdleLoop
                    {
                        break;
                    }
                    case 0x08: // Sleep
                    {
                        output.Write(input.ReadBytes(4));
                        break;
                    }
                    case 0x09: // SetMapFlags
                    case 0x0A: // ClearMapFlags
                    {
                        output.Write(input.ReadBytes(4));
                        break;
                    }
                    case 0x0B: // FadeToDark
                    {
                        output.Write(input.ReadBytes(9));
                        break;
                    }
                    case 0x0C: // FadeToBright
                    {
                        output.Write(input.ReadBytes(8));
                        break;
                    }
                    case 0x0D:
                    {
                        break;
                    }
                    case 0x0E: // Fade
                    {
                        output.Write(input.ReadBytes(4));
                        break;
                    }
                    case 0x0F: // Battle
                    {
                        output.Write(input.ReadBytes(12));
                        break;
                    }
                    case 0x10:
                    {
                        output.Write(input.ReadBytes(2));
                        break;
                    }
                    case 0x11:
                    {
                        output.Write(input.ReadBytes(15));
                        break;
                    }
                    case 0x12: // StopSound
                    {
                        output.Write(input.ReadBytes(12));
                        break;
                    }
                    case 0x13: // SetPlaceName
                    {
                        output.Write(input.ReadBytes(2));
                        break;
                    }
                    case 0x14: // BlurSwitch
                    case 0x15:
                    {
                        break;
                    }
                    case 0x16:
                    {
                        var aux = input.ReadByte();
                        output.Write(aux);
                        if (aux == 2)
                        {
                            output.Write(input.ReadBytes(16));
                        }

                        break;
                    }
                    case 0x17: // ShowSaveMenu
                    case 0x18:
                    {
                        break;
                    }
                    case 0x19: // EventBegin
                    case 0x1A: // EventEnd
                    {
                        output.Write(input.ReadBytes(1));
                        break;
                    }
                    case 0x1B:
                    case 0x1C:
                    {
                        output.Write(input.ReadBytes(4));
                        break;
                    }
                    case 0x1D:
                    {
                        output.Write(input.ReadBytes(1));
                        break;
                    }
                    case 0x1E:
                    {
                        break;
                    }
                    case 0x1F:
                    {
                        output.Write(input.ReadBytes(5));
                        break;
                    }
                    case 0x20:
                    {
                        output.Write(input.ReadBytes(4));
                        break;
                    }
                    case 0x21:
                    {
                        break;
                    }
                    case 0x22:
                    {
                        output.Write(input.ReadBytes(4));
                        break;
                    }
                    case 0x23:
                    {
                        output.Write(input.ReadBytes(2));
                        break;
                    }
                    case 0x24:
                    {
                        output.Write(input.ReadBytes(3));
                        break;
                    }
                    case 0x25: // SoundDistance
                    {
                        output.Write(input.ReadBytes(27));
                        break;
                    }
                    case 0x26: // SoundLoad
                    {
                        output.Write(input.ReadBytes(2));
                        break;
                    }
                    case 0x27: // Yield
                    {
                        break;
                    }
                    case 0x28:
                    {
                        output.Write(input.ReadBytes(2));
                        var aux = input.ReadByte();
                        output.Write(aux);
                        if (aux == 1 || aux == 2)
                        {
                            output.Write(input.ReadBytes(2));
                        }
                        else if (aux == 3 || aux == 4)
                        {
                            output.Write(input.ReadBytes(1));
                        }

                        break;
                    }
                    case 0x29:
                    {
                        output.Write(input.ReadBytes(2));
                        var aux = input.ReadByte();
                        output.Write(aux);
                        output.Write(input.ReadBytes(aux == 1 ? 2 : 1));
                        break;
                    }
                    case 0x2A:
                    {
                        var aux = input.ReadUInt16();
                        output.Write(aux);
                        while (aux != ushort.MaxValue)
                        {
                            aux = input.ReadUInt16();
                            output.Write(aux);
                        }

                        break;
                    }
                    case 0x2B:
                    case 0x2C:
                    {
                        output.Write(input.ReadBytes(4));
                        break;
                    }
                    case 0x2D: // AddParty
                    case 0x2E: // RemoveParty
                    {
                        output.Write(input.ReadBytes(2));
                        break;
                    }
                    case 0x2F: // ClearParty
                    {
                        break;
                    }
                    case 0x30:
                    {
                        output.Write(input.ReadBytes(1));
                        break;
                    }
                    case 0x31:
                    {
                        output.Write(input.ReadBytes(4));
                        break;
                    }
                    case 0x32:
                    case 0x33:
                    case 0x34:
                    case 0x35:
                    case 0x36:
                    case 0x37:
                    case 0x38: // AddSephith
                    case 0x39: // SubSephith
                    {
                        output.Write(input.ReadBytes(3));
                        break;
                    }
                    case 0x3A: // AddMira
                    case 0x3B: // SubMira
                    case 0x3C:
                    case 0x3D:
                    {
                        output.Write(input.ReadBytes(2));
                        break;
                    }
                    case 0x3E:
                    case 0x3F:
                    {
                        output.Write(input.ReadBytes(4));
                        break;
                    }
                    case 0x40:
                    {
                        output.Write(input.ReadBytes(2));
                        break;
                    }
                    case 0x41:
                    {
                        output.Write(input.ReadBytes(1));
                        var aux = input.ReadUInt16();
                        output.Write(aux);
                        if ((aux >= 0x258) && (aux <= 0x276) ||
                            (aux >= 0x27D) && (aux <= 0x287) ||
                            (aux >= 0x28A) && (aux <= 0x28B) ||
                            (aux >= 0x28E) && (aux <= 0x28F) ||
                            (aux == 0x291) ||
                            (aux >= 0x2C1) && (aux <= 0x2C3) ||
                            (aux >= 0x2C6) && (aux <= 0x2CA) ||
                            (aux >= 0x2D0) && (aux <= 0x2D4) ||
                            (aux >= 0x315) && (aux <= 0x31F))
                        {
                            output.Write(input.ReadBytes(1));
                        }

                        break;
                    }
                    case 0x42:
                    {
                        output.Write(input.ReadBytes(1));
                        break;
                    }
                    case 0x43:
                    {
                        output.Write(input.ReadBytes(6));
                        break;
                    }
                    case 0x44:
                    {
                        output.Write(input.ReadBytes(3));
                        break;
                    }
                    case 0x45: // QueueWorkItem
                    {
                        output.Write(input.ReadBytes(5));
                        // Aquí hay una subrutina
                        break;
                    }
                    case 0x46: // QueueWorkItem2
                    {
                        output.Write(input.ReadBytes(5));
                        // Aquí hay varias subrutinas
                        break;
                    }
                    case 0x47: // WaitChrThread
                    {
                        output.Write(input.ReadBytes(4));
                        break;
                    }
                    case 0x48:
                    {
                        break;
                    }
                    case 0x49: // Event
                    {
                        output.Write(input.ReadBytes(3));
                        break;
                    }
                    case 0x4A:
                    case 0x4B:
                    {
                        output.Write(input.ReadBytes(3));
                        break;
                    }
                    case 0x4C:
                    {
                        break;
                    }
                    case 0x4D: // RunExpression
                    {
                        output.Write(input.ReadBytes(2));
                        output.Write(GenerateScpExpression(input, (ushort) (outputOffset + output.Position), subtitles, jumps, variations));
                        break;
                    }
                    case 0x4E:
                    {
                        break;
                    }
                    case 0x4F:
                    {
                        output.Write(input.ReadBytes(1));
                        output.Write(GenerateScpExpression(input, (ushort) (outputOffset + output.Position), subtitles, jumps, variations));
                        break;
                    }
                    case 0x50:
                    {
                        break;
                    }
                    case 0x51:
                    {
                        output.Write(input.ReadBytes(3));
                        output.Write(GenerateScpExpression(input, (ushort) (outputOffset + output.Position), subtitles, jumps, variations));
                        break;
                    }
                    case 0x52: // TalkBegin
                    case 0x53: // TalkEnd
                    {
                        output.Write(input.ReadBytes(2));
                        break;
                    }
                    case 0x54: // AnonymousTalk
                    {
                        var subtitle = ReadSubtitle(input);
                        WriteSubtitle(output, subtitles, subtitle.Offset, output.Position);

                        var variation = new Tuple<long, int>(input.Position, (int) (outputOffset + output.Position - input.Position));
                        variations.Add(variation);
                        break;
                    }
                    case 0x55:
                    {
                        break;
                    }
                    case 0x56:
                    {
                        output.Write(input.ReadBytes(1));
                        break;
                    }
                    case 0x57:
                    case 0x58: // CloseMessageWindow
                    case 0x59:
                    {
                        break;
                    }
                    case 0x5A: // SetMessageWindowPos
                    {
                        output.Write(input.ReadBytes(8));
                        break;
                    }
                    case 0x5B: // ChrTalk
                    {
                        output.Write(input.ReadBytes(2));
                        var subtitle = ReadSubtitle(input);
                        WriteSubtitle(output, subtitles, subtitle.Offset, output.Position);

                        var variation = new Tuple<long, int>(input.Position, (int) (outputOffset + output.Position - input.Position));
                        variations.Add(variation);
                        break;
                    }
                    case 0x5C: // NpcTalk
                    {
                        output.Write(input.ReadBytes(2));

                        var subtitle = ReadSubtitle(input);
                        WriteSubtitle(output, subtitles, subtitle.Offset, output.Position);
                        var variation = new Tuple<long, int>(input.Position, (int) (outputOffset + output.Position - input.Position));
                        variations.Add(variation);

                        subtitle = ReadSubtitle(input);
                        WriteSubtitle(output, subtitles, subtitle.Offset, output.Position);
                        variation = new Tuple<long, int>(input.Position, (int) (outputOffset + output.Position - input.Position));
                        variations.Add(variation);
                        break;
                    }
                    case 0x5D: // Menu
                    {
                        output.Write(input.ReadBytes(7));
                        var subtitle = ReadSubtitle(input);
                        WriteSubtitle(output, subtitles, subtitle.Offset, output.Position);
                        var variation = new Tuple<long, int>(input.Position, (int) (outputOffset + output.Position - input.Position));
                        variations.Add(variation);
                        break;
                    }
                    case 0x5E: // MenuEnd
                    case 0x5F:
                    {
                        output.Write(input.ReadBytes(2));
                        break;
                    }
                    case 0x60: // SetChrName
                    {
                        var subtitle = ReadSubtitle(input);
                        WriteSubtitle(output, subtitles, subtitle.Offset, output.Position);
                        var variation = new Tuple<long, int>(input.Position, (int) (outputOffset + output.Position - input.Position));
                        variations.Add(variation);
                        break;
                    }
                    case 0x61:
                    {
                        output.Write(input.ReadBytes(2));
                        break;
                    }
                    case 0x62:
                    {
                        output.Write(input.ReadBytes(17));
                        break;
                    }
                    case 0x63:
                    {
                        output.Write(input.ReadBytes(2));
                        break;
                    }
                    case 0x64:
                    case 0x65:
                    {
                        output.Write(input.ReadBytes(3));
                        break;
                    }
                    case 0x66:
                    {
                        output.Write(input.ReadBytes(2));
                        break;
                    }
                    case 0x67:
                    {
                        output.Write(input.ReadBytes(16));
                        break;
                    }
                    case 0x68:
                    {
                        output.Write(input.ReadBytes(2));
                        break;
                    }
                    case 0x69:
                    {
                        output.Write(input.ReadBytes(6));
                        break;
                    }
                    case 0x6A:
                    {
                        output.Write(input.ReadBytes(2));
                        break;
                    }
                    case 0x6B:
                    case 0x6C:
                    {
                        output.Write(input.ReadBytes(8));
                        break;
                    }
                    case 0x6D:
                    {
                        output.Write(input.ReadBytes(16));
                        break;
                    }
                    case 0x6E:
                    {
                        output.Write(input.ReadBytes(8));
                        break;
                    }
                    case 0x6F:
                    case 0x70:
                    {
                        output.Write(input.ReadBytes(6));
                        break;
                    }
                    case 0x71:
                    case 0x72:
                    {
                        output.Write(input.ReadBytes(4));
                        break;
                    }
                    case 0x73:
                    {
                        output.Write(input.ReadBytes(2));
                        break;
                    }
                    case 0x74:
                    {
                        output.Write(input.ReadBytes(8));
                        break;
                    }
                    case 0x75:
                    {
                        output.Write(input.ReadBytes(6));
                        break;
                    }
                    case 0x76:
                    {
                        output.Write(input.ReadBytes(22));
                        break;
                    }
                    case 0x77:
                    {
                        output.Write(input.ReadBytes(8));
                        break;
                    }
                    case 0x78:
                    case 0x79:
                    case 0x7A:
                    {
                        output.Write(input.ReadBytes(3));
                        break;
                    }
                    case 0x7B:
                    {
                        break;
                    }
                    case 0x7C:
                    {
                        output.Write(input.ReadBytes(16));
                        break;
                    }
                    case 0x7D:
                    {
                        output.Write(input.ReadBytes(1));
                        break;
                    }
                    case 0x7E:
                    {
                        output.Write(input.ReadBytes(11));
                        break;
                    }
                    case 0x7F: // LoadEffect
                    {
                        output.Write(input.ReadBytes(1));
                        var subtitle = ReadSubtitle(input);
                        output.WriteString(subtitle.Text);
                        break;
                    }
                    case 0x80: // PlayEffect
                    {
                        output.Write(input.ReadBytes(52));
                        break;
                    }
                    case 0x81: // Play3DEffect
                    {
                        output.Write(input.ReadBytes(3));
                        var subtitle = ReadSubtitle(input);
                        output.WriteString(subtitle.Text);
                        output.Write(input.ReadBytes(34));
                        break;
                    }
                    case 0x82:
                    case 0x83:
                    {
                        output.Write(input.ReadBytes(2));
                        break;
                    }
                    case 0x84:
                    {
                        output.Write(input.ReadBytes(1));
                        break;
                    }
                    case 0x85:
                    {
                        output.Write(input.ReadBytes(2));
                        break;
                    }
                    case 0x86: // SetChrChipByIndex
                    case 0x87: // SetChrSubChip
                    {
                        output.Write(input.ReadBytes(4));
                        break;
                    }
                    case 0x88: // SetChrPos
                    case 0x89:
                    {
                        output.Write(input.ReadBytes(16));
                        break;
                    }
                    case 0x8A: // TurnDirection
                    {
                        output.Write(input.ReadBytes(6));
                        break;
                    }
                    case 0x8B:
                    {
                        output.Write(input.ReadBytes(12));
                        break;
                    }
                    case 0x8C:
                    {
                        output.Write(input.ReadBytes(6));
                        break;
                    }
                    case 0x8D:
                    {
                        output.Write(input.ReadBytes(22));
                        break;
                    }
                    case 0x8E:
                    case 0x8F:
                    case 0x90:
                    case 0x91:
                    {
                        output.Write(input.ReadBytes(19));
                        break;
                    }
                    case 0x92:
                    case 0x93:
                    {
                        output.Write(input.ReadBytes(13));
                        break;
                    }
                    case 0x94:
                    {
                        output.Write(input.ReadBytes(14));
                        break;
                    }
                    case 0x95:
                    case 0x96:
                    {
                        output.Write(input.ReadBytes(22));
                        break;
                    }
                    case 0x97:
                    {
                        output.Write(input.ReadBytes(20));
                        break;
                    }
                    case 0x98:
                    {
                        output.Write(input.ReadBytes(18));
                        break;
                    }
                    case 0x99:
                    {
                        output.Write(input.ReadBytes(8));
                        break;
                    }
                    case 0x9A: // SetChrFlags
                    case 0x9B: // ClearChrFlags
                    case 0x9C: // SetChrBattleFlags
                    case 0x9D: // ClearChrBattleFlags
                    {
                        output.Write(input.ReadBytes(4));
                        break;
                    }
                    case 0x9E:
                    {
                        output.Write(input.ReadBytes(18));
                        break;
                    }
                    case 0x9F:
                    {
                        output.Write(input.ReadBytes(10));
                        break;
                    }
                    case 0xA0:
                    {
                        output.Write(input.ReadBytes(11));
                        break;
                    }
                    case 0xA1:
                    {
                        output.Write(input.ReadBytes(4));
                        break;
                    }
                    case 0xA2:
                    case 0xA3:
                    case 0xA4:
                    case 0xA5:
                    case 0xA6:
                    {
                        output.Write(input.ReadBytes(2));
                        break;
                    }
                    case 0xA7:
                    {
                        output.Write(input.ReadBytes(4));
                        break;
                    }
                    case 0xA8:
                    {
                        output.Write(input.ReadBytes(5));
                        break;
                    }
                    case 0xA9:
                    {
                        output.Write(input.ReadBytes(1));
                        break;
                    }
                    case 0xAA:
                    case 0xAB:
                    {
                        break;
                    }
                    case 0xAC:
                    {
                        output.Write(input.ReadBytes(2));
                        break;
                    }
                    case 0xAD:
                    {
                        output.Write(input.ReadBytes(10));
                        break;
                    }
                    case 0xAE:
                    {
                        output.Write(input.ReadBytes(4));
                        break;
                    }
                    case 0xAF:
                    case 0xB0:
                    {
                        output.Write(input.ReadBytes(3));
                        break;
                    }
                    case 0xB1:
                    {
                        var subtitle = ReadSubtitle(input);
                        output.WriteString(subtitle.Text);
                        break;
                    }
                    case 0xB2:
                    {
                        output.Write(input.ReadBytes(4));
                        break;
                    }
                    case 0xB3: // PlayMovie
                    {
                        output.Write(input.ReadBytes(1));
                        var subtitle = ReadSubtitle(input);
                        output.WriteString(subtitle.Text);
                        break;
                    }
                    case 0xB4:
                    {
                        output.Write(input.ReadBytes(1));
                        break;
                    }
                    case 0xB5:
                    {
                        output.Write(input.ReadBytes(3));
                        break;
                    }
                    case 0xB6:
                    {
                        output.Write(input.ReadBytes(1));
                        break;
                    }
                    case 0xB7:
                    {
                        output.Write(input.ReadBytes(3));
                        break;
                    }
                    case 0xB8:
                    {
                        output.Write(input.ReadBytes(1));
                        break;
                    }
                    case 0xB9:
                    {
                        output.Write(input.ReadBytes(4));
                        break;
                    }
                    case 0xBA:
                    {
                        output.Write(input.ReadBytes(3));
                        break;
                    }
                    case 0xBB:
                    {
                        output.Write(input.ReadBytes(2));
                        break;
                    }
                    case 0xDE: // SaveClearData
                    {
                        break;
                    }

                    default:
                    {
                        break;
                    }
                }

                return ms.ToArray();
            }
        }

        private byte[] GenerateScpExpression(ExtendedBinaryReader input, ushort outputOffset, IList<Subtitle> subtitles, IList<Tuple<long, ushort>> jumps,
            IList<Tuple<long, int>> variations)
        {
            using (var ms = new MemoryStream())
            using (var output = new ExtendedBinaryWriter(ms, FileEncoding))
            {
                var finish = false;
                while (!finish)
                {
                    var op = input.ReadByte();
                    output.Write(op);
                    switch (op)
                    {
                        case 0x00: // Push Long
                            output.Write(input.ReadBytes(4));
                            break;
                        case 0x01:
                            finish = true;
                            break;
                        case 0x1C:
                        {
                            var b = input.ReadByte();
                            output.Write(b);
                            output.Write(GenerateOperation(input, (ushort) (outputOffset + output.Position), b, subtitles, jumps, variations));
                            break;
                        }
                        case 0x1E:
                        case 0x1F:
                            output.Write(input.ReadBytes(2));
                            break;
                        case 0x20:
                            output.Write(input.ReadBytes(1));
                            break;
                        case 0x21:
                            output.Write(input.ReadBytes(3));
                            break;
                        case 0x23:
                            output.Write(input.ReadBytes(1));
                            break;
                        default:
                            break;
                    }
                }

                return ms.ToArray();
            }
        }

        protected override long WriteSubtitle(ExtendedBinaryWriter output, IList<Subtitle> subtitles, long inputOffset, long outputOffset)
        {
            var sub = subtitles.First(x => x.Offset == inputOffset);

            var evaluator = new MatchEvaluator(SubtitleMatchEvaluator);

            var newSub = Regex.Replace(sub.Translation, @"<Color: (?<colorValue>[0-9A-Fa-f]{2})>", evaluator);
            newSub = Regex.Replace(newSub, @"<Item: (?<itemValue>[0-9A-Fa-f]{4})>", evaluator);

            output.Seek(outputOffset, SeekOrigin.Begin);
            output.WriteString(newSub);

            var result = output.Position;

            return result;
        }

        private static string SubtitleMatchEvaluator(Match match)
        {
            if (match.Groups[1].Name == "colorValue")
            {
                var value = new byte[]
                {
                    0x07,
                    byte.Parse(match.Groups[1].Value, NumberStyles.AllowHexSpecifier),
                };
                return System.Text.Encoding.ASCII.GetString(value);
            }

            if (match.Groups[1].Name == "itemValue")
            {
                var value = new byte[]
                {
                    0x1F,
                    byte.Parse(match.Groups[1].Value.Substring(0, 2), NumberStyles.AllowHexSpecifier),
                    byte.Parse(match.Groups[1].Value.Substring(2, 2), NumberStyles.AllowHexSpecifier),
                };
                return System.Text.Encoding.ASCII.GetString(value);
            }

            return match.Value;
        }
    }
}
