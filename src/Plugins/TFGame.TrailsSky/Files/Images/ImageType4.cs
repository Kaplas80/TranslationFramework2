namespace TFGame.TrailsSky.Files.Images
{
    class ImageType4 : Argb1555ImageFile
    {
        protected override int ImageWidth => 512;

        public ImageType4(string gameName, string path, string changesFolder) : base(gameName, path, changesFolder)
        {
        }
    }
}