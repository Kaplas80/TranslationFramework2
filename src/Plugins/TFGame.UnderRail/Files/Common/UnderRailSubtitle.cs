using TF.Core.TranslationEntities;

namespace TFGame.UnderRail.Files.Common
{
    public class UnderRailSubtitle : Subtitle
    {
        public string Id { get; set; }

        public UnderRailSubtitle() : base()
        {

        }

        public UnderRailSubtitle(Subtitle s) : base()
        {
            Offset = s.Offset;
            Text = s.Text;
            Translation = s.Translation;
            Loaded = s.Loaded;
        }
    }
}
