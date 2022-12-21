using UnityEngine.UI;
using UnityEngine;
using System;

public class SettingFloatParser : SettingsParser
{
    [SerializeField] private Slider _slider;

    protected override void ValueLoded(object value)
    {
        _slider.value = (float)value;
    }

    public override object ParseLine(string value)
    {
        return float.Parse(value);
    }

    public override string SaveObject()
    {
        return Math.Round(_slider.value, 2).ToString();
    }
}
