using UnityEngine;

public class Strafing : IEngageMovment
{
    private AIController _controller;
    private float _maxRange;
    private float _minRange;
    private float _changeDirectionChance;
    private bool _moveRight;
    private float _changeDirectionFrequancy;
    private Weapon _weapon;

    public Strafing(float maxRange, float minRange, float changeDirectionChance)
    {
        _maxRange = maxRange;
        _minRange = minRange;
        _changeDirectionChance = changeDirectionChance;
    }

    public void Init(AIController controller)
    {
        _controller = controller;
        controller.AttachedUnit.ComponentSystem.TryToGetComponent(out _weapon);
    }

    public void Move(Unit target)
    {
        if (target == null)
        {
            return;
        }
        if (_weapon == null)
        {
            _controller.AttachedUnit.ComponentSystem.TryToGetComponent(out _weapon);
        }
        float distance = Vector2.Distance(target.Position2D, _controller.AttachedUnit.Position2D);
        if (distance > _maxRange + _weapon.EffectiveFireRange)
        {
            _controller.MoveToPoint(target.Position2D);
        }
        else
        {
            if (distance < _minRange + _weapon.EffectiveFireRange)
            {
                _controller.MoveToDirection(_controller.AttachedUnit.Position2D - target.Position2D);
            }
            else
            {
                ChangeDirection();
                var direction = _controller.AttachedUnit.transform.right;
                _controller.MoveToDirection(_moveRight ? direction : -direction);
            }
        }
    }

    private void ChangeDirection()
    {
        if(Random.Range(0, 1f) < _changeDirectionChance)
        {
            _moveRight = !_moveRight;
        }
    }
}