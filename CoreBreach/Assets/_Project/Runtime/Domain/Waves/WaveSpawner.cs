using System;
using System.Collections;
using UnityEngine;
using CoreBreach.Domain.Enemies;
using CoreBreach.Infrastructure.Pooling;

namespace CoreBreach.Domain.Waves
{
    /// <summary>
    /// 5 dalgalı spawn sistemi.
    /// WaveStarted(waveIndex, enemyCount) → WaveTracker + WaveView (Observer).
    /// EnemySpawned(enemy)              → WaveTracker + DropSystem (Observer).
    /// İç alive sayacı → sonraki dalgayı tetikler.
    /// </summary>
    public class WaveSpawner : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Enemy enemyPrefab;
        [SerializeField] private Transform coreTransform;
        [SerializeField] private Transform playerTransform;

        [Header("Timing")]
        [SerializeField] private float spawnInterval    = 0.8f;
        [SerializeField] private float delayBetweenWaves = 3f;
        [SerializeField] private float spawnRadius      = 8f;

        [Header("Wave Definitions  (5 waves)")]
        [SerializeField] private WaveDefinition[] waves = new WaveDefinition[5];

        // ── Events (Observer) ────────────────────────────────────────────
        public event Action<int, int> WaveStarted;   // (waveIndex 1-based, enemyCount)
        public event Action<Enemy>    EnemySpawned;

        // ── Private state ─────────────────────────────────────────────────
        private GenericPool<Enemy> enemyPool;
        private int aliveCount;
        private int currentWaveIndex;          // 0-based internally

        // ─────────────────────────────────────────────────────────────────
        private void Start()
        {
            if (enemyPrefab == null) { Debug.LogError("[WaveSpawner] enemyPrefab atanmamış."); return; }

            enemyPool = new GenericPool<Enemy>(
                prefab: enemyPrefab,
                parent: transform,
                defaultCapacity: 32,
                maxSize: 128);

            StartCoroutine(RunAllWaves());
        }

        private IEnumerator RunAllWaves()
        {
            for (int i = 0; i < waves.Length; i++)
            {
                yield return new WaitForSeconds(delayBetweenWaves);
                yield return StartCoroutine(SpawnWave(i));
                // Bir sonraki dalgayı başlatmadan önce tüm düşmanların ölmesini bekle
                yield return new WaitUntil(() => aliveCount <= 0);
            }
        }

        private IEnumerator SpawnWave(int index)
        {
            currentWaveIndex = index;
            WaveDefinition def = waves[index];
            aliveCount = def.configs.Length;

            WaveStarted?.Invoke(index + 1, aliveCount);   // 1-based wave number for UI

            foreach (EnemyConfig config in def.configs)
            {
                SpawnOne(config);
                yield return new WaitForSeconds(spawnInterval);
            }
        }

        private void SpawnOne(EnemyConfig config)
        {
            Enemy enemy = enemyPool.Get();

            float angle   = UnityEngine.Random.Range(0f, Mathf.PI * 2f);
            Vector2 spawnPos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * spawnRadius;
            enemy.transform.position = spawnPos;

            enemy.Configure(config, enemyPool, coreTransform, playerTransform);

            EnemySpawned?.Invoke(enemy);
            enemy.Died += OnEnemyDied;
        }

        private void OnEnemyDied(Enemy enemy)
        {
            enemy.Died -= OnEnemyDied;
            aliveCount = Mathf.Max(0, aliveCount - 1);
        }
    }

    /// <summary>Bir dalganın spawn listesi. Inspector'da düzenlenebilir.</summary>
    [Serializable]
    public struct WaveDefinition
    {
        public EnemyConfig[] configs;
    }
}
