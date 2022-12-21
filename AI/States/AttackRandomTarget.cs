using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AttackRandomTarget : IWeightState<AIController>
{
    protected Unit[] _lastScanResult;
    protected AIController _owner;
    protected Unit _lastTarget;
    private IEngageMovment _movment;
    protected Weapon _attached;
    private LayerMask _walls;
    protected Action OnTargetLost;
    protected Unit LastSawTarget;

    public AttackRandomTarget(IEngageMovment movment, LayerMask walls)
    {
        _movment = movment;
        _walls = walls;
    }

    public virtual float CalculateEffectivness()
    {
        if (_lastTarget == null)
        {
            PickTarget();
        }
        return _lastTarget == null ? -1f : 1f;
    }

    public virtual void Execute()
    {
        if(_lastScanResult == null || !_lastTarget.gameObject.activeSelf)
        {
            _lastTarget = null;
            return;
        }

        if(_attached == null)
        {
            _owner.AttachedUnit.ComponentSystem.TryToGetComponent(out _attached);
        }

        float distance = Vector2.Distance(_lastTarget.Position2D, _owner.AttachedUnit.Position2D);
        if (!_lastTarget.gameObject.activeSelf || (_lastTarget != null && distance > _owner.EngagmentRadius))
        {
            _lastTarget = null;
			_lastScanResult = null;
            return;
        }

        if (_lastTarget == null)
        {
            PickTarget();
            if (_lastTarget == null)
            {
                return;
            }
        }

        var raycast = Physics2D.Raycast(_owner.AttachedUnit.Position2D, _lastTarget.Position2D - _owner.AttachedUnit.Position2D, distance, _walls);

        if (raycast)
        {
            LastSawTarget = _lastTarget;
            TargetLost(_lastTarget.Position2D, _lastTarget);
            _lastTarget = null;
            return;
        }

        _movment.Move(_lastTarget);
        _owner.LookAtPoint(_lastTarget.Position2D);
        if (_owner.SafeToShoot(_lastTarget.Position2D))
        {
            _attached?.Attack();
        }
    }

    protected virtual void TargetLost(Vector2 lastPosition, Unit target)
    {

    }

    private void PickTarget()
    {
        if (_lastTarget != null || _lastScanResult == null) return;

        _lastTarget = _lastScanResult[Random.Range(0, _lastScanResult.Length)];
        if (_lastTarget == null)
        {
            return;
        }
        float distance = Vector2.Distance(_lastTarget.Position2D, _owner.AttachedUnit.Position2D);
        var raycast = Physics2D.Raycast(_owner.AttachedUnit.Position2D, _lastTarget.Position2D - _owner.AttachedUnit.Position2D, distance, _walls);
        if (raycast || distance > _owner.EngagmentRadius)
        {
            _lastTarget = null;
            return;
        }
    }
    public virtual void Init(AIController owner)
    {
        owner.Vision.OnScan += OnScan;
        _owner = owner;
        _owner.AttachedUnit.ComponentSystem.TryToGetComponent(out _attached);
        _movment.Init(owner);
    }

    private void OnScan()
    {
        if (_owner.Vision.ScanResults.TryGetValue(ScannedUnitType.Enemy, out var list))
        {
            _lastScanResult = list.ToArray();
        }
    }

    public void PreExecute()
    {
        PickTarget();
    }

    public void Undo()
    {
    }
}