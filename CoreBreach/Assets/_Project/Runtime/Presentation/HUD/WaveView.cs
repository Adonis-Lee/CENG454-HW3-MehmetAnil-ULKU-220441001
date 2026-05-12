using UnityEngine;
using TMPro;
using CoreBreach.Domain.Waves;

namespace CoreBreach.Presentation.HUD
{
    /// <summary>
    /// Observer subscriber: WaveSpawner.WaveStarted → dalga sayısını güncelle.
    /// WaveTracker.WaveCompleted → tüm dalgalar bitti mesajı.
    /// </summary>
    public class WaveView : MonoBehaviour
    {
        [SerializeField] private WaveSpawner waveSpawner;
        [SerializeField] private WaveTracker waveTracker;
        [SerializeField] private TextMeshProUGUI waveText;
        [SerializeField] private int totalWaves = 5;

        private void OnEnable()
        {
            if (waveSpawner != null)
                waveSpawner.WaveStarted += OnWaveStarted;
            if (waveTracker != null)
                waveTracker.WaveCompleted += OnWaveCompleted;
        }

        private void OnDisable()
        {
            if (waveSpawner != null)
                waveSpawner.WaveStarted -= OnWaveStarted;
            if (waveTracker != null)
                waveTracker.WaveCompleted -= OnWaveCompleted;
        }

        private void OnWaveStarted(int wave, int count)
        {
            if (waveText != null)
                waveText.text = $"Wave {wave} / {totalWaves}";
        }

        private void OnWaveCompleted(int wave)
        {
            if (wave >= totalWaves && waveText != null)
                waveText.text = "All Waves Cleared!";
        }
    }
}
