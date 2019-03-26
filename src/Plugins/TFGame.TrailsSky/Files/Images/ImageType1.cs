namespace TFGame.TrailsSky.Files.Images
{
    class ImageType1 : TrailsInTheSkyImage
    {
        public ImageType1(string path, string changesFolder) : base(path, changesFolder)
        {
        }

        protected override int ImageWidth => 256;
        protected override ImageFormat ImagePixelFormat => ImageFormat.Format4444;
    }
}