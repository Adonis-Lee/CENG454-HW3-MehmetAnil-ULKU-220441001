using System;
using UnityEngine;
using CoreBreach.Domain.Combat;
using CoreBreach.Infrastructure.Pooling;

namespace CoreBreach.Domain.Enemies
{
    /// <summary>
    /// Düşman MonoBehaviour. Strategy'leri tick eder, IDamageable, IPoolable.
    /// Bootstrap aşamasında stub — gerçek davranış feat/enemy-strategy branch'inde.
    /// </summary>
    public class Enemy : MonoBehaviour, IDamageable, IPoolable
    {
        [SerializeField] private int hp = 2;

        private IMovementStrategy movement;
        private ITargetingStrategy targeting;

        public event Action<Enemy> Died;

        public bool IsAlive => hp > 0;

        public void Configure(IMovementStrategy move, ITargetingStrategy target, int startHp)
        {
            movement = move;
            targeting = target;
            hp = startHp;
        }

        public void TakeDamage(DamageInfo info)
        {
            if (!IsAlive) return;
            hp -= info.Amount;
            if (hp <= 0) Died?.Invoke(this);
        }

        public void OnSpawn()
        {
            // hp ve subscription burada init — feat/enemy-strategy'de doldurulacak
        }

        public void OnDespawn()
        {
            // event listener temizliği — Debug #003 önlemi
            Died = null;
            movement = null;
            targeting = null;
        }
    }
}
