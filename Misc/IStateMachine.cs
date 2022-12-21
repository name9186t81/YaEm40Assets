public interface IStateMachine<TState, TOwner> where TState : IState<TOwner>
{
    void Init(TState[] states, TOwner owner);
    void Execute();
    IState<TOwner> CurrentState { get; }
}
