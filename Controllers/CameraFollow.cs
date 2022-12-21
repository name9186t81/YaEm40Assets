using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private CameraUnitAttacher _attacher;
    private Transform _cached;
    private Transform _target;
    public bool Enabled { get; set; } = true;

    private void Awake()
    {
        if (_attacher.Attached != null)
        {
            _target = _attacher.Attached.transform;
        }
        _cached = transform;
        _attacher.OnUnitChange += ChangeTarget;
    }

    private void ChangeTarget(Unit obj)
    {
        _target = obj.transform;
    }

    private void FixedUpdate()
    {
        if(_target == null || !Enabled)
        {
            return;
        }
        Vector3 newPos = new Vector3(_target.position.x, _target.position.y, _cached.position.z);

        _cached.position = Vector3.Lerp(_cached.position, newPos, 0.1f);
    }
}
