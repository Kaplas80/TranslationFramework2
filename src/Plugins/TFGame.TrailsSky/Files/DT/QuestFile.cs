namespace TFGame.TrailsSky.Files.DT
{
    public class QuestFile : GenericDTFile
    {
        protected override int NumStringsPerItem => 18;
        protected override int UnknownSize => 0x10;

        public QuestFile(string gameName, string path, string changesFolder, System.Text.Encoding encoding) : base(gameName, path, changesFolder, encoding)
        {

        }
    }
}
