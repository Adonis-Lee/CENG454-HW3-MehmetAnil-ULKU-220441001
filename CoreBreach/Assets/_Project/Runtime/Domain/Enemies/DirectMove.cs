using UnityEngine;

namespace CoreBreach.Domain.Enemies
{
    /// <summary>
    /// IMovementStrategy: hedefe düz çizgide hareket et.
    /// Grunt ve Tank tipi düşmanlar kullanır.
    /// </summary>
    [CreateAssetMenu(menuName = "CoreBreach/Strategies/DirectMove")]
    public class DirectMove : MovementStrategySO
    {
        public override void Step(Enemy enemy, Vector2 target, float deltaTime)
        {
            Vector2 pos = enemy.transform.position;
            Vector2 dir = (target - pos).normalized;
            enemy.transform.position = pos + dir * enemy.Speed * deltaTime;
        }
    }
}
