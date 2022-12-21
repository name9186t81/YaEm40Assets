using UnityEngine;

public class LaserPointer : MonoBehaviour
{
    [SerializeField] private float _maxRange;
    [SerializeField] private LayerMask _mask;
    [SerializeField] private LineRenderer _renderer;
    [SerializeField] private Transform _attached;

    private void OnValidate()
    {
        _renderer.positionCount = 2;
    }
    private void Update()
    {
        var raycast = Physics2D.Raycast(_attached.position, _attached.up, _maxRange, _mask);
        _renderer.SetPosition(0, _attached.position);

        if (raycast)
        {
            _renderer.SetPosition(1, raycast.point);
        }
        else
        {
            _renderer.SetPosition(1, _attached.up * _maxRange + _attached.position);
        }
    }
}
