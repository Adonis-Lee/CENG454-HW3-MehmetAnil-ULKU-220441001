using System;
using UnityEngine;
using UnityEngine.UI;
using CoreBreach.Domain.CoreDomain;

namespace CoreBreach.Domain.GameState
{
    /// <summary>
    /// Observer subscriber: Core.CoreDestroyed → Lose, WaveTracker.WaveCompleted(5) → Win.
    /// GameOver / GameWon event'leri → UI panelleri (Observer chain).
    /// </summary>
    public class GameStateMachine : MonoBehaviour
    {
        [SerializeField] private Core core;
        [SerializeField] private int totalWaves = 5;

        [Header("UI Panels")]
        [SerializeField] private GameObject winPanel;
        [SerializeField] private GameObject losePanel;

        public enum State { Playing, Won, Lost }
        public State CurrentState { get; private set; } = State.Playing;

        // Observer events → WaveView veya diğer UI bileşenleri
        public event Action GameWon;
        public event Action GameLost;

        private void OnEnable()
        {
            if (core != null)
                core.CoreDestroyed += OnCoreDestroyed;
        }

        private void OnDisable()
        {
            if (core != null)
                core.CoreDestroyed -= OnCoreDestroyed;
        }

        private void Start()
        {
            if (winPanel  != null) winPanel.SetActive(false);
            if (losePanel != null) losePanel.SetActive(false);
        }

        public void OnWaveCompleted(int waveIndex)
        {
            if (waveIndex >= totalWaves)
                TriggerWin();
        }

        private void OnCoreDestroyed()
        {
            if (CurrentState != State.Playing) return;
            CurrentState = State.Lost;
            Debug.Log("[GameStateMachine] LOSE — Core destroyed.");
            if (losePanel != null) losePanel.SetActive(true);
            GameLost?.Invoke();
        }

        private void TriggerWin()
        {
            if (CurrentState != State.Playing) return;
            CurrentState = State.Won;
            Debug.Log("[GameStateMachine] WIN — All waves cleared.");
            if (winPanel != null) winPanel.SetActive(true);
            GameWon?.Invoke();
        }

        // Inspector'daki butonlara atanabilir
        public void RestartGame()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
    }
}
