using System.Collections;
using UnityEngine;
using CoreBreach.Domain.Enemies;
using CoreBreach.Infrastructure.Pooling;

namespace CoreBreach.Domain.Waves
{
    /// <summary>
    /// Düşman dalgalarını spawn eder. Her enemy için:
    ///   1. Pool'dan Enemy alır
    ///   2. Enemy.Configure() ile EnemyConfig'i (strategy'leri) atar
    ///   3. Spawn pozisyonunu ayarlar
    /// WaveCompleted event'i → GameStateMachine + WaveView (Observer)
    /// Gerçek 5-dalga sistemi feat/waves-and-state'de tamamlanır.
    /// </summary>
    public class WaveSpawner : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Enemy enemyPrefab;
        [SerializeField] private Transform coreTransform;
        [SerializeField] private Transform playerTransform;

        [Header("Wave Config")]
        [SerializeField] private EnemyConfig[] spawnConfigs;  // sırayla spawn et
        [SerializeField] private float spawnInterval = 0.8f;
        [SerializeField] private float spawnRadius = 8f;

        private GenericPool<Enemy> enemyPool;
        private int aliveCount;

        public System.Action<int> WaveCompleted;
        private int currentWave = 0;

        private void Start()
        {
            if (enemyPrefab == null) { Debug.LogError("[WaveSpawner] enemyPrefab atanmamış."); return; }

            enemyPool = new GenericPool<Enemy>(
                prefab: enemyPrefab,
                parent: transform,
                defaultCapacity: 32,
                maxSize: 128);

            StartCoroutine(SpawnWave());
        }

        private IEnumerator SpawnWave()
        {
            currentWave++;
            aliveCount = spawnConfigs.Length;

            foreach (EnemyConfig config in spawnConfigs)
            {
                SpawnOne(config);
                yield return new WaitForSeconds(spawnInterval);
            }
        }

        private void SpawnOne(EnemyConfig config)
        {
            Enemy enemy = enemyPool.Get();

            // Daire etrafında rastgele spawn noktası
            float angle = Random.Range(0f, Mathf.PI * 2f);
            Vector2 spawnPos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * spawnRadius;
            enemy.transform.position = spawnPos;

            enemy.Configure(config, enemyPool, coreTransform, playerTransform);

            // Enemy ölünce aliveCount azalt, dalga bitti mi kontrol et
            enemy.Died += OnEnemyDied;
        }

        private void OnEnemyDied(Enemy enemy)
        {
            enemy.Died -= OnEnemyDied;
            aliveCount--;
            if (aliveCount <= 0)
                WaveCompleted?.Invoke(currentWave);
        }
    }
}
