using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizePingPong : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector2 _maxSize;
    private bool _backwards;
    void Update()
    {
        var scale = _target.localScale;
        if (_backwards)
        {
            _target.localScale = Vector2.Lerp(_target.localScale, Vector2.zero, 0.005f);
            if(_target.localScale.x < 0.1f)
            {
                _backwards = false;
            }
        }
        else
        {
            _target.localScale = Vector2.Lerp(_target.localScale, _maxSize, 0.005f);
            if (_target.localScale.x > (_maxSize.x - 0.1f))
            {
                _backwards = true;
            }
        }
    }
}
