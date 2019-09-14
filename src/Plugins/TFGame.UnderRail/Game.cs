using System.Collections.Generic;
using System.Drawing;
using System.IO;
using TF.Core.Entities;

namespace TFGame.UnderRail
{
    public class Game : IGame
    {
        public string Id => "ad08639d-0f5b-47fb-aa9d-b412e44ecfcd";
        public string Name => "UnderRail";
        public string Description => "v1.1.0.12";
        public Image Icon => Resources.Icon; // https://www.deviantart.com/sony33d/art/UnderRail-584195186
        public int Version => 2;
        public System.Text.Encoding FileEncoding => new Encoding();

        public GameFileContainer[] GetContainers(string path)
        {
            var result = new List<GameFileContainer>();

            var exe = new GameFileSearch()
            {
                RelativePath = ".",
                SearchPattern = "underrail.exe",
                IsWildcard = false,
                RecursiveSearch = false,
                FileType = typeof(Files.Exe.File)
            };

            var root = new GameFileContainer
            {
                Path = @".\",
                Type = ContainerType.Folder
            };
            root.FileSearches.Add(exe);

            result.Add(root);

            var udlgs = new GameFileSearch()
            {
                RelativePath = @".",
                SearchPattern = "*.udlg",
                IsWildcard = true,
                RecursiveSearch = true,
                FileType = typeof(Files.Dialog.File)
            };

            var dialogs = new GameFileContainer()
            {
                Path = @".\data\dialogs",
                Type = ContainerType.Folder
            };

            dialogs.FileSearches.Add(udlgs);
            result.Add(dialogs);

            var ks = new GameFileSearch()
            {
                RelativePath = @".",
                SearchPattern = "*.k",
                IsWildcard = true,
                RecursiveSearch = true,
                FileType = typeof(Files.Knowledge.File)
            };

            var knowledge = new GameFileContainer()
            {
                Path = @".\data\knowledge",
                Type = ContainerType.Folder
            };

            knowledge.FileSearches.Add(ks);
            result.Add(knowledge);

            var items = new GameFileSearch()
            {
                RelativePath = @".",
                SearchPattern = "*.item",
                IsWildcard = true,
                RecursiveSearch = true,
                FileType = typeof(Files.Item.File)
            };

            var itemfolder = new GameFileContainer()
            {
                Path = @".\data\rules\items",
                Type = ContainerType.Folder
            };

            itemfolder.FileSearches.Add(items);
            result.Add(itemfolder);

            return result.ToArray();
        }

        public void ExtractFile(string inputFile, string outputPath)
        {
            var fileName = Path.GetFileName(inputFile);
            var extension = Path.GetExtension(inputFile);

            if (extension.StartsWith(".dat"))
            {
            }
        }

        public void RepackFile(string inputPath, string outputFile, bool compress)
        {
            var fileName = Path.GetFileName(outputFile);
            var extension = Path.GetExtension(outputFile);

            if (extension.StartsWith(".dat"))
            {
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
