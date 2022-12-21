using UnityEngine;
using System;
using System.Collections;
using Random = UnityEngine.Random;
using UnityEngine.Events;
using System.Collections.Generic;

public class Projectile : MonoBehaviour2D
{
    [SerializeField] private float _delayBeforeDisabling;
    [SerializeField] private GameObject[] _disabledComponentsBeforeDisabling;
    [SerializeField] private float _speed;
    [SerializeField] private TeamColorAdapter _adapter;
    [SerializeField] private float _maxLifeTime = 30;
    
    public ComponentSystem<Projectile> ComponentSystem { get; private set; }
    private Pool<Projectile> _pool;
    private DamageArgs _args;
    public event Action OnInit;
    public event Action<Unit, Vector2> OnTargetHit;
    public UnityEvent OnHitInEditor;
    private Vector2 _lastPosition;

    public HashSet<Unit> Ignored = new HashSet<Unit>();
    private SlowMotionNotification _notificator;
    private float _speedModifier;
    private float _elapsedTime;
    private WaitForSeconds _delay;
    private bool _isDisabling = false;
    public event Action<Transform, Vector2, Vector2> OnHit;

    protected virtual void Awake()
    {
        _delay = new WaitForSeconds(_delayBeforeDisabling);
        ComponentSystem = new ComponentSystem<Projectile>(this);
        _notificator = ServiceLocator.GetService<SlowMotionNotification>();
    }

    public virtual void Init(Pool<Projectile> pool, DamageArgs args, Vector2 position, float speedModifier)
    {
        _speedModifier = speedModifier;
        _pool = pool;
        _args = args;
        _lastPosition = position;
        gameObject.SetActive(true);
        Position2D = position;
        SetComponentsActiveState(true);
        OnInit?.Invoke();
        _elapsedTime = 0;

        if (args.Attacker.ComponentSystem.TryToGetComponent<IgnoredUnits>(out var ignored))
        {
            Ignored.Clear();
            for (int i = 0, length = ignored.Ignored.Length; i < length; i++)
            {
                Ignored.Add(ignored.Ignored[i]);
            }
        }

        var check = Physics2D.OverlapPoint(position);
        if (check)
        {
            Hit(check.transform, position, Vector2.zero);
        }
        _adapter?.Init(args.Attacker);
    }

    private void SetComponentsActiveState(bool value)
    {
        for(int i = 0, length = _disabledComponentsBeforeDisabling.Length; i < length; i++)
        {
            _disabledComponentsBeforeDisabling[i].SetActive(value);
        }
    }

    protected virtual void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime > _maxLifeTime)
        {
            StartCoroutine(AwaitingDisabling());
        }
        _lastPosition = Position2D;
    }

    protected virtual void Hit(Transform target, Vector2 hit, Vector2 normal)
    {
        OnHit?.Invoke(target, hit, normal);
        OnHitInEditor?.Invoke();
        if (target.TryGetComponent<Unit>(out var unit))
        {
            if (Ignored.Contains(unit))
            {
                return;
            }
            OnTargetHit?.Invoke(unit, hit);
            unit.Health.TakeDamage(_args);
        }
        Position2D = hit;
        StartCoroutine(AwaitingDisabling());
    }
    private void LateUpdate()
    {
        if (_isDisabling)
        {
            return;
        }
        Position2D += (Vector2)Cached.up * _speed * Time.deltaTime * _speedModifier * _notificator.CurrentTimeScale;

        var result = Physics2D.Raycast(_lastPosition, (Position2D - _lastPosition).normalized, Vector2.Distance(_lastPosition, Position2D));

        if(result && !result.collider.isTrigger)
        {
            Hit(result.transform, result.point, result.normal);
        }
    }

    protected IEnumerator AwaitingDisabling()
    {
        if (_isDisabling)
        {
            yield break;
        }
        _isDisabling = true;
        SetComponentsActiveState(false);
        yield return _delay;
        _isDisabling = false;
        gameObject.SetActive(false);
        _pool?.ReturnToPool(this);   
    }

    protected DamageArgs Args => _args;
    public Unit Attackr => _args.Attacker;
}
