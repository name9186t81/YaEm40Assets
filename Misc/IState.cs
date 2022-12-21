public interface IState<T>
{
    void Init(T owner);
    void Undo();
    void PreExecute();
    void Execute();
}
