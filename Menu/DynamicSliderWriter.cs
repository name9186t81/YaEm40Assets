using UnityEngine;
using System;
using UnityEngine.UI;

public class DynamicSliderWriter : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Text _text;
    [SerializeField] private WriteMode _writeMode;

    private void Update()
    {
        switch (_writeMode)
        {
            case WriteMode.Percentage:
                {
                    _text.text = deltaValue * 100 + "%";
                    break;
                }
            case WriteMode.Double:
                {
                    _text.text = deltaValue.ToString();
                    break;
                }
        }
    }

    private enum WriteMode
    {
        Percentage,
        Double
    }

    private double deltaValue => Math.Round(_slider.value / _slider.maxValue, 2);
}
