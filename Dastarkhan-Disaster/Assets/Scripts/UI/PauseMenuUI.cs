using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private PauseController _controller;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _quitButton;

    private void OnEnable()
    {
        if (_resumeButton != null) _resumeButton.onClick.AddListener(OnResume);
        if (_restartButton != null) _restartButton.onClick.AddListener(OnRestart);
        if (_mainMenuButton != null) _mainMenuButton.onClick.AddListener(OnMainMenu);
        if (_quitButton != null) _quitButton.onClick.AddListener(OnQuit);
    }

    private void OnDisable()
    {
        if (_resumeButton != null) _resumeButton.onClick.RemoveListener(OnResume);
        if (_restartButton != null) _restartButton.onClick.RemoveListener(OnRestart);
        if (_mainMenuButton != null) _mainMenuButton.onClick.RemoveListener(OnMainMenu);
        if (_quitButton != null) _quitButton.onClick.RemoveListener(OnQuit);
    }

    private void OnResume() { if (_controller != null) _controller.Resume(); }
    private void OnRestart() { if (_controller != null) _controller.Restart(); }
    private void OnMainMenu() { if (_controller != null) _controller.ReturnToMainMenu(); }
    private void OnQuit() { if (_controller != null) _controller.Quit(); }
}
