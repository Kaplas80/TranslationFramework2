using System.Text;
using TF.Core.Files;

namespace YakuzaGame.Files.DisposeString
{
    public class File : BinaryTextFileWithOffsetTable
    {
        protected override int StartOffset => 0x08;

        public File(string path, string changesFolder, Encoding encoding) : base(path, changesFolder, encoding)
        {
        }
    }
}
