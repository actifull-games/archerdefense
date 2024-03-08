using System;
using System.Collections.Generic;
using System.Linq;
using MobileFramework.Abilities;
using MobileFramework.Parameters;
using UnityEngine;

namespace Shooting
{
    
   
    public class ProjectileWithEffects : MonoBehaviour
    {
        public List<GameplayEffectReference> Effects;
        
        public GameObject Owner { get; set; }

        protected virtual void OnProjectileHit(GameObject target)
        {
            var abilitySystem = GameplayAbilitySystem.FindAbilitySystem(target);
            var ownerAbilitySystem = GameplayAbilitySystem.FindAbilitySystem(Owner);
            if (abilitySystem != null && ownerAbilitySystem != null)
            {
                foreach (var effect in Effects.Where(x => x.EffectType != null))
                {
                    ownerAbilitySystem.ApplyGameplayEffectToTarget(effect.EffectType, target, effect.EffectLevel);
                }
            }
            Destroy(gameObject);
        }
    }
}