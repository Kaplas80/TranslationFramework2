using System.ComponentModel;
using System.Runtime.CompilerServices;
using TF.Core.Annotations;

namespace TF.Core.TranslationEntities
{
    public class PlainText : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _translation;

        public string Text { get; set; }
        public string Loaded { get; set; }
        public string Translation
        {
            get => _translation;
            set
            {
                _translation = value;
                OnPropertyChanged(nameof(Translation));
            }
        }

        public bool HasChanges => Loaded != Translation;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
