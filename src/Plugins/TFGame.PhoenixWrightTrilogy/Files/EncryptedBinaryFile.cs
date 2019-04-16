using System.IO;
using TF.Core.Files;
using TF.Core.Helpers;
using TFGame.PhoenixWrightTrilogy.Core;

namespace TFGame.PhoenixWrightTrilogy.Files
{
    public class EncryptedBinaryFile : BinaryTextFile
    {
        protected EncryptedBinaryFile(string path, string changesFolder, System.Text.Encoding encoding) : base(path, changesFolder, encoding)
        {
        }

        public override bool Search(string searchString)
        {
            var encryptedBytes = File.ReadAllBytes(Path);
            var bytes = EncryptionManager.DecryptData(encryptedBytes);

            var pattern = FileEncoding.GetBytes(searchString.ToFullWidthChars());

            var index1 = SearchHelper.SearchPattern(bytes, pattern, 0);

            var index2 = -1;
            if (HasChanges)
            {
                bytes = File.ReadAllBytes(ChangesFile);
                pattern = System.Text.Encoding.Unicode.GetBytes(searchString);
                index2 = SearchHelper.SearchPattern(bytes, pattern, 0);
            }

            return index1 != -1 || index2 != -1;
        }
    }
}
