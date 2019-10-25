using TF.Core.TranslationEntities;

namespace TFGame.DiscoElysium.Files.Common
{
    public class DiscoElysiumSubtitle : Subtitle
    {
        public string Id { get; set; }

        public DiscoElysiumSubtitle() : base()
        {

        }

        public DiscoElysiumSubtitle(Subtitle s) : base()
        {
            Offset = s.Offset;
            Text = s.Text;
            Translation = s.Translation;
            Loaded = s.Loaded;
        }
    }
}
