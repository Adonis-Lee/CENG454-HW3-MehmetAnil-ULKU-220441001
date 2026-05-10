using UnityEngine;

namespace CoreBreach.Domain.Enemies
{
    /// <summary>
    /// Strategy ailesi: düşmanın hangi noktayı hedeflediği.
    /// Concrete impl: CoreTarget, PlayerTarget.
    /// </summary>
    public interface ITargetingStrategy
    {
        Vector2 Pick(Enemy enemy);
    }
}
