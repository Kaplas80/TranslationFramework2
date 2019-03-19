using System.Collections.Generic;
using System.IO;
using System.Linq;
using TF.Core.Files;
using TF.Core.TranslationEntities;
using TF.IO;
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

        public File(string path, string changesFolder, Encoding encoding) : base(path, changesFolder, encoding)
        {
        }

        public override void Open(DockPanel panel, ThemeBase theme)
        {
            _view = new View(theme);

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
                var scnInfoOffset = new List<ScenarioEntry> (SCN_INFO_MAXIMUM);
                for (var i = 0; i < SCN_INFO_MAXIMUM; i++)
                {
                    scnInfoOffset.Add(new ScenarioEntry(input.ReadUInt16(), input.ReadUInt16()));
                }

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

                result.AddRange(ParseOp(input, op));
            }

            return result;
        }

        private IEnumerable<Subtitle> ParseOp(ExtendedBinaryReader input, int op)
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
                        result.AddRange(SkipScpExpression(input));
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
                        result.AddRange(SkipScpExpression(input));
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
                        result.AddRange(SkipScpExpression(input));
                        break;
                    }
                case 0x4E:
                    {
                        break;
                    }
                case 0x4F:
                    {
                        input.Skip(1);
                        result.AddRange(SkipScpExpression(input));
                        break;
                    }
                case 0x50:
                    {
                        break;
                    }
                case 0x51:
                    {
                        input.Skip(3);
                        result.AddRange(SkipScpExpression(input));
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
        private IList<Subtitle> SkipScpExpression(ExtendedBinaryReader input)
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
                        result.AddRange(ParseOp(input, b));
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
                        case 0x02: // Espera enter
                        {
                            var buffer = this.FileEncoding.GetBytes("<Enter>");
                            ms.Write(buffer, 0, buffer.Length);
                            break;
                        }
                        case 0x03: // Borrar el texto
                        {
                            var buffer = this.FileEncoding.GetBytes("<Clear>");
                            ms.Write(buffer, 0, buffer.Length);
                            break;
                        }
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
    }
}
