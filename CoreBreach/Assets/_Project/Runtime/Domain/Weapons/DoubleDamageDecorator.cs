using UnityEngine;

namespace CoreBreach.Domain.Weapons
{
    /// <summary>
    /// Decorator: base hasarı ×2 yapar.
    /// </summary>
    public class DoubleDamageDecorator : IWeaponBehavior
    {
        private readonly IWeaponBehavior inner;

        public DoubleDamageDecorator(IWeaponBehavior inner) => this.inner = inner;

        public void Fire(FireContext context)
        {
            inner.Fire(context.WithDamage(Mathf.RoundToInt(context.BaseDamage * 1.5f)));
        }
    }
}
