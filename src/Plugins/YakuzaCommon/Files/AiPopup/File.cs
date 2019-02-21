namespace YakuzaCommon.Files.AiPopup
{
    internal class File : SimpleSubtitle.File
    {
        protected override int HEADER_SIZE => 0x1AC;

        public File(string path, string changesFolder) : base(path, changesFolder)
        {
        }
    }
}
