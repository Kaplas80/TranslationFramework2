namespace TF.Core.TranslationEntities
{
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
    }
}
