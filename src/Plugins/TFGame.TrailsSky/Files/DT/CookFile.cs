namespace TFGame.TrailsSky.Files.DT
{
    public class CookFile : GenericDTFile
    {
        protected override int NumStringsPerItem => 2;
        protected override int UnknownSize => 0x28;

        public CookFile(string path, string changesFolder, System.Text.Encoding encoding) : base(path, changesFolder, encoding)
        {

        }
    }
}
