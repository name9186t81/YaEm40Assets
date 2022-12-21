using UnityEngine;

public class MonoBehaviour2D : MonoBehaviour
{
    private Transform _cached;
    protected Transform Cached { get
        {
            if(_cached == null)
            {
                _cached = transform;
            }
            return _cached;
        } }
    public Vector2 Position2D
    {
        get
        {
            if (_cached == null)
            {
                _cached = transform;
            }
            return _cached.position;
        }
        set
        {
            if (_cached == null)
            {
                _cached = transform;
            }
            _cached.position = value;
        }
    }
}
