using System;
using System.Linq;
using Attributes;
using Game;
using MobileFramework;
using MobileFramework.Abilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Shooting
{
    public class ShootingManager : GameBehaviour<ArchersGameRules>
    {
        public bool autoShoot = true;
        public GameObject projectilePrefab;

        public Transform projectileSpawn;
        
        private StatsAttributes _stats;
        private GameplayAbilitySystem _abilitySystem;
        
        private GameObject _currentTarget;

        private bool _isShooting = false;

        public UnityEvent OnShoot = new();
        private float _lastShootTime = 0.0f;

        public UnityEvent<GameObject> TargetChanged = new();

        public GameObject ChangeTarget()
        {
            if (GameRules.Enemies == null) return null;
            Vector3 position = transform.position;
            return (from e in GameRules.Enemies
                let distance = Vector3.Distance(e.transform.position, position)
                where distance <= _stats.AttackRange.CurrentValue
                orderby distance
                select e).FirstOrDefault();
        }

        public bool HasTarget => _currentTarget != null;

        public float AnimationSpeed => _stats.AttackSpeed.CurrentValue;
        
        private void Start()
        {
            _abilitySystem = GetComponent<GameplayAbilitySystem>();
            _stats = _abilitySystem.GetAttributeSet<StatsAttributes>();
        }

        public void StartShoot()
        {
            _isShooting = true;
            Shoot();
        }

        public void StopShoot()
        {
            _isShooting = false;
        }

        public void Shoot()
        {
            if (_currentTarget == null) return;
            var projectile = Instantiate(projectilePrefab, projectileSpawn.position, projectileSpawn.rotation);
            var projectileComponent = projectile.GetComponent<ProjectileWithEffects>();
            projectileComponent.Owner = gameObject;
            if (projectileComponent is TargetedProjectile t)
            {
                t.LookAtTarget = true;
                t.LaunchProjectile(_currentTarget, 0.5f);
            } 
            else if (projectileComponent is PhysicsProjectile p)
            {
                p.LookAtTarget = true;
                p.LaunchProjectile((_currentTarget.transform.position - projectileSpawn.transform.position).normalized);
                
                Debug.DrawLine(projectileSpawn.transform.position, _currentTarget.transform.position, Color.red, 1.0f);
            }
        }

        public void Update()
        {
            if (_currentTarget == null)
            {
                _currentTarget = ChangeTarget();
                TargetChanged.Invoke(_currentTarget);
            }
            else
            {
                var enemy = _currentTarget.GetComponent<Enemy>();
                if (enemy.isDead)
                {
                    _currentTarget = ChangeTarget();
                    TargetChanged.Invoke(_currentTarget);
                }
                else
                {
                    var distanceToTarget = Vector3.Distance(_currentTarget.transform.position, transform.position);
                    if (distanceToTarget > _stats.AttackRange.CurrentValue)
                    {
                        _currentTarget = ChangeTarget();
                        TargetChanged.Invoke(_currentTarget);
                    }
                }
            }
        }
    }
}