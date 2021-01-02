namespace TFGame.HardcoreMecha.Files.I2Text
{
    public class File : UnityGame.Files.I2Text.File
    {
        public File(string gameName, string path, string changesFolder, System.Text.Encoding encoding) : base(gameName, path, changesFolder, encoding)
        {
        }

        protected override int LanguageIndex => 1;
    }
}
