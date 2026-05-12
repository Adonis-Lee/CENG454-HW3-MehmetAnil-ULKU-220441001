using System;
using UnityEngine;
using CoreBreach.Domain.Enemies;

namespace CoreBreach.Domain.Waves
{
    /// <summary>
    /// Observer subscriber: Enemy.Died → alive count azalt → dalga bitti mi kontrol et.
    /// WaveCompleted event'i → GameStateMachine + WaveView (Observer chain).
    /// WaveSpawner.EnemySpawned'e abone olur, her enemy'yi takip eder.
    /// OnEnable/OnDisable lifecycle simetrisi.
    /// </summary>
    public class WaveTracker : MonoBehaviour
    {
        [SerializeField] private WaveSpawner waveSpawner;

        public event Action<int> WaveCompleted;

        private int aliveCount;
        private int currentWave;

        private void OnEnable()
        {
            if (waveSpawner == null) return;
            waveSpawner.EnemySpawned += OnEnemySpawned;
            waveSpawner.WaveStarted += OnWaveStarted;
        }

        private void OnDisable()
        {
            if (waveSpawner == null) return;
            waveSpawner.EnemySpawned -= OnEnemySpawned;
            waveSpawner.WaveStarted -= OnWaveStarted;
        }

        private void OnWaveStarted(int wave, int count)
        {
            currentWave = wave;
            aliveCount = count;
        }

        private void OnEnemySpawned(Enemy enemy)
        {
            enemy.Died += OnEnemyDied;
        }

        private void OnEnemyDied(Enemy enemy)
        {
            enemy.Died -= OnEnemyDied;
            aliveCount = Mathf.Max(0, aliveCount - 1);
            if (aliveCount <= 0)
                WaveCompleted?.Invoke(currentWave);
        }
    }
}
