using UnityEngine;
using CoreBreach.Domain.Projectiles;
using CoreBreach.Infrastructure.Pooling;

namespace CoreBreach.Domain.Weapons
{
    /// <summary>
    /// Silah ateşleme bağlamı. WeaponHolder doldurur, IWeaponBehavior tüketir.
    /// Decorator zinciri bu context'i okur ve gerekirse pool'dan mermi alır.
    /// </summary>
    public readonly struct FireContext
    {
        public readonly Vector2 Origin;
        public readonly Vector2 Direction;
        public readonly int BaseDamage;
        public readonly IPool<Projectile> ProjectilePool;
        public readonly GameObject Owner;

        public FireContext(Vector2 origin, Vector2 direction, int baseDamage,
                           IPool<Projectile> projectilePool, GameObject owner)
        {
            Origin = origin;
            Direction = direction.normalized;
            BaseDamage = baseDamage;
            ProjectilePool = projectilePool;
            Owner = owner;
        }
    }
}
