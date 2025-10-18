using Unity.VisualScripting;
public interface IState
{
    void Enter();
    void Tick();
    void Exit();
}
public class StateMachine
{
    private IState _currentState;
    public void ChangeState(IState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    public void Tick()
    {
        _currentState?.Tick();
    }
}
