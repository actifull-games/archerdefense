using System;
using DG.Tweening;
using MobileFramework.Abilities;
using MobileFramework.Parameters;
using UnityEngine;

namespace Shooting
{
    public class TargetedProjectile : ProjectileWithEffects
    {
        public bool LookAtTarget = true;
        
        public void LaunchProjectile(GameObject target, float duration)
        {
            var t = this.transform;
            var targetTransform = target.transform;
            if(LookAtTarget) t.LookAt(targetTransform);
            transform.DOMove(targetTransform.position, duration).OnComplete(() =>
            {
                OnProjectileHit(target);
            });
        }
    }
}