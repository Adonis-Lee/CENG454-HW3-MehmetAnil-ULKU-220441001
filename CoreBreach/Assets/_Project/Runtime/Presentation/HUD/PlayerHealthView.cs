using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CoreBreach.Domain.Player;
using CoreBreach.Domain.Combat;

namespace CoreBreach.Presentation.HUD
{
    /// <summary>
    /// Observer subscriber: PlayerHealth.Damaged → slider + text güncelle.
    /// OnEnable/OnDisable lifecycle simetrisi.
    /// </summary>
    public class PlayerHealthView : MonoBehaviour
    {
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private Slider hpSlider;
        [SerializeField] private TextMeshProUGUI hpText;

        private void OnEnable()
        {
            if (playerHealth != null)
                playerHealth.Damaged += OnDamaged;
        }

        private void OnDisable()
        {
            if (playerHealth != null)
                playerHealth.Damaged -= OnDamaged;
        }

        private void Start() => Refresh();

        private void OnDamaged(DamageInfo info) => Refresh();

        private void Refresh()
        {
            if (playerHealth == null) return;
            float ratio = (float)playerHealth.CurrentHp / playerHealth.MaxHp;
            if (hpSlider != null) hpSlider.value = ratio;
            if (hpText != null) hpText.text = $"HP: {playerHealth.CurrentHp}/{playerHealth.MaxHp}";
        }
    }
}
