using System.Collections.Generic;
using System.IO;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using TF.Core.Files;
using TF.Core.TranslationEntities;
using TF.IO;

namespace TFGame.UnderRail.Files.Exe
{
    public class File : BinaryTextFile
    {
        public File(string path, string changesFolder, System.Text.Encoding encoding) : base(path, changesFolder, encoding)
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
                        var str = System.Text.Encoding.Unicode.GetString(input.ReadBytes((int) length - 1));
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

            var dict = subtitles.Where(subtitle => subtitle.Text != subtitle.Translation).ToDictionary(subtitle => subtitle.Text, subtitle => subtitle.Translation);
            var module = ModuleDefMD.Load(Path);
            
            foreach (var t in module.Types)
            {
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

                        var str = (string) instr.Operand;

                        if (dict.TryGetValue(str, out var translation))
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
