namespace TFGame.TrailsSky.Files.Images
{
    class ImageType1 : Argb4444ImageFile
    {
        protected override int ImageWidth => 256;

        public ImageType1(string gameName, string path, string changesFolder) : base(gameName, path, changesFolder)
        {
        }
    }
}