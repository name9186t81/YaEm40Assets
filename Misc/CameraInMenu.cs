using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInMenu : MonoBehaviour2D
{
    [SerializeField] private float _followingSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _smoothTime;
    private Transform _currentFollowing;
    private float _velocity;
    private float _cachedRotationSpeed;

    private void Start()
    {
        _cachedRotationSpeed = _rotationSpeed;
        if (ServiceLocator.TryGetService<SettingsContainer>(out var container) && 
            container.TryGetSetting("RotateCamera", out var value))
        {
            UpdateRotation();
            container.OnValuesUpdate += UpdateRotation;
        }
    }

    private void UpdateRotation()
    {
        ServiceLocator.TryGetService<SettingsContainer>(out var container);
        container.TryGetSetting("RotateCamera", out var value);

        bool rotate = (bool)value;
        if (!rotate)
        {
            _rotationSpeed = 0;
            Cached.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            _rotationSpeed = _cachedRotationSpeed;
        }
    }

    private void FixedUpdate()
    {
        if (_currentFollowing == null)
        {
            _currentFollowing = FindObjectOfType<Unit>()?.transform;
            return;
        }

        var lerped = Vector2.Lerp(Position2D, _currentFollowing.position, _followingSpeed);
        Cached.position = new Vector3(lerped.x, lerped.y, Cached.position.x);

        float angle = Mathf.SmoothDampAngle(Cached.eulerAngles.z, _currentFollowing.eulerAngles.z, ref _velocity, _smoothTime, _rotationSpeed);
        Cached.rotation = Quaternion.Euler(0, 0, angle);
    }
}
