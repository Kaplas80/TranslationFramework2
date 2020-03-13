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
        public string Description => "Build Id: 3675355";
        public Image Icon => Resources.Icon; // https://www.deviantart.com/andonovmarko/art/The-Legend-of-Heroes-Trails-in-the-Sky-Icon-v1-586602301
        public int Version => 4;
        public System.Text.Encoding FileEncoding => new Encoding();
        public bool ExportOnlyModifiedFiles => false;
        public GameFileContainer[] GetContainers(string path)
        {
            var result = new List<GameFileContainer>();

            var exe = new GameFileSearch()
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
                SearchPattern = "C_BTN01 ._CH;C_BTN02 ._CH;C_EMOTIO._CH;C_ICON1 ._CH;C_MOUSE ._CH;C_STATUS._CH",
                IsWildcard = true,
                RecursiveSearch = false,
                FileType = typeof(Files.Images.ImageType1)
            };

            var imagesT2 = new GameFileSearch
            {
                RelativePath = ".",
                SearchPattern = "H_BTN01 ._CH;H_BTN02 ._CH;H_EMOTIO._CH;H_ICON1 ._CH;H_MOUSE ._CH;H_STATUS._CH",
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
                SearchPattern = "C_*._CH;",
                IsWildcard = true,
                RecursiveSearch = false,
                FileType = typeof(Files.Images.ImageType5),
                Exclusions =
                {
                    "C_BBS02 ._CH",
                    "C_BOOK  ._CH",
                    "C_COOK  ._CH",
                    "C_ORB000._CH",
                    "C_ORB001._CH",
                    "C_ORB002._CH",
                    "C_ORB003._CH",
                    "C_ORB004._CH",
                    "C_ORB005._CH",
                    "C_ORB006._CH",
                    "C_ORB007._CH",
                    "C_TUTO20._CH",
                    "C_VIS014._CH", // ImageType8 1024
                }
            };

            var imagesDT04_2 = new GameFileSearch
            {
                RelativePath = ".",
                SearchPattern = "H_*._CH;",
                IsWildcard = true,
                RecursiveSearch = false,
                FileType = typeof(Files.Images.ImageType6),
            };

            var imagesDT04_3 = new GameFileSearch
            {
                RelativePath = ".",
                SearchPattern = "W_*._CH;",
                IsWildcard = true,
                RecursiveSearch = false,
                FileType = typeof(Files.Images.ImageType7),
                Exclusions =
                {
                    "W_BACK01._CH",
                    "W_BACK11._CH",
                    "W_BACK12._CH",
                    "W_BACK13._CH",
                    "W_BACK14._CH",
                    "W_BACK15._CH",
                }
            };

            var imagesDT04_4 = new GameFileSearch
            {
                RelativePath = ".",
                SearchPattern = "C_VIS014._CH;W_BACK??._CH",
                IsWildcard = true,
                RecursiveSearch = false,
                FileType = typeof(Files.Images.ImageType8),
            };

            var imagesDT04_5 = new GameFileSearch
            {
                RelativePath = ".",
                SearchPattern = "C_COOK  ._CH",
                IsWildcard = true,
                RecursiveSearch = false,
                FileType = typeof(Files.Images.ImageType4),
            };

            var dt04 = new GameFileContainer
            {
                Path = @".\ED6_DT04.dat",
                Type = ContainerType.CompressedFile
            };
            dt04.FileSearches.Add(imagesDT04);
            dt04.FileSearches.Add(imagesDT04_2);
            dt04.FileSearches.Add(imagesDT04_3);
            dt04.FileSearches.Add(imagesDT04_4);
            dt04.FileSearches.Add(imagesDT04_5);

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
                FileType = typeof(Files.Images.ImageType2)
            };

            var dt0F = new GameFileContainer
            {
                Path = @".\ED6_DT0F.dat",
                Type = ContainerType.CompressedFile
            };
            dt0F.FileSearches.Add(imagesDT0F);
            dt0F.FileSearches.Add(imagesDT0F_2);

            var msdtDT10 = new GameFileSearch
            {
                RelativePath = ".",
                SearchPattern = "MS*._DT",
                Exclusions = { "MS30100 ._DT" },
                IsWildcard = true,
                RecursiveSearch = false,
                FileType = typeof(Files.MSDT.File)
            };

            var msdtDT10_2 = new GameFileSearch
            {
                RelativePath = ".",
                SearchPattern = "MS30100 ._DT",
                IsWildcard = false,
                RecursiveSearch = false,
                FileType = typeof(Files.MSDT.File2)
            };

            var dt10 = new GameFileContainer
            {
                Path = @".\ED6_DT10.dat",
                Type = ContainerType.CompressedFile
            };
            dt10.FileSearches.Add(msdtDT10);
            dt10.FileSearches.Add(msdtDT10_2);
            

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

            var mnsnote = new GameFileSearch
            {
                RelativePath = ".",
                SearchPattern = "MNSNOTE2._DT",
                IsWildcard = false,
                RecursiveSearch = false,
                FileType = typeof(Files.MNSNOTE2.File)
            };

            var dt1C = new GameFileContainer
            {
                Path = @".\ED6_DT1C.dat",
                Type = ContainerType.CompressedFile
            };

            dt1C.FileSearches.Add(imagesDT1C_1);
            dt1C.FileSearches.Add(imagesDT1C_2);
            dt1C.FileSearches.Add(imagesDT1C_3);
            dt1C.FileSearches.Add(mnsnote);

            result.Add(root);
            result.Add(dt00);
            result.Add(dt01);
            result.Add(dt02);
            result.Add(dt04);
            result.Add(dt0F);
            result.Add(dt10);
            result.Add(dt1C);

            return result.ToArray();
        }

        public void ExtractFile(string inputFile, string outputPath)
        {
            string extension = Path.GetExtension(inputFile);

            if (!string.IsNullOrEmpty(extension) && extension.StartsWith(".dat"))
            {
                DatFile.Extract(inputFile, outputPath);
            }
        }

        public void RepackFile(string inputPath, string outputFile, bool compress)
        {
            string extension = Path.GetExtension(outputFile);

            if (!string.IsNullOrEmpty(extension) && extension.StartsWith(".dat"))
            {
                DatFile.Repack(inputPath, outputFile, compress);
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
