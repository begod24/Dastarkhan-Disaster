public struct OrderSpawnedEvent
{
    public ActiveOrder Order;
}

public struct OrderTimerUpdatedEvent
{
    public ActiveOrder Order;
}

public struct OrderCompletedEvent
{
    public ActiveOrder Order;
    public int Score;
}

public struct OrderExpiredEvent
{
    public ActiveOrder Order;
}

public struct ScoreChangedEvent
{
    public int TotalScore;
    public int Delta;
}

public struct SessionPhaseChangedEvent
{
    public SessionPhase Phase;
}

public struct SessionTimerUpdatedEvent
{
    public float TimeRemaining;
    public float TotalDuration;
    public SessionPhase Phase;
}

public struct SessionEndedEvent
{
    public int FinalScore;
    public int OrdersCompleted;
    public int OrdersExpired;
    public bool Failed;
}

public enum SessionPhase
{
    Idle,
    Prep,
    Rush,
    Crisis,
    FinalPush,
    Ended
}
