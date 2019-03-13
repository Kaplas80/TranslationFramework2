namespace TF.Core.TranslationEntities
{
    public class FixedLengthSubtitle : Subtitle
    {
        public int MaxLength { get; }

        public FixedLengthSubtitle(int maxLength)
        {
            MaxLength = maxLength;
        }

        public FixedLengthSubtitle(Subtitle s, int maxLength)
        {
            Offset = s.Offset;
            Text = s.Text;
            Translation = s.Translation;
            Loaded = s.Loaded;
            MaxLength = maxLength;
        }
    }
}
