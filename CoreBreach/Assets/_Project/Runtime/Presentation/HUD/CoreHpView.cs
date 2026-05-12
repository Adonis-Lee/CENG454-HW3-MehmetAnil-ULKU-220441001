using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CoreBreach.Domain.CoreDomain;
using CoreBreach.Domain.Combat;

namespace CoreBreach.Presentation.HUD
{
    /// <summary>
    /// Observer subscriber: Core.Damaged event'ine abone olur,
    /// HP bar ve text'i günceller. Core'u tanımaz — sadece event dinler.
    /// OnEnable/OnDisable simetrisi ile Ghost Subscriber önlenir.
    /// </summary>
    public class CoreHpView : MonoBehaviour
    {
        [SerializeField] private Core core;
        [SerializeField] private Slider hpSlider;
        [SerializeField] private TextMeshProUGUI hpText;

        private void OnEnable()
        {
            if (core != null)
                core.Damaged += OnCoreDamaged;
        }

        private void OnDisable()
        {
            if (core != null)
                core.Damaged -= OnCoreDamaged;
        }

        private void Start()
        {
            Refresh();
        }

        private void OnCoreDamaged(DamageInfo info)
        {
            Refresh();
        }

        private void Refresh()
        {
            if (core == null) return;
            float ratio = (float)core.CurrentHp / core.MaxHp;
            if (hpSlider != null) hpSlider.value = ratio;
            if (hpText != null) hpText.text = $"Core: {core.CurrentHp}/{core.MaxHp}";
        }
    }
}
