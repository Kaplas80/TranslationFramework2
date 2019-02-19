using System.ComponentModel;
using System.Runtime.CompilerServices;
using YakuzaCommon.Annotations;

namespace YakuzaCommon.Files.CmnBin
{
    internal abstract class Subtitle : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _translation;

        public long Offset { get; set; }
        public string Text { get; set; }
        public string Loaded { get; set; }
        public string Translation {
            get => _translation;
            set
            {
                _translation = value;
                OnPropertyChanged(nameof(Translation));
            }
        }
        public abstract int MaxLength { get; }
        
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    internal class LongSubtitle : Subtitle
    {
        public override int MaxLength => 256;
    }

    internal class ShortSubtitle : Subtitle
    {
        public override int MaxLength => 128;
    }
}
