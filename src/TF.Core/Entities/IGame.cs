using System.Drawing;

namespace TF.Core.Entities
{
    public interface IGame
    {
        string Id { get; }
        string Name { get; }
        string Description { get; }
        Image Icon { get; }
        int Version { get; }

        GameFileContainer[] GetContainers(string path);

        TranslationFile GetFile(string path, string changesFolder);
        void ExtractFile(string inputFile, string outputPath);
        void RepackFile(string inputPath, string outputFile, bool compress);
    }
}
