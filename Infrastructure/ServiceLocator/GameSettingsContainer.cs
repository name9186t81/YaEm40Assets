using System.Collections.Generic;

public class GameSettingsContainer : IService
{
    private Dictionary<string, bool> _enabledUnits = new Dictionary<string, bool>();

    public GameSettingsContainer((string, bool)[] units)
    {
        for (int i = 0, length = units.Length; i < length; i++)
        {
            _enabledUnits.Add(units[i].Item1, units[i].Item2);
        }
    }

    public bool TryGetBool(string name, out bool value)
    {
        if (_enabledUnits.TryGetValue(name, out value))
        {
            return true;
        }
        value = false;
        return false;
    }
    public void UpdateContainer(GameSettingsContainer container)
    {
        _enabledUnits = container._enabledUnits;
    }
}
