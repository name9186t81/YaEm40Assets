using UnityEngine;
using UnityEngine.UI;

public class TextColorRGBLerp : MonoBehaviour
{
    [SerializeField] private Text _text;
    private float _currentT;

    private void Update()
    {
        _text.color = Color.HSVToRGB(_currentT, 1, 1);
        _currentT += 0.05f;
        if (_currentT > 1f)
        {
            _currentT = 0;
        }
    }
}
