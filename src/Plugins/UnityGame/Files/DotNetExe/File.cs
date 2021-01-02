namespace UnityGame.Files.DotNetExe
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using dnlib.DotNet;
    using dnlib.DotNet.Emit;
    using dnlib.DotNet.MD;
    using TF.Core.Files;
    using TF.Core.TranslationEntities;

    public class File : BinaryTextFileWithIds
    {
        public File(string gameName, string path, string changesFolder, System.Text.Encoding encoding) : base(gameName, path, changesFolder, encoding)
        {
        }

        protected override IList<Subtitle> GetSubtitles()
        {
            var result = new List<Subtitle>();
            ModuleDefMD module = ModuleDefMD.Load(Path);
            
            foreach (TypeDef t in module.Types)
            {
                foreach (MethodDef m in t.Methods)
                {
                    if (!m.HasBody)
                    {
                        continue;
                    }

                    foreach (Instruction instr in m.Body.Instructions)
                    {
                        if (instr.OpCode != OpCodes.Ldstr)
                        {
                            continue;
                        }

                        string str = (string) instr.Operand;
                        var subtitle = new SubtitleWithId {Text = str, Loaded = str, Translation = str, Id = $"{t.Name}/{m.Name}/{m.RVA.ToString()}/{instr.Offset}"};
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
            string outputPath = System.IO.Path.Combine(outputFolder, RelativePath);
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath));

            IList<Subtitle> subtitles = GetSubtitles();

            Dictionary<string, SubtitleWithId> dict = (from subtitle in subtitles where (!string.IsNullOrEmpty(subtitle.Text) && subtitle.Text != subtitle.Translation) select subtitle as SubtitleWithId).ToDictionary(subtitleWithId => subtitleWithId.Id);

            ModuleDefMD module = ModuleDefMD.Load(Path);
            
            foreach (TypeDef t in module.Types)
            {
                foreach (MethodDef m in t.Methods)
                {
                    if (!m.HasBody)
                    {
                        continue;
                    }

                    foreach (Instruction instr in m.Body.Instructions)
                    {
                        if (instr.OpCode != OpCodes.Ldstr)
                        {
                            continue;
                        }

                        string str = (string) instr.Operand;
                        string id = $"{t.Name}/{m.Name}/{m.RVA.ToString()}/{instr.Offset}";

                        if (dict.TryGetValue(id, out SubtitleWithId subtitle))
                        {
                            instr.Operand = subtitle.Translation;
                        }
                    }
                }
            }
            
            module.Write(outputPath);
        }
    }
}
