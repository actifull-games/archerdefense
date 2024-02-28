using System;
using System.Collections.Generic;
using System.Linq;
using MobileFramework.Abilities;
using MobileFramework.Parameters;
using UnityEngine;

namespace Shooting
{
    
    [Serializable]
    public class ProjectileEffect
    {
        public ScriptType<GameplayEffectBase> Effect;
        public int EffectLevel;
    }
    
    public class ProjectileWithEffects : MonoBehaviour
    {
        public List<ProjectileEffect> Effects;
        
        public GameObject Owner { get; set; }

        protected virtual void OnProjectileHit(GameObject target)
        {
            var abilitySystem = GameplayAbilitySystem.FindAbilitySystem(target);
            var ownerAbilitySystem = GameplayAbilitySystem.FindAbilitySystem(Owner);
            if (abilitySystem != null && ownerAbilitySystem != null)
            {
                foreach (var effect in Effects.Where(x => x.Effect.Type != null))
                {
                    ownerAbilitySystem.ApplyGameplayEffectToTarget(effect.Effect.Type, target, effect.EffectLevel);
                }
            }
            Destroy(gameObject);
        }
    }
}