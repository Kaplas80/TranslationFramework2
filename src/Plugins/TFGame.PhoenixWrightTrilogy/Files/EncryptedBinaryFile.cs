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

        public override bool Search(string searchString, string path = "")
        {
            if (string.IsNullOrEmpty(path))
            {
                path = Path;
            }
            var encryptedBytes = File.ReadAllBytes(path);
            var bytes = EncryptionManager.DecryptData(encryptedBytes);

            var pattern = FileEncoding.GetBytes(searchString.ToFullWidthChars());

            var searchHelper = new SearchHelper(pattern);
            var index1 = searchHelper.Search(bytes);

            var index2 = -1;
            if (HasChanges)
            {
                bytes = File.ReadAllBytes(ChangesFile);
                pattern = System.Text.Encoding.Unicode.GetBytes(searchString);
                searchHelper = new SearchHelper(pattern);
                index2 = searchHelper.Search(bytes);
            }

            return index1 != -1 || index2 != -1;
        }
    }
}
