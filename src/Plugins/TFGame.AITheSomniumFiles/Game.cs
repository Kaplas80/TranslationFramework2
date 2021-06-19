using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using TF.Core.Entities;
using TF.Core.Files;
using TFGame.AITheSomniumFiles.Files;


namespace TFGame.AITheSomniumFiles
{
    public class Game : UnityGame.Game
    {
        public override string Id => "a7af21f8-4ce3-4c1f-887e-ab9a8dce772f";
        public override string Name => "AI - The Somnium Files";
        public override string Description => "Steam Build Id: 4031380 / Nintendo Switch";
        public override Image Icon => Resources.Icon;
        public override int Version => 2;
        public override System.Text.Encoding FileEncoding => new UTF8Encoding(false);

        protected override string[] AllowedExtensions => new[]
        {
            ".assets", ""
        };

        public override GameFileContainer[] GetContainers(string path)
        {
            var result = new List<GameFileContainer>();

            var ttfFonts = new GameFileSearch()
            {
                RelativePath = @".",
                SearchPattern = "*.ttf",
                IsWildcard = true,
                RecursiveSearch = true,
                FileType = typeof(TrueTypeFontFile)
            };

            var textures = new GameFileSearch()
            {
                RelativePath = @".",
                SearchPattern = "*.tex.dds;",
                IsWildcard = true,
                RecursiveSearch = true,
                FileType = typeof(DDS2File)
            };

            var resources = new GameFileContainer
            {
                Path = @".\AI_TheSomniumFiles_Data\resources.assets",
                Type = ContainerType.CompressedFile
            };
            resources.FileSearches.Add(ttfFonts);
            resources.FileSearches.Add(textures);

            result.Add(resources);

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
                Path = @".\AI_TheSomniumFiles_Data\StreamingAssets\AssetBundles\StandaloneWindows64\luabytecode",
                Type = ContainerType.CompressedFile
            };
            luaByteCode.FileSearches.Add(textSearch);

            result.Add(luaByteCode);

            luaByteCode = new GameFileContainer
            {
                Path = @".\AI_TheSomniumFiles_Data\StreamingAssets\AssetBundles\Switch\luabytecode",
                Type = ContainerType.CompressedFile
            };
            luaByteCode.FileSearches.Add(textSearch);

            result.Add(luaByteCode);

            var textSearch2 = new GameFileSearch()
            {
                RelativePath = @"Unity_Assets_Files\scene_dance\BuildPlayer-Dance.sharedAssets\",
                SearchPattern = "lyrics-us.txt;lyrics-us2.txt",
                IsWildcard = true,
                RecursiveSearch = false,
                FileType = typeof(TextFile)
            };

            var scene_dance = new GameFileContainer
            {
                Path = @".\AI_TheSomniumFiles_Data\StreamingAssets\AssetBundles\StandaloneWindows64\scene_dance",
                Type = ContainerType.CompressedFile
            };
            scene_dance.FileSearches.Add(textSearch2);
            scene_dance.FileSearches.Add(textures);
            result.Add(scene_dance);

            scene_dance = new GameFileContainer
            {
                Path = @".\AI_TheSomniumFiles_Data\StreamingAssets\AssetBundles\Switch\scene_dance",
                Type = ContainerType.CompressedFile
            };
            scene_dance.FileSearches.Add(textSearch2);
            scene_dance.FileSearches.Add(textures);
            result.Add(scene_dance);

            var streamingAssets = new GameFileContainerSearch
            {
                RelativePath = @".\AI_TheSomniumFiles_Data\StreamingAssets\AssetBundles\StandaloneWindows64",
                SearchPattern =
                    @"bg_md_bg22_00_common;clue_image;etc;file_image_album;file_image_album_thumbnail;image;image_name_us;item;item_ii006;item_ii011;item_ii027;operation_guide;saveload;scene_a0-open10_m10_00;scene_autosaveguide;scene_fiction;scene_file;scene_flowchart;scene_investigation;scene_languageselect;scene_optionmenu;scene_options;scene_root;scene_save;scene_somnium;scene_title;scene_to-witter;ui_option;",
                RecursiveSearch = false,
                TypeSearch = ContainerType.CompressedFile
            };

            streamingAssets.FileSearches.Add(ttfFonts);
            streamingAssets.FileSearches.Add(textures);

            result.AddRange(streamingAssets.GetContainers(path));

            streamingAssets = new GameFileContainerSearch
            {
                RelativePath = @".\AI_TheSomniumFiles_Data\StreamingAssets\AssetBundles\Switch",
                SearchPattern =
                    @"bg_md_bg22_00_common;clue_image;etc;file_image_album;file_image_album_thumbnail;image;image_name_us;item;item_ii006;item_ii011;item_ii027;operation_guide;saveload;scene_a0-open10_m10_00;scene_autosaveguide;scene_fiction;scene_file;scene_flowchart;scene_investigation;scene_languageselect;scene_optionmenu;scene_options;scene_root;scene_save;scene_somnium;scene_title;scene_to-witter;ui_option;",
                RecursiveSearch = false,
                TypeSearch = ContainerType.CompressedFile
            };

            streamingAssets.FileSearches.Add(ttfFonts);
            streamingAssets.FileSearches.Add(textures);

            result.AddRange(streamingAssets.GetContainers(path));
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

