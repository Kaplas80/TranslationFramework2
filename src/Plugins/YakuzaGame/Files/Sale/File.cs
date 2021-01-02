using System.Text;
using TF.Core.Files;

namespace YakuzaGame.Files.Sale
{
    public class File : BinaryTextFileWithOffsetTable
    {
        protected override int StartOffset => 4;

        public File(string gameName, string path, string changesFolder, System.Text.Encoding encoding) : base(gameName, path, changesFolder, encoding)
        {
        }
    }
}
