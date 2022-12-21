using UnityEngine;

public class Engaging : IEngageMovment
{
    private AIController _controller;
    private float _maxRange;
    private Weapon _attached;

    public Engaging(float maxRange)
    {
        _maxRange = maxRange;
    }

    public void Init(AIController controller)
    {
        _controller = controller;
        _controller.AttachedUnit.ComponentSystem.TryToGetComponent(out _attached);
    }

    public void Move(Unit target)
    {
        if(target == null)
        {
            return;
        }
        if (_attached == null)
        {
            _controller.AttachedUnit.ComponentSystem.TryToGetComponent(out _attached);
        }
        if (Vector2.Distance(target.Position2D, _controller.AttachedUnit.Position2D) > (_maxRange + _attached.EffectiveFireRange))
        {
            _controller.MoveToPoint(target.Position2D);
        }
        else
        {
            _controller.MoveToDirection(_controller.AttachedUnit.Position2D - target.Position2D);
        }
    }
}