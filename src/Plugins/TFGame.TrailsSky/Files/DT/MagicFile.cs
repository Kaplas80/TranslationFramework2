namespace TFGame.TrailsSky.Files.DT
{
    public class MagicFile : GenericDTFile
    {
        protected override int NumStringsPerItem => 2;
        protected override int UnknownSize => 0x1A;

        public MagicFile(string path, string changesFolder, System.Text.Encoding encoding) : base(path, changesFolder, encoding)
        {

        }
    }
}
