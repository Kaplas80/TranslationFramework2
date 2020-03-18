namespace TFGame.NightCry
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using TF.Core.Entities;
    using TFGame.NightCry.Files;

    public class Game : UnityGame.Game
    {
        public override string Id => "9ea6a553-117a-452c-981a-57198ea2396b";
        public override string Name => "NightCry";
        public override string Description => "Build Id: 1188829";
        public override Image Icon => Resources.Icon; // https://www.deviantart.com/oufai/art/night-cry-Game-icon-605213305
        public override int Version => 1;
        public override System.Text.Encoding FileEncoding => System.Text.Encoding.GetEncoding("UTF-8");

        protected override string[] AllowedExtensions => new[]
        {
            ".assets", ""
        };

        public override GameFileContainer[] GetContainers(string path)
        {
            var result = new List<GameFileContainer>();

            var textSearch = new GameFileSearch()
            {
                RelativePath = @".",
                SearchPattern = "EventList.txt",
                IsWildcard = false,
                RecursiveSearch = true,
                FileType = typeof(Files.EventList.File)
            };

            var textureSearch = new GameFileSearch()
            {
                RelativePath = @".",
                SearchPattern = "GoodEnd_*.dds;staff_*.dds;TX_OBJ_Gimmick201_D*.dds;TX_107*.dds;TX_Default_61*.dds;*UI_*.dds;",
                IsWildcard = true,
                RecursiveSearch = true,
                FileType = typeof(TF.Core.Files.DDS2File),
            };

            var resources = new GameFileContainer
            {
                Path = @"NightCry_Data\resources.assets",
                Type = ContainerType.CompressedFile
            };
            resources.FileSearches.Add(textSearch);
            resources.FileSearches.Add(textureSearch);

            result.Add(resources);

            textureSearch = new GameFileSearch()
            {
                RelativePath = @".",
                SearchPattern = "*UI_*.dds;",
                IsWildcard = true,
                RecursiveSearch = true,
                FileType = typeof(TF.Core.Files.DDS2File),
            };

            var sharedAssets0 = new GameFileContainer
            {
                Path = @"NightCry_Data\sharedassets0.assets",
                Type = ContainerType.CompressedFile
            };

            sharedAssets0.FileSearches.Add(textureSearch);

            result.Add(sharedAssets0);

            return result.ToArray();
        }

        public override void ExtractFile(string inputFile, string outputPath)
        {
            string extension = Path.GetExtension(inputFile);

            if (AllowedExtensions.Contains(extension))
            {
                Unity3DFile.Extract(inputFile, outputPath);
            }
        }

        public override void RepackFile(string inputPath, string outputFile, bool compress)
        {
            string extension = Path.GetExtension(outputFile);

            if (AllowedExtensions.Contains(extension))
            {
                Unity3DFile.Repack(inputPath, outputFile, compress);
            }
        }

        public override void PreprocessContainer(TranslationFileContainer container, string containerPath,
            string extractionPath)
        {
            if (container.Type != ContainerType.CompressedFile)
            {
                return;
            }

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(containerPath);
            
            string inputFolder = Path.GetDirectoryName(containerPath);
            IEnumerable<string> files = Directory.EnumerateFiles(inputFolder, $"{fileNameWithoutExtension}.*");
            foreach (string file in files)
            {
                string outputFilePath = Path.Combine(extractionPath, Path.GetFileName(file));
                File.Copy(file, outputFilePath);
            }

            files = Directory.EnumerateFiles(inputFolder, "globalgamemanagers.*");
            foreach (string file in files)
            {
                string outputFilePath = Path.Combine(extractionPath, Path.GetFileName(file));
                File.Copy(file, outputFilePath);
            }

            Directory.CreateDirectory(Path.Combine(extractionPath, "Resources"));
            files = Directory.EnumerateFiles(Path.Combine(inputFolder, "Resources"), "*.*");
            foreach (string file in files)
            {
                string outputFilePath = Path.Combine(extractionPath, "Resources", Path.GetFileName(file));
                File.Copy(file, outputFilePath);
            }
        }

        public override void PostprocessContainer(TranslationFileContainer container, string containerPath,
            string extractionPath)
        {
            if (container.Type != ContainerType.CompressedFile)
            {
                return;
            }

            string[] extractedFiles = Directory.GetFiles(Path.Combine(extractionPath, "Unity_Assets_Files"), "*.*", SearchOption.AllDirectories);
            foreach (string file in extractedFiles)
            {
                if (container.Files.All(x => x.Path != file))
                {
                    File.Delete(file);
                }
            }
        }
    }
}
