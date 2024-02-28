using UnityEngine;

namespace Shooting
{
    [RequireComponent(typeof(Rigidbody))]
    public class PhysicsProjectile : ProjectileWithEffects
    {
        public bool LookAtTarget = true;

        private bool _launched = false;
        
        public void LaunchProjectile(Vector3 direction, float speed)
        {
            var physBody = GetComponent<Rigidbody>();
            if (LookAtTarget) transform.LookAt(transform.position + direction * speed);
            physBody.AddForce(direction * speed);
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