using ScintillaNET;

namespace TF.Core.POCO
{
    public enum ScintillaLineEndings
    {
        CrLf,
        Cr,
        Lf
    }

    public class LineEnding
    {
        public string RealLineEnding { get; set; }
        public string ShownLineEnding { get; set; }
        public string PoLineEnding { get; set; }

        public ScintillaLineEndings ScintillaLineEnding { get; set; }
    }
}
