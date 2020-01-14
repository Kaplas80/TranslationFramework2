namespace TFGame.LoveEsquire
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using TF.Core.Entities;
    using TF.Core.Files;
    using TFGame.LoveEsquire.Files;

    public class Game : UnityGame.Game
    {
        public override string Id => "938fb146-8fca-4ddd-8111-2d60f2bf6239";
        public override string Name => "Love Esquire";
        public override string Description => "Version: 1.1.1c\r\nNecesita el Assembly-CSharp.dll y el vntext.sq parcheados previamente.";
        public override Image Icon => Resources.Icon; 
        public override int Version => 1;
        public override System.Text.Encoding FileEncoding => new Encoding();

        protected override string[] AllowedExtensions => new[]
        {
            ".assets", 
            "",
        };

        public override GameFileContainer[] GetContainers(string path)
        {
            var result = new List<GameFileContainer>();

            var subtitleSearch = new GameFileSearch
            {
                RelativePath = @".",
                SearchPattern = "prologueSubtitle??.txt",
                IsWildcard = false,
                RecursiveSearch = false,
                FileType = typeof(TextFile)
            };

            var subtitles = new GameFileContainer
            {
                Path = @"Love Esquire_Data\StreamingAssets\Subtitles",
                Type = ContainerType.Folder
            };
            subtitles.FileSearches.Add(subtitleSearch);
            result.Add(subtitles);

            var xmlSearch = new GameFileSearch
            {
                RelativePath = @".",
                SearchPattern = "*.xml;*.txt",
                IsWildcard = true,
                RecursiveSearch = true,
                FileType = typeof(TextFile),
            };

            var textfiles = new GameFileContainer
            {
                Path = @"Love Esquire_Data\StreamingAssets\Windows\textfiles",
                Type = ContainerType.CompressedFile
            };
            textfiles.FileSearches.Add(xmlSearch);
            result.Add(textfiles);
            
            var dbSearch = new GameFileSearch
            {
                RelativePath = ".",
                SearchPattern = "*.sq",
                IsWildcard = true,
                RecursiveSearch = false,
                FileType = typeof(Files.SqlText.File),
            };

            var sqlText = new GameFileContainer
            {
                Path = @"Love Esquire_Data\StreamingAssets\SqlText",
                Type = ContainerType.Folder
            };
            sqlText.FileSearches.Add(dbSearch);
            result.Add(sqlText);

            var textuiSearch = new GameFileSearch
            {
                RelativePath = ".",
                SearchPattern = "*.Text_00001",
                IsWildcard = true,
                RecursiveSearch = true,
                FileType = typeof(UnityGame.Files.UnityUIText.File),
            };

            var focusObjSearch = new GameFileSearch
            {
                RelativePath = ".",
                SearchPattern = "*.ShowFocusObjWithDialogue",
                IsWildcard = true,
                RecursiveSearch = true,
                FileType = typeof(Files.FocusObj.File),
            };

            var resources = new GameFileContainer
            {
                Path = @"Love Esquire_Data\resources.assets",
                Type = ContainerType.CompressedFile
            };
            resources.FileSearches.Add(textuiSearch);
            resources.FileSearches.Add(focusObjSearch);
            result.Add(resources);

            /*
            var txtSearch = new GameFileSearch
            {
                RelativePath = @".",
                SearchPattern = "*.xml;*.txt",
                IsWildcard = true,
                RecursiveSearch = true,
                FileType = typeof(TextFile),
            };

            var vnSearches = new GameFileContainerSearch
            {
                RelativePath = @"Love Esquire_Data\StreamingAssets\Windows\vnscenes",
                RecursiveSearch = false,
                SearchPattern = "*.",
                TypeSearch = ContainerType.CompressedFile
            };
            vnSearches.FileSearches.Add(txtSearch);
            result.AddRange(vnSearches.GetContainers(path));
            */

            return result.ToArray();
        }

        public override void ExtractFile(string inputFile, string outputPath)
        {
            string fileName = Path.GetFileName(inputFile);
            string extension = Path.GetExtension(inputFile);

            if (AllowedExtensions.Contains(extension))
            {
                Unity3DFile.Extract(inputFile, outputPath);
            }
        }

        public override void RepackFile(string inputPath, string outputFile, bool compress)
        {
            string fileName = Path.GetFileName(outputFile);
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

            if (Directory.Exists(Path.Combine(inputFolder, "Resources")))
            {
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
