using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] private int _expiredPenalty = 50;

    public int TotalScore { get; private set; }
    public int OrdersCompleted { get; private set; }
    public int OrdersExpired { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void OnEnable()
    {
        EventBus.Subscribe<OrderCompletedEvent>(OnOrderCompleted);
        EventBus.Subscribe<OrderExpiredEvent>(OnOrderExpired);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<OrderCompletedEvent>(OnOrderCompleted);
        EventBus.Unsubscribe<OrderExpiredEvent>(OnOrderExpired);
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public void ResetScore()
    {
        TotalScore = 0;
        OrdersCompleted = 0;
        OrdersExpired = 0;
        EventBus.Raise(new ScoreChangedEvent { TotalScore = TotalScore, Delta = 0 });
    }

    private void OnOrderCompleted(OrderCompletedEvent e)
    {
        TotalScore += e.Score;
        OrdersCompleted++;
        EventBus.Raise(new ScoreChangedEvent { TotalScore = TotalScore, Delta = e.Score });
    }

    private void OnOrderExpired(OrderExpiredEvent e)
    {
        TotalScore -= _expiredPenalty;
        OrdersExpired++;
        EventBus.Raise(new ScoreChangedEvent { TotalScore = TotalScore, Delta = -_expiredPenalty });
    }
}
