using System.Text;
using TF.Core.Files;

namespace YakuzaGame.Files.ArmsRepair
{
    public class File : BinaryTextFileWithOffsetTable
    {
        protected override int StartOffset => 0x00;

        public File(string gameName, string path, string changesFolder, System.Text.Encoding encoding) : base(gameName, path, changesFolder, encoding)
        {
        }
    }
}
