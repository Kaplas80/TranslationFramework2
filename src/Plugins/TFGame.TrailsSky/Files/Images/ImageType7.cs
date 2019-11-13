namespace TFGame.TrailsSky.Files.Images
{
    class ImageType7 : Argb1555ImageFile
    {
        protected override int ImageWidth => 2048;

        public ImageType7(string gameName, string path, string changesFolder) : base(gameName, path, changesFolder)
        {
        }
    }
}