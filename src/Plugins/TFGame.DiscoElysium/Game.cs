namespace TFGame.DiscoElysium
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using TF.Core.Entities;
    using TFGame.DiscoElysium.Files;

    public class Game : UnityGame.Game
    {
        public override string Id => "111967dd-effc-47bc-a8cb-bf3e11d61439";
        public override string Name => "Disco Elysium";
        public override string Description => "Build Id: 4297737";
        public override Image Icon => Resources.Icon; // https://www.deviantart.com/m-1618/art/Disco-Elysium-Game-Icon-512x512--748478143
        public override int Version => 1;
        public override System.Text.Encoding FileEncoding => new Encoding();

        protected override string[] AllowedExtensions => new[]
        {
            ".assets"
        };

        public override GameFileContainer[] GetContainers(string path)
        {
            var result = new List<GameFileContainer>();

            var textSearch = new GameFileSearch()
            {
                RelativePath = @".",
                SearchPattern = "*.LanguageSourceAsset",
                IsWildcard = true,
                RecursiveSearch = true,
                FileType = typeof(Files.I2Text.File)
            };

            var textureSearch = new GameFileSearch()
            {
                RelativePath = @".",
                SearchPattern = "buyable?.tex.dds;button*.tex.dds;continue-tight.tex.dds;furies.tex.dds;loading-panel.tex.dds",
                IsWildcard = true,
                RecursiveSearch = true,
                FileType = typeof(TF.Core.Files.DDSFile),
                Exclusions = { "-ru", "-zh" }
            };

            var resources = new GameFileContainer
            {
                Path = @"disco_Data\resources.assets",
                Type = ContainerType.CompressedFile
            };
            resources.FileSearches.Add(textSearch);
            resources.FileSearches.Add(textureSearch);

            result.Add(resources);

            var dialogueSearch = new GameFileSearch()
            {
                RelativePath = @".",
                SearchPattern = "Disco Elysium_SELFNAME.DialogueDatabase_00001",
                IsWildcard = false,
                RecursiveSearch = true,
                FileType = typeof(Files.DialogueSystem.File)
            };

            var sharedAssets1 = new GameFileContainer
            {
                Path = @"disco_Data\sharedassets1.assets",
                Type = ContainerType.CompressedFile
            };
            sharedAssets1.FileSearches.Add(dialogueSearch);
            result.Add(sharedAssets1);

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

        public override void PreprocessContainer(TranslationFileContainer container, string containerPath,
            string extractionPath)
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

                files = Directory.EnumerateFiles(inputFolder, "globalgamemanagers.*");
                foreach (var file in files)
                {
                    var outputFilePath = Path.Combine(extractionPath, Path.GetFileName(file));
                    File.Copy(file, outputFilePath);
                }

                Directory.CreateDirectory(Path.Combine(extractionPath, "Resources"));
                files = Directory.EnumerateFiles(Path.Combine(inputFolder, "Resources"), "*.*");
                foreach (var file in files)
                {
                    var outputFilePath = Path.Combine(extractionPath, "Resources", Path.GetFileName(file));
                    File.Copy(file, outputFilePath);
                }
            }
        }

        public override void PostprocessContainer(TranslationFileContainer container, string containerPath,
            string extractionPath)
        {
            if (container.Type == ContainerType.CompressedFile)
            {
                var extractedFiles = Directory.GetFiles(Path.Combine(extractionPath, "Unity_Assets_Files"), "*.*",
                    SearchOption.AllDirectories);
                foreach (var file in extractedFiles)
                {
                    if (container.Files.All(x => x.Path != file))
                    {
                        File.Delete(file);
                    }
                }
            }
        }
    }
}
