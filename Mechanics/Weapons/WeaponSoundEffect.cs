using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSoundEffect : MonoBehaviour
{
    [SerializeField] private Weapon _attached;
    [SerializeField] private AudioSource _OnAttack;

    private void Start()
    {
        _attached.OnFire += OnFire;
        if (ServiceLocator.TryGetService<SettingsContainer>(out var service))
        {
            service.OnValuesUpdate += UpdateVolume;
            UpdateVolume();
        }
    }

    private void UpdateVolume()
    {
        if (ServiceLocator.TryGetService<SettingsContainer>(out var service))
        {
            _OnAttack.volume = (float)service.GetSetting("TotalVolume");
        }
    }
    private void OnFire()
    {
		if(!_OnAttack.isPlaying)
			_OnAttack.Play();
    }
}
