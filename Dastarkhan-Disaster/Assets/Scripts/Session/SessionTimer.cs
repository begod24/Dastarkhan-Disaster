using UnityEngine;

public class SessionTimer : MonoBehaviour
{
    public static SessionTimer Instance { get; private set; }

    [SerializeField] private float _prepDuration = 20f;
    [SerializeField] private float _rushDuration = 130f;
    [SerializeField] private float _crisisDuration = 60f;
    [SerializeField] private float _finalPushDuration = 30f;
    [SerializeField] private bool _autoStart = true;

    private float _totalDuration;
    private float _elapsed;
    private SessionPhase _currentPhase = SessionPhase.Idle;
    private bool _running;

    public SessionPhase CurrentPhase => _currentPhase;
    public float TimeRemaining => Mathf.Max(0f, _totalDuration - _elapsed);
    public float TotalDuration => _totalDuration;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        _totalDuration = _prepDuration + _rushDuration + _crisisDuration + _finalPushDuration;
    }

    private void Start()
    {
        if (_autoStart) StartSession();
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public void StartSession()
    {
        _elapsed = 0f;
        _running = true;
        ChangePhase(SessionPhase.Prep);
    }

    public void EndSession(bool failed)
    {
        _running = false;
        ChangePhase(SessionPhase.Ended);

        int score = ScoreManager.Instance != null ? ScoreManager.Instance.TotalScore : 0;
        int completed = ScoreManager.Instance != null ? ScoreManager.Instance.OrdersCompleted : 0;
        int expired = ScoreManager.Instance != null ? ScoreManager.Instance.OrdersExpired : 0;

        EventBus.Raise(new SessionEndedEvent
        {
            FinalScore = score,
            OrdersCompleted = completed,
            OrdersExpired = expired,
            Failed = failed
        });
    }

    private void Update()
    {
        if (!_running) return;

        _elapsed += Time.deltaTime;
        EvaluatePhase();

        EventBus.Raise(new SessionTimerUpdatedEvent
        {
            TimeRemaining = TimeRemaining,
            TotalDuration = _totalDuration,
            Phase = _currentPhase
        });

        if (_elapsed >= _totalDuration) EndSession(false);
    }

    private void EvaluatePhase()
    {
        SessionPhase next;
        if (_elapsed < _prepDuration) next = SessionPhase.Prep;
        else if (_elapsed < _prepDuration + _rushDuration) next = SessionPhase.Rush;
        else if (_elapsed < _prepDuration + _rushDuration + _crisisDuration) next = SessionPhase.Crisis;
        else next = SessionPhase.FinalPush;

        if (next != _currentPhase) ChangePhase(next);
    }

    private void ChangePhase(SessionPhase phase)
    {
        _currentPhase = phase;
        EventBus.Raise(new SessionPhaseChangedEvent { Phase = phase });
    }
}
