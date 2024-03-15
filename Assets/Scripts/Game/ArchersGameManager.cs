using MobileFramework;

namespace Game
{
    public class ArchersGameManager : GameManagerBase
    {
        public ArchersGameManager()
        {
            SaveGameContextValuesToPersistence = false;
            SavePlayerContextValuesToPersistence = false;
        }
    }
}