using UnityEngine;

namespace CoreBreach.Domain.Enemies
{
    /// <summary>
    /// Düşman tipi konfigürasyonu (ScriptableObject).
    /// Grunt / Hunter / Tank için ayrı asset instance'ları oluşturulur.
    /// Strategy referansları buradan gelir — Enemy concrete tipi bilmez.
    /// </summary>
    [CreateAssetMenu(menuName = "CoreBreach/EnemyConfig")]
    public class EnemyConfig : ScriptableObject
    {
        [Header("Stats")]
        public int hp = 2;
        public int damage = 1;
        public float speed = 2.5f;
        public float scale = 1f;
        public Color color = Color.gray;

        [Header("Strategies")]
        public MovementStrategySO movementStrategy;
        public TargetingStrategySO targetingStrategy;
    }
}
