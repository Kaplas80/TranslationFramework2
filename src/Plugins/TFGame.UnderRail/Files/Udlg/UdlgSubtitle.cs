using TF.Core.TranslationEntities;

namespace TFGame.UnderRail.Files.Udlg
{
    public class UdlgSubtitle : Subtitle
    {
        public string Id { get; set; }

        public UdlgSubtitle() : base()
        {

        }

        public UdlgSubtitle(Subtitle s) : base()
        {
            Offset = s.Offset;
            Text = s.Text;
            Translation = s.Translation;
            Loaded = s.Loaded;
        }
    }
}
