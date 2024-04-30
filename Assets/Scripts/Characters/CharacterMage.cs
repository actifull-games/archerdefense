using Shooting;
using UnityEngine;

namespace Characters
{
    public class CharacterMage : CharacterBase
    {
        private ShootingManager _shootingManager;
        protected override void Awake()
        {
            base.Awake();
            _shootingManager = GetComponent<ShootingManager>();
            _shootingManager.TargetChanged.AddListener(TargetChanged);
        }

        protected override void Start()
        {
            base.Start();
            GameRules.AddCharacter(gameObject);
        }

        protected override void OnDeath()
        {
            GameRules.RemoveCharacter(gameObject);
            base.OnDeath();
        }

        private void TargetChanged(GameObject arg0)
        {
            if(arg0 != null) 
                SetFocus(arg0.transform);
            else 
                ClearFocus();
        }
    }
}