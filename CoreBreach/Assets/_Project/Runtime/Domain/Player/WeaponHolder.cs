using System;
using UnityEngine;
using CoreBreach.Domain.Weapons;
using CoreBreach.Domain.Projectiles;
using CoreBreach.Infrastructure.Pooling;

namespace CoreBreach.Domain.Player
{
    /// <summary>
    /// Player'a takılan silah sahibi. Aktif IWeaponBehavior zincirini tutar
    /// (decorator stack'i), cooldown yönetir, Fire çağrılarını gerçekleştirir.
    /// Upgrade pickup'ları yeni decorator ekler — UpgradeApplied event'i yayar.
    /// </summary>
    public class WeaponHolder : MonoBehaviour
    {
        [SerializeField] private Transform muzzle;
        [SerializeField] private float cooldown = 0.25f;
        [SerializeField] private int baseDamage = 1;

        private IWeaponBehavior weapon;
        private IPool<Projectile> projectilePool;
        private float nextFireTime;

        public event Action<IWeaponBehavior> UpgradeApplied;

        public void Configure(IPool<Projectile> pool)
        {
            projectilePool = pool;
            weapon = new BaseWeapon();
        }

        /// <summary>
        /// Decorator'ı mevcut silahın etrafına sarar. Pickup tetikler.
        /// </summary>
        public void ApplyUpgrade(Func<IWeaponBehavior, IWeaponBehavior> wrapper)
        {
            if (wrapper == null || weapon == null) return;
            weapon = wrapper(weapon);
            UpgradeApplied?.Invoke(weapon);
        }

        public void TryFire(Vector2 aimDirection)
        {
            if (weapon == null || projectilePool == null) return;
            if (Time.time < nextFireTime) return;
            nextFireTime = Time.time + cooldown;

            Vector2 origin = muzzle != null ? (Vector2)muzzle.position : (Vector2)transform.position;
            FireContext ctx = new FireContext(origin, aimDirection, baseDamage, projectilePool, gameObject);
            weapon.Fire(ctx);
        }
    }
}
