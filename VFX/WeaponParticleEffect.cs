using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParticleEffect : WeaponComponent
{
    [SerializeField] private ParticleSystem _system;

    private void Awake()
    {
        Owner.OnFire += Fire;
    }

    private void Fire()
    {
        _system.Play();
    }

    private void OnDestroy()
    {
        Owner.OnFire -= Fire;
    }
}
