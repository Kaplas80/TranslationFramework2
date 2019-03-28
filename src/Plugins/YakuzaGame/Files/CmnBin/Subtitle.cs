namespace YakuzaGame.Files.CmnBin
{
    internal enum SubtitleLanguage
    {
        English,
        Japanese
    }

    internal class Subtitle : TF.Core.TranslationEntities.FixedLengthSubtitle
    {
        public SubtitleLanguage Language { get; set; }

        public Subtitle(int maxLength) : base(maxLength)
        {

        }

        public Subtitle(TF.Core.TranslationEntities.Subtitle s, int maxLength) : base(s, maxLength)
        {
        }
    }
}
