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
        public override string Description => "Build Id: 4297470";
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

            var ddsSearch = new GameFileSearch()
            {
                RelativePath = @".",
                //SearchPattern = "*SDF*.dds;CrimsonFlame_Melee?_*.dds;Cuirassier_Melee?_*.dds;Deca-Destry-*.tex.dds;Enigma_*.tex.dds;Geier_Melee?_*.tex.dds;Haldberd_Melee?_*.tex.dds;JumpInTip_*.tex.dds;KnellA3_Melee?_*.tex.dds;Mech_*.tex.dds;MecSelectionTips_*.tex.dds;Melee_*.tex.dds;NvLiu_*.tex.dds;Penta-Destroy-*.tex.dds;Pilot_*.tex.dds;PilotFragMech_*.tex.dds;Ready_*.tex.dds;RoundHammer_Melee?_*.tex.dds;SafehouseReached_*.tex.dds;Select Menu Info*.tex.dds;TakehitoSoeda_*.tex.dds;Team?icon.tex.dds;Thanks*.tex.dds;ThunderBolt_Melee?_*.tex.dds;Title_LOGO*.tex.dds;Viperid_Melee?_*.tex.dds;WeaponTips_*.tex.dds;Wpn_*.tex.dds;You.tex.dds",
                SearchPattern = "*.dds",
                IsWildcard = true,
                RecursiveSearch = true,
                FileType = typeof(UnityGame.Files.Texture2D.File)
            };

            var crnSearch = new GameFileSearch()
            {
                RelativePath = @".",
                SearchPattern = "Begin.tex.crn",
                IsWildcard = false,
                RecursiveSearch = true,
                FileType = typeof(Files.CrunchDDS.File)
            };

            var textSearch = new GameFileSearch()
            {
                RelativePath = @"Unity_Assets_Files\resources\Mono\Assembly-CSharp\I2.Loc",
                SearchPattern = "I2Languages_SELFNAME.LanguageSourceAsset",
                IsWildcard = false,
                RecursiveSearch = false,
                FileType = typeof(Files.I2Text.File)
            };

            var resources = new GameFileContainer
            {
                Path = @"CHC_Data\resources.assets",
                Type = ContainerType.CompressedFile
            };
            resources.FileSearches.Add(fontSearch);
            resources.FileSearches.Add(ddsSearch);
            //resources.FileSearches.Add(crnSearch);
            resources.FileSearches.Add(textSearch);

            result.Add(resources);

            var languageSearch = new GameFileSearch()
            {
                RelativePath = @"Unity_Assets_Files\level2\Mono\Assembly-CSharp\CHC.Utilities.UI",
                SearchPattern = "LanguageSelector.CHCLanguageSelector",
                IsWildcard = false,
                RecursiveSearch = false,
                FileType = typeof(Files.Language.File)
            };

            var level2 = new GameFileContainer
            {
                Path = @"CHC_Data\level2",
                Type = ContainerType.CompressedFile
            };
            level2.FileSearches.Add(languageSearch);

            result.Add(level2);
            
            ddsSearch = new GameFileSearch()
            {
                RelativePath = @".",
                //SearchPattern = "comingsoon.tex.dds;*SDF*.dds;RPlogo.tex.dds;UnityWwiseLogo.tex.dds",
                SearchPattern = "*.dds",
                IsWildcard = true,
                RecursiveSearch = true,
                FileType = typeof(UnityGame.Files.Texture2D.File)
            };

            crnSearch = new GameFileSearch
            {
                RelativePath = @".",
                SearchPattern = "Loading.tex.crn",
                IsWildcard = false,
                RecursiveSearch = true,
                FileType = typeof(Files.CrunchDDS.File)
            };
            
            var sharedAssets0 = new GameFileContainer
            {
                Path = @"CHC_Data\sharedAssets0.assets",
                Type = ContainerType.CompressedFile
            };
            sharedAssets0.FileSearches.Add(fontSearch);
            sharedAssets0.FileSearches.Add(ddsSearch);
            //sharedAssets0.FileSearches.Add(crnSearch);

            result.Add(sharedAssets0);

            ddsSearch = new GameFileSearch()
            {
                RelativePath = @".",
                //SearchPattern = "Ctrl_*.tex.dds;Keyboard_*.tex.dds;Xbox_*.tex.dds;",
                SearchPattern = "*.dds",
                IsWildcard = true,
                RecursiveSearch = true,
                FileType = typeof(UnityGame.Files.Texture2D.File)
            };

            var sharedAssets5 = new GameFileContainer
            {
                Path = @"CHC_Data\sharedAssets5.assets",
                Type = ContainerType.CompressedFile
            };
            sharedAssets5.FileSearches.Add(ddsSearch);
            
            result.Add(sharedAssets5);

            ddsSearch = new GameFileSearch()
            {
                RelativePath = @".",
                //SearchPattern = "Storage.tex.dds",
                SearchPattern = "*.dds",
                IsWildcard = false,
                RecursiveSearch = true,
                FileType = typeof(UnityGame.Files.Texture2D.File)
            };

            var sharedAssets7 = new GameFileContainer
            {
                Path = @"CHC_Data\sharedAssets7.assets",
                Type = ContainerType.CompressedFile
            };
            sharedAssets7.FileSearches.Add(ddsSearch);
            
            result.Add(sharedAssets7);

            ddsSearch = new GameFileSearch()
            {
                RelativePath = @".",
                //SearchPattern = "Settlement?.tex.dds",
                SearchPattern = "*.dds",
                IsWildcard = true,
                RecursiveSearch = true,
                FileType = typeof(UnityGame.Files.Texture2D.File)
            };

            crnSearch = new GameFileSearch
            {
                RelativePath = @".",
                SearchPattern = "Settlement?.tex.crn",
                IsWildcard = true,
                RecursiveSearch = true,
                FileType = typeof(Files.CrunchDDS.File)
            };
            
            var sharedAssets15 = new GameFileContainer
            {
                Path = @"CHC_Data\sharedAssets15.assets",
                Type = ContainerType.CompressedFile
            };
            sharedAssets15.FileSearches.Add(ddsSearch);
            //sharedAssets15.FileSearches.Add(crnSearch);

            result.Add(sharedAssets15);

            ddsSearch = new GameFileSearch()
            {
                RelativePath = @".",
                //SearchPattern = "Settlement?.tex.dds",
                SearchPattern = "*.dds",
                IsWildcard = true,
                RecursiveSearch = true,
                FileType = typeof(UnityGame.Files.Texture2D.File)
            };

            crnSearch = new GameFileSearch
            {
                RelativePath = @".",
                SearchPattern = "BigComputer.tex.crn",
                IsWildcard = false,
                RecursiveSearch = true,
                FileType = typeof(Files.CrunchDDS.File)
            };
            
            var sharedAssets70 = new GameFileContainer
            {
                Path = @"CHC_Data\sharedAssets70.assets",
                Type = ContainerType.CompressedFile
            };
            sharedAssets70.FileSearches.Add(ddsSearch);
            //sharedAssets70.FileSearches.Add(crnSearch);

            result.Add(sharedAssets70);

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

        public override void PostprocessContainer(TranslationFileContainer container, string containerPath, string extractionPath)
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

