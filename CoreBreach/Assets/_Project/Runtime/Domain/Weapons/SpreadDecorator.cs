using UnityEngine;

namespace CoreBreach.Domain.Weapons
{
    /// <summary>
    /// Decorator: 3 mermi ±spreadAngle açıyla atar (uzaysal katmanlama).
    /// Ana mermi + 2 yan mermi.
    /// </summary>
    public class SpreadDecorator : IWeaponBehavior
    {
        private readonly IWeaponBehavior inner;
        private readonly float spreadAngle;

        public SpreadDecorator(IWeaponBehavior inner, float spreadAngle = 15f)
        {
            this.inner = inner;
            this.spreadAngle = spreadAngle;
        }

        public void Fire(FireContext context)
        {
            inner.Fire(context); // merkez
            inner.Fire(context.WithDirection(Rotate(context.Direction, spreadAngle)));
            inner.Fire(context.WithDirection(Rotate(context.Direction, -spreadAngle)));
        }

        private static Vector2 Rotate(Vector2 dir, float degrees)
        {
            float rad = degrees * Mathf.Deg2Rad;
            float cos = Mathf.Cos(rad);
            float sin = Mathf.Sin(rad);
            return new Vector2(cos * dir.x - sin * dir.y, sin * dir.x + cos * dir.y).normalized;
        }
    }
}
