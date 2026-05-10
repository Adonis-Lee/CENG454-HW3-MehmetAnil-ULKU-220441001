using UnityEngine;
using CoreBreach.Domain.Combat;
using CoreBreach.Infrastructure.Pooling;

namespace CoreBreach.Domain.Projectiles
{
    /// <summary>
    /// Mermi MonoBehaviour. IPoolable + IDamageable çağrısı yapar (concrete bilmez).
    /// Bootstrap aşamasında stub — feat/player-weapon branch'inde tamamlanacak.
    /// </summary>
    public class Projectile : MonoBehaviour, IPoolable
    {
        [SerializeField] private float speed = 12f;
        [SerializeField] private float lifetime = 3f;

        private Vector2 direction;
        private int damage;
        private float age;

        public void Launch(Vector2 dir, int dmg)
        {
            direction = dir.normalized;
            damage = dmg;
            age = 0f;
        }

        public void OnSpawn()
        {
            age = 0f;
        }

        public void OnDespawn()
        {
            // pool'a iade öncesi state temizliği
            direction = Vector2.zero;
            damage = 0;
            age = 0f;
        }

        private void Update()
        {
            transform.position += (Vector3)(direction * speed * Time.deltaTime);
            age += Time.deltaTime;
            if (age >= lifetime) gameObject.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<IDamageable>(out var target) && target.IsAlive)
            {
                target.TakeDamage(new DamageInfo(damage, transform.position, gameObject));
                gameObject.SetActive(false);
            }
        }
    }
}
