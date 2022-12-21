using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettingsSaver : MonoBehaviour
{
    [SerializeField] private UnitAllower[] _toggles;

    public void Save()
    {
        (string, bool)[] values = new (string, bool)[_toggles.Length];

        for (int i = 0, length = _toggles.Length; i < length; i++)
        {
            values[i] = (_toggles[i].Name, _toggles[i].isOn);
        }

        if (ServiceLocator.TryGetService<GameSettingsContainer>(out var container))
        {
            container.UpdateContainer(new GameSettingsContainer(values));
        }
        else
        {
            ServiceLocator.AddService<GameSettingsContainer>(new GameSettingsContainer(values));
        }
    }
}
