namespace YakuzaCommon.Files.CmnBin
{
    internal enum SubtitleLanguage
    {
        English,
        Japanese
    }

    internal abstract class Subtitle : SimpleSubtitle.Subtitle
    {
        public abstract int MaxLength { get; }
        public SubtitleLanguage Language { get; set; }
    }

    internal class LongSubtitle : Subtitle
    {
        public override int MaxLength => 256; //0x0100
    }

    internal class ShortSubtitle : Subtitle
    {
        public override int MaxLength => 128; //0x80
    }
}
