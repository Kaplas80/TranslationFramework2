using System.Text;
using TF.Core.Files;

namespace YakuzaGame.Files.AiPopup
{
    public class File : BinaryTextFileWithOffsetTable
    {
        protected override int StartOffset => 0x1AC;

        public File(string path, string changesFolder, System.Text.Encoding encoding) : base(path, changesFolder, encoding)
        {
        }
    }
}
