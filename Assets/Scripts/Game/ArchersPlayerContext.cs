using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MobileFramework.Game;

namespace Game
{
    public class ArchersPlayerContext : PlayerContext, INotifyPropertyChanged
    {
        public int PowerCount
        {
            get => _powerCount;
            set
            {
                if (_powerCount == value) return;
                if (value > _maxPowerCount) return;
                if (value < 0) return;
                _powerCount = value;
                OnPropertyChanged();
            }
        }
        private int _powerCount;
        private int _maxPowerCount;

        public int MaxPowerCount
        {
            get => _maxPowerCount;
            set
            {
                if (value == _maxPowerCount) return;
                _maxPowerCount = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}