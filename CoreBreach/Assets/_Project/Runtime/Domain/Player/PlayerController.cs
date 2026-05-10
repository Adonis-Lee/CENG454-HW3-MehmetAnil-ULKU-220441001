using UnityEngine;
using UnityEngine.InputSystem;

namespace CoreBreach.Domain.Player
{
    /// <summary>
    /// Aktif defender input handler.
    /// WASD → hareket, mouse pozisyonu → aim, sol-tık → Fire.
    /// Yeni Input System (Keyboard/Mouse.current) üzerinden okur.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(WeaponHolder))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 6f;

        private Rigidbody2D body;
        private WeaponHolder weapon;
        private Camera mainCamera;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            weapon = GetComponent<WeaponHolder>();
            mainCamera = Camera.main;
        }

        private void Update()
        {
            HandleAimAndFire();
        }

        private void FixedUpdate()
        {
            HandleMovement();
        }

        private void HandleMovement()
        {
            Vector2 input = Vector2.zero;
            Keyboard kb = Keyboard.current;
            if (kb == null) return;

            if (kb.wKey.isPressed) input.y += 1f;
            if (kb.sKey.isPressed) input.y -= 1f;
            if (kb.aKey.isPressed) input.x -= 1f;
            if (kb.dKey.isPressed) input.x += 1f;

            body.linearVelocity = input.normalized * moveSpeed;
        }

        private void HandleAimAndFire()
        {
            Mouse mouse = Mouse.current;
            if (mouse == null || mainCamera == null) return;

            Vector2 screenPoint = mouse.position.ReadValue();
            Vector3 world = mainCamera.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, -mainCamera.transform.position.z));
            Vector2 aimDirection = ((Vector2)world - (Vector2)transform.position).normalized;

            if (mouse.leftButton.isPressed)
            {
                weapon.TryFire(aimDirection);
            }
        }
    }
}
