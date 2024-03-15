using System.Linq;
using Shooting;
using UnityEngine;

namespace Characters
{
    public class CharacterKnight : CharacterBase
    {
        private MeleeAttackManager _meleeAttackManager;

        private GameObject _target;
        
        protected override void Awake()
        {
            base.Awake();
            _meleeAttackManager = GetComponent<MeleeAttackManager>();
        }

        protected override void Start()
        {
            base.Start();
            _meleeAttackManager.attackRange = Stats.AttackRange.CurrentValue;
            Stats.AttackRange.Changed.AddListener(AttackRangeChanged);
        }

        private void AttackRangeChanged()
        {
            _meleeAttackManager.attackRange = Stats.AttackRange.CurrentValue;
        }

        public override GameObject FindNewTarget()
        {
            var position = transform.position;
            return (from e in GameRules.Enemies
                let dist = Vector3.Distance(position, e.transform.position)
                let comp = e.GetComponent<Enemy>()
                where !comp.isDead && comp.CheckVisibility()
                orderby dist
                select e).FirstOrDefault();

        }

        protected override void Update()
        {
            if (IsDead) return;
            base.Update();
            GameObject newTarget = null;
            if (_target == null)
            {
                newTarget = FindNewTarget();
            }
            else
            {
                var enemy = _target.GetComponent<Enemy>();
                if (enemy != null && enemy.isDead)
                {
                    newTarget = FindNewTarget();
                }
            }
            _target = newTarget;
            if (_target != null)
            {
                _meleeAttackManager.SetTarget(_target);
                var canAttack = _meleeAttackManager.CanAttack();
                if (canAttack && !_meleeAttackManager.IsAttacking)
                {
                    aiPath.canMove = false;
                    _meleeAttackManager.StartAttack();
                }
                else
                {
                    _meleeAttackManager.StopAttack();
                    SetDestination(_target.transform.position);
                }
            }
        }
    }
}