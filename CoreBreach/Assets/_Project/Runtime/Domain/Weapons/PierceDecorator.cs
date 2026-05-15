namespace CoreBreach.Domain.Weapons
{
    /// <summary>
    /// Decorator: mermi N düşman deler.
    /// FireContext.PierceCount'u artırır, Projectile bu değeri okur.
    /// </summary>
    public class PierceDecorator : IWeaponBehavior
    {
        private readonly IWeaponBehavior inner;
        private readonly int pierceCount;

        public PierceDecorator(IWeaponBehavior inner, int pierceCount = 1)
        {
            this.inner = inner;
            this.pierceCount = pierceCount;
        }

        public void Fire(FireContext context)
        {
            inner.Fire(context.WithPierce(pierceCount));
        }
    }
}
