using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TF.Core.Files;
using TF.Core.TranslationEntities;
using TF.IO;

namespace TFGame.TrailsColdSteel.Files.Dat
{
    public class File : TextFile
    {
        private class Op
        {
            public byte Code;
            public string Name;
            public int ParamCount;
        }

        private readonly Op[] GameOps;

        public File(string path, string changesFolder, Encoding encoding) : base(path, changesFolder, encoding)
        {
            GameOps = new Op[160];

            GameOps[0x00] = new Op { Code = 0x00, Name = "Nop", ParamCount = 0 };
            GameOps[0x01] = new Op {Code = 0x01, Name = "Return", ParamCount = 0};
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
                    input.Seek(functionOffset[i], SeekOrigin.Begin);
                    if (i > 0)
                    {
                        txt.AppendLine();
                    }

                    var name = string.IsNullOrEmpty(functionName[i]) ? "AnonymousFunc" : functionName[i];
                    txt.AppendLine($"{name}({functionOffset[i]:X8})");

                    var count = 0;
                    while (count < functionSize[i])
                    {
                        var opCode = input.ReadByte();
                        count++;

                        if (opCode < 160 && GameOps[opCode] != null)
                        {
                            var op = GameOps[opCode];

                            var args = new string[op.ParamCount];
                            for (var j = 0; j < op.ParamCount; j++)
                            {
                                args[j] = string.Concat(input.ReadByte().ToString());
                            }

                            txt.AppendLine($"{op.Name}({string.Join(", ", args)});");
                        }
                        else
                        {
                            txt.AppendLine($"Op_{opCode:X2}();");
                        }

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
    }
}
