using System;
using UnityEngine;
using CoreBreach.Domain.Combat;

namespace CoreBreach.Domain.CoreDomain
{
    /// <summary>
    /// Savunulan enerji çekirdeği. IDamageable implementasyonu.
    /// Damaged event'i çoklu bağımsız subscriber'a yayar (Observer pattern):
    ///   → CoreHpView  (HUD güncelleme)
    ///   → GameStateMachine (HP≤0 → Lose)
    ///   → AudioCue (çarpma sesi)
    ///   → ScreenShake (VFX)
    /// </summary>
    public class Core : MonoBehaviour, IDamageable
    {
        [SerializeField] private int maxHp = 30;
        private int currentHp;

        public event Action<DamageInfo> Damaged;
        public event Action CoreDestroyed;

        public bool IsAlive => currentHp > 0;
        public int CurrentHp => currentHp;
        public int MaxHp => maxHp;

        private void Awake()
        {
            currentHp = maxHp;
        }

        public void TakeDamage(DamageInfo info)
        {
            if (!IsAlive) return;

            currentHp = Mathf.Max(0, currentHp - info.Amount);
            Damaged?.Invoke(info);

            if (currentHp <= 0)
                CoreDestroyed?.Invoke();
        }
    }
}
