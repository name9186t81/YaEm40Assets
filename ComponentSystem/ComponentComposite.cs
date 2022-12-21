using System.Collections.Generic;

public class ComponentComposite<TOwner, TComponent> : IComponent<TOwner>
    where TComponent : IComponent<TOwner>
{
    public ComponentType ComponentType => ComponentType.Singletone;
    private List<TComponent> _components = new List<TComponent>();
    public void Init(TOwner owner)
    {
    }
    public void AddComponent(TComponent component)
    {
        _components.Add(component);
    }

    public TComponent[] GetComponents => _components.ToArray();
}