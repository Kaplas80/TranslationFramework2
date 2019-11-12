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
        public override string Description => "Build Id: 4375452";
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
                SearchPattern = "buyable?.tex.dds;button*.tex.dds;continue-tight.tex.dds;furies.tex.dds;loading-panel.tex.dds",
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
                SearchPattern = "SkillNameText*.Text",
                IsWildcard = true,
                RecursiveSearch = true,
                FileType = typeof(Files.UnityUIText.File)
            };

            textureSearch = new GameFileSearch()
            {
                RelativePath = @".",
                SearchPattern = "button_close_small_dark.tex.dds;charsheet_v5.tex.dds;container_back.tex.dds;disco-build-tutorial.tex.dds;F1Screen.tex.dds;INV_equipped.tex.dds;label_church.tex.dds;label_shacks.tex.dds;label_waterfront.tex.dds;map_label.tex.dds;notify-level-up.tex.dds;no-truce-loading-screen.tex.dds;saving-panel.tex.dds;skill_crown.tex.dds;skill_levelup.tex.dds;THC-menu-v6-7-normalfix.tex.dds",
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
                SearchPattern = "VISCAL-fence.tex.dds;viscal-fencecrash.tex.dds;viscal-footprints-label_8PAIRS.tex.dds;viscal-footprintsl-label-FOOTPRINTS.tex.dds",
                IsWildcard = true,
                RecursiveSearch = true,
                FileType = typeof(TF.Core.Files.DDS2File),
            };

            var sharedAssets8 = new GameFileContainer
            {
                Path = @"disco_Data\sharedassets8.assets",
                Type = ContainerType.CompressedFile
            };
            sharedAssets8.FileSearches.Add(textureSearch);
            result.Add(sharedAssets8);

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

            var closeTextSearch = new GameFileSearch
            {
                RelativePath = @".",
                SearchPattern = "Close Button.Text_00001",
                IsWildcard = false,
                RecursiveSearch = true,
                FileType = typeof(Files.UnityUIText.File)
            };

            var level1 = new GameFileContainer
            {
                Path = @"disco_Data\level1",
                Type = ContainerType.CompressedFile
            };

            level1.FileSearches.Add(closeTextSearch);
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
