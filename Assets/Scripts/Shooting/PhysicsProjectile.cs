using UnityEngine;

namespace Shooting
{
    [RequireComponent(typeof(Rigidbody))]
    public class PhysicsProjectile : ProjectileWithEffects
    {
        public bool LookAtTarget = true;

        private bool _launched = false;

        public float Speed = 300.0f;
        
        public void LaunchProjectile(Vector3 direction)
        {
            var physBody = GetComponent<Rigidbody>();
            if (LookAtTarget) transform.LookAt(transform.position + direction * Speed);
            physBody.AddForce(direction * Speed);
            _launched = true;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject != gameObject)
            {
                OnProjectileHit(other.gameObject);
            }
        }
    }
}