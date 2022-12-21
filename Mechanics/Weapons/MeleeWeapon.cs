using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    [SerializeField] private Vector2 _size;
    [SerializeField] private Vector2 _offset;
    [SerializeField] private float _attackDelay;
    private Timer _delay;
    private bool _canAttack = true;

    public override void Init(Unit owner)
    {
        base.Init(owner);
        _delay = new Timer(_attackDelay);
        _delay.OnPeriodReached += ChangeState;
    }

    private void ChangeState()
    {
        _canAttack = true;
        _delay.Stop();
    }

    private void Update()
    {
        _delay.Update(Time.deltaTime * Owner.LocalTimeScale);
    }

    public override void Attack()
    {
        if (!CanAttack())
        {
            return;
        }

        float angle = Owner.transform.eulerAngles.z * Mathf.Deg2Rad;
        var overlap = Physics2D.OverlapAreaAll(Center - _size.Rotate(angle) / 2 + _offset.Rotate(angle), Center + _size.Rotate(angle) / 2 + _offset.Rotate(angle));

        if (overlap != null)
        {
            for (int i = 0, length = overlap.Length; i < length; i++)
            {
                if (overlap[i].TryGetComponent<Unit>(out var unit))
                {
                    if (unit == Owner) continue;
                    unit.Health.TakeDamage(new DamageArgs(Damage, Owner));
                }
            }
        }

        _canAttack = false;
        _delay.Start();
        InvokeFire();
    }

    public override bool CanAttack() => _canAttack;

    public override bool IsLockedInRotation()
    {
        return false;
    }

    protected override void AddToComponentSystem()
    {
        if (Owner.ComponentSystem.TryToGetComponent<Weapon>(out _))
        {
            Owner.ComponentSystem.RemoveComponent<Weapon>();
        }
        Owner.ComponentSystem.AddComponent(typeof(Weapon), this);
    }

    private void OnDrawGizmos()
    {
        float angle = Owner.transform.eulerAngles.z * Mathf.Deg2Rad;
        Vector2 downLeft = Center - _size.Rotate(angle) / 2 + _offset.Rotate(angle);
        Vector2 upRight = Center + _size.Rotate(angle) / 2 + _offset.Rotate(angle);

        Gizmos.DrawLine(downLeft, upRight);
    }

    private Vector2 Center => ShootPosition;
}
