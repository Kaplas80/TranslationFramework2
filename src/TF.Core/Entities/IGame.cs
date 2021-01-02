using System.Drawing;
using System.Text;

namespace TF.Core.Entities
{
    public interface IGame
    {
        string Id { get; }
        string Name { get; }
        string Description { get; }
        Image Icon { get; }
        int Version { get; }
        Encoding FileEncoding { get; }
        bool ExportOnlyModifiedFiles { get; }

        GameFileContainer[] GetContainers(string path);

        void ExtractFile(string inputFile, string outputPath);
        void RepackFile(string inputPath, string outputFile, bool compress);
        void PreprocessContainer(TranslationFileContainer container, string containerPath, string extractionPath);
        void PostprocessContainer(TranslationFileContainer container, string containerPath, string extractionPath);
    }
}
