using UnityEngine;

public class WeaponTrailDisplayer : MonoBehaviour
{
    [SerializeField] private LineRenderer _renderer;
    [SerializeField] private CameraUnitAttacher _attacher;
    [SerializeField] private MobileController _controller;
    [SerializeField] private float _maxRange;
    private Weapon _attached;
    private bool _enabled = true;

    private void Start()
    {
        if (ServiceLocator.TryGetService<SettingsContainer>(out var container) && container.TryGetSetting("EnableTrajectoryDisplay", out var setting))
        {
            _enabled = (bool)setting;
        }

        _attacher.OnUnitChange += Change;
        if (_attacher.Attached == null)
        {
            return;
        }
        Change(_attacher.Attached);

    }

    private void Change(Unit obj)
    {
        if (_attached != null)
        {
            _renderer.positionCount = 0;
        }
        _attached = null;
        if (obj.ComponentSystem.TryToGetComponent(out Weapon weapon))
        {
            _attached = weapon;
            _attached.OnShootingFinish += AllowFire;
        }
        else
        {
            obj.ComponentSystem.OnComponentAdded += OnComponentAdded;
        }
        _controller.AttackJoystick.OnMove += Look;
        _controller.AttackJoystick.OnRelease += Release;
        _renderer.positionCount = 0;
    }

    private void OnComponentAdded(System.Type arg1, object arg2)
    {
        if(_attached != null)
        {
            return;
        }
        if(arg1 == typeof(Weapon))
        {
            _attached = (Weapon)arg2;
            _attached.OnShootingFinish += AllowFire;
        }
    }

    private void AllowFire()
    {
        _controller.AllowRotation(true);
    }

    private void Release()
    {
        if (_attached == null)
        {
            return;
        }

        if (TryGetAttached.Type == PlayerWeaponData.PreFired)
        {
            _attached.Attack();
            _renderer.positionCount = 0;
            _controller.AllowRotation(!_attached.IsLockedInRotation());
        }
    }

    private void Look(Vector2 obj)
    {
        if(_attached == null)
        {
            if (_attached == null)
            {
                _attacher.Attached?.ComponentSystem.TryToGetComponent<Weapon>(out _attached);
                return;
            }
            return;
        }

        if (TryGetAttached.Type == PlayerWeaponData.PreFired && _enabled)
        {
            _renderer.positionCount = 2;
            _renderer.SetPosition(0, _attached.ShootPosition);
            var raycast = Physics2D.Raycast(_attached.ShootPosition, _attached.ShootDirection, _maxRange);
            if(raycast)
            {
                _renderer.SetPosition(1, raycast.point);
            }
            else
            {
                _renderer.SetPosition(1, _attached.ShootDirection * _maxRange);
            }
        }
        if (TryGetAttached.Type == PlayerWeaponData.BurstFire)
        {
            _attached.Attack();
        }
    }

    private Weapon TryGetAttached
    {
        get
        {
            if (_attached != null)
            {
                return _attached;
            }
            if (_attacher.Attached.ComponentSystem.TryToGetComponent(out _attached)) { }
            else
            {
                Debug.LogWarning("There is no weapon attached to " + _attacher.Attached.gameObject.name);
            }
            return _attached;
        }
    }
}
