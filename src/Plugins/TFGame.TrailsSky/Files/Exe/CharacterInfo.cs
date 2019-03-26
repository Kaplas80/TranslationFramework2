using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TF.Core.Annotations;

namespace TFGame.TrailsSky.Files.Exe
{
    public class CharacterInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _width;

        public int Width
        {
            get => _width;
            set
            {
                _width = value;
                OnPropertyChanged(nameof(Width));
            }

        }
        public int OriginalWidth;
        private int _loadedWidth;

        public byte Id { get; private set; }
        public char Chr => Convert.ToChar(Id);

        public bool HasChanged => Width != _loadedWidth;

        public CharacterInfo(byte id)
        {
            Id = id;
            Width = 0;
            OriginalWidth = 0;
            _loadedWidth = 0;
        }

        public void SetLoadedWidth()
        {
            _loadedWidth = Width;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
