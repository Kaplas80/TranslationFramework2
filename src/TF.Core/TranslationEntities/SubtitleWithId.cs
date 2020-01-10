namespace TF.Core.TranslationEntities
{
    using System;

    public class SubtitleWithId : Subtitle
    {
        public string Id { get; set; }

        public SubtitleWithId() : base()
        {

        }

        public SubtitleWithId(Subtitle s) : base()
        {
            Offset = s.Offset;
            Text = s.Text;
            Translation = s.Translation;
            Loaded = s.Loaded;
        }

        protected override int CompareToInternal(Subtitle other)
        {
            if (other is SubtitleWithId otherWithId)
            {
                return string.Compare(Id, otherWithId.Id, StringComparison.Ordinal);
            }

            return Offset.CompareTo(other.Offset);
        }
    }
}
