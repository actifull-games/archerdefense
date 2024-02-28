using Attributes;
using MobileFramework.Abilities;

namespace Effects
{
    public class DamageEffectInstant : GameplayEffectBase
    {
        public DamageEffectInstant()
        {
            Type = GameplayEffectType.Instant;
        }
        public override string EffectTag => "Damage.Instant";
        public override float CalculateDuration(GameplayEffectApplyContext effectContext) => 0.0f;

        public override float CalculatePeriod(GameplayEffectApplyContext effectApplyContext) => 0.0f;

        [AttributesChangeMethod(AttributeChangeOn.Apply)]
        public void Apply(VitalityAttributes attributes, GameplayEffectApplyContext context)
        {
            var stats = context.OwnerAbilitySystem.GetAttributeSet<StatsAttributes>();
            var currentDamage = stats.Damage.CurrentValue;
            attributes.Health.CurrentValue -= currentDamage;
        }

        [AttributesChangeMethod(AttributeChangeOn.Clear)]
        public void Clear(VitalityAttributes attributes, GameplayEffectApplyContext context)
        {
            
        }
    }
}