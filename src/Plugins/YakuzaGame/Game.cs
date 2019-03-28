using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using TF.Core.Entities;
using YakuzaGame.Files;

namespace YakuzaGame
{
    public abstract class Game : IGame
    {
        public virtual string Id { get; }
        public virtual string Name { get; }
        public virtual string Description { get; }
        public virtual Image Icon { get; }
        public virtual int Version { get; }

        public virtual Encoding FileEncoding => Encoding.UTF8;

        public virtual GameFileContainer[] GetContainers(string path)
        {
            var result = new List<GameFileContainer>();
            return result.ToArray();
        }

        public void ExtractFile(string inputFile, string outputPath)
        {
            var fileName = Path.GetFileName(inputFile);
            var extension = Path.GetExtension(inputFile);

            if (extension.StartsWith(".par"))
            {
                ParFile.Extract(inputFile, outputPath);
            }
        }

        public void RepackFile(string inputPath, string outputFile, bool compress)
        {
            var fileName = Path.GetFileName(outputFile);
            var extension = Path.GetExtension(outputFile);

            if (extension.StartsWith(".par"))
            {
                ParFile.Repack(inputPath, outputFile, compress);
            }
        }
    }
}
