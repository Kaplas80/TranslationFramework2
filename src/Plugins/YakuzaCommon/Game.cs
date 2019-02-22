using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using TF.Core.Entities;
using YakuzaCommon.Files;

namespace YakuzaCommon
{
    public abstract class Game : IGame
    {
        public virtual string Id { get; }
        public virtual string Name { get; }
        public virtual string Description { get; }
        public virtual Image Icon { get; }
        public virtual int Version { get; }

        public virtual GameFileContainer[] GetContainers(string path)
        {
            var result = new List<GameFileContainer>();
            return result.ToArray();
        }

        public TranslationFile GetFile(string path, string changesFolder)
        {
            TranslationFile result;

            var fileName = Path.GetFileName(path);
            var extension = Path.GetExtension(path);

            if (fileName.EndsWith("cmn.bin"))
            {
                result = new Files.CmnBin.File(path, changesFolder);
            }
            else if (fileName.EndsWith("ai_popup.bin"))
            {
                result = new Files.AiPopup.File(path, changesFolder);
            }
            else if (fileName.EndsWith("arms_repair.bin"))
            {
                result = new Files.SimpleSubtitle.File(path, changesFolder);
            }
            else if (Regex.IsMatch(fileName, @"bar[\d]{4}\.bin"))
            {
                result = new Files.Bar.File(path, changesFolder);
            }
            else if (fileName.EndsWith("blacksmith.bin"))
            {
                result = new Files.Blacksmith.File(path, changesFolder);
            }
            else
            {
                switch (extension)
                {
                    case ".dds":
                        {
                            result = new TF.Core.Files.DDS.DDSFile(path, changesFolder);
                            break;
                        }

                    default:
                        {
                            result = new TranslationFile(path, changesFolder);
                            break;
                        }
                }
            }


            return result;
        }

        public void ExtractFile(string inputFile, string outputPath)
        {
            var extension = Path.GetExtension(inputFile);

            if (extension == ".par")
            {
                var parFile = new ParFile(inputFile);
                parFile.Extract(outputPath);
            }
        }

        public void RepackFile(string inputPath, string outputFile, bool compress)
        {
            var extension = Path.GetExtension(outputFile);

            if (extension == ".par")
            {
                ParFile.Repack(inputPath, outputFile, compress);
            }
        }
    }
}
