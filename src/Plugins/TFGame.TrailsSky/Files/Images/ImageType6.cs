namespace TFGame.TrailsSky.Files.Images
{
    class ImageType6 : Argb1555ImageFile
    {
        protected override int ImageWidth => 1536;

        public ImageType6(string gameName, string path, string changesFolder) : base(gameName, path, changesFolder)
        {
        }
    }
}