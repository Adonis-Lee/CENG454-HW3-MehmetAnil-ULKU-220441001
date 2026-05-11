using UnityEngine;
using CoreBreach.Domain.Enemies;
using CoreBreach.Domain.Waves;
using CoreBreach.Infrastructure.Pooling;

namespace CoreBreach.Domain.Pickups
{
    /// <summary>
    /// Observer subscriber: Enemy.Died → %dropChance ihtimalle Pickup spawn eder.
    /// WaveSpawner'dan enemy referansları alır, Died event'lerine abone olur.
    /// OnEnable/OnDisable ile lifecycle yönetimi.
    /// </summary>
    public class DropSystem : MonoBehaviour
    {
        [SerializeField] private Pickup pickupPrefab;
        [SerializeField] private WaveSpawner waveSpawner;
        [Range(0f, 1f)]
        [SerializeField] private float dropChance = 0.2f;

        private GenericPool<Pickup> pickupPool;

        private void Awake()
        {
            if (pickupPrefab == null) return;
            pickupPool = new GenericPool<Pickup>(
                prefab: pickupPrefab,
                parent: transform,
                defaultCapacity: 8,
                maxSize: 32);
        }

        private void OnEnable()
        {
            if (waveSpawner != null)
                waveSpawner.EnemySpawned += OnEnemySpawned;
        }

        private void OnDisable()
        {
            if (waveSpawner != null)
                waveSpawner.EnemySpawned -= OnEnemySpawned;
        }

        private void OnEnemySpawned(Enemy enemy)
        {
            enemy.Died += OnEnemyDied;
        }

        private void OnEnemyDied(Enemy enemy)
        {
            enemy.Died -= OnEnemyDied;
            if (pickupPool == null) return;
            if (Random.value > dropChance) return;

            Pickup pickup = pickupPool.Get();
            pickup.transform.position = enemy.transform.position;
            UpgradeType type = (UpgradeType)Random.Range(0, System.Enum.GetValues(typeof(UpgradeType)).Length);
            pickup.Configure(type, pickupPool);
        }
    }
}
