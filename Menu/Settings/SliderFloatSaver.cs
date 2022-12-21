using UnityEngine;
using UnityEngine.UI;

public class SliderFloatSaver : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private SettingFloatParser _parser;

    private void Start()
    {
        _slider.onValueChanged.AddListener(Change);
    }

    private void Change(float value)
    {
        _parser.Object = value;
    }
}
