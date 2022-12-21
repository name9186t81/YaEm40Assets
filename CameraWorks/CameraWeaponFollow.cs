using UnityEngine;

public class CameraWeaponFollow : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField, Range(0, 1f)] private float _speed;
    [SerializeField] private CameraUnitAttacher _attacher;
    [SerializeField] private MobileController _controller;
    [SerializeField] private CameraFollow _follow;

    private Weapon _attached;
    private Unit _attachedUnit;

    private void Awake()
    {
        _attacher.OnUnitChange += ChangeUnit;
        _controller.AttackJoystick.OnMove += ChangeDir;
        _controller.AttackJoystick.OnRelease += Relese;
    }

    private void Relese()
    {
        _follow.Enabled = true;
    }

    private void ChangeDir(Vector2 obj)
    {
        if (_attached == null)
        {
            _attachedUnit?.ComponentSystem.TryToGetComponent<Weapon>(out _attached);
            return;
        }

        _follow.Enabled = false;
        Vector2 newPosition = Vector2.Lerp(_camera.position, _attachedUnit.Position2D + obj * _attached.ScopeScale, _speed);
        _camera.position = new Vector3(newPosition.x, newPosition.y, _camera.position.z);
    }

    private void ChangeUnit(Unit obj)
    {
        if (obj != null)
        {
            _attached = null;
            _attachedUnit = obj;
            return;
        }
    }
}
