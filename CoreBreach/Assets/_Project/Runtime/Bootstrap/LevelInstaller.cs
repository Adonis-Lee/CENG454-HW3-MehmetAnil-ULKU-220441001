using UnityEngine;
using CoreBreach.Domain.Player;
using CoreBreach.Domain.Projectiles;
using CoreBreach.Domain.CoreDomain;
using CoreBreach.Domain.GameState;
using CoreBreach.Domain.Waves;
using CoreBreach.Infrastructure.Pooling;

namespace CoreBreach.Bootstrap
{
    /// <summary>
    /// Composition root — sahnedeki tüm bileşenleri birbirine bağlar.
    /// "Tek dev GameManager" anti-pattern'i yerine bu sınıf sadece wiring yapar;
    /// runtime davranış kendi sahibinde kalır.
    ///
    /// Observer chain:
    ///   WaveSpawner.WaveStarted/EnemySpawned → WaveTracker
    ///   WaveTracker.WaveCompleted            → GameStateMachine.OnWaveCompleted
    /// </summary>
    public class LevelInstaller : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] private WeaponHolder playerWeaponHolder;

        [Header("Core & State")]
        [SerializeField] private Core core;
        [SerializeField] private GameStateMachine gameStateMachine;

        [Header("Waves")]
        [SerializeField] private WaveSpawner waveSpawner;
        [SerializeField] private WaveTracker waveTracker;

        [Header("Projectiles")]
        [SerializeField] private Projectile projectilePrefab;
        [SerializeField] private Transform projectilePoolParent;
        [SerializeField] private int projectilePoolDefault = 32;
        [SerializeField] private int projectilePoolMax = 256;

        private GenericPool<Projectile> projectilePool;

        private void Awake()
        {
            if (projectilePrefab == null)
            {
                Debug.LogError("[LevelInstaller] projectilePrefab atanmamış.");
                return;
            }

            projectilePool = new GenericPool<Projectile>(
                prefab: projectilePrefab,
                parent: projectilePoolParent != null ? projectilePoolParent : transform,
                defaultCapacity: projectilePoolDefault,
                maxSize: projectilePoolMax);

            if (playerWeaponHolder != null)
                playerWeaponHolder.Configure(projectilePool);
            else
                Debug.LogError("[LevelInstaller] playerWeaponHolder atanmamış.");

            // WaveTracker → GameStateMachine (Observer)
            // WaveSpawner → WaveTracker bağlantısı WaveTracker.waveSpawner field'ından geliyor.
            if (waveTracker != null && gameStateMachine != null)
                waveTracker.WaveCompleted += gameStateMachine.OnWaveCompleted;
            else
                Debug.LogWarning("[LevelInstaller] waveTracker veya gameStateMachine atanmamış.");
        }
    }
}
