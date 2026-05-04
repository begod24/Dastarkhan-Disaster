using UnityEngine;

public class BootLoader : MonoBehaviour
{
    [SerializeField] private float _delay = 0.1f;

    private void Start()
    {
        Invoke(nameof(GoToMainMenu), _delay);
    }

    private void GoToMainMenu()
    {
        if (SceneLoader.Instance == null)
        {
            Debug.LogError("[BootLoader] SceneLoader not found. Make sure it lives on a DontDestroyOnLoad GameObject in the Boot scene.");
            return;
        }
        SceneLoader.Instance.LoadMainMenu();
    }
}
