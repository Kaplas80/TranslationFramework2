using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using CRIWareGame.Files;
using TF.Core.Entities;

namespace CRIWareGame
{
    public abstract class Game : IGame
    {
        public virtual string Id { get; }
        public virtual string Name { get; }
        public virtual string Description { get; }
        public virtual Image Icon { get; }
        public virtual int Version { get; }
        public virtual Encoding FileEncoding { get; }

        public virtual bool ExportOnlyModifiedFiles => false;

        public virtual GameFileContainer[] GetContainers(string path)
        {
            var result = new List<GameFileContainer>();
            return result.ToArray();
        }

        public void ExtractFile(string inputFile, string outputPath)
        {
            var fileName = Path.GetFileName(inputFile);
            var extension = Path.GetExtension(inputFile);

            if (extension.StartsWith(".cpk"))
            {
                CpkFile.Extract(inputFile, outputPath);
            }
        }

        public void RepackFile(string inputPath, string outputFile, bool compress)
        {
            var fileName = Path.GetFileName(outputFile);
            var extension = Path.GetExtension(outputFile);

            if (extension.StartsWith(".cpk"))
            {
                CpkFile.Repack(inputPath, outputFile, compress);
            }
        }

        public void PreprocessContainer(TranslationFileContainer container, string containerPath, string extractionPath)
        {
        }

        public void PostprocessContainer(TranslationFileContainer container, string containerPath, string extractionPath)
        {
        }
    }
}
