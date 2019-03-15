using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using TF.Core.Entities;
using TFGame.TrailsSky.Files;

namespace TFGame.TrailsSky
{
    public class Game : IGame
    {
        public string Id => "307504c9-90f7-45dd-a37f-4e3cd146a826";
        public string Name => "The Legend of Heroes: Trails in the Sky";
        public string Description => "Versión en inglés GOG";
        public Image Icon => Resources.Icon;
        public int Version => 1;
        public Encoding FileEncoding => Encoding.GetEncoding(936);

        public GameFileContainer[] GetContainers(string path)
        {
            var result = new List<GameFileContainer>();

            var datSearch = new GameFileContainerSearch
            {
                RelativePath = @".\",
                TypeSearch = ContainerType.CompressedFile,
                RecursiveSearch = false,
                SearchPattern = "ED6_DT01.dat",
                Exclusions = {"ED6_DT17.dat", "ED6_DT18.dat"}
            };

            result.AddRange(datSearch.GetContainers(path));

            return result.ToArray();
        }

        public void ExtractFile(string inputFile, string outputPath)
        {
            var fileName = Path.GetFileName(inputFile);
            var extension = Path.GetExtension(inputFile);

            if (extension.StartsWith(".dat"))
            {
                DatFile.Extract(inputFile, outputPath);
            }
        }

        public void RepackFile(string inputPath, string outputFile, bool compress)
        {
            throw new NotImplementedException();
        }
    }
}
