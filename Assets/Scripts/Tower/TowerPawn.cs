using System;
using Attributes;
using Effects;
using Game;
using MobileFramework;
using MobileFramework.Abilities;
using Unity.VisualScripting;
using UnityEngine;

namespace Tower
{
    public class TowerPawn : GameBehaviour<ArchersGameRules>
    {
        private GameplayAbilitySystem _abilitySystem;

        public float baseDamage = 5.0f;
        public float baseAttackSpeed = 4.0f;
        public float baseAttackRange = 10.0f;

        public float maxHealth = 50.0f;

        private ArchersPlayerController _playerController;

        public StatsAttributes Stats { get; private set; }

        public VitalityAttributes Vitality { get; private set; }

        private void Awake()
        {
            _abilitySystem = GetComponent<GameplayAbilitySystem>();
            if (_abilitySystem == null)
            {
                _abilitySystem = this.AddComponent<GameplayAbilitySystem>();
            }
            //Initialize attributes
            Stats = _abilitySystem.AddAttributeSet<StatsAttributes>((ctx, attr) =>
            {
                attr.Damage.BaseValue = baseDamage;
                attr.AttackSpeed.BaseValue = baseAttackSpeed;
                attr.AttackRange.BaseValue = baseAttackRange;
                attr.Reset();
            });
            Vitality = _abilitySystem.AddAttributeSet<VitalityAttributes>((ctx, attr) =>
            {
                attr.MaxHealth.BaseValue = maxHealth;
                attr.Health.BaseValue = maxHealth;
                attr.Reset();
            });
            Vitality.Health.Changed.AddListener(DamageTaken);
            Vitality.Death.AddListener(OnDeath);
        }

        private void DamageTaken()
        {
            
        }

        private void OnDeath()
        {
            GameRules.LevelFailed();
        }

        public void ApplyUpgrades()
        {
            _playerController = GameRules.PlayerController as ArchersPlayerController;
            if (_playerController == null) throw new InvalidCastException("Player controller type invalid!");
            var context = _playerController.GetContext<ArchersPlayerContext>();
            if (context == null)
                throw new NullReferenceException("Player context is invalid!");
            _abilitySystem.ApplyGameplayEffectToSelf<UpgradeDamageMultiplierStatic>(context.DamageLevel, gameObject);
            _abilitySystem.ApplyGameplayEffectToSelf<UpgradeAttackSpeedMultiplierStatic>(context.AttackSpeedLevel, gameObject);
            _abilitySystem.ApplyGameplayEffectToSelf<UpgradeAttackRangeMultiplierStatic>(context.AttackRangeLevel, gameObject);
        }
    }
}