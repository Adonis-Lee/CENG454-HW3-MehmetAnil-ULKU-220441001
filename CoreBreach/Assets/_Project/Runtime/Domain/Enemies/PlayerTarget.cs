using UnityEngine;

namespace CoreBreach.Domain.Enemies
{
    /// <summary>
    /// ITargetingStrategy: her zaman Player'ı hedefle.
    /// Hunter kullanır.
    /// </summary>
    [CreateAssetMenu(menuName = "CoreBreach/Strategies/PlayerTarget")]
    public class PlayerTarget : TargetingStrategySO
    {
        private Transform playerTransform;

        public void SetPlayer(Transform player) => playerTransform = player;

        public override Vector2 Pick(Enemy enemy)
        {
            if (playerTransform != null) return playerTransform.position;
            // Fallback: düşmanın mevcut pozisyonu (durur)
            return enemy.transform.position;
        }
    }
}
