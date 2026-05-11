using UnityEngine;
using CoreBreach.Domain.Player;
using CoreBreach.Infrastructure.Pooling;

namespace CoreBreach.Domain.Pickups
{
    public enum UpgradeType { Pierce, DoubleDamage, Spread, Burst, ChainLightning }

    /// <summary>
    /// Pickup MonoBehaviour. IPoolable.
    /// Oyuncu üstüne gelince WeaponHolder.ApplyUpgrade() çağırır.
    /// Hangi upgrade tipini taşıdığı UpgradeType ile belirlenir.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class Pickup : MonoBehaviour, IPoolable
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        private UpgradeType upgradeType;
        private IPool<Pickup> ownerPool;
        private bool collected;

        private static readonly Color[] TypeColors =
        {
            Color.cyan,    // Pierce
            Color.red,     // DoubleDamage
            Color.green,   // Spread
            Color.yellow,  // Burst
            Color.blue,    // ChainLightning
        };

        public void Configure(UpgradeType type, IPool<Pickup> pool)
        {
            upgradeType = type;
            ownerPool = pool;
            collected = false;
            if (spriteRenderer != null)
                spriteRenderer.color = TypeColors[(int)type];
        }

        public void OnSpawn()  { collected = false; }
        public void OnDespawn() { ownerPool = null; }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (collected) return;
            if (!other.TryGetComponent<WeaponHolder>(out var holder)) return;
            collected = true;
            ApplyUpgrade(holder);
            ownerPool?.Release(this);
        }

        private void ApplyUpgrade(WeaponHolder holder)
        {
            switch (upgradeType)
            {
                case UpgradeType.Pierce:
                    holder.ApplyUpgrade(inner => new Weapons.PierceDecorator(inner));
                    break;
                case UpgradeType.DoubleDamage:
                    holder.ApplyUpgrade(inner => new Weapons.DoubleDamageDecorator(inner));
                    break;
                case UpgradeType.Spread:
                    holder.ApplyUpgrade(inner => new Weapons.SpreadDecorator(inner));
                    break;
                case UpgradeType.Burst:
                    holder.ApplyUpgrade(inner => new Weapons.BurstDecorator(inner, holder));
                    break;
                case UpgradeType.ChainLightning:
                    holder.ApplyUpgrade(inner => new Weapons.ChainLightningDecorator(inner));
                    break;
            }
        }
    }
}
