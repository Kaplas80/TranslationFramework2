namespace TFGame.TrailsSky.Files.Exe
{
    public class DX9File : File
    {
        protected override long FontTableOffset => 0x117DA8;

        public DX9File(string path, string changesFolder) : base(path, changesFolder)
        {
        }
    }
}
