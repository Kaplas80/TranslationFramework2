namespace UnityGame.Files.Texture2D
{
    using System.IO;
    using TF.Core.Files;

    public class File : DDS2File
    {
        public File(string gameName, string path, string changesFolder) : base(gameName, path, changesFolder)
        {
        }

        public override void Rebuild(string outputFolder)
        {
            string outputFile = System.IO.Path.Combine(outputFolder, RelativePath);
            string dir = System.IO.Path.GetDirectoryName(outputFile);
            Directory.CreateDirectory(dir);

            if (outputFile.EndsWith(".crn.dds"))
            {
                // Elimino el .crn para dejar solo el .dds
                string crnFile = outputFile.Replace(".crn.dds", ".crn");
                System.IO.File.Delete(crnFile);

                outputFile = outputFile.Replace(".crn.dds", ".dds");
            }

            System.IO.File.Copy(HasChanges ? ChangesFile : Path, outputFile, true);
        }
    }
}
