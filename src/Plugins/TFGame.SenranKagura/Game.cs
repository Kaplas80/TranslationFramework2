using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using TF.Core.Entities;
using TFGame.SenranKagura.Files;

namespace TFGame.SenranKagura
{
    public class Game : IGame
    {
        public string Id => "141e1cba-1b95-4ba9-aa19-efea22e952fd";
        public string Name => "SENRAN KAGURA Burst Re:Newal";
        public string Description => "Versión 1.06 Steam (inglés)";
        public Image Icon => Resources.Icon; 
        public int Version => 1;
        public System.Text.Encoding FileEncoding => Encoding.UTF8;

        public GameFileContainer[] GetContainers(string path)
        {
            var result = new List<GameFileContainer>();

            return result.ToArray();
        }

        public void ExtractFile(string inputFile, string outputPath)
        {
            var fileName = Path.GetFileName(inputFile);
            var extension = Path.GetExtension(inputFile);

            if (extension.StartsWith(".cat"))
            {
                //CatFile.Extract(inputFile, outputPath);
            }
        }

        public void RepackFile(string inputPath, string outputFile, bool compress)
        {
            var fileName = Path.GetFileName(outputFile);
            var extension = Path.GetExtension(outputFile);

            if (extension.StartsWith(".cat"))
            {
                //CatFile.Repack(inputPath, outputFile, compress);
            }
        }
    }
}
