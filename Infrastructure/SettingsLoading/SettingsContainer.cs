using System;
using System.Collections.Generic;

public class SettingsContainer : IService
{
    private Dictionary<string, object> _values = new Dictionary<string, object>();
    public event Action OnValuesUpdate;

    public void Append(string name, object value)
    {
        _values.Add(name, value);
    }

    public bool TryGetSetting(string name, out object setting)
    {
        return _values.TryGetValue(name, out setting);
    }

    public void UpdateContainer(SettingsContainer newContainer)
    {
        _values = newContainer._values;
        OnValuesUpdate?.Invoke();
    }

    public object GetSetting(string name)
    {
        if (TryGetSetting(name, out var obj))
        {
            return obj;
        }
        throw new System.Exception($"Setting with a name {name} is not found");
    }
}