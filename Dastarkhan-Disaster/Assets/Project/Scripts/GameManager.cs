using UnityEngine;
using DastarkhanDisaster.Core;

namespace DastarkhanDisaster.Gameplay
{
    public enum GameState { Boot, Playing, Paused, RoundEnd }

    /// <summary>
    /// Top-level game state holder. Persists across scenes.
    /// Registers itself in the ServiceLocator for cross-system access.
    /// </summary>
    public class GameManager : PersistentSingleton<GameManager>
    {
        public GameState State { get; private set; } = GameState.Boot;

        protected override void Awake()
        {
            base.Awake();
            ServiceLocator.Register(this);
        }

        private void Start() => SetState(GameState.Playing);

        public void SetState(GameState next)
        {
            State = next;
            Time.timeScale = next == GameState.Paused ? 0f : 1f;
        }

        public void TogglePause() =>
            SetState(State == GameState.Paused ? GameState.Playing : GameState.Paused);
    }
}
