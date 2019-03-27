namespace TFGame.TrailsSky.Files.Images
{
    class ImageType3 : Argb1555ImageFile
    {
        protected override int ImageWidth => 256;

        public ImageType3(string path, string changesFolder) : base(path, changesFolder)
        {
        }
    }
}