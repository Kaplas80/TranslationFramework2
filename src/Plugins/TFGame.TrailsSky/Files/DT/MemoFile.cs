namespace TFGame.TrailsSky.Files.DT
{
    public class MemoFile : GenericDTFile
    {
        protected override int NumStringsPerItem => 1;
        protected override int UnknownSize => 0x02;

        public MemoFile(string gameName, string path, string changesFolder, System.Text.Encoding encoding) : base(gameName, path, changesFolder, encoding)
        {

        }
    }
}
