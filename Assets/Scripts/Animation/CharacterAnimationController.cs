using System;
using Attributes;
using Characters;
using Game;
using MobileFramework;
using MobileFramework.Abilities;
using Shooting;
using UnityEngine;

namespace Animation
{
    public class CharacterAnimationController : GameBehaviour<ArchersGameRules>
    {
        private Animator _animator;

        private VitalityAttributes _attributes;
        private static readonly int IsDead = Animator.StringToHash("IsDead");

        private StatsAttributes _stats;

        private CharacterBase _character;
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int AttackSpeed = Animator.StringToHash("AttackSpeed");


        private ShootingManager _shooting;
        private static readonly int IsAttack = Animator.StringToHash("IsAttack");
        private static readonly int IsMelee = Animator.StringToHash("IsMelee");

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _character = GetComponent<CharacterBase>();
            var abilitySystem = GetComponent<GameplayAbilitySystem>();
            _attributes = abilitySystem.GetAttributeSet<VitalityAttributes>();
            _attributes.Death.AddListener(Die);
            _stats = abilitySystem.GetAttributeSet<StatsAttributes>();
            _shooting = GetComponent<ShootingManager>();
            _animator.SetBool(IsMelee, _shooting == null);
        }

        private void Die()
        {
            _animator.SetBool(IsDead, true);
        }

        public void NotifyShoot()
        {
            if (_shooting != null)
            {
                _shooting.Shoot();
            }
        }

        private void Update()
        {
            _animator.SetBool(IsMoving, _character.aiPath.velocity.magnitude > 0);
            _animator.SetFloat(AttackSpeed, _stats.AttackSpeed.CurrentValue);
            if (_shooting != null)
            {
                _animator.SetBool(IsAttack, _shooting.HasTarget);
            }
        }
    }
}