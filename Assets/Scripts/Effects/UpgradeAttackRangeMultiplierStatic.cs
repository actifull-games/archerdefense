using Attributes;
using MobileFramework.Abilities;
using UnityEngine;

namespace Effects
{
    public class UpgradeAttackRangeMultiplierStatic : GameplayEffectBase
    {
        public override string EffectTag => "Tower.Range.StaticMultiplier";
        public override float CalculateDuration(GameplayEffectApplyContext effectContext) => 0.0f;

        public override float CalculatePeriod(GameplayEffectApplyContext effectApplyContext) => 0.0f;
        
        private float _addedValue = 0.0f;

        public UpgradeAttackRangeMultiplierStatic()
        {
            Type = GameplayEffectType.Static;
        }

        [AttributesChangeMethod(AttributeChangeOn.Apply)]
        public void ApplyEffectToAttributes(StatsAttributes attributes, GameplayEffectApplyContext effectApplyContext)
        {
            attributes.MakeTransactionChanges<StatsAttributes>((t, attr) =>
            {
                _addedValue = (float)effectApplyContext.Level;
                attr.AttackRange.CurrentValue += _addedValue;
            });
        }
    }
}