using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using TF.Core.Entities;
using TFGame.Shenmue.Files;


namespace TFGame.Shenmue
{
    public class Game : IGame
    {
        public string Id => "d71a256b-9816-4597-b5e6-220066f51230";
        public string Name => "Shenmue I & II";
        public string Description => "Build Id: 3291213";
        public Image Icon => Resources.Icon; // https://www.deviantart.com/clarence1996/art/Shenmue-I-and-II-760577046
        public int Version => 1;
        public System.Text.Encoding FileEncoding => new Encoding();

        public GameFileContainer[] GetContainers(string path)
        {
            var result = new List<GameFileContainer>();

            var subs = new GameFileSearch()
            {
                RelativePath = @".\tex\assets\subs",
                SearchPattern = "english.sub.0db084a7",
                IsWildcard = false,
                RecursiveSearch = false,
                FileType = typeof(Files.Sub.File)
            };

            var common = new GameFileContainer
            {
                Path = @".\sm1\archives\dx11\data\common_5be2c578.tac",
                Type = ContainerType.CompressedFile
            };
            common.FileSearches.Add(subs);


            result.Add(common);
            return result.ToArray();
        }

        public void ExtractFile(string inputFile, string outputPath)
        {
            var fileName = Path.GetFileName(inputFile);
            var extension = Path.GetExtension(inputFile);

            if (extension.StartsWith(".tac"))
            {
                TacFile.Extract(inputFile, outputPath);
            }
        }

        public void RepackFile(string inputPath, string outputFile, bool compress)
        {
            var fileName = Path.GetFileName(outputFile);
            var extension = Path.GetExtension(outputFile);

            if (extension.StartsWith(".tac"))
            {
                TacFile.Repack(inputPath, outputFile, compress);
            }
        }

        public void PreprocessContainer(TranslationFileContainer container, string containerPath, string extractionPath)
        {
        }

        public void PostprocessContainer(TranslationFileContainer container, string containerPath, string extractionPath)
        {
        }
    }
}
