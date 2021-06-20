using TF.Core.Files;
using TF.Core.POCO;

namespace TFGame.AITheSomniumFiles.Files.Dance
{
    public class File : TextFile
    {
        public File(string gameName, string path, string changesFolder, System.Text.Encoding encoding) : base(gameName, path, changesFolder, encoding)
        {
        }

        public override LineEnding LineEnding => new LineEnding
        {
            RealLineEnding = "\r\n",
            ShownLineEnding = "\\r\\n",
            PoLineEnding = "\n",
            ScintillaLineEnding = ScintillaLineEndings.CrLf,
        };
    }
}
