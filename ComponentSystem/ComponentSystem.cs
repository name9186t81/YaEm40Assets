using System;
using System.Collections.Generic;

public class ComponentSystem<TOwner> : IDisposable
{
    private Dictionary<Type, object> _components = new Dictionary<Type, object>();
    private TOwner _owner;
    public event Action<Type, object> OnComponentAdded;

    public ComponentSystem(TOwner owner)
    {
        _owner = owner;
    }
    public void AddComponent<T>(T instance) where T : IComponent<TOwner>
    {
        if (TryToGetComponent<T>(out _))
        {
            if (instance.ComponentType == ComponentType.Multiple)
            {
                (_components[typeof(T)] as ComponentComposite<TOwner, T>).AddComponent(instance);
                OnComponentAdded?.Invoke(typeof(T), instance);
            }
        }
        else
        {
            _components.Add(typeof(T), instance);
            OnComponentAdded?.Invoke(typeof(T), instance);
        }
        instance.Init(_owner);
    }

    public T GetOrCreateComponent<T>() where T : IComponent<TOwner>
    {
        if (TryToGetComponent<T>(out var result))
        {
            return result;
        }

        var obj = (T)Activator.CreateInstance(typeof(T));
        AddComponent(obj);
        return obj;
    }

    public void AddComponent(Type type, IComponent<TOwner> instance)
    {
        if (_components.TryGetValue(type, out _))
        {
            if (instance.ComponentType == ComponentType.Multiple)
            {
                (_components[type] as ComponentComposite<TOwner, IComponent<TOwner>>).AddComponent(instance);
                OnComponentAdded?.Invoke(type, instance);
            }
        }
        else
        {
            _components.Add(type, instance);
            OnComponentAdded?.Invoke(type, instance);
        }
        instance.Init(_owner);
    }

    public bool RemoveComponent<T>()
    {
        return _components.Remove(typeof(T));
    }
    public bool TryToGetComponent<T>(out T value)
    {
        if (_components.TryGetValue(typeof(T), out var val))
        {
            value = (T)val;
            return true;
        }
        value = default;
        return false;
    }

    public void Dispose()
    {
        _components.Clear();
    }
}