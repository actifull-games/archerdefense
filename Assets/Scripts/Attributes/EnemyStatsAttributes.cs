using MobileFramework.Abilities;

namespace Attributes
{
    public class EnemyStatsAttributes : StatsAttributes
    {
        public GameplayAttribute<float> MoveSpeed { get; } = 1.5f;
    }
}