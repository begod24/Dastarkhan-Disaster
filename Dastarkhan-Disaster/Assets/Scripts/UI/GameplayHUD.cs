using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameplayHUD : MonoBehaviour
{
    [SerializeField] private Transform _orderCardsRoot;
    [SerializeField] private OrderCardUI _orderCardPrefab;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _phaseText;

    private readonly Dictionary<int, OrderCardUI> _cards = new();

    private void OnEnable()
    {
        EventBus.Subscribe<OrderSpawnedEvent>(OnOrderSpawned);
        EventBus.Subscribe<OrderTimerUpdatedEvent>(OnOrderTimerUpdated);
        EventBus.Subscribe<OrderCompletedEvent>(OnOrderCompleted);
        EventBus.Subscribe<OrderExpiredEvent>(OnOrderExpired);
        EventBus.Subscribe<ScoreChangedEvent>(OnScoreChanged);
        EventBus.Subscribe<SessionTimerUpdatedEvent>(OnSessionTimer);
        EventBus.Subscribe<SessionPhaseChangedEvent>(OnPhaseChanged);

        RefreshScore(0);
        RefreshTimer(0f);
        RefreshPhase(SessionPhase.Idle);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<OrderSpawnedEvent>(OnOrderSpawned);
        EventBus.Unsubscribe<OrderTimerUpdatedEvent>(OnOrderTimerUpdated);
        EventBus.Unsubscribe<OrderCompletedEvent>(OnOrderCompleted);
        EventBus.Unsubscribe<OrderExpiredEvent>(OnOrderExpired);
        EventBus.Unsubscribe<ScoreChangedEvent>(OnScoreChanged);
        EventBus.Unsubscribe<SessionTimerUpdatedEvent>(OnSessionTimer);
        EventBus.Unsubscribe<SessionPhaseChangedEvent>(OnPhaseChanged);
    }

    private void OnOrderSpawned(OrderSpawnedEvent e)
    {
        if (_orderCardPrefab == null || _orderCardsRoot == null) return;
        var card = Instantiate(_orderCardPrefab, _orderCardsRoot);
        card.Bind(e.Order);
        _cards[e.Order.Id] = card;
    }

    private void OnOrderTimerUpdated(OrderTimerUpdatedEvent e)
    {
        if (_cards.TryGetValue(e.Order.Id, out var card)) card.UpdateTime(e.Order);
    }

    private void OnOrderCompleted(OrderCompletedEvent e) => RemoveCard(e.Order.Id);
    private void OnOrderExpired(OrderExpiredEvent e) => RemoveCard(e.Order.Id);

    private void RemoveCard(int orderId)
    {
        if (_cards.TryGetValue(orderId, out var card))
        {
            _cards.Remove(orderId);
            if (card != null) Destroy(card.gameObject);
        }
    }

    private void OnScoreChanged(ScoreChangedEvent e) => RefreshScore(e.TotalScore);

    private void OnSessionTimer(SessionTimerUpdatedEvent e) => RefreshTimer(e.TimeRemaining);

    private void OnPhaseChanged(SessionPhaseChangedEvent e) => RefreshPhase(e.Phase);

    private void RefreshScore(int score)
    {
        if (_scoreText != null) _scoreText.text = $"Score: {score}";
    }

    private void RefreshTimer(float seconds)
    {
        if (_timerText == null) return;
        int mins = Mathf.FloorToInt(seconds / 60f);
        int secs = Mathf.FloorToInt(seconds % 60f);
        _timerText.text = $"{mins:0}:{secs:00}";
    }

    private void RefreshPhase(SessionPhase phase)
    {
        if (_phaseText == null) return;
        _phaseText.text = phase == SessionPhase.Idle ? "" : phase.ToString().ToUpper();
    }
}
