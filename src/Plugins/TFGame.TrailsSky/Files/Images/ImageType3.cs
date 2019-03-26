namespace TFGame.TrailsSky.Files.Images
{
    class ImageType3 : TrailsInTheSkyImage
    {
        public ImageType3(string path, string changesFolder) : base(path, changesFolder)
        {
        }

        protected override int ImageWidth => 256;
        protected override ImageFormat ImagePixelFormat => ImageFormat.Format1555;
    }
}