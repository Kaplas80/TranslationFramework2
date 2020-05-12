using System.Collections.Generic;
using System.IO;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using TF.Core.Files;
using TF.Core.POCO;
using TF.Core.TranslationEntities;
using TF.IO;

namespace TFGame.UnderRail.Files.Exe
{
    public class File : BinaryTextFileWithIds
    {
        public override LineEnding LineEnding => new LineEnding
        {
            RealLineEnding = "\r\n",
            ShownLineEnding = "\\r\\n",
            PoLineEnding = "\n",
            ScintillaLineEnding = ScintillaLineEndings.CrLf,
        };

        public File(string gameName, string path, string changesFolder, System.Text.Encoding encoding) : base(gameName, path, changesFolder, encoding)
        {
        }

        protected override IList<Subtitle> GetSubtitles()
        {
            var result = new List<Subtitle>();
            var module = ModuleDefMD.Load(Path);

            foreach (var t in module.Types)
            {
                foreach (var a in t.CustomAttributes)
                {
                    for (var i = a.ConstructorArguments.Count - 1; i >= 0; i--)
                    {
                        var arg = a.ConstructorArguments[i];
                        if (arg.Type.FullName == "System.String")
                        {
                            var str = ((UTF8String)arg.Value).String.Replace("\r\n", "\\r\\n");
                            var id = $"{t.Name}_attr_{a.TypeFullName}_arg_{i}";
                            var subtitle = new SubtitleWithId { Id = id, Text = str, Loaded = str, Translation = str, Offset = 0 };
                            subtitle.PropertyChanged += SubtitlePropertyChanged;
                            result.Add(subtitle);
                        }
                    }
                }

                foreach (var f in t.Fields)
                {
                    if (f.HasConstant && f.FieldType.FullName == "System.String")
                    {
                        var str = ((string)f.Constant.Value).Replace("\r\n", "\\r\\n");

                        var id = $"{t.Name}_field_{f.Name.String}";
                        var subtitle = new SubtitleWithId { Id = id, Text = str, Loaded = str, Translation = str, Offset = 0 };
                        subtitle.PropertyChanged += SubtitlePropertyChanged;
                        result.Add(subtitle);
                    }
                }

                foreach (var m in t.Methods)
                {
                    if (!m.HasBody)
                    {
                        continue;
                    }

                    foreach (var instr in m.Body.Instructions)
                    {
                        if (instr.OpCode != OpCodes.Ldstr)
                        {
                            continue;
                        }

                        var str = ((string)(instr.Operand)).Replace("\r\n", "\\r\\n");

                        var id = $"{t.Name}_method_{m.Name}_rva_{m.RVA}_{instr.Offset}";
                        var subtitle = new SubtitleWithId { Id = id, Text = str, Loaded = str, Translation = str, Offset = 0 };
                        subtitle.PropertyChanged += SubtitlePropertyChanged;
                        result.Add(subtitle);
                    }
                }
            }

            result.Sort();
            LoadChanges(result);

            return result;
        }

        public override void Rebuild(string outputFolder)
        {
            var outputPath = System.IO.Path.Combine(outputFolder, RelativePath);
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath));

            var subtitles = GetSubtitles();

            var dict = subtitles.Where(subtitle => subtitle.Text != subtitle.Translation).Select(subtitle => subtitle as SubtitleWithId).ToDictionary(subtitle => subtitle.Id, subtitle => subtitle.Translation.Replace("\\r\\n", "\r\n"));
            var module = ModuleDefMD.Load(Path);
            
            foreach (var t in module.Types)
            {
                foreach (var a in t.CustomAttributes)
                {
                    for (var i = a.ConstructorArguments.Count - 1; i >= 0; i--)
                    {
                        var arg = a.ConstructorArguments[i];
                        if (arg.Type.FullName == "System.String")
                        {
                            var str = ((UTF8String)arg.Value).String;
                            var id = $"{t.Name}_attr_{a.TypeFullName}_arg_{i}";
                            if (dict.TryGetValue(id, out var translation))
                            {
                                arg.Value = new UTF8String(translation);

                                a.ConstructorArguments.RemoveAt(i);
                                a.ConstructorArguments.Insert(i, arg);
                            }
                        }
                    }
                }

                foreach (var f in t.Fields)
                {
                    if (f.HasConstant && f.FieldType.FullName == "System.String")
                    {
                        var str = (string)f.Constant.Value;
                        var id = $"{t.Name}_field_{f.Name.String}";
                        if (dict.TryGetValue(id, out var translation))
                        {
                            f.Constant.Value = translation;
                        }
                    }
                }

                foreach (var m in t.Methods)
                {
                    if (!m.HasBody)
                    {
                        continue;
                    }

                    foreach (var instr in m.Body.Instructions)
                    {
                        if (instr.OpCode != OpCodes.Ldstr)
                        {
                            continue;
                        }

                        var id = $"{t.Name}_method_{m.Name}_rva_{m.RVA}_{instr.Offset}";
                        var str = (string)instr.Operand;

                        if (dict.TryGetValue(id, out var translation))
                        {
                            instr.Operand = translation;
                        }
                    }
                }
            }
            
            module.Write(outputPath);
        }
    }
}
