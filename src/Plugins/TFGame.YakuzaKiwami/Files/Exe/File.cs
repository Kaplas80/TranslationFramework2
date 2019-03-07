namespace TFGame.YakuzaKiwami.Files.Exe
{
    public class File : YakuzaCommon.Files.Exe.File
    {
        protected override long FontTableOffset => 0xD05FD0;

        public File(string path, string changesFolder, System.Text.Encoding encoding) : base(path, changesFolder, encoding)
        {
        }
    }
}
