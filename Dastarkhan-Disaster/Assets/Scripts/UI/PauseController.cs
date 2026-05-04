using UnityEngine;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;

    public bool IsPaused { get; private set; }

    private void Start()
    {
        SetPaused(false);
    }

    private void Update()
    {
        bool pausePressed = false;

        var keyboard = Keyboard.current;
        if (keyboard != null && keyboard.escapeKey.wasPressedThisFrame) pausePressed = true;

        var gamepad = Gamepad.current;
        if (gamepad != null && gamepad.startButton.wasPressedThisFrame) pausePressed = true;

        if (pausePressed) Toggle();
    }

    public void Toggle() => SetPaused(!IsPaused);

    public void Resume() => SetPaused(false);

    public void Restart()
    {
        SetPaused(false);
        if (SceneLoader.Instance != null) SceneLoader.Instance.ReloadCurrent();
    }

    public void ReturnToMainMenu()
    {
        SetPaused(false);
        if (SceneLoader.Instance != null) SceneLoader.Instance.LoadMainMenu();
    }

    public void Quit()
    {
        SetPaused(false);
        if (SceneLoader.Instance != null) SceneLoader.Instance.QuitGame();
    }

    private void SetPaused(bool paused)
    {
        IsPaused = paused;
        Time.timeScale = paused ? 0f : 1f;
        if (_pausePanel != null) _pausePanel.SetActive(paused);
    }
}
