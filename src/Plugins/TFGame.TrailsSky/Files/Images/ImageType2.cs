namespace TFGame.TrailsSky.Files.Images
{
    class ImageType2 : TrailsInTheSkyImage
    {
        public ImageType2(string path, string changesFolder) : base(path, changesFolder)
        {
        }

        protected override int ImageWidth => 512;
        protected override ImageFormat ImagePixelFormat => ImageFormat.Format4444;
    }
}