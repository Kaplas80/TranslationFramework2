using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TF.Core.TranslationEntities;
using TF.IO;
using TFGame.PhoenixWrightTrilogy.Core;

namespace TFGame.PhoenixWrightTrilogy.Files.Mdt
{
    public class File : EncryptedTextFile
    {
        private class Op
        {
            public ushort Code;
            public string Name;
            public int ParamCount;
        }

        private readonly Op[] GameOps;

        public override string LineEnding => "\r\n";

        public File(string path, string changesFolder, System.Text.Encoding encoding) : base(path, changesFolder, encoding)
        {
            GameOps = new Op[128];

            GameOps[0x00] = new Op { Code = 0x00, Name = "Nop", ParamCount = 0 };
            GameOps[0x01] = new Op { Code = 0x01, Name = "NewLine", ParamCount = 0 };
            GameOps[0x02] = new Op { Code = 0x02, Name = "ReadKey", ParamCount = 0 };
            GameOps[0x03] = new Op { Code = 0x03, Name = "SetTextColor", ParamCount = 1 };
            GameOps[0x04] = new Op { Code = 0x04, Name = "Op_04", ParamCount = 1 }; // Parece algo de una tecla pulsada (GSJoy.Trg)
            GameOps[0x05] = new Op { Code = 0x05, Name = "PlayBGM", ParamCount = 2 };
            GameOps[0x06] = new Op { Code = 0x06, Name = "ControlSE", ParamCount = 2 };
            GameOps[0x07] = new Op { Code = 0x07, Name = "Op_07", ParamCount = 0 }; // Comparte código con 0x02 (ReadKey) pero no se exactamente qué hace
            GameOps[0x08] = new Op { Code = 0x08, Name = "Op_08", ParamCount = 2 };
            GameOps[0x09] = new Op { Code = 0x09, Name = "Op_09", ParamCount = 3 };
            GameOps[0x0A] = new Op { Code = 0x0A, Name = "Op_0A", ParamCount = 1 }; // Comparte código con 0x02 (ReadKey) pero no se exactamente qué hace
            GameOps[0x0B] = new Op { Code = 0x0B, Name = "SetMessageTime", ParamCount = 1 };
            GameOps[0x0C] = new Op { Code = 0x0C, Name = "Wait", ParamCount = 1 };
            GameOps[0x0D] = new Op { Code = 0x0D, Name = "Exit", ParamCount = 0 };
            GameOps[0x0E] = new Op { Code = 0x0E, Name = "SetSpeakerId", ParamCount = 1 };
            GameOps[0x0F] = new Op { Code = 0x0F, Name = "SetTukkomi", ParamCount = 1 };
            GameOps[0x10] = new Op { Code = 0x10, Name = "SetGSFlag", ParamCount = 1 };
            GameOps[0x11] = new Op { Code = 0x11, Name = "Op_11", ParamCount = 0 };
            GameOps[0x12] = new Op { Code = 0x12, Name = "PlayFadeCtrl", ParamCount = 3 };
            GameOps[0x13] = new Op { Code = 0x13, Name = "SetItemPlateCtrl", ParamCount = 1 };
            GameOps[0x14] = new Op { Code = 0x14, Name = "CloseItemPlateCtrl", ParamCount = 0 };
            GameOps[0x15] = new Op { Code = 0x15, Name = "Op_15", ParamCount = 0 };
            GameOps[0x16] = new Op { Code = 0x16, Name = "NextScenario", ParamCount = 0 };
            GameOps[0x17] = new Op { Code = 0x17, Name = "AddRecord", ParamCount = 1 };
            GameOps[0x18] = new Op { Code = 0x18, Name = "DeleteRecord", ParamCount = 1 };
            GameOps[0x19] = new Op { Code = 0x19, Name = "UpdateRecord", ParamCount = 2 };
            GameOps[0x1A] = new Op { Code = 0x1A, Name = "CourtScroll", ParamCount = 4 };
            GameOps[0x1B] = new Op { Code = 0x1B, Name = "SetBackground", ParamCount = 1 };
            GameOps[0x1C] = new Op { Code = 0x1C, Name = "Op_1C", ParamCount = 1 };
            GameOps[0x1D] = new Op { Code = 0x1D, Name = "ScrollBackground", ParamCount = 1 };
            GameOps[0x1E] = new Op { Code = 0x1E, Name = "PlayCharacterAnimation", ParamCount = 3 };
            GameOps[0x1F] = new Op { Code = 0x1F, Name = "StopCutUpScroll", ParamCount = 0 };
            GameOps[0x20] = new Op { Code = 0x20, Name = "SetNextNumber", ParamCount = 1 };
            GameOps[0x21] = new Op { Code = 0x21, Name = "Op_21", ParamCount = 0 };
            GameOps[0x22] = new Op { Code = 0x22, Name = "FadeOutBGM", ParamCount = 2 };
            GameOps[0x23] = new Op { Code = 0x23, Name = "ReplayBGM", ParamCount = 2 };
            GameOps[0x24] = new Op { Code = 0x24, Name = "Op_24", ParamCount = 0 };
            GameOps[0x25] = new Op { Code = 0x25, Name = "Nop2", ParamCount = 0 };
            GameOps[0x26] = new Op { Code = 0x26, Name = "SetStatusFlag", ParamCount = 1 };
            GameOps[0x27] = new Op { Code = 0x27, Name = "Quake", ParamCount = 2 };
            GameOps[0x28] = new Op { Code = 0x28, Name = "Op_28", ParamCount = 1 };
            GameOps[0x29] = new Op { Code = 0x29, Name = "Op_29", ParamCount = 1 };
            GameOps[0x2A] = new Op { Code = 0x2A, Name = "Op_2A", ParamCount = 3 };
            GameOps[0x2B] = new Op { Code = 0x2B, Name = "DoDamage", ParamCount = 0 };
            GameOps[0x2C] = new Op { Code = 0x2C, Name = "Op_2C", ParamCount = 1 };
            GameOps[0x2D] = new Op { Code = 0x2D, Name = "Op_2D", ParamCount = 0 }; // Comparte código con 0x02 (ReadKey) pero no se exactamente qué hace
            GameOps[0x2E] = new Op { Code = 0x2E, Name = "ClearText", ParamCount = 0 };
            GameOps[0x2F] = new Op { Code = 0x2F, Name = "PlayObjectAnimation", ParamCount = 2 };
            GameOps[0x30] = new Op { Code = 0x30, Name = "SetMessageSE", ParamCount = 1 };
            GameOps[0x31] = new Op { Code = 0x31, Name = "CharFade", ParamCount = 2 };
            GameOps[0x32] = new Op { Code = 0x32, Name = "MapData", ParamCount = 2 };
            GameOps[0x33] = new Op { Code = 0x33, Name = "MapData2", ParamCount = 5 };
            GameOps[0x34] = new Op { Code = 0x34, Name = "Op_34", ParamCount = 0 };
            GameOps[0x35] = new Op { Code = 0x35, Name = "Op_35", ParamCount = 2 };
            GameOps[0x36] = new Op { Code = 0x36, Name = "Op_36", ParamCount = 1 };
            GameOps[0x37] = new Op { Code = 0x37, Name = "SetTalkDataSw", ParamCount = 2 };
            GameOps[0x38] = new Op { Code = 0x38, Name = "SetActiveCharacterAnimation", ParamCount = 1 };
            GameOps[0x39] = new Op { Code = 0x39, Name = "LoadSprite", ParamCount = 1 };
            GameOps[0x3A] = new Op { Code = 0x3A, Name = "SetMapIconPosition", ParamCount = 3 };
            GameOps[0x3B] = new Op { Code = 0x3B, Name = "SetMapIconParameters", ParamCount = 2 };
            GameOps[0x3C] = new Op { Code = 0x3C, Name = "MapIconBlink", ParamCount = 1 };
            GameOps[0x3D] = new Op { Code = 0x3D, Name = "MapIconVisible", ParamCount = 1 };
            GameOps[0x3E] = new Op { Code = 0x3E, Name = "Op_3E", ParamCount = 1 };
            GameOps[0x3F] = new Op { Code = 0x3F, Name = "Op_3F", ParamCount = 0 };
            GameOps[0x40] = new Op { Code = 0x40, Name = "SetStatusPointCursolOff", ParamCount = 0 };
            GameOps[0x41] = new Op { Code = 0x41, Name = "SetSubWindowReqTMain", ParamCount = 0 };
            GameOps[0x42] = new Op { Code = 0x42, Name = "SetSoundFlag", ParamCount = 1 };
            GameOps[0x43] = new Op { Code = 0x43, Name = "SetLifeGauge", ParamCount = 1 };
            GameOps[0x44] = new Op { Code = 0x44, Name = "Judgment", ParamCount = 1 };
            GameOps[0x45] = new Op { Code = 0x45, Name = "Op_45", ParamCount = 0 };
            GameOps[0x46] = new Op { Code = 0x46, Name = "CutIn", ParamCount = 1 };
            GameOps[0x47] = new Op { Code = 0x47, Name = "VolumeChangeBGM", ParamCount = 2 };
            GameOps[0x48] = new Op { Code = 0x48, Name = "SetMessageBoardPos", ParamCount = 2 };
            GameOps[0x49] = new Op { Code = 0x49, Name = "Op_49", ParamCount = 0 };
            GameOps[0x4A] = new Op { Code = 0x4A, Name = "CheckGuilty", ParamCount = 1 };
            GameOps[0x4B] = new Op { Code = 0x4B, Name = "MapIconChangeUV", ParamCount = 2 };
            GameOps[0x4C] = new Op { Code = 0x4C, Name = "IsBackgroundScrolling", ParamCount = 0 };
            GameOps[0x4D] = new Op { Code = 0x4D, Name = "Op_4D", ParamCount = 0 };
            GameOps[0x4E] = new Op { Code = 0x4E, Name = "AnimationGoIdle", ParamCount = 2 };
            GameOps[0x4F] = new Op { Code = 0x4F, Name = "SetPsylockData", ParamCount = 7 };
            GameOps[0x50] = new Op { Code = 0x50, Name = "ClearPsylock", ParamCount = 1 };
            GameOps[0x51] = new Op { Code = 0x51, Name = "RoomSeqChange", ParamCount = 2 };
            GameOps[0x52] = new Op { Code = 0x52, Name = "SetSubWindowReqMagatamaMenuOn", ParamCount = 1 };
            GameOps[0x53] = new Op { Code = 0x53, Name = "DisablePsyMenu", ParamCount = 0 };
            GameOps[0x54] = new Op { Code = 0x54, Name = "SetLifeGauge2", ParamCount = 2 };
            GameOps[0x55] = new Op { Code = 0x55, Name = "Op_55", ParamCount = 1 };
            GameOps[0x56] = new Op { Code = 0x56, Name = "Op_56", ParamCount = 0 };
            GameOps[0x57] = new Op { Code = 0x57, Name = "Op_57", ParamCount = 1 };
            GameOps[0x58] = new Op { Code = 0x58, Name = "PsylockDispResetStatic", ParamCount = 0 };
            GameOps[0x59] = new Op { Code = 0x59, Name = "Op_59", ParamCount = 1 };
            GameOps[0x5A] = new Op { Code = 0x5A, Name = "Op_5A", ParamCount = 1 };
            GameOps[0x5B] = new Op { Code = 0x5B, Name = "TanteiMenuRecov", ParamCount = 2 };
            GameOps[0x5C] = new Op { Code = 0x5C, Name = "MosaicRun", ParamCount = 3 };
            GameOps[0x5D] = new Op { Code = 0x5D, Name = "SetTextFlag", ParamCount = 1 };
            GameOps[0x5E] = new Op { Code = 0x5E, Name = "Op_5E", ParamCount = 0 };
            GameOps[0x5F] = new Op { Code = 0x5F, Name = "MonochromeSet", ParamCount = 3 };
            GameOps[0x60] = new Op { Code = 0x60, Name = "Op_60", ParamCount = 4 };
            GameOps[0x61] = new Op { Code = 0x61, Name = "Op_61", ParamCount = 3 };
            GameOps[0x62] = new Op { Code = 0x62, Name = "PsylockToNormalBackground", ParamCount = 0 };
            GameOps[0x63] = new Op { Code = 0x63, Name = "PsylockRedisp", ParamCount = 0 };
            GameOps[0x64] = new Op { Code = 0x64, Name = "Op_64", ParamCount = 1 };
            GameOps[0x65] = new Op { Code = 0x65, Name = "SetBackgroundParts", ParamCount = 2 };
            GameOps[0x66] = new Op { Code = 0x66, Name = "Op_66", ParamCount = 3 };
            GameOps[0x67] = new Op { Code = 0x67, Name = "MessageInit", ParamCount = 0 };
            GameOps[0x68] = new Op { Code = 0x68, Name = "Op_68", ParamCount = 0 };
            GameOps[0x69] = new Op { Code = 0x69, Name = "Op_69", ParamCount = 2 };
            GameOps[0x6A] = new Op { Code = 0x6A, Name = "LoadScenario", ParamCount = 1 };
            GameOps[0x6B] = new Op { Code = 0x6B, Name = "Op_6B", ParamCount = 3 };
            GameOps[0x6C] = new Op { Code = 0x6C, Name = "Op_6C", ParamCount = 0 }; // En el GS3 creo que tiene 1 parametro
            GameOps[0x6D] = new Op { Code = 0x6D, Name = "Op_6D", ParamCount = 1 };
            GameOps[0x6E] = new Op { Code = 0x6E, Name = "SetStatusThreeLine", ParamCount = 1 };
            GameOps[0x6F] = new Op { Code = 0x6F, Name = "SetBkEndMess", ParamCount = 1 };
            GameOps[0x70] = new Op { Code = 0x70, Name = "Op_70", ParamCount = 0 };
            GameOps[0x71] = new Op { Code = 0x71, Name = "Op_71", ParamCount = 3 };
            GameOps[0x72] = new Op { Code = 0x72, Name = "Op_72", ParamCount = 0 };
            GameOps[0x73] = new Op { Code = 0x73, Name = "Op_73", ParamCount = 0 };
            GameOps[0x74] = new Op { Code = 0x74, Name = "Op_74", ParamCount = 2 };
            GameOps[0x75] = new Op { Code = 0x75, Name = "SetVideo", ParamCount = 4 };
            GameOps[0x76] = new Op { Code = 0x76, Name = "Op_76", ParamCount = 2 };
            GameOps[0x77] = new Op { Code = 0x77, Name = "FadeOutSE", ParamCount = 2 };
            GameOps[0x78] = new Op { Code = 0x78, Name = "Op_78", ParamCount = 1 };
            GameOps[0x79] = new Op { Code = 0x79, Name = "Op_79", ParamCount = 0 };
            GameOps[0x7A] = new Op { Code = 0x7A, Name = "Op_7A", ParamCount = 1 };
            GameOps[0x7B] = new Op { Code = 0x7B, Name = "Op_7B", ParamCount = 2 };
            GameOps[0x7C] = new Op { Code = 0x7C, Name = "GS2UpdateSc3Opening", ParamCount = 0 };
            GameOps[0x7D] = new Op { Code = 0x7D, Name = "GS2HanabiraMove", ParamCount = 1 };
            GameOps[0x7E] = new Op { Code = 0x7E, Name = "GS2SpotlightMoveFocus", ParamCount = 1 };
            GameOps[0x7F] = new Op { Code = 0x7F, Name = "DrawIconText", ParamCount = 1 };
        }

        protected override PlainText GetText()
        {
            var result = new PlainText();
            var txt = new StringBuilder();

            var encryptedData = System.IO.File.ReadAllBytes(Path);
            var data = EncryptionManager.DecryptData(encryptedData);

            using (var ms = new MemoryStream(data))
            using (var input = new ExtendedBinaryReader(ms))
            {
                var messageCount = input.ReadUInt16();
                var dummy = input.ReadUInt16();

                var messageOffset = new uint[messageCount];
                var messageSize = new uint[messageCount];
                for (var i = 0; i < messageCount; i++)
                {
                    messageOffset[i] = input.ReadUInt32();
                }

                for (var i = 0; i < messageCount - 1; i++)
                {
                    messageSize[i] = messageOffset[i + 1] - messageOffset[i];
                }

                messageSize[messageCount - 1] = (uint) (data.Length - messageOffset[messageCount - 1]);

                for (var i = 0; i < messageCount; i++)
                {
                    input.Seek(messageOffset[i], SeekOrigin.Begin);
                    if (i > 0)
                    {
                        txt.AppendLine();
                    }

                    txt.AppendLine(messageOffset[i] < data.Length ? $"StartMessage({input.Position});" : $"StartMessageExtra({input.Position});");

                    while (input.Position < input.Length && input.Position - messageOffset[i] < messageSize[i])
                    {
                        var offset = input.Position;
                        var type = input.ReadUInt16();

                        if (type < 128)
                        {
                            var op = GameOps[type];

                            var args = new string[op.ParamCount];
                            for (var j = 0; j < op.ParamCount; j++)
                            {
                                args[j] = string.Concat(input.ReadUInt16().ToString());
                            }

                            txt.AppendLine($"{op.Name}({string.Join(", ", args)});");
                        }
                        else
                        {
                            var sb = new StringBuilder();
                            while (input.Position < input.Length && type >= 128)
                            {
                                var c = type - 128;
                                sb.Append((char) c);
                                type = input.ReadUInt16();
                            }

                            var value = sb.ToString().ToHalfWidthChars();

                            var text = string.Concat("Text(\"", value, "\");");

                            txt.AppendLine(text);

                            if (input.Position < input.Length)
                            {
                                input.Seek(-2, SeekOrigin.Current);
                            }
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

        public override void Rebuild(string outputFolder)
        {
            var outputPath = System.IO.Path.Combine(outputFolder, RelativePath);
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath));

            var subtitles = GetText();

            byte[] outputData;
            var outputMessageOffset = new List<uint>();
            var outputMessageExtraOffset = new List<uint>();

            using (var msOutput = new MemoryStream())
            using (var output = new ExtendedBinaryWriter(msOutput, FileEncoding))
            {
                var outputOffset = 0u;
                var lines = subtitles.Translation.Split(new [] {LineEnding}, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    var pattern1 = "(?<op>\\w+)\\((?<params>.*)\\)\\;";
                    var pattern2 = "(?<op>\\w+)\\(\"(?<params>.*)\"\\)\\;";

                    var pattern = Regex.IsMatch(line, pattern2) ? pattern2 : pattern1;
                    var matches = Regex.Matches(line, pattern);
                    var opName = matches[0].Groups["op"].Value;
                    var opParam = matches[0].Groups["params"].Value;

                    if (opName != "Text")
                    {
                        if (opName == "StartMessage")
                        {
                            outputMessageOffset.Add(outputOffset);
                            continue;
                        }

                        if (opName == "StartMessageExtra")
                        {
                            outputMessageExtraOffset.Add(uint.Parse(opParam));
                            continue;
                        }

                        var op = GameOps.FirstOrDefault(x => x.Name == opName);
                        if (op != null)
                        {
                            output.Write(op.Code);
                            var split = opParam.Split(',');
                            foreach (var s in split)
                            {
                                if (!string.IsNullOrWhiteSpace(s))
                                {
                                    output.Write(ushort.Parse(s.Trim()));
                                }
                            }
                        }
                    }
                    else
                    {
                        var outputText = opParam.ToFullWidthChars();
                        foreach (var chr in outputText)
                        {
                            var c = (ushort) (chr + 128);
                            output.Write(c);
                        }
                    }

                    outputOffset = (uint)output.Position;
                }

                outputData = msOutput.ToArray();
            }

            byte[] unencryptedFile;
            using (var msOutput = new MemoryStream())
            using (var output = new ExtendedBinaryWriter(msOutput, FileEncoding))
            {
                var totalMessageCount = outputMessageOffset.Count + outputMessageExtraOffset.Count;
                output.Write((ushort) totalMessageCount);
                output.Write((ushort) 0);
                foreach (var offset in outputMessageOffset)
                {
                    output.Write((uint)(offset + 4 + totalMessageCount * 4));
                }

                foreach (var offset in outputMessageExtraOffset)
                {
                    output.Write(offset);
                }
                output.Write(outputData);

                unencryptedFile = msOutput.ToArray();
            }

            var encryptedFile = EncryptionManager.EncryptData(unencryptedFile);
            System.IO.File.WriteAllBytes(outputPath, encryptedFile);
        }
    }
}
