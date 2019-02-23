using System.Text;

namespace YakuzaCommon.Files.AiPopup
{
    public class File : SimpleSubtitle.File
    {
        protected override int HEADER_SIZE => 0x1AC;

        public File(string path, string changesFolder, Encoding encoding) : base(path, changesFolder, encoding)
        {
        }
    }
}
