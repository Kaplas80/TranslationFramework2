using System.Text;
using TF.Core.Files;

namespace YakuzaGame.Files.Sale
{
    public class File : BinaryTextFileWithOffsetTable
    {
        protected override int StartOffset => 4;

        public File(string path, string changesFolder, Encoding encoding) : base(path, changesFolder, encoding)
        {
        }
    }
}
