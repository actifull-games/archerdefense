using MobileFramework.Game;
using MobileFramework.Persistence;

namespace Game
{
    public class ArchersGameContext : GameContext
    {
        [PersistenceValue("Money")]
        private int _playerMoney = 0;

        public int playerMoney => _playerMoney;

        [PersistenceValue("Level")]
        private int _playerLevel = 1;

        public int playerLevel => _playerLevel;
    }
}