using System;
using Attributes;
using Game;
using MobileFramework;
using MobileFramework.Abilities;
using Shooting;
using UnityEngine;

namespace Tower
{
    public class TowerAnimationController : GameBehaviour<ArchersGameRules>
    {
        private ShootingManager _shootingManager;

        private VitalityAttributes _attributes;

        private Animator _animator;
        private static readonly int IsDead = Animator.StringToHash("IsDead");
        private static readonly int IsShooting = Animator.StringToHash("IsShooting");
        private static readonly int AttackSpeed = Animator.StringToHash("AttackSpeed");
        private static readonly int IsWin = Animator.StringToHash("IsWin");

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _shootingManager = GetComponentInParent<ShootingManager>();
            var abilitySystem = GetComponentInParent<GameplayAbilitySystem>();
            _attributes = abilitySystem.GetAttributeSet<VitalityAttributes>();
            _attributes.Death.AddListener(Die);
        }

        private void Die()
        {
            _animator.SetBool(IsDead, true);
        }

        public void NotifyAttack()
        {
            _shootingManager.Shoot();
        }

        public void Update()
        {
            _animator.SetBool(IsShooting, _shootingManager.HasTarget);
            _animator.SetFloat(AttackSpeed, _shootingManager.AnimationSpeed);
            _animator.SetBool(IsWin, GameRules.IsWin);
        }
    }
}