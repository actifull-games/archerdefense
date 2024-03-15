using System;
using Attributes;
using Game;
using MobileFramework;
using MobileFramework.Abilities;
using Pathfinding;
using UnityEngine;

namespace Characters
{
    public class CharacterBase : GameBehaviour<ArchersGameRules>
    {
        private GameplayAbilitySystem _abilitySystem;
        protected CharacterStatsAttributes Stats;
        protected VitalityAttributes Vitality;

        public float damage = 4.0f;
        public float attackSpeed = 4.0f;
        public float attackRange = 1.0f;
        public float moveSpeed = 1.5f;
        
        public float maxHealth = 20.0f;

        public float destroyOnDeathDelay = 5.0f;

        public float rotationSpeed = 10.0f;

        public AIPath aiPath;
        
        public bool IsDead { get; private set; }

        private Transform _focus = null;
        
        
        
        protected virtual void Awake()
        {
            IsDead = false;
            _abilitySystem = FindOrCreateAbilitySystem();
            Stats = _abilitySystem.AddAttributeSet<CharacterStatsAttributes>((ctx, s) =>
            {
                s.Damage.BaseValue = damage;
                s.AttackSpeed.BaseValue = attackSpeed;
                s.AttackRange.BaseValue = attackRange;
                s.MoveSpeed.BaseValue = moveSpeed;
                s.Reset();
            });
            Vitality = _abilitySystem.AddAttributeSet<VitalityAttributes>((ctx, s) =>
            {
                s.MaxHealth.BaseValue = maxHealth;
                s.Health.BaseValue = maxHealth;
                s.Reset();
            });
            Vitality.Death.AddListener(OnDeath);
            Stats.MoveSpeed.Changed.AddListener(MoveSpeedChanged);
        }

        private void MoveSpeedChanged()
        {
            aiPath.maxSpeed = Stats.MoveSpeed.CurrentValue;
        }

        protected virtual void Start()
        {
            MoveSpeedChanged();
        }

        public void SetDestination(Vector3 destination)
        {
            aiPath.destination = destination;
            aiPath.canMove = true;
        }

        public void StopMovement()
        {
            aiPath.canMove = false;
        }
        
        protected virtual void OnDeath()
        {
            IsDead = true;
            ClearFocus();
            StopMovement();
            Destroy(gameObject, destroyOnDeathDelay);
        }

        public void SetFocus(Transform to)
        {
            _focus = to;
        }

        public void ClearFocus()
        {
            _focus = null;
        }

        private void Update()
        {
            if (_focus != null)
            {
                var moveDirection = _focus.position - transform.position;
                var look = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, look, Time.deltaTime * rotationSpeed);
            }
        }
    }
}