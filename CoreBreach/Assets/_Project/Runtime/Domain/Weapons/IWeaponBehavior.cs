namespace CoreBreach.Domain.Weapons
{
    /// <summary>
    /// Silah davranışı sözleşmesi. Decorator pattern'in core arayüzü.
    /// Concrete impl: BaseWeapon + Pierce/DoubleDamage/Spread/Burst/ChainLightning decorator'ları.
    /// </summary>
    public interface IWeaponBehavior
    {
        void Fire(FireContext context);
    }
}
