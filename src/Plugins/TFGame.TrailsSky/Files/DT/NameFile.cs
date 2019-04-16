namespace TFGame.TrailsSky.Files.DT
{
    public class NameFile : GenericDTFile
    {
        protected override int NumStringsPerItem => 1;
        protected override int UnknownSize => 0x1A;

        public NameFile(string path, string changesFolder, System.Text.Encoding encoding) : base(path, changesFolder, encoding)
        {

        }
    }
}
