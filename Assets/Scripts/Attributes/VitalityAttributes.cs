using MobileFramework.Abilities;
using UnityEngine;
using UnityEngine.Events;

namespace Attributes
{
    public class VitalityAttributes : GameplayAttributesBase
    {
        public GameplayAttribute<float> Health { get; } = 50.0f;
        public GameplayAttribute<float> MaxHealth { get; } = 50.0f;

        public UnityEvent Death = new();
        
        public override void PreAttributeChange(string attributeName, GameplayAttributeBase attribute, ref bool cancelChange)
        {
            if (Health.CurrentValue <= 0.0f)
            {
                cancelChange = true;
            }
        }

        public override void PostAttributeChange(string attributeName, GameplayAttributeBase attribute)
        {
            if (attributeName == nameof(Health))
            {
                if (Health.CurrentValue < 0.0f || Health.CurrentValue > MaxHealth.CurrentValue)
                {
                    Health.CurrentValue = Mathf.Clamp(Health.CurrentValue, 0.0f, MaxHealth.CurrentValue);
                }

                if (Health.CurrentValue == 0.0f)
                {
                    Death?.Invoke();
                }
            }
        }
    }
}