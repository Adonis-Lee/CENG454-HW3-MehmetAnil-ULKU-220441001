using UnityEngine;

namespace CoreBreach.Domain.Enemies
{
    /// <summary>
    /// IMovementStrategy ScriptableObject base.
    /// DirectMove ve ChasePlayerMove buradan türer.
    /// EnemyConfig type-safe referans edebilir.
    /// </summary>
    public abstract class MovementStrategySO : ScriptableObject, IMovementStrategy
    {
        public abstract void Step(Enemy enemy, Vector2 target, float deltaTime);
    }
}
