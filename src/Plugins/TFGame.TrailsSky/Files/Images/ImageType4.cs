namespace TFGame.TrailsSky.Files.Images
{
    class ImageType4 : TrailsInTheSkyImage
    {
        public ImageType4(string path, string changesFolder) : base(path, changesFolder)
        {
        }

        protected override int ImageWidth => 512;
        protected override ImageFormat ImagePixelFormat => ImageFormat.Format1555;
    }
}