using UnityEngine;
using System;

public abstract class Weapon : UnitComponent
{
    [SerializeField] protected int Damage;
    [SerializeField] protected float EffectiveRange;
    [SerializeField] protected PlayerWeaponData Data;
    [SerializeField] protected Transform _shootPosition;
    [SerializeField] private float _scopeScale = 1f;

    [SerializeField] private WeaponComponent[] _components;
    public event Action OnFire;
    public event Action OnShootingFinish;
    public event Action OnReloadingStarted;
    public event Action OnReloadingEnded;

    public abstract bool IsLockedInRotation();

    public override void Init(Unit owner)
    {
        base.Init(owner);
        if(owner.CurrentController != null)
        {
            owner.CurrentController.OnCommandInput += TryToFire;
        }
        owner.OnControllerChange += ControllerChanged;

        if(_components != null)
        {
            for(int i = 0, length = _components.Length; i < length; i++)
            {
                _components[i].Init(this);
            }
        }
    }

    private void ControllerChanged(IController old, IController obj)
    {
        if(old != null)
        {
            old.OnCommandInput -= TryToFire;
        }
        obj.OnCommandInput += TryToFire;
    }

    private void TryToFire(CommandKey obj)
    {
        if (obj == CommandKey.Attack && CanAttack())
        {
            Attack();
        }
    }

    protected void InvokeFire()
    {
        OnFire?.Invoke();
    }
    protected void InvokeFireFinished()
    {
        OnShootingFinish?.Invoke();
    }
    public abstract void Attack();
    public abstract bool CanAttack();

    public float ScopeScale => _scopeScale;
    public int WeaponDamage => Damage;
    public Unit Unit => Owner;
    public float EffectiveFireRange => EffectiveRange;
    public Vector2 ShootPosition => _shootPosition.position;
    public Vector2 ShootDirection => _shootPosition.up;
    public PlayerWeaponData Type => Data;
}
