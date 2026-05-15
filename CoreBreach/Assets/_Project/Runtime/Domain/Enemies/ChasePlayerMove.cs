using UnityEngine;

namespace CoreBreach.Domain.Enemies
{
    /// <summary>
    /// IMovementStrategy: oyuncuya doğru sürekli koval.
    /// Hunter tipi düşman kullanır. Hedef olarak PlayerTarget ile çalışır.
    /// </summary>
    [CreateAssetMenu(menuName = "CoreBreach/Strategies/ChasePlayerMove")]
    public class ChasePlayerMove : MovementStrategySO
    {
        public override void Step(Enemy enemy, Vector2 target, float deltaTime)
        {
            Vector2 pos = enemy.Rb.position;
            Vector2 dir = (target - pos).normalized;
            enemy.Rb.MovePosition(pos + dir * enemy.Speed * deltaTime);
        }
    }
}
