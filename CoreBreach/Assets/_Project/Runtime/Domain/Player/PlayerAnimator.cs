using UnityEngine;

namespace CoreBreach.Domain.Player
{
    /// <summary>
    /// Oyuncu animasyonlarını hareket yönüne göre yönetir.
    /// Animator Controller'da "Speed" float parametresi kullanır.
    /// Yatay harekette SpriteRenderer.flipX ile yön çevirme.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerAnimator : MonoBehaviour
    {
        private static readonly int SpeedHash = Animator.StringToHash("Speed");

        private Animator animator;
        private SpriteRenderer sr;
        private Rigidbody2D rb;

        private void Awake()
        {
            animator    = GetComponent<Animator>();
            sr          = GetComponent<SpriteRenderer>();
            rb          = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            Vector2 vel = rb != null ? rb.linearVelocity : Vector2.zero;
            float speed = vel.magnitude;

            animator.SetFloat(SpeedHash, speed);

            // Yatay hareket varsa sprite'ı çevir
            if (Mathf.Abs(vel.x) > 0.1f)
                sr.flipX = vel.x < 0;
        }
    }
}
