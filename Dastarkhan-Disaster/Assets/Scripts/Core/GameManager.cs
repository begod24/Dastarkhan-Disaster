using UnityEngine;

public enum GameState { Boot, Playing, Paused, RoundEnd }

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
