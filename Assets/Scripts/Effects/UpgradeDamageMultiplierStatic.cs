using Attributes;
using MobileFramework.Abilities;
using Unity.VisualScripting;
using UnityEngine;

namespace Effects
{
    public class UpgradeDamageMultiplierStatic : GameplayEffectBase
    {

        public UpgradeDamageMultiplierStatic()
        {
            Type = GameplayEffectType.Static;
        }

        public override string EffectTag => "Tower.Damage.StaticMultiplier";

        public override float CalculateDuration(GameplayEffectApplyContext effectContext) => 0.0f;

        public override float CalculatePeriod(GameplayEffectApplyContext effectApplyContext) => 0.0f;

        private float _addedDamage = 0.0f;

        [AttributesChangeMethod(AttributeChangeOn.Apply)]
        public void ApplyEffectToAttributes(StatsAttributes attributes, GameplayEffectApplyContext effectApplyContext)
        {
            attributes.MakeTransactionChanges<StatsAttributes>((t, attr) =>
            {
                _addedDamage = Mathf.Pow(1.2f, (float)effectApplyContext.Level);
                attr.Damage.CurrentValue += _addedDamage;
            });
        }

        [AttributesChangeMethod(AttributeChangeOn.Clear)]
        public void ClearEffectFromAttributes(StatsAttributes attributes, GameplayEffectApplyContext effectApplyContext)
        {
            attributes.MakeTransactionChanges<StatsAttributes>((t, attr) =>
            {
                attr.Damage.CurrentValue -= _addedDamage;
            });
        }
    }
}