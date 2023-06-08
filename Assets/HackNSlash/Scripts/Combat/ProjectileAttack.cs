using System;
using UnityEngine;

namespace Combat
{
    [Serializable]
    public class ProjectileAttack : Attack
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float fireForce;
        [SerializeField] private float projectileLifetime;

        public void Execute(Transform origin, LayerMask mask)
        {
            var projectileGameObject = UnityEngine.Object.Instantiate(projectilePrefab, origin.position, origin.rotation);
            
            if (projectileGameObject.TryGetComponent(out Projectile projectile))
            {
                projectile.Rigidbody.AddForce(origin.forward * fireForce, ForceMode.Impulse);
                projectile.Hitbox.SetValues(hitboxPosition, hitboxSize, damage, knockbackAmount);
                projectile.Hitbox.mask = mask;
                projectile.Hitbox.StartTryHitOnce(projectile.transform);
                projectile.SetLastLifetime(projectileLifetime);
            }
            else
            {
                Debug.LogError($"{projectilePrefab.name} should have a Projectile component");
            }
        }
    }
}