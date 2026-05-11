using UnityEngine;
using UnityEngine.SceneManagement;
using CoreBreach.Domain.CoreDomain;
using CoreBreach.Domain.Combat;

namespace CoreBreach.Domain.GameState
{
    /// <summary>
    /// Observer subscriber: Core.CoreDestroyed → Lose, WaveTracker.WaveCompleted(5) → Win.
    /// Oyun durumunu yönetir; UI'a event yayar.
    /// </summary>
    public class GameStateMachine : MonoBehaviour
    {
        [SerializeField] private Core core;
        [SerializeField] private int totalWaves = 5;

        public enum State { Playing, Won, Lost }
        public State CurrentState { get; private set; } = State.Playing;

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
            // TODO: lose screen UI — feat/waves-and-state'de
        }

        private void TriggerWin()
        {
            if (CurrentState != State.Playing) return;
            CurrentState = State.Won;
            Debug.Log("[GameStateMachine] WIN — All waves cleared.");
            // TODO: win screen UI — feat/waves-and-state'de
        }
    }
}
