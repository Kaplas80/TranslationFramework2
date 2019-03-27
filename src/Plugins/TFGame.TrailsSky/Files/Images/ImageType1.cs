namespace TFGame.TrailsSky.Files.Images
{
    class ImageType1 : Argb4444ImageFile
    {
        protected override int ImageWidth => 256;

        public ImageType1(string path, string changesFolder) : base(path, changesFolder)
        {
        }
    }
}