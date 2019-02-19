namespace YakuzaCommon.Files.CmnBin
{
    internal abstract class CmnSubtitle : SimpleSubtitle.Subtitle
    {
        public abstract int MaxLength { get; }
    }

    internal class LongSubtitle : CmnSubtitle
    {
        public override int MaxLength => 256;
    }

    internal class ShortSubtitle : CmnSubtitle
    {
        public override int MaxLength => 128;
    }
}
