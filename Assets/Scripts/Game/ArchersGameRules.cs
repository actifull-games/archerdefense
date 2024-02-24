using MobileFramework.Game;

namespace Game
{
    public class ArchersGameRules : GameRules
    {
        public ArchersGameRules()
        {
            gameContextClass = typeof(ArchersGameContext);
        }

        public ArchersGameContext TypedGameContext => (ArchersGameContext)GameContext;
    }
}