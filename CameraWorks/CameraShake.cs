using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private CameraUnitAttacher _cameraAttacher;
    [SerializeField] private Weapon _attached;
    private bool _enabled = true;

    private void Start()
    {
        _cameraAttacher.OnUnitChange += ApplyWeapon;
        if (_attached != null)
        {
            _attached.OnFire += OnWeaponFired;
        }

        if (ServiceLocator.TryGetService<SettingsContainer>(out var container) && container.TryGetSetting("EnableCameraShake", out var setting))
        {
            _enabled = (bool)setting;
        }
    }

    private void ApplyWeapon(Unit unit)
    {
        if (_attached != null)
        {
            _attached.OnFire -= OnWeaponFired;
        }

        if (unit.ComponentSystem.TryToGetComponent<Weapon>(out var weapon))
        {
            _attached = weapon;
            _attached.OnFire += OnWeaponFired;
        }
    }

    private void OnWeaponFired()
    {
        Shake(0.5f, (int)Mathf.Ceil(_attached.WeaponDamage / 30f), 0.03f);
    }

    public void Shake(float radius, int shakeCount, float delay)
    {
        if (_enabled)
        {
            StartCoroutine(Shaking(radius, shakeCount, delay));
        }
    }

    private IEnumerator Shaking(float radius, int shakeCount, float shakeDelay)
    {
        WaitForSeconds delay = new WaitForSeconds(shakeDelay);
        for(int i = 0; i < shakeCount; i++)
        {
            float randomAngle = Random.Range(0, Mathf.PI * 2);
            Vector2 randomPoint = (Vector2.up * Mathf.Sin(randomAngle) + Vector2.right * Mathf.Cos(randomAngle)) * radius + (Vector2)_camera.position;
            _camera.position = new Vector3(randomPoint.x, randomPoint.y, _camera.position.z);
            yield return delay;
        }
    }
}
