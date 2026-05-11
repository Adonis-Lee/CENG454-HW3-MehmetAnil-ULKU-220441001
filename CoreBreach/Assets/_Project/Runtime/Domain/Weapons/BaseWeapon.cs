using UnityEngine;
using CoreBreach.Domain.Projectiles;

namespace CoreBreach.Domain.Weapons
{
    /// <summary>
    /// Decorator zincirinin "leaf"i — IWeaponBehavior somut implementasyonu.
    /// Tek bir mermi atar. Decorator'lar bunu sarıp davranışı katmanlar
    /// (Pierce, DoubleDamage, Spread, Burst, ChainLightning).
    /// </summary>
    public class BaseWeapon : IWeaponBehavior
    {
        public void Fire(FireContext context)
        {
            if (context.ProjectilePool == null) return;
            Projectile projectile = context.ProjectilePool.Get();
            projectile.transform.position = context.Origin;
            projectile.Launch(context.Direction, context.BaseDamage, context.Owner, context.ProjectilePool, context.PierceCount);
        }
    }
}
