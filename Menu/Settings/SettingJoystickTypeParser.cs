using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingJoystickTypeParser : SettingsParser
{
    [SerializeField] private Dropdown _dropdown;

    //todo: update this once from menu loading?
    private void OnEnable()
    {
        if (ServiceLocator.TryGetService<SettingsContainer>(out var container) && container.TryGetSetting(Line, out var obj))
        {
            string name = obj.ToString();
            int index = 0;
            for (int i = 0, length = _dropdown.options.Count; i < length; i++)
            {
                if (_dropdown.options[i].text == name)
                {
                    index = i;
                    break;
                }
            }
            _dropdown.value = index;
        }
    }

    public override object ParseLine(string value)
    {
        switch(value)
        {
            case ("Fixed"):
                {
                    return JoystickType.Fixed;
                }
            case ("Dynamic"):
                {
                    return JoystickType.Dynamic;
                }
            case ("Floating"):
                {
                    return JoystickType.Floating;
                }
            default:
                {
                    throw new ArgumentException();
                }
        }
    }

    public override string SaveObject()
    {
        return DropDownOption;
    }

    private string DropDownOption => _dropdown.options[_dropdown.value].text;
}