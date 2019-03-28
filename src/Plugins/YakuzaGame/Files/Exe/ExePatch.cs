using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using YakuzaGame.Annotations;

namespace YakuzaGame.Files.Exe
{
    public class ExePatch : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _enabled;

        public string Name { get; set; }
        public string Description { get; set; }
        public List<Tuple<long, byte[]>> Patches { get; set; }

        public bool Enabled {
            get => _enabled;
            set
            {
                _enabled = value;
                OnPropertyChanged(nameof(Enabled));
            }
        }
        public bool LoadedStatus { get; set; }

        public bool HasChanges => Enabled != LoadedStatus;
    
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
