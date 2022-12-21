
using System;
using System.Collections.Generic;

public static class ServiceLocator
{
    private static Dictionary<Type, object> _seriveces = new Dictionary<Type, object>();
    
    public static T GetService<T>() where T : IService
    {
        if (TryGetService<T>(out T service))
        {
            return service;
        }
        throw new Exception("Service locator doesnt contain service with type " + typeof(T));
    }
    
    public static void AddService<T>(T instance) where T : IService
    {
        _seriveces.Add(typeof(T), instance);
    }

    public static bool TryGetService<T>(out T service) where T : IService
    {
        if (_seriveces.TryGetValue(typeof(T), out var obj))
        {
            service = (T)obj;
            return true;
        }
        service = default;
        return false;
    }
}