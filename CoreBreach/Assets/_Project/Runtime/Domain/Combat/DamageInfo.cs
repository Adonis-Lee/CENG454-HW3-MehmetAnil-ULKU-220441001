using UnityEngine;

namespace CoreBreach.Domain.Combat
{
    /// <summary>
    /// Hasar olayının taşıdığı bilgi. Event payload olarak yayınlanır.
    /// </summary>
    public readonly struct DamageInfo
    {
        public readonly int Amount;
        public readonly Vector2 HitPoint;
        public readonly GameObject Source;

        public DamageInfo(int amount, Vector2 hitPoint, GameObject source)
        {
            Amount = amount;
            HitPoint = hitPoint;
            Source = source;
        }
    }
}
