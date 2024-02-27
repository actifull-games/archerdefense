using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MobileFramework.Game;
using MobileFramework.Persistence;

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

        [PersistenceValue("DamageLevel")]
        private int _damageLevel = 0;
        public int DamageLevel
        {
            get => _damageLevel;
            set => _damageLevel = value;
        }

        [PersistenceValue("SpeedLevel")]
        private int _attackSpeedLevel = 0;
        public int AttackSpeedLevel
        {
            get => _attackSpeedLevel;
            set => _attackSpeedLevel = value;
        }

        [PersistenceValue("RangeLevel")]
        private int _rangeLevel = 0;
        public int AttackRangeLevel
        {
            get => _rangeLevel;
            set => _rangeLevel = value;
        }

        public void ApplyChanges()
        {
            SaveProperties();
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