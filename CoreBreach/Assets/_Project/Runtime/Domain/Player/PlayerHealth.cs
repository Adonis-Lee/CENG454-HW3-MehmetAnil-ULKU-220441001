using System;
using UnityEngine;
using CoreBreach.Domain.Combat;

namespace CoreBreach.Domain.Player
{
    /// <summary>
    /// Oyuncunun can sistemi. IDamageable.
    /// Damaged event → PlayerHealthView (Observer).
    /// Düşman çarpmasıyla hasar alır; 0'a düşünce PlayerDied event'i yayar.
    /// </summary>
    public class PlayerHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private int maxHp = 100;

        private int currentHp;

        public event Action<DamageInfo> Damaged;
        public event Action PlayerDied;

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
                PlayerDied?.Invoke();
        }
    }
}
