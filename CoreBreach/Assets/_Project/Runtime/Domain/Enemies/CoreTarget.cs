using UnityEngine;

namespace CoreBreach.Domain.Enemies
{
    /// <summary>
    /// ITargetingStrategy: her zaman Core objesini hedefle.
    /// Grunt ve Tank kullanır.
    /// </summary>
    [CreateAssetMenu(menuName = "CoreBreach/Strategies/CoreTarget")]
    public class CoreTarget : TargetingStrategySO
    {
        [SerializeField] private Transform coreTransform;

        public void SetCore(Transform core) => coreTransform = core;

        public override Vector2 Pick(Enemy enemy)
        {
            if (coreTransform != null) return coreTransform.position;
            // Fallback: sahne ortası
            return Vector2.zero;
        }
    }
}
