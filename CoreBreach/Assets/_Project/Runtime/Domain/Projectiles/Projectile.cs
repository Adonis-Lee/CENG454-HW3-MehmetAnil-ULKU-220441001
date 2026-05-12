using UnityEngine;
using CoreBreach.Domain.Combat;
using CoreBreach.Domain.CoreDomain;
using CoreBreach.Infrastructure.Pooling;

namespace CoreBreach.Domain.Projectiles
{
    /// <summary>
    /// Mermi MonoBehaviour. Pool'lanabilir (IPoolable). Çarpışma anında IDamageable'a
    /// hasar verir, sonra kendi pool'una geri döner. Owner concrete tipi tanımaz.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class Projectile : MonoBehaviour, IPoolable
    {
        [SerializeField] private float speed = 14f;
        [SerializeField] private float lifetime = 2.5f;

        private Vector2 direction;
        private int damage;
        private float age;
        private GameObject owner;
        private IPool<Projectile> ownerPool;
        private bool released;
        private int pierceRemaining;

        public void Launch(Vector2 dir, int dmg, GameObject ownerObject, IPool<Projectile> pool, int pierce = 0)
        {
            direction = dir.normalized;
            damage = dmg;
            owner = ownerObject;
            ownerPool = pool;
            age = 0f;
            released = false;
            pierceRemaining = pierce;
            transform.up = direction;
        }

        public void OnSpawn()
        {
            // Pool'dan alındığında — state init. Pool zaten SetActive(true) yapar.
            age = 0f;
            released = false;
        }

        public void OnDespawn()
        {
            // Pool'a iade edilirken — referansları temizle, pasifleştir.
            direction = Vector2.zero;
            damage = 0;
            age = 0f;
            owner = null;
            ownerPool = null;
        }

        private void Update()
        {
            if (released) return;
            transform.position += (Vector3)(direction * speed * Time.deltaTime);
            age += Time.deltaTime;
            if (age >= lifetime) ReturnToPool();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (released) return;
            if (other.gameObject == owner) return;             // kendi sahibine vurma
            if (other.TryGetComponent<Core>(out _)) return;   // Core'a vurma — sadece enemy

            if (other.TryGetComponent<IDamageable>(out var target) && target.IsAlive)
            {
                target.TakeDamage(new DamageInfo(damage, transform.position, gameObject));
                if (pierceRemaining > 0)
                    pierceRemaining--;
                else
                    ReturnToPool();
            }
        }

        private void ReturnToPool()
        {
            if (released) return;
            released = true;
            if (ownerPool != null) ownerPool.Release(this);
            else gameObject.SetActive(false);
        }
    }
}
