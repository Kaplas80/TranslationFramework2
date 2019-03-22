namespace TFGame.TrailsSky.Files.DT
{
    public class QuestFile : GenericDTFile
    {
        protected override int NumStringsPerItem => 18;
        protected override int UnknownSize => 0x10;

        public QuestFile(string path, string changesFolder, System.Text.Encoding encoding) : base(path, changesFolder, encoding)
        {

        }
    }
}
