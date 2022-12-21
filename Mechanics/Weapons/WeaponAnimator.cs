using UnityEngine;

public class WeaponAnimator : WeaponComponent
{
    [SerializeField] private Animator _animator;

    private void Start()
    {
        Owner.OnFire += Fire;
    }

    private void Fire()
    {
        _animator.SetTrigger("Shoot");
    }

    private void OnDestroy()
    {
        Owner.OnFire -= Fire;
    }
}
