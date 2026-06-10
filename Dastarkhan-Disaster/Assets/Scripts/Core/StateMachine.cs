using System;

public interface IState
{
    void Enter();
    void Tick(float deltaTime);
    void Exit();
}

public class StateMachine
{
    private IState _current;
    public IState Current => _current;
    public event Action<IState, IState> OnStateChanged;

    public void ChangeState(IState next)
    {
        if (_current == next) return;
        var prev = _current;
        _current?.Exit();
        _current = next;
        _current?.Enter();
        OnStateChanged?.Invoke(prev, _current);
    }

    public void Tick(float deltaTime) => _current?.Tick(deltaTime);
}
