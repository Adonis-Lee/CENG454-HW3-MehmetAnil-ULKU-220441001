using System;
using UnityEngine;
using CoreBreach.Domain.Combat;
using CoreBreach.Domain.CoreDomain;
using CoreBreach.Infrastructure.Pooling;

namespace CoreBreach.Domain.Enemies
{
    /// <summary>
    /// Düşman MonoBehaviour. Strategy'leri tick eder (IMovementStrategy + ITargetingStrategy).
    /// IDamageable + IPoolable. Core'a çarpınca hasar verir.
    /// Died event'i çoklu subscriber'a yayar (Observer):
    ///   → EnemyPool.Release, WaveTracker, DropSystem, AudioCue
    /// OnEnable/OnDisable simetrisi: Ghost Subscriber önlenir (Debug #003).
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class Enemy : MonoBehaviour, IDamageable, IPoolable
    {
        private int hp;
        private int damage;
        private IMovementStrategy movement;
        private ITargetingStrategy targeting;
        private IPool<Enemy> ownerPool;
        private bool released;

        public float Speed { get; private set; }
        public bool IsAlive => hp > 0;

        public event Action<Enemy> Died;

        public void Configure(EnemyConfig config, IPool<Enemy> pool, Transform coreTransform, Transform playerTransform)
        {
            hp = config.hp;
            damage = config.damage;
            Speed = config.speed;
            ownerPool = pool;

            transform.localScale = Vector3.one * config.scale;
            GetComponent<SpriteRenderer>().color = config.color;

            movement = config.movementStrategy;
            targeting = config.targetingStrategy;

            // Strategy'lere gerekli referansları ilet
            if (targeting is CoreTarget ct) ct.SetCore(coreTransform);
            if (targeting is PlayerTarget pt) pt.SetPlayer(playerTransform);
        }

        public void OnSpawn()
        {
            released = false;
            // OnEnable zaten subscribe'ı tetikler
        }

        public void OnDespawn()
        {
            // OnDisable zaten unsubscribe'ı tetikler
            movement = null;
            targeting = null;
            ownerPool = null;
            Died = null;
        }

        private void OnEnable()
        {
            released = false;
        }

        private void OnDisable()
        {
            // Event temizliği — Ghost Subscriber Debug #003 önlemi
            // Died = null burada yapılmaz (WaveTracker subscription'ı kırılır)
            // Pool'a iade OnDespawn'da temizler
        }

        private void Update()
        {
            if (!IsAlive || released) return;
            if (movement == null || targeting == null) return;

            Vector2 target = targeting.Pick(this);
            movement.Step(this, target, Time.deltaTime);
        }

        public void TakeDamage(DamageInfo info)
        {
            if (!IsAlive || released) return;
            hp -= info.Amount;
            if (hp <= 0) Die();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!IsAlive || released) return;
            if (other.TryGetComponent<Core>(out var core) && core.IsAlive)
            {
                core.TakeDamage(new DamageInfo(damage, transform.position, gameObject));
                Die();
            }
        }

        private void Die()
        {
            if (released) return;
            released = true;
            Died?.Invoke(this);
            ownerPool?.Release(this);
        }
    }
}
