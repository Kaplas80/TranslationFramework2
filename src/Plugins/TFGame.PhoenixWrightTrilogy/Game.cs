using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using TF.Core.Entities;
using TF.Core.Files;
using TFGame.PhoenixWrightTrilogy.Files;

namespace TFGame.PhoenixWrightTrilogy
{
    using System.Linq;

    public class Game : UnityGame.Game
    {
        public override string Id => "762580b0-6bf3-49df-8d09-d6b8804b79c6";
        public override string Name => "Phoenix Wright: Ace Attorney Trilogy";
        public override string Description => "Build Id: 3633830";
        public override Image Icon => Resources.Icon; // https://www.deviantart.com/clarence1996/art/Phoenix-Wright-Ace-Attorney-Trilogy-793438577
        public override int Version => 1;
        public override System.Text.Encoding FileEncoding => Encoding.Unicode;

        private IEnumerable<GameFileContainer> GetScenario(string scenario, string path)
        {
            var result = new List<GameFileContainer>();

            var scenarioSingleLineSearch = new GameFileSearch()
            {
                RelativePath = @".",
                SearchPattern = $"{scenario}*.cho;{scenario}_sel*.bin;{scenario}_ba*.bin;{scenario}_nolb*.bin;",
                IsWildcard = true,
                RecursiveSearch = false,
                FileType = typeof(Files.SingleLine.File)
            };

            var scenarioMultiLineSearch = new GameFileSearch()
            {
                RelativePath = @".",
                SearchPattern = $"{scenario}_note*.bin;",
                IsWildcard = true,
                RecursiveSearch = false,
                FileType = typeof(Files.MultiLine.File)
            };

            var scenarioMdtSearch = new GameFileSearch()
            {
                RelativePath = @".",
                SearchPattern = $"*.mdt;",
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

            result.Add(container);

            var ddsSearch = new GameFileSearch()
            {
                RelativePath = @".",
                SearchPattern = "*.dds",
                IsWildcard = true,
                RecursiveSearch = true,
                FileType = typeof(DDS2File)
            };

            var etc = new GameFileContainerSearch
            {
                RelativePath = $@"PWAAT_Data\StreamingAssets\{scenario.ToUpper()}",
                TypeSearch = ContainerType.CompressedFile,
                RecursiveSearch = true,
                SearchPattern = "*.unity3d",
                Exclusions = { "AnimationDictionaries", "Movie", "science", "OtherInclusions", },
            };

            etc.FileSearches.Add(ddsSearch);

            result.AddRange(etc.GetContainers(path));

            return result.ToArray();
        }

        public override GameFileContainer[] GetContainers(string path)
        {
            var result = new List<GameFileContainer>();

            var menuMultiLineSearch = new GameFileSearch()
            {
                RelativePath = @".",
                SearchPattern = "title_text*.bin;option_text*.bin;save_text*.bin;system_text*.bin;credit_text.bin",
                IsWildcard = true,
                RecursiveSearch = false,
                FileType = typeof(Files.MultiLine.File)
            };

            var menuSingleLineSearch = new GameFileSearch()
            {
                RelativePath = @".",
                SearchPattern = "common_text*.bin;platform_text*.bin",
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

            var ddsSearch = new GameFileSearch()
            {
                RelativePath = @".",
                SearchPattern = "*.dds",
                IsWildcard = true,
                RecursiveSearch = true,
                FileType = typeof(DDS2File)
            };

            var images = new GameFileContainerSearch
            {
                RelativePath = $@"PWAAT_Data\StreamingAssets",
                TypeSearch = ContainerType.CompressedFile,
                RecursiveSearch = true,
                SearchPattern = "*.unity3d",
                Exclusions = { "GS1", "GS2", "GS3", "Sound" }
            };

            images.FileSearches.Add(ddsSearch);

            result.AddRange(images.GetContainers(path));

            result.AddRange(GetScenario("gs1", path));
            result.AddRange(GetScenario("gs2", path));
            result.AddRange(GetScenario("gs3", path));

            return result.ToArray();
        }

        public override void ExtractFile(string inputFile, string outputPath)
        {
            var fileName = Path.GetFileName(inputFile);
            var extension = Path.GetExtension(inputFile);

            var unencryptedFiles = new[]
            {
                "film01_confront.unity3d", 
                "film02_confront.unity3d",
            };
            
            if (!extension.StartsWith(".unity3d"))
            {
                return;
            }

            if (unencryptedFiles.Contains(fileName))
            {
                Unity3DFile.Extract(inputFile, outputPath);
            }
            else
            {
                EncryptedUnity3DFile.Extract(inputFile, outputPath);
            }
        }

        public override void RepackFile(string inputPath, string outputFile, bool compress)
        {
            var fileName = Path.GetFileName(outputFile);
            var extension = Path.GetExtension(outputFile);

            var unencryptedFiles = new[]
            {
                "film01_confront.unity3d", 
                "film02_confront.unity3d",
            };

            if (!extension.StartsWith(".unity3d"))
            {
                return;
            }

            if (unencryptedFiles.Contains(fileName))
            {
                Unity3DFile.Repack(inputPath, outputFile, compress);
            }
            else
            {
                EncryptedUnity3DFile.Repack(inputPath, outputFile, compress);
            }
        }
    }
}

