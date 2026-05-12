using UnityEngine;
using CoreBreach.Domain.Combat;
using CoreBreach.Domain.CoreDomain;

namespace CoreBreach.Domain.Weapons
{
    /// <summary>
    /// Decorator: mermi hedefe çarptıktan sonra en yakın 2 düşmana zincir hasar verir.
    /// IDamageable arayüzünü kullanır — concrete Enemy tipini tanımaz.
    /// Zincir mantığı Projectile yerine burada tutulur (tek sorumluluk).
    /// </summary>
    public class ChainLightningDecorator : IWeaponBehavior
    {
        private readonly IWeaponBehavior inner;
        private readonly int chainCount;
        private readonly float chainRadius;
        private readonly int chainDamage;

        public ChainLightningDecorator(IWeaponBehavior inner,
                                       int chainCount = 2,
                                       float chainRadius = 3f,
                                       int chainDamage = 1)
        {
            this.inner = inner;
            this.chainCount = chainCount;
            this.chainRadius = chainRadius;
            this.chainDamage = chainDamage;
        }

        public void Fire(FireContext context)
        {
            // inner ateşlenir; zincir etkisi için Projectile'ın OnHit callback'i
            // bu scope'ta bağlanamaz — ChainLightning basit versiyonda
            // mevcut pozisyonun etrafındaki hedeflere doğrudan hasar uygular.
            inner.Fire(context);
            ApplyChainAtOrigin(context);
        }

        private void ApplyChainAtOrigin(FireContext context)
        {
            var hits = Physics2D.OverlapCircleAll(context.Origin, chainRadius);
            int count = 0;
            foreach (var hit in hits)
            {
                if (count >= chainCount) break;
                if (hit.gameObject == context.Owner) continue;
                if (hit.TryGetComponent<Core>(out _)) continue;  // Core'a zincir vurma
                if (hit.TryGetComponent<IDamageable>(out var target) && target.IsAlive)
                {
                    target.TakeDamage(new DamageInfo(chainDamage, hit.transform.position, context.Owner));
                    count++;
                }
            }
        }
    }
}
