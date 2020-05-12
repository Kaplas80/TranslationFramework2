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
    public class File : BinaryTextFile
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
            var usStream = module.USStream;
            
            using (var ms = new MemoryStream(usStream.CreateReader().ToArray()))
            using (var input = new ExtendedBinaryReader(ms, System.Text.Encoding.Unicode))
            {
                input.Skip(1);
                while (input.Position < input.Length)
                {
                    var offset = input.Position;
                    var length = input.ReadCompressedUInt32();
                    if (length > 0)
                    {
                        var str = System.Text.Encoding.Unicode.GetString(input.ReadBytes((int) length - 1)).Replace("\r\n", "\\r\\n");
                        input.Skip(1);

                        var subtitle = new Subtitle {Text = str, Loaded = str, Translation = str, Offset = offset};
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

            var dict = subtitles.Where(subtitle => subtitle.Text != subtitle.Translation).ToDictionary(subtitle => subtitle.Text.Replace("\\r\\n", "\r\n"), subtitle => subtitle.Translation.Replace("\\r\\n", "\r\n"));
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

                            if (dict.TryGetValue(str, out var translation))
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

                        if (dict.TryGetValue(str, out var translation))
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

                    var count = 0;

                    foreach (var instr in m.Body.Instructions)
                    {
                        if (instr.OpCode != OpCodes.Ldstr)
                        {
                            continue;
                        }

                        if ((t.Name != "d87") || (m.Name != "a") || (m.GetParamCount() != 0) || (count % 3 != 0))
                        {
                            var str = (string)instr.Operand;

                            if (dict.TryGetValue(str, out var translation))
                            {
                                instr.Operand = translation;
                            }
                        }

                        count++;
                    }
                }
            }
            
            module.Write(outputPath);
        }
    }
}
