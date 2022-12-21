
public interface IComponent<TOwner>
{
    void Init(TOwner owner);
    ComponentType ComponentType { get; }
}