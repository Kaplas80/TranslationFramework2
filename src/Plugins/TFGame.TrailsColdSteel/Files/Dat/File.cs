using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TF.Core.Files;
using TF.Core.TranslationEntities;
using TF.IO;

namespace TFGame.TrailsColdSteel.Files.Dat
{
    public class File : TextFile
    {
        private enum OpParameter
        {
            Byte,
            ByteHex,
            UInt16,
            UInt16Hex,
            UInt32,
            UInt32Hex,
            AsciiString,
            Utf8String,
            String,
            ScpExpression
        }

        private class OpSwitch
        {
            public byte? SwitchByte;
            public OpParameter[] Parameters;
        }

        private class Op
        {
            public byte OpCode;
            public string OpName;
            public OpSwitch[] OpParameters;
        }

        public override int SubtitleCount => 1;

        private Op[] OpCodes;

        public File(string path, string changesFolder, Encoding encoding) : base(path, changesFolder, encoding)
        {
            OpCodes = new Op[160];

            OpCodes[0x00] = new Op
            {
                OpCode = 0x00,
                OpParameters = new[]
                {
                    new OpSwitch {SwitchByte = 0x00, Parameters = new OpParameter[0]}
                }
            };
            OpCodes[0x01] = new Op
            {
                OpCode = 0x01,
                OpParameters = new[]
                {
                    new OpSwitch {SwitchByte = 0x00, Parameters = new OpParameter[0]}
                }
            };
            OpCodes[0x02] = new Op
            {
                OpCode = 0x02,
                OpParameters = new[]
                {
                    new OpSwitch {SwitchByte = 0x0A, Parameters = new[] {OpParameter.Utf8String}},
                    new OpSwitch {SwitchByte = 0x0B, Parameters = new[] {OpParameter.Utf8String}},
                    new OpSwitch {SwitchByte = null, Parameters = new[] {OpParameter.UInt32Hex}}
                }
            };
            OpCodes[0x03] = new Op
            {
                OpCode = 0x03,
                OpParameters = new[] {new OpSwitch {SwitchByte = 0x00, Parameters = new OpParameter[0]}}
            };
            OpCodes[0x04] = new Op
            {
                OpCode = 0x04,
                OpParameters = new[]
                {
                    new OpSwitch {SwitchByte = 0x0B, Parameters = new[] {OpParameter.Utf8String}},
                    new OpSwitch {SwitchByte = null, Parameters = new[] {OpParameter.UInt32Hex}}
                }
            };
            OpCodes[0x05] = new Op
            {
                OpCode = 0x05,
                OpName = "Jc",
                OpParameters = new[]
                {
                    new OpSwitch {SwitchByte = 0x00, Parameters = new[] {OpParameter.ScpExpression, OpParameter.UInt32Hex}},
                }
            };
            OpCodes[0x06] = new Op
            {
                OpCode = 0x06,
                OpName = "Switch",
                OpParameters = new[]
                {
                    // El caso es especial y no se definen los parámetros aquí
                    new OpSwitch {SwitchByte = 0x00, Parameters = new OpParameter[0]}
                }
            };
            OpCodes[0x07] = new Op
            {
                OpCode = 0x07,
                OpParameters = new[]
                {
                    new OpSwitch {SwitchByte = 0x00, Parameters = new[] {OpParameter.UInt32}}
                }
            };
            OpCodes[0x08] = new Op
            {
                OpCode = 0x08,
                OpParameters = new[]
                {
                    new OpSwitch {SwitchByte = 0x00, Parameters = new[] {OpParameter.Byte, OpParameter.ScpExpression}}
                }
            };
            OpCodes[0x09] = new Op
            {
                OpCode = 0x09,
                OpParameters = new[]
                {
                    new OpSwitch {SwitchByte = 0x00, Parameters = new OpParameter[0]}
                }
            };
            OpCodes[0x0A] = new Op
            {
                OpCode = 0x0A,
                OpParameters = new[]
                {
                    new OpSwitch {SwitchByte = 0x00, Parameters = new[] {OpParameter.Byte, OpParameter.ScpExpression}}
                }
            };
            OpCodes[0x0B] = new Op
            {
                OpCode = 0x0B,
                OpParameters = new[]
                {
                    new OpSwitch {SwitchByte = 0x00, Parameters = new OpParameter[0]}
                }
            };
            OpCodes[0x0C] = new Op
            {
                OpCode = 0x0C,
                OpParameters = new[]
                {
                    new OpSwitch {SwitchByte = 0x00, Parameters = new[] {OpParameter.UInt16}}
                }
            };
            OpCodes[0x0D] = new Op
            {
                OpCode = 0x0D,
                OpName = "SetFlags",
                OpParameters = new[]
                {
                    new OpSwitch {SwitchByte = 0x00, Parameters = new[] {OpParameter.UInt16}}
                }
            };
            OpCodes[0x0E] = new Op
            {
                OpCode = 0x0E,
                OpParameters = new[]
                {
                    new OpSwitch {SwitchByte = 0x00, Parameters = new[] {OpParameter.UInt32}}
                }
            };
            OpCodes[0x0F] = new Op
            {
                OpCode = 0x0F,
                OpParameters = new[]
                {
                    new OpSwitch {SwitchByte = 0x00, Parameters = new[] {OpParameter.UInt32}}
                }
            };
            OpCodes[0x10] = new Op
            {
                OpCode = 0x10,
                OpParameters = new[]
                {
                    new OpSwitch {SwitchByte = 0x00, Parameters = new[] {OpParameter.UInt16}}
                }
            };
            OpCodes[0x11] = null;
            OpCodes[0x12] = null;
            OpCodes[0x13] = new Op
            {
                OpCode = 0x13,
                OpParameters = new[]
                {
                    new OpSwitch
                    {
                        SwitchByte = 0x00,
                        Parameters = new[]
                        {
                            OpParameter.UInt16, OpParameter.Utf8String, OpParameter.Utf8String, OpParameter.Utf8String, OpParameter.Byte, OpParameter.UInt32,
                            OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32,
                            OpParameter.UInt32, OpParameter.Utf8String, OpParameter.Utf8String, OpParameter.UInt32, OpParameter.Byte, OpParameter.UInt32,
                            OpParameter.UInt32, OpParameter.UInt16
                        }
                    }
                }
            };
            OpCodes[0x14] = new Op
            {
                OpCode = 0x14,
                OpParameters = new[]
                {
                    new OpSwitch {SwitchByte = 0x00, Parameters = new[] {OpParameter.UInt16, OpParameter.Byte, OpParameter.Byte, OpParameter.Utf8String}}
                }
            };
            OpCodes[0x15] = null;
            OpCodes[0x16] = new Op
            {
                OpCode = 0x16,
                OpParameters = new[]
                {
                    new OpSwitch {SwitchByte = 0x00, Parameters = new[] {OpParameter.Byte}}
                }
            };
            OpCodes[0x17] = null;
            OpCodes[0x18] = null;
            OpCodes[0x19] = null;
            OpCodes[0x1A] = new Op
            {
                OpCode = 0x1A,
                OpParameters = new[]
                {
                    // caso especial
                    new OpSwitch {SwitchByte = 0x00, Parameters = new OpParameter[0]}
                }
            };
            OpCodes[0x1B] = null;
            OpCodes[0x1C] = new Op
            {
                OpCode = 0x1C,
                OpParameters = new[] {new OpSwitch {SwitchByte = 0x00, Parameters = new OpParameter[0]}}
            };
            OpCodes[0x1D] = null;
            OpCodes[0x1E] = new Op
            {
                OpCode = 0x1E,
                OpParameters = new[]
                {
                    new OpSwitch {SwitchByte = 0x00, Parameters = new[] {OpParameter.Utf8String, OpParameter.Utf8String}}
                }
            };
            OpCodes[0x1F] = null;
            OpCodes[0x20] = new Op
            {
                OpCode = 0x20,
                OpParameters = new[]
                {
                    new OpSwitch {SwitchByte = 0x00, Parameters = new[] {OpParameter.Byte, OpParameter.UInt16, OpParameter.Utf8String, OpParameter.Utf8String, OpParameter.Byte}}
                }
            };
            OpCodes[0x21] = null;
            OpCodes[0x22] = new Op
            {
                OpCode = 0x22,
                OpParameters = new[]
                {
                    new OpSwitch
                    {
                        SwitchByte = 0x00,
                        Parameters = new[]
                        {
                            OpParameter.UInt16, OpParameter.Utf8String, OpParameter.Byte, OpParameter.Byte, OpParameter.Byte, OpParameter.Byte,
                            OpParameter.Byte, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32
                        }
                    }
                }
            };
            OpCodes[0x23] = new Op
            {
                OpCode = 0x23,
                OpParameters = new[]
                {
                    new OpSwitch {SwitchByte = 0x00, Parameters = new[] {OpParameter.UInt16, OpParameter.UInt32}}
                }
            };
            OpCodes[0x24] = null;
            OpCodes[0x25] = null;
            OpCodes[0x26] = null;
            OpCodes[0x27] = new Op
            {
                OpCode = 0x27,
                OpParameters = new[]
                {
                    new OpSwitch {SwitchByte = 0x0A, Parameters = new[] {OpParameter.UInt16, OpParameter.Byte, OpParameter.Utf8String}},
                    new OpSwitch {SwitchByte = 0x0B, Parameters = new[] {OpParameter.UInt16, OpParameter.Byte}},
                    new OpSwitch {SwitchByte = 0x0C, Parameters = new[] {OpParameter.UInt16, OpParameter.UInt16, OpParameter.UInt16, OpParameter.UInt16, OpParameter.Utf8String, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.Byte}},
                    new OpSwitch {SwitchByte = 0x0D, Parameters = new[] {OpParameter.UInt16, OpParameter.Byte}},
                    new OpSwitch {SwitchByte = 0x0E, Parameters = new[] {OpParameter.UInt16, OpParameter.Byte}},
                    new OpSwitch {SwitchByte = 0x0F, Parameters = new[] {OpParameter.UInt16, OpParameter.Byte}},
                    new OpSwitch {SwitchByte = 0x10, Parameters = new[] {OpParameter.UInt16, OpParameter.Byte}},
                    new OpSwitch {SwitchByte = 0x11, Parameters = new[] {OpParameter.UInt16, OpParameter.Byte}},
                    new OpSwitch {SwitchByte = 0x12, Parameters = new[] {OpParameter.UInt16, OpParameter.Byte, OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x13, Parameters = new[] {OpParameter.UInt16, OpParameter.Byte, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32}},
                    new OpSwitch {SwitchByte = 0x14, Parameters = new[] {OpParameter.UInt16, OpParameter.Byte, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32}},
                    new OpSwitch {SwitchByte = 0x15, Parameters = new[] {OpParameter.Byte}},
                    new OpSwitch {SwitchByte = null, Parameters = new OpParameter[0]}
                }
            };
            OpCodes[0x28] = null;
            OpCodes[0x29] = null;
            OpCodes[0x2A] = null;
            OpCodes[0x2B] = new Op
            {
                OpCode = 0x2B,
                OpParameters = new[]
                {
                    new OpSwitch {SwitchByte = 0x00, Parameters = new[] {OpParameter.UInt16, OpParameter.UInt32}}
                }
            };
            OpCodes[0x2C] = new Op
            {
                OpCode = 0x2C,
                OpParameters = new[]
                {
                    new OpSwitch {SwitchByte = 0x00, Parameters = new[] {OpParameter.UInt16, OpParameter.UInt32}}
                }
            };
            OpCodes[0x2D] = new Op
            {
                OpCode = 0x2D,
                OpParameters = new[]
                {
                    new OpSwitch {SwitchByte = 0x02, Parameters = new[] {OpParameter.Byte, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x03, Parameters = new[] {OpParameter.Byte, OpParameter.UInt16, OpParameter.Byte, OpParameter.Utf8String, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x04, Parameters = new[] {OpParameter.Byte, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt16, OpParameter.Byte}},
                    new OpSwitch {SwitchByte = 0x05, Parameters = new[] {OpParameter.Byte, OpParameter.UInt32, OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x07, Parameters = new[] {OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x08, Parameters = new[] {OpParameter.Byte, OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x09, Parameters = new[] {OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32}},
                    new OpSwitch {SwitchByte = 0x0B, Parameters = new[] {OpParameter.Byte, OpParameter.UInt32, OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x0C, Parameters = new[] {OpParameter.Byte, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x0D, Parameters = new[] {OpParameter.Byte, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x0E, Parameters = new[] {OpParameter.UInt16, OpParameter.UInt16, OpParameter.UInt32, OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x0F, Parameters = new[] {OpParameter.UInt16, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32}},
                    new OpSwitch {SwitchByte = 0x11, Parameters = new[] {OpParameter.Byte, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt16, OpParameter.Byte}},
                    new OpSwitch {SwitchByte = 0x12, Parameters = new[] {OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x13, Parameters = new[] {OpParameter.UInt16, OpParameter.Byte, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt16, OpParameter.Byte}},
                    new OpSwitch {SwitchByte = 0x14, Parameters = new[] {OpParameter.Byte, OpParameter.UInt16, OpParameter.Byte, OpParameter.Utf8String, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x15, Parameters = new[] {OpParameter.UInt32, OpParameter.UInt32, OpParameter.Byte}},
                    new OpSwitch {SwitchByte = 0x16, Parameters = new[] {OpParameter.Byte, OpParameter.UInt32, OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x17, Parameters = new[] {OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = null, Parameters = new OpParameter[0]}
                }
            };
            OpCodes[0x2E] = new Op
            {
                OpCode = 0x2E,
                OpParameters = new[]
                {
                    new OpSwitch {SwitchByte = 0x00, Parameters = new[] {OpParameter.UInt16, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32}}
                }
            };
            OpCodes[0x2F] = new Op
            {
                OpCode = 0x2F,
                OpParameters = new[]
                {
                    new OpSwitch {SwitchByte = 0x00, Parameters = new[] {OpParameter.UInt16, OpParameter.Byte, OpParameter.Utf8String}}
                }
            };
            OpCodes[0x30] = new Op
            {
                OpCode = 0x30,
                OpParameters = new[]
                {
                    new OpSwitch
                    {
                        SwitchByte = 0x00, Parameters = new[] {OpParameter.UInt16, OpParameter.UInt32, OpParameter.UInt16, OpParameter.UInt32, OpParameter.Byte}
                    },
                    new OpSwitch {SwitchByte = 0x01, Parameters = new[] {OpParameter.UInt16, OpParameter.Byte}},
                    new OpSwitch {SwitchByte = 0x02, Parameters = new[] {OpParameter.Byte}},
                    new OpSwitch {SwitchByte = 0x03, Parameters = new[] {OpParameter.UInt32, OpParameter.UInt16, OpParameter.Byte}},
                    new OpSwitch
                    {
                        SwitchByte = 0x04, Parameters = new[] {OpParameter.UInt16, OpParameter.UInt32, OpParameter.UInt16, OpParameter.UInt32, OpParameter.Byte}
                    },
                    new OpSwitch {SwitchByte = 0x05, Parameters = new[] {OpParameter.UInt16, OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x06, Parameters = new[] {OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = null, Parameters = new OpParameter[0]}
                }
            };
            OpCodes[0x31] = new Op
            {
                OpCode = 0x31,
                OpParameters = new[]
                {
                    new OpSwitch
                    {
                        SwitchByte = 0x00,
                        Parameters = new[]
                        {
                            OpParameter.UInt16, OpParameter.UInt32, OpParameter.UInt16, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt16,
                            OpParameter.UInt16, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.Utf8String
                        }
                    },
                    new OpSwitch {SwitchByte = 0x01, Parameters = new[] {OpParameter.UInt16, OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x02, Parameters = new[] {OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x03, Parameters = new[] {OpParameter.UInt16, OpParameter.UInt32, OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x05, Parameters = new[] {OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x06, Parameters = new[] {OpParameter.UInt16}},
                    new OpSwitch
                    {
                        SwitchByte = 0x32,
                        Parameters = new[]
                        {
                            OpParameter.UInt16, OpParameter.UInt32, OpParameter.UInt16, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt16,
                            OpParameter.UInt16, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.Utf8String
                        }
                    },
                    new OpSwitch {SwitchByte = 0x33, Parameters = new[] {OpParameter.UInt16, OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x34, Parameters = new[] {OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x35, Parameters = new[] {OpParameter.UInt16, OpParameter.UInt32, OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x37, Parameters = new[] {OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x38, Parameters = new[] {OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x39, Parameters = new[] {OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x3A, Parameters = new[] {OpParameter.UInt16, OpParameter.UInt16, OpParameter.UInt16, OpParameter.UInt16, OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x64, Parameters = new[] {OpParameter.UInt16, OpParameter.UInt32, OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x65, Parameters = new[] {OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32}},
                    new OpSwitch {SwitchByte = 0x96, Parameters = new[] {OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0xFE, Parameters = new[] {OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0xFF, Parameters = new[] {OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32}},
                    new OpSwitch {SwitchByte = null, Parameters = new OpParameter[0]}
                }
            };
            OpCodes[0x32] = new Op
            {
                OpCode = 0x32,
                OpParameters = new[]
                {
                    new OpSwitch {SwitchByte = 0x01, Parameters = new[] {OpParameter.UInt16, OpParameter.Utf8String, OpParameter.Utf8String}},
                    new OpSwitch {SwitchByte = 0x02, Parameters = new[] {OpParameter.UInt16, OpParameter.Byte, OpParameter.Byte, OpParameter.Byte, OpParameter.Byte, }},
                    new OpSwitch {SwitchByte = 0x03, Parameters = new[] {OpParameter.UInt16, OpParameter.Utf8String, OpParameter.Utf8String, OpParameter.Utf8String, OpParameter.Utf8String}},
                    new OpSwitch {SwitchByte = 0x04, Parameters = new[] {OpParameter.UInt16, OpParameter.Utf8String}},
                    new OpSwitch {SwitchByte = null, Parameters = new[] {OpParameter.UInt16}}
                }
            };

            OpCodes[0x33] = null;
            OpCodes[0x34] = null;
            OpCodes[0x35] = null;
            OpCodes[0x36] = new Op
            {
                OpCode = 0x36,
                OpParameters = new[]
                {
                    // caso especial
                    new OpSwitch {SwitchByte = 0x00, Parameters = new OpParameter[0]}
                }
            };
            OpCodes[0x37] = new Op
            {
                OpCode = 0x37,
                OpParameters = new[]
                {
                    new OpSwitch {SwitchByte = 0x00, Parameters = new [] {OpParameter.UInt16, OpParameter.Byte}}
                }
            };
            OpCodes[0x38] = null;
            OpCodes[0x39] = new Op
            {
                OpCode = 0x39,
                OpParameters = new[]
                {
                    new OpSwitch {SwitchByte = 0x05, Parameters = new[] {OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x0A, Parameters = new[] {OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x0B, Parameters = new[] {OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x0C, Parameters = new[] {OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x10, Parameters = new[] {OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x69, Parameters = new[] {OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0xFE, Parameters = new[] {OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0xFF, Parameters = new[] {OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = null, Parameters = new[] {OpParameter.UInt16, OpParameter.UInt32}}
                }
            };
            OpCodes[0x3A] = null;
            OpCodes[0x3B] = null;
            OpCodes[0x3C] = new Op
            {
                OpCode = 0x3C,
                OpParameters = new[]
                {
                    // El caso es especial y no se definen los parámetros aquí
                    new OpSwitch {SwitchByte = 0x00, Parameters = new OpParameter[0]}
                }
            };
            OpCodes[0x3D] = new Op
            {
                OpCode = 0x3D,
                OpParameters = new[]
                {
                    new OpSwitch {SwitchByte = 0x00, Parameters = new[] {OpParameter.Byte, OpParameter.Utf8String, OpParameter.UInt16}}
                }
            };
            OpCodes[0x3E] = null;
            OpCodes[0x3F] = new Op
            {
                OpCode = 0x3F,
                OpParameters = new[]
                {
                    new OpSwitch {SwitchByte = 0x00, Parameters = new[] {OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x01, Parameters = new[] {OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x04, Parameters = new[] {OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x03, Parameters = new OpParameter[0]},
                    new OpSwitch {SwitchByte = 0x05, Parameters = new[] {OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x06, Parameters = new[] {OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x08, Parameters = new[] {OpParameter.UInt16, OpParameter.Byte, OpParameter.Byte, OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x09, Parameters = new[] {OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x0A, Parameters = new[] {OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x0B, Parameters = new[] {OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = null, Parameters = new OpParameter[0]}
                }
            };
            OpCodes[0x40] = null;
            OpCodes[0x41] = null;
            OpCodes[0x42] = null;
            OpCodes[0x43] = new Op
            {
                OpCode = 0x43,
                OpParameters = new[]
                {
                    new OpSwitch {SwitchByte = 0x00, Parameters = new[] {OpParameter.UInt32}}
                }
            };
            OpCodes[0x44] = null;
            OpCodes[0x45] = new Op
            {
                OpCode = 0x45,
                OpParameters = new[]
                {
                    new OpSwitch {SwitchByte = 0x00, Parameters = new[] {OpParameter.Utf8String, OpParameter.Utf8String}}
                }
            };
            OpCodes[0x46] = null;
            OpCodes[0x47] = null;
            OpCodes[0x48] = null;
            OpCodes[0x49] = null;
            OpCodes[0x4A] = new Op
            {
                OpCode = 0x4A,
                OpParameters = new[]
                {
                    new OpSwitch
                    {
                        SwitchByte = 0x00,
                        Parameters = new[]
                        {
                            OpParameter.Byte, OpParameter.UInt16, OpParameter.UInt16, OpParameter.UInt16, OpParameter.UInt16, OpParameter.UInt16,
                            OpParameter.UInt16, OpParameter.UInt16, OpParameter.UInt16, OpParameter.UInt16, OpParameter.UInt16, OpParameter.UInt32,
                            OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.Byte, OpParameter.Utf8String
                        }
                    }
                }
            };
            OpCodes[0x4B] = new Op
            {
                OpCode = 0x4B,
                OpParameters = new[]
                {
                    new OpSwitch
                    {
                        SwitchByte = 0x00,
                        Parameters = new[]
                        {
                            OpParameter.Byte, OpParameter.Byte, OpParameter.Byte, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32,
                            OpParameter.UInt32, OpParameter.UInt32
                        }
                    }
                }
            };
            OpCodes[0x4C] = new Op
            {
                OpCode = 0x4C,
                OpParameters = new[]
                {
                    new OpSwitch {SwitchByte = 0x00, Parameters = new[] {OpParameter.Byte, OpParameter.Byte}}
                }
            };
            OpCodes[0x4D] = new Op
            {
                OpCode = 0x4D,
                OpParameters = new[]
                {
                    new OpSwitch {SwitchByte = 0x00, Parameters = new[] {OpParameter.Byte}}
                }
            };
            OpCodes[0x4E] = null;
            OpCodes[0x4F] = null;
            OpCodes[0x50] = null;
            OpCodes[0x51] = null;
            OpCodes[0x52] = null;
            OpCodes[0x53] = new Op
            {
                OpCode = 0x53,
                OpParameters = new[]
                {
                    new OpSwitch {SwitchByte = 0x00, Parameters = new[] {OpParameter.Byte, OpParameter.UInt16, OpParameter.UInt16, OpParameter.UInt32}}
                }
            };
            OpCodes[0x54] = null;
            OpCodes[0x55] = null;
            OpCodes[0x56] = null;
            OpCodes[0x57] = null;
            OpCodes[0x58] = null;
            OpCodes[0x59] = null;
            OpCodes[0x5A] = null;
            OpCodes[0x5B] = null;
            OpCodes[0x5C] = new Op
            {
                OpCode = 0x5C,
                OpParameters = new[]
                {
                    new OpSwitch {SwitchByte = 0x00, Parameters = new[] {OpParameter.UInt16, OpParameter.UInt32}},
                    new OpSwitch {SwitchByte = 0x01, Parameters = new[] {OpParameter.UInt16, OpParameter.UInt32}},
                    new OpSwitch {SwitchByte = null, Parameters = new[] {OpParameter.UInt16}}
                }
            };
            OpCodes[0x5D] = new Op
            {
                OpCode = 0x5D,
                OpParameters = new[]
                {
                    new OpSwitch {SwitchByte = 0x00, Parameters = new[] {OpParameter.Utf8String, OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x01, Parameters = new[] {OpParameter.Utf8String, OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x02, Parameters = new[] {OpParameter.Utf8String, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32}},
                    new OpSwitch {SwitchByte = 0x03, Parameters = new[] {OpParameter.Utf8String, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32}},
                    new OpSwitch {SwitchByte = 0x04, Parameters = new[] {OpParameter.Utf8String, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32}},
                    new OpSwitch {SwitchByte = 0x07, Parameters = new[] {OpParameter.Utf8String, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt16, OpParameter.Byte}},
                    new OpSwitch {SwitchByte = 0x08, Parameters = new[] {OpParameter.Utf8String, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt16, OpParameter.Byte}},
                    new OpSwitch {SwitchByte = null, Parameters = new[] {OpParameter.Utf8String}}
                }
            };
            OpCodes[0x5E] = null;
            OpCodes[0x5F] = null;
            OpCodes[0x60] = null;
            OpCodes[0x61] = null;
            OpCodes[0x62] = null;
            OpCodes[0x63] = null;
            OpCodes[0x64] = null;
            OpCodes[0x65] = null;
            OpCodes[0x66] = new Op
            {
                OpCode = 0x66,
                OpParameters = new[]
                {
                    new OpSwitch {SwitchByte = 0x00, Parameters = new[] {OpParameter.UInt32, OpParameter.UInt16}}
                }
            };
            OpCodes[0x67] = null;
            OpCodes[0x68] = null;
            OpCodes[0x69] = null;
            OpCodes[0x6A] = null;
            OpCodes[0x6B] = null;
            OpCodes[0x6C] = null;
            OpCodes[0x6D] = null;
            OpCodes[0x6E] = null;
            OpCodes[0x6F] = null;
            OpCodes[0x70] = null;
            OpCodes[0x71] = null;
            OpCodes[0x72] = null;
            OpCodes[0x73] = new Op
            {
                OpCode = 0x73,
                OpParameters = new[]
                {
                    new OpSwitch {SwitchByte = 0x00, Parameters = new OpParameter[0]},
                    new OpSwitch {SwitchByte = 0x01, Parameters = new OpParameter[0]},
                    new OpSwitch {SwitchByte = 0x02, Parameters = new[] {OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x03, Parameters = new[] {OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32}},
                    new OpSwitch {SwitchByte = 0x04, Parameters = new[] {OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = 0x05, Parameters = new[] {OpParameter.UInt32}},
                    new OpSwitch {SwitchByte = 0x06, Parameters = new[] {OpParameter.UInt16}},
                    new OpSwitch {SwitchByte = null, Parameters = new OpParameter[0]}
                }
            };
            OpCodes[0x74] = null;
            OpCodes[0x75] = null;
            OpCodes[0x76] = null;
            OpCodes[0x77] = null;
            OpCodes[0x78] = null;
            OpCodes[0x79] = null;
            OpCodes[0x7A] = null;
            OpCodes[0x7B] = null;
            OpCodes[0x7C] = null;
            OpCodes[0x7D] = null;
            OpCodes[0x7E] = null;
            OpCodes[0x7F] = null;
            OpCodes[0x80] = new Op
            {
                OpCode = 0x80,
                OpParameters = new[]
                {
                    new OpSwitch {SwitchByte = 0x00, Parameters = new[] {OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.UInt32, OpParameter.Byte}}
                }
            };
            OpCodes[0x81] = null;
            OpCodes[0x82] = null;
            OpCodes[0x83] = null;
            OpCodes[0x84] = null;
            OpCodes[0x85] = null;
            OpCodes[0x86] = null;
            OpCodes[0x87] = null;
            OpCodes[0x88] = null;
            OpCodes[0x89] = null;
            OpCodes[0x8A] = null;
            OpCodes[0x8B] = null;
            OpCodes[0x8C] = null;
            OpCodes[0x8D] = null;
            OpCodes[0x8E] = null;
            OpCodes[0x8F] = null;
            OpCodes[0x90] = null;
            OpCodes[0x91] = null;
            OpCodes[0x92] = null;
            OpCodes[0x93] = null;
            OpCodes[0x94] = null;
            OpCodes[0x95] = null;
            OpCodes[0x96] = null;
            OpCodes[0x97] = null;
            OpCodes[0x98] = null;
            OpCodes[0x99] = null;
            OpCodes[0x9A] = null;
            OpCodes[0x9B] = null;
            OpCodes[0x9C] = null;
            OpCodes[0x9D] = null;
            OpCodes[0x9E] = null;
            OpCodes[0x9F] = null;
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
                var op = input.ReadByte();

                //result.AddRange(ExtractOperationSubtitles(input, op));
                result.Add(ParseOp(input, op));
            }

            return result;
        }

        private string ParseOp(ExtendedBinaryReader input, byte opByte)
        {
            var opOffset = input.Position - 1;

            if (opByte >= 160)
            {
                return $"0x{opOffset:X8}\tNOT_OP_{opByte:X2}()";
            }

            var op = OpCodes[opByte];
            
            if (op == null)
            {
                return $"0x{opOffset:X8}\tOP_UNKNOWN{opByte:X2}()";
            }

            if (op.OpCode == 0x06)
            {
                // El switch es un op especial
                var scpExpression = ParseScpExpression(input);
                var caseCount = input.ReadByte();
                var cases = new List<string>();
                for (var i = 0; i < caseCount; i++)
                {
                    cases.Add($"[{input.ReadUInt16()}: {input.ReadInt32():X8}]");
                }
                cases.Add($"[default: {input.ReadInt32():X8}]");

                return $"0x{opOffset:X8}\t{op.OpName}({scpExpression}, {string.Join(", ", cases)})";
            }

            if (op.OpCode == 0x36)
            {
                // op especial
                var p1 = input.ReadUInt16();
                var p2 = input.ReadUInt16();

                if (p2 == 0xFE02 || p2 == 0xFE03)
                {
                    return $"0x{opOffset:X8}\tOp_{op.OpCode:X2}({p1}, {p2}, {input.ReadUInt32()}, {input.ReadUInt32()}, {input.ReadUInt32()}, {input.ReadUInt32()}, {input.ReadUInt32()}, {input.ReadByte()}, {input.ReadUInt16()})";
                }

                return $"0x{opOffset:X8}\tOp_{op.OpCode:X2}({p1}, {p2}, {input.ReadUInt32()}, {input.ReadUInt32()}, {input.ReadUInt32()}, {input.ReadUInt32()}, {input.ReadByte()}, {input.ReadUInt16()})";
            }

            if (op.OpCode == 0x3C)
            {
                // op especial
                var p1 = input.ReadUInt16();
                var p2 = input.ReadUInt16();

                if (p2 == ushort.MaxValue)
                {
                    return $"0x{opOffset:X8}\tOp_{op.OpCode:X2}({p1}, {p2}, {input.ReadUInt16()}, {input.ReadUInt32()}, {input.ReadUInt32()}, {input.ReadUInt32()})";
                }

                return $"0x{opOffset:X8}\tOp_{op.OpCode:X2}({p1}, {p2}, {input.ReadUInt16()})";
            }

            if (op.OpCode == 0x1A)
            {
                // op especial
                var p1 = input.ReadUInt16();
                var tmp = new List<string>();

                
                var p2 = input.PeekByte();

                if (p2 < 0x20)
                {
                    p2 = input.ReadByte();
                    tmp.Add($"{p2}");
                    switch (p2)
                    {
                        case 0x10:
                            tmp.Add($"{input.ReadUInt16()}");
                            break;
                        case 0x11:
                        case 0x12:
                            tmp.Add($"{input.ReadUInt32()}");
                            break;
                    }
                }
                tmp.Add($"\"{input.ReadString()}\"");

                return $"0x{opOffset:X8}\tOp_{op.OpCode:X2}({p1}, {string.Join(", ", tmp)})";
                
            }

            OpSwitch selected;
            string opName;
            if (op.OpParameters.Length == 1)
            {
                selected = op.OpParameters[0];
                opName = !string.IsNullOrEmpty(op.OpName) ? op.OpName : $"Op_{opByte:X2}";
            }
            else
            {
                var switchByte = input.ReadByte();
                selected = op.OpParameters.FirstOrDefault(x => x.SwitchByte == switchByte);
                if (selected == null)
                {
                    var defaultSwitch = op.OpParameters.FirstOrDefault(x => x.SwitchByte == null);
                    if (defaultSwitch != null)
                    {
                        selected = defaultSwitch;
                    }
                    else
                    {
                        return $"0x{opOffset:X8}\tOP_UNKNOWN{opByte:X2}{switchByte:X2}()";
                    }
                }
                opName = !string.IsNullOrEmpty(op.OpName) ? op.OpName : $"Op_{opByte:X2}{switchByte:X2}";
            }

            var paramList = new List<string>(selected.Parameters.Length);
            foreach (var opParameter in selected.Parameters)
            {
                switch (opParameter)
                {
                    case OpParameter.Byte:
                        paramList.Add($"{input.ReadByte()}");
                        break;
                    case OpParameter.ByteHex:
                        paramList.Add($"0x{input.ReadByte():X2}");
                        break;
                    case OpParameter.UInt16:
                        paramList.Add($"{input.ReadUInt16()}");
                        break;
                    case OpParameter.UInt16Hex:
                        paramList.Add($"0x{input.ReadUInt16():X4}");
                        break;
                    case OpParameter.UInt32:
                        paramList.Add($"{input.ReadUInt32()}");
                        break;
                    case OpParameter.UInt32Hex:
                        paramList.Add($"0x{input.ReadUInt32():X8}");
                        break;
                    case OpParameter.AsciiString:
                        paramList.Add($"\"{input.ReadString(System.Text.Encoding.ASCII)}\"");
                        break;
                    case OpParameter.Utf8String:
                        paramList.Add($"\"{input.ReadString(System.Text.Encoding.UTF8)}\"");
                        break;
                    case OpParameter.String:
                        paramList.Add($"\"{input.ReadString()}\"");
                        break;
                    case OpParameter.ScpExpression:
                        paramList.Add($"{ParseScpExpression(input)}");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            var parameters = string.Join(", ", paramList);
            return $"0x{opOffset:X8}\t{opName}({parameters})";
        }

        private string ParseScpExpression(ExtendedBinaryReader input)
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
                            var tmp = ParseOp(input, b);

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

            return $"EVAL_SCP({string.Join(", ", result)})";
        }
    }
}


