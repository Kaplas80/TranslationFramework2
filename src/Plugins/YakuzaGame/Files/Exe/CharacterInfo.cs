using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using YakuzaGame.Annotations;

namespace YakuzaGame.Files.Exe
{
    public class CharacterInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public float[] Data;
        public float[] OriginalData;
        private float[] _loadedData;

        public byte Id { get; private set; }
        public char Chr => Convert.ToChar(Id);

        public bool HasChanged
        {
            get
            {
                for (var i = 0; i < 6; i++)
                {
                    if (Data[i] != _loadedData[i])
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public CharacterInfo(byte id)
        {
            Id = id;
            Data = new float[6];
            OriginalData = new float[6];
            _loadedData = new float[6];
        }

        public void SetLoadedData()
        {
            for (var i = 0; i < 6; i++)
            {
                _loadedData[i] = Data[i];
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public float this[int i]
        {
            get => Data[i];
            set
            {
                Data[i] = value;
                OnPropertyChanged(nameof(Data));
            }
        }
    }
}
