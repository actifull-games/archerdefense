using Attributes;
using MobileFramework.Abilities;
using UnityEngine;

namespace Effects
{
    public class UpgradeAttackSpeedMultiplierStatic : GameplayEffectBase
    {
        public override string EffectTag => "Tower.AttackSpeed.StaticMultiplier";
        public override float CalculateDuration(GameplayEffectApplyContext effectContext) => 0.0f;

        public override float CalculatePeriod(GameplayEffectApplyContext effectApplyContext) => 0.0f;
        
        private float _addedValue = 0.0f;

        [AttributesChangeMethod(AttributeChangeOn.Apply)]
        public void ApplyEffectToAttributes(StatsAttributes attributes, GameplayEffectApplyContext effectApplyContext)
        {
            attributes.MakeTransactionChanges<StatsAttributes>((t, attr) =>
            {
                _addedValue = Mathf.Pow(1.1f, (float)effectApplyContext.Level) - 1f;
                attr.AttackSpeed.CurrentValue += _addedValue;
            });
        }

        [AttributesChangeMethod(AttributeChangeOn.Clear)]
        public void ClearEffectFromAttributes(StatsAttributes attributes, GameplayEffectApplyContext effectApplyContext)
        {
            attributes.MakeTransactionChanges<StatsAttributes>((t, attr) =>
            {
                attr.AttackSpeed.CurrentValue -= _addedValue;
            });
        }
    }
}