namespace TFGame.TrailsSky.Files.Images
{
    class ImageType2 : Argb4444ImageFile
    {
        protected override int ImageWidth => 512;

        public ImageType2(string gameName, string path, string changesFolder) : base(gameName, path, changesFolder)
        {
        }
    }
}