using UnityEngine;
using UnityEngine.UI;

public class SwitchSaver : SettingsParser
{
    [SerializeField] private Toggle _toggle;

    protected override void ValueLoded(object value)
    {
        _toggle.isOn = (bool)value;
    }

    public override object ParseLine(string value)
    {
        return bool.Parse(value);
    }

    public override string SaveObject()
    {
        return _toggle.isOn.ToString();
    }
}
