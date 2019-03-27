namespace TFGame.TrailsSky.Files.Images
{
    class ImageType2 : Argb4444ImageFile
    {
        protected override int ImageWidth => 512;

        public ImageType2(string path, string changesFolder) : base(path, changesFolder)
        {
        }
    }
}