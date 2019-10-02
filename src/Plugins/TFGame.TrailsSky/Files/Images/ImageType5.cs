namespace TFGame.TrailsSky.Files.Images
{
    class ImageType5 : Argb1555ImageFile
    {
        protected override int ImageWidth => 768;

        public ImageType5(string gameName, string path, string changesFolder) : base(gameName, path, changesFolder)
        {
        }
    }
}