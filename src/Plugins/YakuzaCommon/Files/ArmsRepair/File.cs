using System.Text;
using TF.Core.Files;

namespace YakuzaCommon.Files.ArmsRepair
{
    public class File : BinaryTextFileWithOffsetTable
    {
        protected override int StartOffset => 0x00;

        public File(string path, string changesFolder, Encoding encoding) : base(path, changesFolder, encoding)
        {
        }
    }
}
