namespace YakuzaCommon.Files.CmnBin
{
    internal abstract class Subtitle : SimpleSubtitle.Subtitle
    {
        public abstract int MaxLength { get; }
    }

    internal class LongSubtitle : Subtitle
    {
        public override int MaxLength => 256;
    }

    internal class ShortSubtitle : Subtitle
    {
        public override int MaxLength => 128;
    }
}
