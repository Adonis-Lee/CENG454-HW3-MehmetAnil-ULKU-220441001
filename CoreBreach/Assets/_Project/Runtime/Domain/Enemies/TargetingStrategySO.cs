using UnityEngine;

namespace CoreBreach.Domain.Enemies
{
    /// <summary>
    /// ITargetingStrategy ScriptableObject base.
    /// CoreTarget ve PlayerTarget buradan türer.
    /// </summary>
    public abstract class TargetingStrategySO : ScriptableObject, ITargetingStrategy
    {
        public abstract Vector2 Pick(Enemy enemy);
    }
}
