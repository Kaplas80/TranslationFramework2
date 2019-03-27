namespace TFGame.TrailsSky.Files.Images
{
    class ImageType6 : Argb4444ImageFile
    {
        protected override int ImageWidth => 128;

        public ImageType6(string path, string changesFolder) : base(path, changesFolder)
        {
        }
    }
}