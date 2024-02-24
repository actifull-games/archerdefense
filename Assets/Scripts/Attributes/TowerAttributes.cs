using MobileFramework.Abilities;

namespace Attributes
{
    public class TowerAttributes : GameplayAttributesBase
    {
        public GameplayAttribute<float> Damage { get; } = 5.0f;
        public GameplayAttribute<float> AttackSpeed { get; } = 4.0f;
        public GameplayAttribute<float> AttackRange { get; } = 10.0f;
        
        public override void PreAttributeChange(string attributeName, GameplayAttributeBase attribute, ref bool cancelChange)
        {
            cancelChange = false;
        }

        public override void PostAttributeChange(string attributeName, GameplayAttributeBase attribute)
        {
            
        }
    }
}