using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using TF.Core.Entities;
using TF.Core.Files;
using TFGame.HardcoreMecha.Files;


namespace TFGame.HardcoreMecha
{
    public class Game : UnityGame.Game
    {
        public override string Id => "201fd831-c546-4022-be11-e5e9493d0c8b";
        public override string Name => "HARDCORE MECHA";
        public override string Description => "Build Id: 4086834";
        public override Image Icon => Resources.Icon;
        public override int Version => 1;
        public override System.Text.Encoding FileEncoding => new Encoding();

        protected override string[] AllowedExtensions => new[]
        {
            ".assets", ""
        };

        public override GameFileContainer[] GetContainers(string path)
        {
            var result = new List<GameFileContainer>();

            var fontSearch = new GameFileSearch()
            {
                RelativePath = @".",
                SearchPattern = "*.ttf",
                IsWildcard = true,
                RecursiveSearch = true,
                FileType = typeof(TrueTypeFontFile)
            };

            var textSearch = new GameFileSearch()
            {
                RelativePath = @".",
                SearchPattern = "resources_00001.-110",
                IsWildcard = false,
                RecursiveSearch = true,
                FileType = typeof(Files.I2Text.File)
            };

            var main = new GameFileContainer
            {
                Path = @"CHC_Data\resources.assets",
                Type = ContainerType.CompressedFile
            };
            main.FileSearches.Add(fontSearch);
            main.FileSearches.Add(textSearch);

            result.Add(main);

            var languageSearch = new GameFileSearch()
            {
                RelativePath = @".",
                SearchPattern = "level2_00002.-12",
                IsWildcard = false,
                RecursiveSearch = true,
                FileType = typeof(Files.Language.File)
            };

            var level2 = new GameFileContainer
            {
                Path = @"CHC_Data\level2",
                Type = ContainerType.CompressedFile
            };
            level2.FileSearches.Add(languageSearch);

            result.Add(level2);
            
            var sharedAssets = new GameFileContainer
            {
                Path = @"CHC_Data\sharedAssets0.assets",
                Type = ContainerType.CompressedFile
            };
            sharedAssets.FileSearches.Add(fontSearch);

            result.Add(sharedAssets);

            return result.ToArray();
        }

        public override void ExtractFile(string inputFile, string outputPath)
        {
            var fileName = Path.GetFileName(inputFile);
            var extension = Path.GetExtension(inputFile);

            if (AllowedExtensions.Contains(extension))
            {
                Unity3DFile.Extract(inputFile, outputPath);
            }
        }

        public override void RepackFile(string inputPath, string outputFile, bool compress)
        {
            var fileName = Path.GetFileName(outputFile);
            var extension = Path.GetExtension(outputFile);

            if (AllowedExtensions.Contains(extension))
            {
                Unity3DFile.Repack(inputPath, outputFile, compress);
            }
        }

        public override void PreprocessContainer(TranslationFileContainer container, string containerPath, string extractionPath)
        {
            if (container.Type == ContainerType.CompressedFile)
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(containerPath);

                var inputFolder = Path.GetDirectoryName(containerPath);
                var files = Directory.EnumerateFiles(inputFolder, $"{fileNameWithoutExtension}.*");
                foreach (var file in files)
                {
                    var outputFilePath = Path.Combine(extractionPath, Path.GetFileName(file));
                    File.Copy(file, outputFilePath);
                }
            }
        }

        public override void PostprocessContainer(TranslationFileContainer container, string containerPath, string extractionPath)
        {
            if (container.Type == ContainerType.CompressedFile)
            {
                var extractedFiles = Directory.GetFiles(Path.Combine(extractionPath, "Unity_Assets_Files"), "*.*", SearchOption.AllDirectories);
                foreach (var file in extractedFiles)
                {
                    if (container.Files.All(x => x.Path != file) && !file.EndsWith("mplus-1m-regular.ttf") && !file.EndsWith("resources_00001.-13"))
                    {
                        File.Delete(file);
                    }
                }
            }
        }
    }
}

