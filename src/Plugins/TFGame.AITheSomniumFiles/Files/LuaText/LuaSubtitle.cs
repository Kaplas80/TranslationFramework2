using TF.Core.TranslationEntities;

namespace TFGame.AITheSomniumFiles.Files
{
    public class LuaSubtitle : Subtitle
    {
        public string Id { get; set; }

        public LuaSubtitle() : base()
        {

        }

        public LuaSubtitle(Subtitle s) : base()
        {
            Offset = s.Offset;
            Text = s.Text;
            Translation = s.Translation;
            Loaded = s.Loaded;
        }
    }
}
