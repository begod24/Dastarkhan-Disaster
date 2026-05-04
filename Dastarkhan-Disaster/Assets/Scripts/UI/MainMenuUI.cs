using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _quitButton;

    private void OnEnable()
    {
        if (_playButton != null) _playButton.onClick.AddListener(OnPlay);
        if (_quitButton != null) _quitButton.onClick.AddListener(OnQuit);
    }

    private void OnDisable()
    {
        if (_playButton != null) _playButton.onClick.RemoveListener(OnPlay);
        if (_quitButton != null) _quitButton.onClick.RemoveListener(OnQuit);
    }

    private void OnPlay()
    {
        if (SceneLoader.Instance != null) SceneLoader.Instance.LoadGameplay();
    }

    private void OnQuit()
    {
        if (SceneLoader.Instance != null) SceneLoader.Instance.QuitGame();
    }
}
