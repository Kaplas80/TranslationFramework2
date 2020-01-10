using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TF.Core.Annotations;

namespace TF.Core.TranslationEntities
{
    public class Subtitle : INotifyPropertyChanged, IComparable<Subtitle>
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

        public virtual bool HasChanges => Loaded != Translation;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int CompareTo(Subtitle other)
        {
            return CompareToInternal(other);
        }

        protected virtual int CompareToInternal(Subtitle other)
        {
            return Offset.CompareTo(other.Offset);
        }
    }
}
