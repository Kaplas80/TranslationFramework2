namespace TFGame.DiscoElysium.Files.AssemblyCSharp
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using dnlib.DotNet;
    using dnlib.DotNet.Emit;
    using TF.Core.Files;
    using TF.Core.TranslationEntities;

    public class File : BinaryTextFile
    {
        public File(string gameName, string path, string changesFolder, System.Text.Encoding encoding) : base(gameName, path, changesFolder, encoding)
        {
        }

        protected override IList<Subtitle> GetSubtitles()
        {
            var result = new List<Subtitle>();

            var youSubtitle = new Subtitle {Text = "You", Loaded = "You", Translation = "You", Offset = 0};
            youSubtitle.PropertyChanged += SubtitlePropertyChanged;
            result.Add(youSubtitle);

            ModuleDefMD module = ModuleDefMD.Load(Path);

            TypeDef skillType = module.Find("Sunshine.Metric.Skill", false);
            MethodDef constructor = skillType.FindMethod(".cctor");

            int i = 1;
            foreach (Instruction instr in constructor.Body.Instructions)
            {
                if (instr.OpCode != OpCodes.Ldstr)
                {
                    continue;
                }

                string str = (string) instr.Operand;

                var subtitle = new Subtitle {Text = str, Loaded = str, Translation = str, Offset = i};
                subtitle.PropertyChanged += SubtitlePropertyChanged;
                result.Add(subtitle);
                i++;
            }

            var agentSubtitle = new Subtitle {Text = "Tutorial Agent", Loaded = "Tutorial Agent", Translation = "Tutorial Agent", Offset = i};
            agentSubtitle.PropertyChanged += SubtitlePropertyChanged;
            result.Add(agentSubtitle);

            result.Sort();
            LoadChanges(result);

            return result;
        }

        public override void Rebuild(string outputFolder)
        {
            string outputPath = System.IO.Path.Combine(outputFolder, RelativePath);
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath));

            IList<Subtitle> subtitles = GetSubtitles();

            Dictionary<string, string> dict = subtitles.Where(subtitle => subtitle.Text != subtitle.Translation).ToDictionary(subtitle => subtitle.Text, subtitle => subtitle.Translation);
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

                        if (dict.TryGetValue(str, out string translation))
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
