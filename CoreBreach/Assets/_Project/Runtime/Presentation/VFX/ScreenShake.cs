using System.Collections;
using UnityEngine;
using CoreBreach.Domain.CoreDomain;
using CoreBreach.Domain.Combat;

namespace CoreBreach.Presentation.VFX
{
    /// <summary>
    /// Observer subscriber: Core.Damaged → kamera sarsıntısı.
    /// OnEnable/OnDisable ile abone yönetimi.
    /// </summary>
    public class ScreenShake : MonoBehaviour
    {
        [SerializeField] private Core core;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private float duration = 0.15f;
        [SerializeField] private float magnitude = 0.12f;

        private Vector3 originalPos;

        private void Awake()
        {
            if (cameraTransform == null && Camera.main != null)
                cameraTransform = Camera.main.transform;
        }

        private void OnEnable()
        {
            if (core != null) core.Damaged += OnCoreDamaged;
        }

        private void OnDisable()
        {
            if (core != null) core.Damaged -= OnCoreDamaged;
        }

        private void OnCoreDamaged(DamageInfo info)
        {
            StopAllCoroutines();
            StartCoroutine(Shake());
        }

        private IEnumerator Shake()
        {
            if (cameraTransform == null) yield break;
            originalPos = cameraTransform.localPosition;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                float x = Random.Range(-1f, 1f) * magnitude;
                float y = Random.Range(-1f, 1f) * magnitude;
                cameraTransform.localPosition = originalPos + new Vector3(x, y, 0f);
                elapsed += Time.deltaTime;
                yield return null;
            }
            cameraTransform.localPosition = originalPos;
        }
    }
}
