using System.Text;
using TF.Core.Files;

namespace TFGame.YakuzaIshin.Files.AiPopup
{
    public class File : BinaryTextFileWithOffsetTable
    {
        protected override int StartOffset => 0x60C;

        public File(string path, string changesFolder, Encoding encoding) : base(path, changesFolder, encoding)
        {
        }
    }
}
