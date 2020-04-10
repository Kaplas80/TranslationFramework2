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
        public override string Description => "Versión: 6d543183";
        public override Image Icon => Resources.Icon; // https://www.deviantart.com/m-1618/art/Disco-Elysium-Game-Icon-512x512--748478143
        public override int Version => 1;
        public override System.Text.Encoding FileEncoding => new Encoding();

        protected override string[] AllowedExtensions => new[]
        {
            ".assets", ""
        };

        public override GameFileContainer[] GetContainers(string path)
        {
            var result = new List<GameFileContainer>();

            var dllSearch = new GameFileSearch
            {
                RelativePath = @".",
                SearchPattern = "Assembly-CSharp.dll",
                IsWildcard = false,
                RecursiveSearch = false,
                FileType = typeof(Files.AssemblyCSharp.File)
            };

            var managed = new GameFileContainer
            {
                Path = @"disco_Data\Managed",
                Type = ContainerType.Folder
            };
            managed.FileSearches.Add(dllSearch);

            result.Add(managed);

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
                SearchPattern = "*.tex.dds",
                IsWildcard = true,
                RecursiveSearch = true,
                FileType = typeof(TF.Core.Files.DDS2File),
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

            var skillTextSearch = new GameFileSearch
            {
                RelativePath = @".",
                SearchPattern = "*.Text;*.Text_00001",
                IsWildcard = true,
                RecursiveSearch = true,
                FileType = typeof(UnityGame.Files.UnityUIText2019.FileWithBestFit)
            };

            textureSearch = new GameFileSearch()
            {
                RelativePath = @".",
                SearchPattern = "*.tex.dds",
                IsWildcard = true,
                RecursiveSearch = true,
                FileType = typeof(TF.Core.Files.DDS2File),
            };

            var sharedAssets1 = new GameFileContainer
            {
                Path = @"disco_Data\sharedassets1.assets",
                Type = ContainerType.CompressedFile
            };
            sharedAssets1.FileSearches.Add(dialogueSearch);
            sharedAssets1.FileSearches.Add(skillTextSearch);
            sharedAssets1.FileSearches.Add(textureSearch);
            result.Add(sharedAssets1);

            textureSearch = new GameFileSearch()
            {
                RelativePath = @".",
                SearchPattern = "*.tex.dds",
                IsWildcard = true,
                RecursiveSearch = true,
                FileType = typeof(TF.Core.Files.DDS2File),
            };

            var sharedAssets7 = new GameFileContainer
            {
                Path = @"disco_Data\sharedassets7.assets",
                Type = ContainerType.CompressedFile
            };
            sharedAssets7.FileSearches.Add(textureSearch);
            result.Add(sharedAssets7);

            dialogueSearch = new GameFileSearch()
            {
                RelativePath = @".",
                SearchPattern = "Disco Elysium_SELFNAME.*",
                IsWildcard = true,
                RecursiveSearch = true,
                FileType = typeof(Files.DialogueSystemBundle.File)
            };

            var dialoguebundle = new GameFileContainer
            {
                Path = @"disco_Data\StreamingAssets\AssetBundles\Windows\dialoguebundle",
                Type = ContainerType.CompressedFile
            };
            dialoguebundle.FileSearches.Add(dialogueSearch);
            result.Add(dialoguebundle);

            var level1TextSearch = new GameFileSearch
            {
                RelativePath = @".",
                SearchPattern = "*.Text;*.Text_00001",
                IsWildcard = true,
                RecursiveSearch = true,
                Exclusions = { "Label_000", "Close Button.Text", },
                FileType = typeof(UnityGame.Files.UnityUIText2019.File)
            };

            var level1TextSearchWithBestFit = new GameFileSearch
            {
                RelativePath = @".",
                SearchPattern = "Label_*.Text;Close Button.Text",
                IsWildcard = true,
                RecursiveSearch = true,
                FileType = typeof(UnityGame.Files.UnityUIText2019.FileWithBestFit)
            };

            var level1 = new GameFileContainer
            {
                Path = @"disco_Data\level1",
                Type = ContainerType.CompressedFile
            };

            level1.FileSearches.Add(level1TextSearch);
            level1.FileSearches.Add(level1TextSearchWithBestFit);
            result.Add(level1);

            return result.ToArray();
        }

        public override void ExtractFile(string inputFile, string outputPath)
        {
            string fileName = Path.GetFileName(inputFile);
            string extension = Path.GetExtension(inputFile);

            if (fileName == "dialoguebundle")
            {
                UnityFsFile.Extract(inputFile, outputPath);
            }
            else if (AllowedExtensions.Contains(extension))
            {
                Unity3DFile.Extract(inputFile, outputPath);
            }
        }

        public override void RepackFile(string inputPath, string outputFile, bool compress)
        {
            string fileName = Path.GetFileName(outputFile);
            string extension = Path.GetExtension(outputFile);

            if (fileName == "dialoguebundle")
            {
                UnityFsFile.Repack(inputPath, outputFile, compress);
            }
            else if (AllowedExtensions.Contains(extension))
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
            if (fileNameWithoutExtension == "dialoguebundle")
            {
                return;
            }

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

            string[] extractedFiles = Directory.GetFiles(Path.Combine(extractionPath, "Unity_Assets_Files"), "*.*",
                SearchOption.AllDirectories);
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
