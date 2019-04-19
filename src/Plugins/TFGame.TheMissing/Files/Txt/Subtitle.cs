namespace TFGame.TheMissing.Files.Txt
{
    public class Subtitle : TF.Core.TranslationEntities.Subtitle
    {
        public int MsgId { get; set; }

        private float _width;
        private float _height;

        public float LoadedWidth { get; set; }
        public float Width
        {
            get => _width;
            set
            {
                _width = value;
                OnPropertyChanged(nameof(Width));
            }
        }

        public float LoadedHeight { get; set; }
        public float Height
        {
            get => _height;
            set
            {
                _height = value;
                OnPropertyChanged(nameof(Height));
            }
        }

        public override bool HasChanges => base.HasChanges || Height != LoadedHeight || Width != LoadedWidth;

        public Subtitle(TF.Core.TranslationEntities.Subtitle s, int id)
        {
            Offset = s.Offset;
            Text = s.Text;
            Translation = s.Translation;
            Loaded = s.Loaded;
            MsgId = id;
        }
    }
}
