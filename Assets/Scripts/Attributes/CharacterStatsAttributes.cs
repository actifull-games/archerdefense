using MobileFramework.Abilities;

namespace Attributes
{
    public class CharacterStatsAttributes : StatsAttributes
    {
        public GameplayAttribute<float> MoveSpeed { get; } = 1.5f;
    }
}