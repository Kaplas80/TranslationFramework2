using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using TF.Core.Entities;

namespace TFGame.PhoenixWrightTrilogy
{
    public class Game : IGame
    {
        public string Id => "762580b0-6bf3-49df-8d09-d6b8804b79c6";
        public string Name => "Phoenix Wright: Ace Attorney Trilogy";
        public string Description => "Build Id 3633830";
        public Image Icon => Resources.Icon; // https://www.deviantart.com/dovellas/art/Phoenix-Wright-56731742
        public int Version => 1;
        public System.Text.Encoding FileEncoding => new Encoding();

        private GameFileContainer GetScenario(string scenario)
        {
            var scenarioSingleLineSearch = new GameFileSearch()
            {
                RelativePath = @".",
                SearchPattern = $"{scenario}_u.cho;{scenario}_sel_u.bin;{scenario}_ba_u.bin;{scenario}_nolb_u.bin;",
                IsWildcard = true,
                RecursiveSearch = false,
                FileType = typeof(Files.SingleLine.File)
            };

            var scenarioMultiLineSearch = new GameFileSearch()
            {
                RelativePath = @".",
                SearchPattern = $"{scenario}_note_u.bin;",
                IsWildcard = true,
                RecursiveSearch = false,
                FileType = typeof(Files.MultiLine.File)
            };

            var scenarioMdtSearch = new GameFileSearch()
            {
                RelativePath = @".",
                SearchPattern = $"*_u.mdt;",
                IsWildcard = true,
                RecursiveSearch = false,
                FileType = typeof(Files.Mdt.File)
            };

            var container = new GameFileContainer
            {
                Path = $@"PWAAT_Data\StreamingAssets\{scenario.ToUpper()}\scenario",
                Type = ContainerType.Folder
            };
            container.FileSearches.Add(scenarioSingleLineSearch);
            container.FileSearches.Add(scenarioMultiLineSearch);
            container.FileSearches.Add(scenarioMdtSearch);

            return container;
        }

        public GameFileContainer[] GetContainers(string path)
        {
            var result = new List<GameFileContainer>();

            var menuMultiLineSearch = new GameFileSearch()
            {
                RelativePath = @".",
                SearchPattern = "title_text_u.bin;option_text_u.bin;save_text_u.bin;system_text_u.bin;credit_text.bin",
                IsWildcard = true,
                RecursiveSearch = false,
                FileType = typeof(Files.MultiLine.File)
            };

            var menuSingleLineSearch = new GameFileSearch()
            {
                RelativePath = @".",
                SearchPattern = "common_text_u.bin;platform_text_u.bin",
                IsWildcard = true,
                RecursiveSearch = false,
                FileType = typeof(Files.SingleLine.File)
            };

            var menu = new GameFileContainer
            {
                Path = @"PWAAT_Data\StreamingAssets\menu\text",
                Type = ContainerType.Folder
            };
            menu.FileSearches.Add(menuMultiLineSearch);
            menu.FileSearches.Add(menuSingleLineSearch);

            result.Add(menu);

            result.Add(GetScenario("gs1"));
            result.Add(GetScenario("gs2"));
            result.Add(GetScenario("gs3"));

            return result.ToArray();
        }

        public void ExtractFile(string inputFile, string outputPath)
        {
        }

        public void RepackFile(string inputPath, string outputFile, bool compress)
        {
        }
    }
}

