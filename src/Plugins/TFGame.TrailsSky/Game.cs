using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using TF.Core.Entities;
using TFGame.TrailsSky.Files;

namespace TFGame.TrailsSky
{
    public class Game : IGame
    {
        public string Id => "307504c9-90f7-45dd-a37f-4e3cd146a826";
        public string Name => "The Legend of Heroes: Trails in the Sky";
        public string Description => "Versión en inglés GOG";
        public Image Icon => Resources.Icon;
        public int Version => 1;
        public System.Text.Encoding FileEncoding => new Encoding();

        public GameFileContainer[] GetContainers(string path)
        {
            var result = new List<GameFileContainer>();

            var fonts = new GameFileSearch()
            {
                RelativePath = ".",
                SearchPattern = "FONT*._DA",
                IsWildcard = true,
                RecursiveSearch = false,
                FileType = typeof(TF.Core.Files.DDSFile)
            };

            var dt00 = new GameFileContainer
            {
                Path = @".\ED6_DT00.dat",
                Type = ContainerType.CompressedFile
            };
            dt00.FileSearches.Add(fonts);

            var scenarios = new GameFileSearch
            {
                RelativePath = ".",
                SearchPattern = "*._SN",
                IsWildcard = true,
                RecursiveSearch = false,
                FileType = typeof(Files.SN.File)
            };

            var dt01 = new GameFileContainer
            {
                Path = @".\ED6_DT01.dat",
                Type = ContainerType.CompressedFile
            };

            dt01.FileSearches.Add(scenarios);

            var bookFiles = new GameFileSearch
            {
                RelativePath = ".",
                SearchPattern = "T_BOOK??._DT",
                IsWildcard = true,
                RecursiveSearch = false,
                FileType = typeof(Files.DT.BookFile),
                Exclusions = { "T_BOOK00._DT" }
            };

            var cookFile = new GameFileSearch
            {
                RelativePath = ".",
                SearchPattern = "T_COOK2 ._DT",
                IsWildcard = false,
                RecursiveSearch = false,
                FileType = typeof(Files.DT.CookFile),
            };

            var itemFile = new GameFileSearch
            {
                RelativePath = ".",
                SearchPattern = "T_ITEM2 ._DT",
                IsWildcard = false,
                RecursiveSearch = false,
                FileType = typeof(Files.DT.ItemFile),
            };

            var magicFile = new GameFileSearch
            {
                RelativePath = ".",
                SearchPattern = "T_MAGIC ._DT",
                IsWildcard = false,
                RecursiveSearch = false,
                FileType = typeof(Files.DT.MagicFile),
            };

            var memoFile = new GameFileSearch
            {
                RelativePath = ".",
                SearchPattern = "T_MEMO  ._DT",
                IsWildcard = false,
                RecursiveSearch = false,
                FileType = typeof(Files.DT.MemoFile),
            };

            var nameFile = new GameFileSearch
            {
                RelativePath = ".",
                SearchPattern = "T_NAME  ._DT",
                IsWildcard = false,
                RecursiveSearch = false,
                FileType = typeof(Files.DT.NameFile),
            };

            var questFile = new GameFileSearch
            {
                RelativePath = ".",
                SearchPattern = "T_QUEST ._DT",
                IsWildcard = false,
                RecursiveSearch = false,
                FileType = typeof(Files.DT.QuestFile),
            };

            var shopFile = new GameFileSearch
            {
                RelativePath = ".",
                SearchPattern = "T_SHOP  ._DT",
                IsWildcard = false,
                RecursiveSearch = false,
                FileType = typeof(Files.DT.ShopFile),
            };

            var townFile = new GameFileSearch
            {
                RelativePath = ".",
                SearchPattern = "T_TOWN  ._DT",
                IsWildcard = false,
                RecursiveSearch = false,
                FileType = typeof(Files.DT.TownFile),
            };

            var dt02 = new GameFileContainer
            {
                Path = @".\ED6_DT02.dat",
                Type = ContainerType.CompressedFile
            };

            dt02.FileSearches.Add(bookFiles);
            dt02.FileSearches.Add(cookFile);
            dt02.FileSearches.Add(itemFile);
            dt02.FileSearches.Add(magicFile);
            dt02.FileSearches.Add(memoFile);
            dt02.FileSearches.Add(nameFile);
            dt02.FileSearches.Add(questFile);
            dt02.FileSearches.Add(shopFile);
            dt02.FileSearches.Add(townFile);

            var dt04 = new GameFileContainer
            {
                Path = @".\ED6_DT04.dat",
                Type = ContainerType.CompressedFile
            };

            var dt0F = new GameFileContainer
            {
                Path = @".\ED6_DTF.dat",
                Type = ContainerType.CompressedFile
            };

            var dt1C = new GameFileContainer
            {
                Path = @".\ED6_DT1C.dat",
                Type = ContainerType.CompressedFile
            };

            result.Add(dt00);
            result.Add(dt01);
            result.Add(dt02);
            result.Add(dt04);
            result.Add(dt0F);
            result.Add(dt1C);

            return result.ToArray();
        }

        public void ExtractFile(string inputFile, string outputPath)
        {
            var fileName = Path.GetFileName(inputFile);
            var extension = Path.GetExtension(inputFile);

            if (extension.StartsWith(".dat"))
            {
                DatFile.Extract(inputFile, outputPath);
            }
        }

        public void RepackFile(string inputPath, string outputFile, bool compress)
        {
            var fileName = Path.GetFileName(outputFile);
            var extension = Path.GetExtension(outputFile);

            if (extension.StartsWith(".dat"))
            {
                DatFile.Repack(inputPath, outputFile, compress);
            }
        }
    }
}
