namespace CoreBreach.Domain.Combat
{
    /// <summary>
    /// Hasar alabilen herhangi bir varlık. Core ve Enemy implement eder.
    /// Projectile bu sözleşmeye bağlı kalır; concrete tipi tanımaz.
    /// </summary>
    public interface IDamageable
    {
        bool IsAlive { get; }
        void TakeDamage(DamageInfo info);
    }
}
