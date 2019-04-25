using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using TF.Core.Entities;

namespace TFGame.TrailsColdSteel
{
    public class Game : IGame
    {
        public string Id => "3e8301f7-0417-4707-8db9-ee856cea5761";
        public string Name => "The Legend of Heroes: Trails of Cold Steel";
        public string Description => "Build Id: 3170008";
        public Image Icon => Resources.Icon; // https://www.deviantart.com/andonovmarko/art/The-Legend-of-Heroes-Trails-of-Cold-Steel-Icon-704948381
        public int Version => 1;
        public System.Text.Encoding FileEncoding => new Encoding();

        public GameFileContainer[] GetContainers(string path)
        {
            var result = new List<GameFileContainer>();

            var scenarioSearch = new GameFileSearch
            {
                RelativePath = ".",
                SearchPattern = "*.dat",
                IsWildcard = true,
                RecursiveSearch = false,
                FileType = typeof(Files.Dat.File)
            };

            var scenarioContainer = new GameFileContainer
            {
                Path = @"data\scripts\scena\dat_us",
                Type = ContainerType.Folder
            };

            scenarioContainer.FileSearches.Add(scenarioSearch);

            result.Add(scenarioContainer);

            return result.ToArray();
        }

        public void ExtractFile(string inputFile, string outputPath)
        {
            var fileName = Path.GetFileName(inputFile);
            var extension = Path.GetExtension(inputFile);
        }

        public void RepackFile(string inputPath, string outputFile, bool compress)
        {
            var fileName = Path.GetFileName(outputFile);
            var extension = Path.GetExtension(outputFile);
        }

        public void PreprocessContainer(TranslationFileContainer container, string containerPath, string extractionPath)
        {
        }

        public void PostprocessContainer(TranslationFileContainer container, string containerPath, string extractionPath)
        {
        }
    }
}
