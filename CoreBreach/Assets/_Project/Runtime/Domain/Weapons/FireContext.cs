using UnityEngine;
using CoreBreach.Domain.Projectiles;
using CoreBreach.Infrastructure.Pooling;

namespace CoreBreach.Domain.Weapons
{
    /// <summary>
    /// Silah ateşleme bağlamı. WeaponHolder doldurur, IWeaponBehavior zinciri tüketir.
    /// Decorator'lar bu context'i okur/değiştirir ve yeni context oluşturur.
    /// </summary>
    public readonly struct FireContext
    {
        public readonly Vector2 Origin;
        public readonly Vector2 Direction;
        public readonly int BaseDamage;
        public readonly IPool<Projectile> ProjectilePool;
        public readonly GameObject Owner;
        public readonly int PierceCount;   // 0 = no pierce, N = N extra hits

        public FireContext(Vector2 origin, Vector2 direction, int baseDamage,
                           IPool<Projectile> projectilePool, GameObject owner,
                           int pierceCount = 0)
        {
            Origin = origin;
            Direction = direction.normalized;
            BaseDamage = baseDamage;
            ProjectilePool = projectilePool;
            Owner = owner;
            PierceCount = pierceCount;
        }

        public FireContext WithDamage(int newDamage) =>
            new FireContext(Origin, Direction, newDamage, ProjectilePool, Owner, PierceCount);

        public FireContext WithDirection(Vector2 newDirection) =>
            new FireContext(Origin, newDirection, BaseDamage, ProjectilePool, Owner, PierceCount);

        public FireContext WithPierce(int pierce) =>
            new FireContext(Origin, Direction, BaseDamage, ProjectilePool, Owner, pierce);
    }
}
