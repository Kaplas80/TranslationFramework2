using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TF.Core.Files;
using TF.Core.TranslationEntities;
using TF.IO;

namespace TFGame.TrailsColdSteel.Files.Dat
{
    public class File : TextFile
    {
        public override int SubtitleCount => 1;

        public File(string path, string changesFolder, Encoding encoding) : base(path, changesFolder, encoding)
        {
        }

        protected override PlainText GetText()
        {
            var result = new PlainText();
            var txt = new StringBuilder();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding))
            {
                // Header
                var headerSize = input.ReadUInt32(); // 0x20
                var scnNamePointer = input.ReadUInt32(); // 0x20
                var functionOffsetTablePointer = input.ReadUInt32();
                var functionOffsetTableSize = input.ReadUInt32();
                var functionNameOffsetTablePointer = input.ReadUInt32();
                var functionCount = input.ReadUInt32();
                var dataStartPointer = input.ReadUInt32();
                var dummy = input.ReadUInt32(); // 0xABCDEF00

                input.Seek(scnNamePointer, SeekOrigin.Begin);
                var scnName = input.ReadString(Encoding.ASCII);

                var functionOffset = new uint[functionCount];
                var functionSize = new uint[functionCount];

                var functionNameOffset = new ushort[functionCount];
                var functionName = new string[functionCount];

                input.Seek(functionOffsetTablePointer, SeekOrigin.Begin);
                for (var i = 0; i < functionCount; i++)
                {
                    functionOffset[i] = input.ReadUInt32();
                }

                for (var i = 0; i < functionCount - 1; i++)
                {
                    functionSize[i] = functionOffset[i + 1] - functionOffset[i];
                }
                functionSize[functionCount - 1] = (uint) (fs.Length - functionOffset[functionCount - 1]);

                input.Seek(functionNameOffsetTablePointer, SeekOrigin.Begin);
                for (var i = 0; i < functionCount; i++)
                {
                    functionNameOffset[i] = input.ReadUInt16();
                }

                for (var i = 0; i < functionCount; i++)
                {
                    input.Seek(functionNameOffset[i], SeekOrigin.Begin);
                    functionName[i] = input.ReadString(Encoding.ASCII);
                }

                input.Seek(dataStartPointer, SeekOrigin.Begin);
                input.SkipPadding(0x04);

                for (var i = 0; i < functionCount; i++)
                {
                    if (i > 0)
                    {
                        txt.AppendLine();
                    }

                    var fName = string.IsNullOrEmpty(functionName[i]) ? "Anonymous" : functionName[i];
                    txt.AppendLine($"FUNCTION_{fName}({functionOffset[i]:X8})");

                    var lines = ExtractFunctionSubtitles(input, functionOffset[i], functionSize[i]);
                    foreach (var line in lines)
                    {
                        txt.AppendLine(line);
                    }
                }
            }

            result.Text = txt.ToString();
            result.Translation = txt.ToString();
            result.Loaded = txt.ToString();
            result.PropertyChanged += SubtitlePropertyChanged;

            LoadChanges(result);

            return result;
        }

        private IEnumerable<string> ExtractFunctionSubtitles(ExtendedBinaryReader input, uint startOffset, uint size)
        {
            var result = new List<string>();

            input.Seek(startOffset, SeekOrigin.Begin);

            while (input.Position < startOffset + size)
            {
                int op = input.ReadByte();

                result.AddRange(ExtractOperationSubtitles(input, op));
            }

            return result;
        }

        private IEnumerable<string> ExtractOperationSubtitles(ExtendedBinaryReader input, int op)
        {
            var result = new List<string>();
            var offset = input.Position - 1;
            switch (op)
            {
                case 0x02:
                {
                    var type = input.ReadByte();
                    if (type == 0x0B)
                    {
                        result.Add($"0x{offset:X8}\tOp_{op:X2}{type:X2}(\"{input.ReadString(Encoding.ASCII)}\")");
                    }
                    else
                    {
                        result.Add($"0x{offset:X8}\tOp_{op:X2}{type:X2}(0x{input.ReadInt32():X8})");
                    }
                    break;
                }
                case 0x04: // Igual que 02??
                    result.Add($"0x{offset:X8}\tOp_{op:X2}({input.ReadByte()}, \"{input.ReadString(Encoding.ASCII)}\")");
                    break;
                case 0x05: 
                {
                    var tmp = string.Join(", ", ExtractScpExpressionSubtitles(input));
                    result.Add($"0x{offset:X8}\tJc(Eval({tmp}), 0x{input.ReadUInt32():X8})"); // No se si este uint32 se da siempre
                    break;
                }

                case 0x06: // Switch
                {
                    var tmp = string.Join(", ", ExtractScpExpressionSubtitles(input));
                    var count = input.ReadByte();
                    var tmp2 = new List<string>();
                    for (var i = 0; i < count; i++)
                    {
                        tmp2.Add($"{input.ReadUInt16()}");
                        tmp2.Add($"{input.ReadUInt32()}");
                    }
                    tmp2.Add($"{input.ReadUInt32()}");

                    result.Add($"0x{offset:X8}\tOp_{op:X2}(Eval({tmp}), {tmp2})");
                    break;
                }
                case 0x07:
                case 0x0E:
                case 0x0F:
                    result.Add($"0x{offset:X8}\tOp_{op:X2}({input.ReadUInt32()})");
                    break;

                case 0x08:
                case 0x0A:    
                {
                    var p1 = input.ReadByte();
                    var tmp = string.Join(", ", ExtractScpExpressionSubtitles(input));
                    result.Add($"0x{offset:X8}\tOp_{op:X2}({p1}, Eval({tmp}))");
                    break;
                }
                case 0x0C:
                {
                    result.Add($"0x{offset:X8}\tOp_{op:X2}({input.ReadUInt16()})");
                    break;
                }
                case 0x0D:
                {
                    result.Add($"0x{offset:X8}\tSetFlags({input.ReadUInt16()})");
                    break;
                }
                case 0x13:
                    Por aqui
                    result.Add($"0x{offset:X8}\tOp_{op:X2}({input.ReadUInt16()}, \"{input.ReadString(Encoding.ASCII)}\", \"{input.ReadString(Encoding.ASCII)}\", \"{input.ReadString(Encoding.ASCII)}\", {input.ReadUInt16()}, {input.ReadUInt32()}, )");
                    break;

                case 0x14:
                    result.Add($"0x{offset:X8}\tOp_{op:X2}({input.ReadUInt16()}, {input.ReadByte()}, {input.ReadByte()}, \"{input.ReadString(Encoding.ASCII)}\")");
                    break;
                case 0x16:
                    result.Add($"0x{offset:X8}\tOp_{op:X2}({input.ReadByte()})");
                    break;
                case 0x1E:
                    result.Add($"0x{offset:X8}\tOp_{op:X2}(\"{input.ReadString(Encoding.ASCII)}\", \"{input.ReadString(Encoding.ASCII)}\")");
                    break;
                case 0x23:
                    result.Add($"0x{offset:X8}\tOp_{op:X2}({input.ReadUInt16()}, {input.ReadUInt32()})");
                    break;
                case 0x2D:
                {
                    var p1 = input.ReadByte();
                    switch (p1)
                    {
                        case 0x02:
                        case 0x0C:
                        case 0x0D:
                            result.Add($"0x{offset:X8}\tOp_{op:X2}({p1}, {input.ReadByte()}, {input.ReadUInt32()}, {input.ReadUInt32()}, {input.ReadUInt32()}, {input.ReadUInt16()})");
                            break;
                        case 0x03:
                        case 0x14:
                            result.Add($"0x{offset:X8}\tOp_{op:X2}({p1}, {input.ReadByte()}, {input.ReadUInt16()}, {input.ReadByte()}, \"{input.ReadString(Encoding.ASCII)}\", {input.ReadUInt32()}, {input.ReadUInt32()}, {input.ReadUInt32()}, {input.ReadUInt16()})");
                            break;
                        case 0x04:
                        case 0x11:
                            result.Add($"0x{offset:X8}\tOp_{op:X2}({p1}, {input.ReadByte()}, {input.ReadUInt32()}, {input.ReadUInt32()}, {input.ReadUInt32()}, {input.ReadUInt16()}, {input.ReadByte()})");
                            break;
                        case 0x05:
                        case 0x0B:
                        case 0x16:
                            result.Add($"0x{offset:X8}\tOp_{op:X2}({p1}, {input.ReadByte()}, {input.ReadUInt32()}, {input.ReadUInt16()})");
                            break;
                        case 0x07:
                        case 0x12:
                            result.Add($"0x{offset:X8}\tOp_{op:X2}({p1}, {input.ReadUInt16()})");
                            break;
                        case 0x08:
                            result.Add($"0x{offset:X8}\tOp_{op:X2}({p1}, {input.ReadByte()}, {input.ReadUInt16()})");
                            break;
                        case 0x09:
                            result.Add($"0x{offset:X8}\tOp_{op:X2}({p1}, {input.ReadUInt32()}, {input.ReadUInt32()}, {input.ReadUInt32()})");
                            break;
                        case 0x0E:
                            result.Add($"0x{offset:X8}\tOp_{op:X2}({p1}, {input.ReadUInt16()}, {input.ReadUInt16()}, {input.ReadUInt32()}, {input.ReadUInt16()})");
                            break;
                        case 0x0F:
                            result.Add($"0x{offset:X8}\tOp_{op:X2}({p1}, {input.ReadUInt16()}, {input.ReadUInt32()}, {input.ReadUInt32()}, {input.ReadUInt32()})");
                            break;
                        case 0x13:
                            result.Add($"0x{offset:X8}\tOp_{op:X2}({p1}, {input.ReadUInt16()}, {input.ReadByte()}, {input.ReadUInt32()}, {input.ReadUInt32()}, {input.ReadUInt32()}, {input.ReadUInt16()}, {input.ReadByte()})");
                            break;
                        case 0x15:
                            result.Add($"0x{offset:X8}\tOp_{op:X2}({p1}, {input.ReadUInt32()}, {input.ReadUInt32()}, {input.ReadByte()})");
                            break;
                        case 0x17:
                            result.Add($"0x{offset:X8}\tOp_{op:X2}({p1}, {input.ReadUInt16()})");
                            break;
                        default:
                        {
                            result.Add($"0x{offset:X8}\tOp_{op:X2}({p1})");
                            break;
                        }

                    }

                    break;
                }
                case 0x30:
                {
                    var p1 = input.ReadByte();
                    switch (p1)
                    {
                        case 0x00:
                        case 0x04:
                            result.Add($"0x{offset:X8}\tOp_{op:X2}({p1}, {input.ReadUInt16()}, {input.ReadUInt32()}, {input.ReadUInt16()}, {input.ReadUInt32()}, {input.ReadByte()})");
                            break;
                        case 0x01:
                            result.Add($"0x{offset:X8}\tOp_{op:X2}({p1}, {input.ReadUInt16()}, {input.ReadByte()})");
                            break;
                        case 0x02:
                            result.Add($"0x{offset:X8}\tOp_{op:X2}({p1}, {input.ReadByte()})");
                            break;
                        case 0x03:
                            result.Add($"0x{offset:X8}\tOp_{op:X2}({p1}, {input.ReadUInt32()}, {input.ReadUInt16()}, {input.ReadByte()})");
                            break;
                        case 0x05:
                            result.Add($"0x{offset:X8}\tOp_{op:X2}({p1}, {input.ReadUInt16()}, {input.ReadUInt16()})");
                            break;
                        case 0x06:
                            result.Add($"0x{offset:X8}\tOp_{op:X2}({p1}, {input.ReadUInt16()})");
                            break;
                        default:
                        {
                            result.Add($"0x{offset:X8}\tOp_{op:X2}({p1})");
                            break;
                        }

                    }

                    break;
                }
                case 0x39:
                {
                    var p1 = input.ReadByte();
                    var p2 = input.ReadUInt16();
                    switch (p1)
                    {
                        case 0x05:
                        case 0x0A:
                        case 0x0B:
                        case 0x0C:
                        case 0x10:
                        case 0x69:
                        case 0xFF:
                            result.Add($"0x{offset:X8}\tOp_{op:X2}({p1}, {p2})");
                            break;
                        default:
                        {
                            var p3 = input.ReadUInt32();
                            result.Add($"0x{offset:X8}\tOp_{op:X2}({p1}, {p2}, {p3})");
                            break;
                        }

                    }
                    
                    break;
                }
                case 0x3D:
                    result.Add($"0x{offset:X8}\tOp_{op:X2}({input.ReadByte()}, \"{input.ReadString(Encoding.ASCII)}\", {input.ReadUInt16()})");
                    break;
                case 0x3F:
                {
                    var p1 = input.ReadByte();
                    switch (p1)
                    {
                        case 0x00:
                        case 0x01:
                        case 0x04:
                        case 0x05:
                        case 0x06:
                        case 0x09:
                        case 0x0A:
                        case 0x0B:
                            result.Add($"0x{offset:X8}\tOp_{op:X2}({p1}, {input.ReadUInt16()})");
                            break;
                        case 0x08:
                            result.Add($"0x{offset:X8}\tOp_{op:X2}({p1}, {input.ReadUInt16()}, {input.ReadByte()}, {input.ReadByte()}, {input.ReadUInt16()})");
                            break;
                        default: // No estoy seguro de que el caso 0x03 entre aquí
                            result.Add($"0x{offset:X8}\tOp_{op:X2}({p1})");
                            break;
                    }

                    break;
                }
                case 0x4A:
                    result.Add($"0x{offset:X8}\tOp_{op:X2}({input.ReadByte()}, {input.ReadUInt16()}, {input.ReadUInt16()}, {input.ReadUInt16()}, {input.ReadUInt16()}, {input.ReadUInt16()}, {input.ReadUInt16()}, {input.ReadUInt16()}, {input.ReadUInt16()}, {input.ReadUInt16()}, {input.ReadUInt16()}, {input.ReadUInt32()}, {input.ReadUInt32()}, {input.ReadUInt32()}, {input.ReadUInt32()}, {input.ReadByte()}, \"{input.ReadString(Encoding.ASCII)}\")");
                    break;
                case 0x66:
                    // En el código lo que hace es avanzar 6 posiciones
                    // Yo lo pongo como int + short para ver los parámetros
                    result.Add($"0x{offset:X8}\tOp_{op:X2}({input.ReadUInt32()}, {input.ReadUInt16()})");
                    break;
                default:
                    result.Add($"0x{offset:X8}\tOp_{op:X2}()");
                    break;
            }

            return result;
        }

        private IEnumerable<string> ExtractScpExpressionSubtitles(ExtendedBinaryReader input)
        {
            var result = new List<string>();
            var finish = false;
            while (!finish)
            {
                var op = input.ReadByte();
                switch (op)
                {
                    case 0x00: // Push Long
                        result.Add($"PUSH({input.ReadUInt32()})");
                        break;
                    case 0x01:
                        finish = true;
                        break;
                    case 0x02:
                        result.Add("EQU");
                        break;
                    case 0x03:
                        result.Add("NEQ");
                        break;
                    case 0x04:
                        result.Add("LSS");
                        break;
                    case 0x05:
                        result.Add("GTR");
                        break;
                    case 0x06:
                        result.Add("LEQ");
                        break;
                    case 0x07:
                        result.Add("GEQ");
                        break;
                    case 0x08:
                        result.Add("EQUZ");
                        break;
                    case 0x09:
                        result.Add("NEQUZ64");
                        break;
                    case 0x0A:
                        result.Add("AND");
                        break;
                    case 0x0B:
                        result.Add("OR");
                        break;
                    case 0x0C:
                        result.Add("ADD");
                        break;
                    case 0x0D:
                        result.Add("SUB");
                        break;
                    case 0x0E:
                        result.Add("NEG");
                        break;
                    case 0x0F:
                        result.Add("XOR");
                        break;
                    case 0x10:
                        result.Add("IMUL");
                        break;
                    case 0x11:
                        result.Add("IDIV");
                        break;
                    case 0x12:
                        result.Add("IMOD");
                        break;
                    case 0x14:
                        result.Add("IMUL_SAVE");
                        break;
                    case 0x15:
                        result.Add("IDIV_SAVE");
                        break;
                    case 0x16:
                        result.Add("IMOD_SAVE");
                        break;
                    case 0x17:
                        result.Add("ADD_SAVE");
                        break;
                    case 0x18:
                        result.Add("SUB_SAVE");
                        break;
                    case 0x19:
                        result.Add("AND_SAVE");
                        break;
                    case 0x1A:
                        result.Add("XOR_SAVE");
                        break;
                    case 0x1B:
                        result.Add("OR_SAVE");
                        break;
                    case 0x1C:
                        {
                            var b = input.ReadByte();
                            var tmp = string.Join(", ", ExtractOperationSubtitles(input, b));

                            result.Add($"EXEC_OP({tmp})");
                            break;
                        }
                    case 0x1D:
                        result.Add("NOT");
                        break;
                    case 0x1E:
                        result.Add($"TEST_FLAGS({input.ReadUInt16()})");
                        break;
                    case 0x1F:
                        result.Add($"GET_RESULT({input.ReadByte()})");
                        break;
                    case 0x20:
                        result.Add($"PUSH_INDEX({input.ReadByte()})");
                        break;
                    case 0x21: //??
                        result.Add($"GET_CHR_WORK({input.ReadUInt16()}, {input.ReadByte()})");
                        break;
                    case 0x22:
                        result.Add("RAND");
                        break;
                    case 0x23:
                        result.Add($"EXP_23({input.ReadByte()})");
                        break;
                    default:
                        break;
                }
            }

            return result;
        }
    }
}


