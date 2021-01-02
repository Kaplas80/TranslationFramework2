namespace TFGame.TrailsSky.Files.Images
{
    class ImageType8 : Argb1555ImageFile
    {
        protected override int ImageWidth => 1024;

        public ImageType8(string gameName, string path, string changesFolder) : base(gameName, path, changesFolder)
        {
        }
    }
}