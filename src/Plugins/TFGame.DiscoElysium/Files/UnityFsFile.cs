namespace TFGame.DiscoElysium.Files
{
    using System.IO;

    public class UnityFsFile
    {
        public static void Extract(string inputPath, string outputFolder)
        {
            string fileName = Path.GetFileName(inputPath);
            string copyPath = Path.Combine(outputFolder, fileName);
            AssetsToolsWrapper.Api.Unpack(inputPath, copyPath);

            string extractPath = Path.Combine(Path.GetDirectoryName(copyPath), "Unity_Assets_Files");
            AssetsToolsWrapper.Api.Extract(copyPath, extractPath, "Disco Elysium_SELFNAME");
        }

        public static void Repack(string inputFolder, string outputPath, bool useCompression)
        {
            string copyPath = Path.Combine(inputFolder, Path.GetFileName(outputPath));
            string extractPath = Path.Combine(Path.GetDirectoryName(copyPath), "Unity_Assets_Files");
            string dir = Path.GetDirectoryName(outputPath);
            Directory.CreateDirectory(dir);

            AssetsToolsWrapper.Api.Repack(copyPath, extractPath, outputPath, "Disco Elysium_SELFNAME");
        }

    }
}
