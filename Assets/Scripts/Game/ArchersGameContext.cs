using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MobileFramework.Game;
using MobileFramework.Persistence;

namespace Game
{
    public class ArchersGameContext : GameContext, INotifyPropertyChanged
    {
        [PersistenceValue("Money")]
        private int _playerMoney = 0;

        public int PlayerMoney
        {
            get => _playerMoney;
            set
            {
                if (value == _playerMoney) return;
                if (value < 0) return;
                _playerMoney = value;
                OnPropertyChanged();
                SaveProperties();
            }
        }

        [PersistenceValue("Level")]
        private int _playerLevel = 1;

        public int PlayerLevel
        {
            get => _playerLevel;
            set
            {
                if (value == _playerLevel) return;
                _playerLevel = value;
                OnPropertyChanged();
                SaveProperties();
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