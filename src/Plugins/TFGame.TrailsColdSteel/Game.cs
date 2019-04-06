using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using TF.Core.Entities;
using TFGame.TrailsColdSteel;

namespace TFGame.TrailsSky
{
    public class Game : IGame
    {
        public string Id => "7d38ad5e-2f86-4e8d-85c2-87571a7b333e";
        public string Name => "The Legend of Heroes: Trails of Cold Steel";
        public string Description => "Versión en inglés Steam v1.6";
        public Image Icon => Resources.Icon; // https://www.deviantart.com/andonovmarko/art/The-Legend-of-Heroes-Trails-of-Cold-Steel-Icon-704948381
        public int Version => 1;
        public System.Text.Encoding FileEncoding => Encoding.UTF8;

        public GameFileContainer[] GetContainers(string path)
        {
            var result = new List<GameFileContainer>();

            /*var exe = new GameFileSearch()
            {
                RelativePath = ".",
                SearchPattern = "ed6_win.exe",
                IsWildcard = false,
                RecursiveSearch = false,
                FileType = typeof(Files.Exe.File)
            };

            var exe2 = new GameFileSearch()
            {
                RelativePath = ".",
                SearchPattern = "ed6_win_DX9.exe",
                IsWildcard = false,
                RecursiveSearch = false,
                FileType = typeof(Files.Exe.DX9File)
            };

            var root = new GameFileContainer
            {
                Path = @".\",
                Type = ContainerType.Folder
            };
            root.FileSearches.Add(exe);
            root.FileSearches.Add(exe2);

            var fonts = new GameFileSearch
            {
                RelativePath = ".",
                SearchPattern = "FONT*._DA",
                IsWildcard = true,
                RecursiveSearch = false,
                FileType = typeof(Files.FONT.File)
            };

            var imagesT1 = new GameFileSearch
            {
                RelativePath = ".",
                SearchPattern = "C_BTN01 ._CH;C_BTN02 ._CH;C_EMOTIO._CH;C_ICON1 ._CH;C_MOUSE ._CH",
                IsWildcard = true,
                RecursiveSearch = false,
                FileType = typeof(Files.Images.ImageType1)
            };

            var imagesT2 = new GameFileSearch
            {
                RelativePath = ".",
                SearchPattern = "H_BTN01 ._CH;H_BTN02 ._CH;H_EMOTIO._CH;H_ICON1 ._CH;H_MOUSE ._CH",
                IsWildcard = true,
                RecursiveSearch = false,
                FileType = typeof(Files.Images.ImageType2)
            };

            var imagesT3 = new GameFileSearch
            {
                RelativePath = ".",
                SearchPattern = "C_CAMP02._CH",
                IsWildcard = false,
                RecursiveSearch = false,
                FileType = typeof(Files.Images.ImageType3)
            };

            var imagesT4 = new GameFileSearch
            {
                RelativePath = ".",
                SearchPattern = "H_CAMP02._CH",
                IsWildcard = false,
                RecursiveSearch = false,
                FileType = typeof(Files.Images.ImageType4)
            };

            var dt00 = new GameFileContainer
            {
                Path = @".\ED6_DT00.dat",
                Type = ContainerType.CompressedFile
            };
            dt00.FileSearches.Add(fonts);
            dt00.FileSearches.Add(imagesT1);
            dt00.FileSearches.Add(imagesT2);
            dt00.FileSearches.Add(imagesT3);
            dt00.FileSearches.Add(imagesT4);

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

            var imagesDT04 = new GameFileSearch
            {
                RelativePath = ".",
                SearchPattern = "C_ENCNT1._CH;C_GAMEOV._CH;C_NOTE??._CH;C_VIS018._CH;C_VIS019._CH;C_VIS020._CH;C_VIS021._CH;C_VIS022._CH;C_VIS023._CH;C_VIS024._CH;C_VIS025._CH;C_VIS026._CH;C_VIS027._CH;C_VIS040._CH;C_VIS041._CH;C_VIS042._CH;C_VIS043._CH;C_VIS044._CH;C_VIS045._CH;C_VIS046._CH;C_VIS047._CH;C_VIS048._CH;C_VIS049._CH;",
                IsWildcard = true,
                RecursiveSearch = false,
                FileType = typeof(Files.Images.ImageType5),
                Exclusions =
                {
                    "C_NOTE01._CH",
                    "C_NOTE02._CH",
                    "C_NOTE21._CH",
                    "C_NOTE22._CH",
                }
            };

            var imagesDT04_2 = new GameFileSearch
            {
                RelativePath = ".",
                SearchPattern = "H_ENCNT1._CH;H_GAMEOV._CH;H_NOTE??._CH;H_VIS018._CH;H_VIS019._CH;H_VIS020._CH;H_VIS021._CH;H_VIS022._CH;H_VIS023._CH;H_VIS024._CH;H_VIS025._CH;H_VIS026._CH;H_VIS027._CH;H_VIS040._CH;H_VIS041._CH;H_VIS042._CH;H_VIS043._CH;H_VIS044._CH;H_VIS045._CH;H_VIS046._CH;H_VIS047._CH;H_VIS048._CH;H_VIS049._CH;",
                IsWildcard = true,
                RecursiveSearch = false,
                FileType = typeof(Files.Images.ImageType6),
                Exclusions =
                {
                    "H_NOTE01._CH",
                    "H_NOTE02._CH",
                    "H_NOTE21._CH",
                    "H_NOTE22._CH",
                }
            };

            var dt04 = new GameFileContainer
            {
                Path = @".\ED6_DT04.dat",
                Type = ContainerType.CompressedFile
            };
            dt04.FileSearches.Add(imagesDT04);
            dt04.FileSearches.Add(imagesDT04_2);

            var imagesDT0F = new GameFileSearch
            {
                RelativePath = ".",
                SearchPattern = "BATTLE  ._CH;BATTLE2 ._CH;BTLMENU ._CH",
                IsWildcard = true,
                RecursiveSearch = false,
                FileType = typeof(Files.Images.ImageType1)
            };

            var imagesDT0F_2 = new GameFileSearch
            {
                RelativePath = ".",
                SearchPattern = "HBATTLE ._CH;HBATTLE2._CH;HBTLMENU._CH",
                IsWildcard = true,
                RecursiveSearch = false,
                FileType = typeof(Files.Images.ImageType1)
            };

            var dt0F = new GameFileContainer
            {
                Path = @".\ED6_DT0F.dat",
                Type = ContainerType.CompressedFile
            };
            dt0F.FileSearches.Add(imagesDT0F);
            dt0F.FileSearches.Add(imagesDT0F_2);

            var imagesDT1C_1 = new GameFileSearch
            {
                RelativePath = ".",
                SearchPattern = "AREA    ._CH",
                IsWildcard = false,
                RecursiveSearch = false,
                FileType = typeof(Files.Images.ImageType1)
            };

            var imagesDT1C_2 = new GameFileSearch
            {
                RelativePath = ".",
                SearchPattern = "ICONS   ._CH",
                IsWildcard = false,
                RecursiveSearch = false,
                FileType = typeof(Files.Images.ImageType1)
            };

            var imagesDT1C_3 = new GameFileSearch
            {
                RelativePath = ".",
                SearchPattern = "HICONS  ._CH",
                IsWildcard = false,
                RecursiveSearch = false,
                FileType = typeof(Files.Images.ImageType2)
            };

            var dt1C = new GameFileContainer
            {
                Path = @".\ED6_DT1C.dat",
                Type = ContainerType.CompressedFile
            };

            dt1C.FileSearches.Add(imagesDT1C_1);
            dt1C.FileSearches.Add(imagesDT1C_2);
            dt1C.FileSearches.Add(imagesDT1C_3);

            result.Add(root);
            result.Add(dt00);
            result.Add(dt01);
            result.Add(dt02);
            result.Add(dt04);
            result.Add(dt0F);
            result.Add(dt1C);
            */
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
    }
}
