using System;
using Characters;
using Game;
using MobileFramework;
using MobileFramework.Abilities;
using UnityEngine;
using UnityEngine.Events;

namespace Shooting
{
    public class MeleeAttackManager : GameBehaviour<ArchersGameRules>
    {

        private GameObject _currentTarget = null;

        public void SetTarget(GameObject target)
        {
            _currentTarget = target;
        }

        private GameplayAbilitySystem _abilitySystem;

        private CharacterBase _character;
        
        public bool IsAttacking { get; private set; }

        public void StartAttack()
        {
            IsAttacking = true;
        }

        public void StopAttack()
        {
            IsAttacking = false;
        }
        
        private void Start()
        {
            _abilitySystem = GameplayAbilitySystem.FindAbilitySystem(gameObject);
            _character = GetComponent<CharacterBase>();
        }

        public bool CanAttack()
        {
            var position = _currentTarget.transform.position;
            return _currentTarget != null &&
                   Vector3.Distance(position, transform.position) <=
                   attackRange;
        }

        public void Attack()
        {
            if (!IsAttacking) return;
            if (CanAttack())
            {
                _abilitySystem.ApplyGameplayEffectToTarget(damageEffect.EffectType, _currentTarget, damageEffect.EffectLevel);
            }
            else
            {
                IsAttacking = false;
            }
        }

        public float attackRange;

        public GameplayEffectReference damageEffect;
    }
}