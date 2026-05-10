using UnityEngine;

namespace CoreBreach.Domain.Enemies
{
    /// <summary>
    /// Strategy ailesi: düşmanın hedefe doğru nasıl hareket ettiği.
    /// Concrete impl: DirectMove, ChasePlayerMove.
    /// </summary>
    public interface IMovementStrategy
    {
        void Step(Enemy enemy, Vector2 target, float deltaTime);
    }
}
