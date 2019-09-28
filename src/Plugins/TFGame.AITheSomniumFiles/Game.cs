using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using TF.Core.Entities;
using TFGame.AITheSomniumFiles.Files;


namespace TFGame.AITheSomniumFiles
{
    public class Game : UnityGame.Game
    {
        public override string Id => "a7af21f8-4ce3-4c1f-887e-ab9a8dce772f";
        public override string Name => "AI - The Somnium Files";
        public override string Description => "Build Id: ";
        public override Image Icon => Resources.Icon;
        public override int Version => 1;
        public override System.Text.Encoding FileEncoding => Encoding.UTF8;

        protected override string[] AllowedExtensions => new[]
        {
            ""
        };

        public override GameFileContainer[] GetContainers(string path)
        {
            var result = new List<GameFileContainer>();

            var textSearch = new GameFileSearch()
            {
                RelativePath = @"Unity_Assets_Files\luabytecode\CAB-0670e8eb4b419284c6de5d2d82066179\",
                SearchPattern = "*-us.txt",
                IsWildcard = true,
                RecursiveSearch = false,
                FileType = typeof(Files.LuaText.File)
            };

            var luaByteCode = new GameFileContainer
            {
                Path = @"AI_TheSomniumFiles_Data\StreamingAssets\AssetBundles\StandaloneWindows64\luabytecode",
                Type = ContainerType.CompressedFile
            };
            luaByteCode.FileSearches.Add(textSearch);

            result.Add(luaByteCode);

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
                    if (container.Files.All(x => x.Path != file))
                    {
                        File.Delete(file);
                    }
                }
            }
        }
    }
}

