using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpreadDisplayer : MonoBehaviour
{
    [SerializeField] private MobileController _controller;
    [SerializeField] private CameraUnitAttacher _attacher;
    [SerializeField] private float _maxRange;
    [SerializeField] private LayerMask _walls;
    [SerializeField] private LineRenderer _leftRenderer;
    [SerializeField] private LineRenderer _rightRenderer;
    private Weapon _attached;
    private float _spread;
    private bool _enabled = true;

    private void Start()
    {
        if (ServiceLocator.TryGetService<SettingsContainer>(out var container) && container.TryGetSetting("EnableSpreadDisplay", out var res))
        {
            _enabled = (bool)res;
        }
        _attacher.OnUnitChange += ChangeUnit;
        ChangeUnit(_attacher.Attached);
        _controller.AttackJoystick.OnMove += Display;
        _controller.AttackJoystick.OnRelease += Release;
    }

    private void Release()
    {
        _leftRenderer.positionCount = 0;
        _rightRenderer.positionCount = 0;
    }

    private void Display(Vector2 obj)
    {
        if (_attached == null || !_enabled)
        {
            if (_attached == null)
            {
                _attacher.Attached?.ComponentSystem.TryToGetComponent<Weapon>(out _attached);
                return;
            }
            return;
        }

        _leftRenderer.positionCount = 2;
        _rightRenderer.positionCount = 2;

        var unit = _attacher.Attached;
        float weaponAngle = Mathf.Atan2(_attached.ShootDirection.y, _attached.ShootDirection.x) * Mathf.Rad2Deg;

        _leftRenderer.SetPosition(0, _attached.ShootPosition);
        _rightRenderer.SetPosition(0, _attached.ShootPosition);
        _leftRenderer.SetPosition(1, GetDiretcionForRenderer((weaponAngle - _spread) * Mathf.Deg2Rad));
        _rightRenderer.SetPosition(1, GetDiretcionForRenderer((weaponAngle + _spread) * Mathf.Deg2Rad));
    }

    private Vector2 GetDiretcionForRenderer(float angle)
    {
        var direction = VectorExtensions.GetDirectionFromAngle(angle);
        var raycast = Physics2D.Raycast(_attached.ShootPosition, direction, _maxRange, _walls);
        if (raycast)
        {
            return raycast.point;
        }
        else
        {
            return direction * _maxRange;
        }
    }
    private void ChangeUnit(Unit obj)
    {
        if (obj == null)
        {
            return;
        }
        if (obj.ComponentSystem.TryToGetComponent<Weapon>(out var weapon))
        {
            _attached = weapon;
            if (_attached is RangedWeapom)
            {
                _spread = ((RangedWeapom)_attached).Spread / 2;
            }
        }
        else
        {
            obj.ComponentSystem.OnComponentAdded += GetWeapon;
        }
    }

    private void GetWeapon(System.Type arg1, object arg2)
    {
        if (arg1 == typeof(Weapon))
        {
            _attached = (Weapon)arg2;
            if (_attached is RangedWeapom)
            {
                _spread = ((RangedWeapom)_attached).Spread / 2;
            }
        }
    }
}
