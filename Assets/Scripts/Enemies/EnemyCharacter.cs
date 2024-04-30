using Characters;
using MobileFramework.Parameters;
using Shooting;

namespace Enemies
{
    public class EnemyCharacter : CharacterBase
    {
        public ScriptType<ITypeSelectable> TargetSelector;

        private MeleeAttackManager _attackManager;
        protected override void Awake()
        {
            base.Awake();
            _attackManager = GetComponent<MeleeAttackManager>();
        }

        protected override void Start()
        {
            base.Start();
            _attackManager.attackRange = Stats.AttackRange.CurrentValue;
            Stats.AttackRange.Changed.AddListener(AttackRangeChanged);
            GameRules.AddEnemy(gameObject);
        }

        protected override void OnDeath()
        {
            GameRules.RemoveEnemy(gameObject);
            base.OnDeath();
        }

        private void AttackRangeChanged()
        {
            _attackManager.attackRange = Stats.AttackRange.CurrentValue;
        }
    }
}