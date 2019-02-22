using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using TF.Core.Entities;
using YakuzaCommon.Files;

namespace YakuzaCommon
{
    public abstract class Game : IGame
    {
        public virtual string Id { get; }
        public virtual string Name { get; }
        public virtual string Description { get; }
        public virtual Image Icon { get; }
        public virtual int Version { get; }

        public virtual GameFileContainer[] GetContainers(string path)
        {
            var result = new List<GameFileContainer>();
            return result.ToArray();
        }

        public void ExtractFile(string inputFile, string outputPath)
        {
            var extension = Path.GetExtension(inputFile);

            if (extension.StartsWith(".par"))
            {
                var parFile = new ParFile(inputFile);
                parFile.Extract(outputPath);
            }
        }

        public void RepackFile(string inputPath, string outputFile, bool compress)
        {
            var extension = Path.GetExtension(outputFile);

            if (extension.StartsWith(".par"))
            {
                ParFile.Repack(inputPath, outputFile, compress);
            }
        }
    }
}
