using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoundEndUI : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject _panel;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _completedText;
    [SerializeField] private TextMeshProUGUI _expiredText;
    [SerializeField] private TextMeshProUGUI _ratingText;

    [Header("Stars")]
    [SerializeField] private Image _star1;
    [SerializeField] private Image _star2;
    [SerializeField] private Image _star3;
    [SerializeField] private Color _starOnColor = new Color(1f, 0.85f, 0.3f);
    [SerializeField] private Color _starOffColor = new Color(0.3f, 0.25f, 0.2f);

    [Header("Buttons")]
    [SerializeField] private Button _retryButton;
    [SerializeField] private Button _mainMenuButton;

    [Header("Tuning")]
    [SerializeField] private float _threeStarRatio = 0.85f;
    [SerializeField] private float _twoStarRatio = 0.5f;
    [SerializeField] private string _victoryTitle = "FEAST COMPLETE";
    [SerializeField] private string _failTitle = "DASTARKHAN DISASTER";

    private void Awake()
    {
        if (_panel != null) _panel.SetActive(false);
    }

    private void OnEnable()
    {
        EventBus.Subscribe<SessionEndedEvent>(OnSessionEnded);
        if (_retryButton != null) _retryButton.onClick.AddListener(OnRetry);
        if (_mainMenuButton != null) _mainMenuButton.onClick.AddListener(OnMainMenu);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<SessionEndedEvent>(OnSessionEnded);
        if (_retryButton != null) _retryButton.onClick.RemoveListener(OnRetry);
        if (_mainMenuButton != null) _mainMenuButton.onClick.RemoveListener(OnMainMenu);
    }

private void OnSessionEnded(SessionEndedEvent e)
{
    Debug.Log($"[RoundEndUI] Session ended! Failed={e.Failed}, Score={e.FinalScore}, Completed={e.OrdersCompleted}");
    Show(e);
    Time.timeScale = 0f;
}

    private void Show(SessionEndedEvent e)
    {
        if (_panel != null) _panel.SetActive(true);

        int stars = CalculateStars(e);

        if (_titleText != null) _titleText.text = e.Failed ? _failTitle : _victoryTitle;
        if (_scoreText != null) _scoreText.text = $"Score: {e.FinalScore}";
        if (_completedText != null) _completedText.text = $"Orders Completed: {e.OrdersCompleted}";
        if (_expiredText != null) _expiredText.text = $"Orders Expired: {e.OrdersExpired}";
        if (_ratingText != null) _ratingText.text = GetRatingText(stars);

        SetStar(_star1, stars >= 1);
        SetStar(_star2, stars >= 2);
        SetStar(_star3, stars >= 3);
    }

    private int CalculateStars(SessionEndedEvent e)
    {
        int total = e.OrdersCompleted + e.OrdersExpired;
        if (total == 0 || e.OrdersCompleted == 0) return 0;

        float ratio = (float)e.OrdersCompleted / total;
        if (ratio >= _threeStarRatio) return 3;
        if (ratio >= _twoStarRatio) return 2;
        return 1;
    }

    private string GetRatingText(int stars)
    {
        switch (stars)
        {
            case 3: return "Legendary Host";
            case 2: return "Welcome Guest";
            case 1: return "Acceptable Effort";
            default: return "Try Again";
        }
    }

    private void SetStar(Image star, bool on)
    {
        if (star == null) return;
        star.color = on ? _starOnColor : _starOffColor;
    }

    private void OnRetry()
    {
        if (SceneLoader.Instance != null) SceneLoader.Instance.ReloadCurrent();
    }

    private void OnMainMenu()
    {
        if (SceneLoader.Instance != null) SceneLoader.Instance.LoadMainMenu();
    }
}
