using System.Collections;
using UnityEngine;

namespace CoreBreach.Domain.Weapons
{
    /// <summary>
    /// Decorator: 3 ardışık atış yapar (zamansal katmanlama).
    /// MonoBehaviour gerektirir — WeaponHolder'dan coroutine başlatır.
    /// </summary>
    public class BurstDecorator : IWeaponBehavior
    {
        private readonly IWeaponBehavior inner;
        private readonly int burstCount;
        private readonly float burstDelay;
        private readonly MonoBehaviour runner;

        public BurstDecorator(IWeaponBehavior inner, MonoBehaviour runner,
                               int burstCount = 3, float burstDelay = 0.08f)
        {
            this.inner = inner;
            this.runner = runner;
            this.burstCount = burstCount;
            this.burstDelay = burstDelay;
        }

        public void Fire(FireContext context)
        {
            runner.StartCoroutine(BurstRoutine(context));
        }

        private IEnumerator BurstRoutine(FireContext context)
        {
            for (int i = 0; i < burstCount; i++)
            {
                inner.Fire(context);
                yield return new WaitForSeconds(burstDelay);
            }
        }
    }
}
