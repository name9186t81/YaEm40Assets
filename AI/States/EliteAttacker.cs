using UnityEngine;

public class EliteAttacker : IWeightState<AIController>
{
    private Unit _target;
    private AIController _controller;
    private Unit _controlledUnit;
    private AIVision _vision;
    private IEngageMovment _engageMovment;
    private IDodgeStrategy _dodge;
    private LayerMask _walls;
    private Weapon _attached;

    public EliteAttacker(IEngageMovment engageMovment, IDodgeStrategy dodgeStrategy, LayerMask walls)
    {
        _engageMovment = engageMovment;
        _dodge = dodgeStrategy;
        _walls = walls;
    }

    public float CalculateEffectivness()
    {
        if (_target == null)
        {
            PickTarget();
        }
        return _target == null ? -1f : 5f;
    }

    public void Execute()
    {
        if (_attached == null)
        {
            _controlledUnit.ComponentSystem.TryToGetComponent(out _attached);
        }


        if (_target == null)
        {
            PickTarget();
            if (_target == null)
            {
                return;
            }
        }

        float distance = Vector2.Distance(_target.Position2D, _controlledUnit.Position2D);
        if (distance > _controller.EngagmentRadius)
        {
            _target = null;
            return;
        }

        var raycast = Physics2D.Raycast(_controlledUnit.Position2D, _target.Position2D - _controlledUnit.Position2D, distance, _walls);

        if (raycast)
        {
            _controlledUnit.Position2D = _target.Position2D;
            return;
        }

        _engageMovment.Move(_target);
        _controller.LookAtPoint(_target.Position2D);
        _attached?.Attack();
    }

    public void Init(AIController owner)
    {
        _controller = owner;
        _controlledUnit = owner.AttachedUnit;
        _vision = owner.Vision;
        _engageMovment.Init(_controller);
        _controlledUnit.ComponentSystem.TryToGetComponent<Weapon>(out var weapon);
        _dodge.Init(_controller);
        _vision.OnScan += PickTarget;
        _controlledUnit.Health.OnDamage += ChangeTarget;
    }

    private void ChangeTarget(DamageArgs obj)
    {
        if (_target != null && _target.ComponentSystem.TryToGetComponent<Weapon>(out Weapon weapon))
        {
            weapon.OnFire -= TargetFired;
        }

        _target = obj.Attacker;
        TargetFired();

        if (_target.ComponentSystem.TryToGetComponent<Weapon>(out Weapon weapon2))
        {
            weapon2.OnFire += TargetFired;
        }
    }

    private void PickTarget()
    {
        if (_vision.ScanResults.TryGetValue(ScannedUnitType.Enemy, out var enemys))
        {
            Unit current = default;
            float closest = float.MaxValue;

            for (int i = 0, length = enemys.Count; i < length; i++)
            {
                if (enemys[i] == null)
                {
                    continue;
                }
                if (enemys[i].CurrentController is MobileController)
                {
                    TargetDied();
                    _target = enemys[i];
                    return;
                }

                float currentDistance = (enemys[i].Position2D - _controlledUnit.Position2D).magnitude;
                if (currentDistance < closest)
                {
                    closest = currentDistance;
                    current = enemys[i];
                }
            }
            TargetDied();

            _target = current;
            _target.Health.OnDeath += TargetDied;

            if (_target.ComponentSystem.TryToGetComponent<Weapon>(out Weapon weapon))
            {
                weapon.OnFire += TargetFired;
            }
        }
    }

    private void TargetFired()
    {
        if (_dodge.CanDodge(_target))
        {
            _dodge.Dodge(_target);
        }
    }

    private void TargetDied()
    {
        if (_target == null)
        {
            return;
        }
        _target.Health.OnDeath -= TargetDied;

        if (_target.ComponentSystem.TryToGetComponent<Weapon>(out Weapon weapon))
        {
            weapon.OnFire -= TargetFired;
        }
    }

    private void TargetDied(DamageArgs obj) => TargetDied();

    public void PreExecute()
    {
    }

    public void Undo()
    {
    }
}
