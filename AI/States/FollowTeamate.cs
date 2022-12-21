using UnityEngine;

public class FollowTeamate : IWeightState<AIController>
{
    private AIController _controller;
    private Unit _teamate;
    private AIVision _vision;
    private LayerMask _walls;

    public FollowTeamate(LayerMask walls)
    {
        _walls = walls;
    }

    public void Execute()
    {
        var ownPosition = _controller.AttachedUnit.Position2D;
        float distance = Vector2.Distance(_teamate.Position2D, ownPosition);
        if (distance < SafeDistance)
        {
            _controller.SafeWalk(_teamate.Position2D);
            _controller.LookAtPoint(_teamate.Position2D);
            return;
        }

        if (Physics2D.Raycast(ownPosition, _teamate.Position2D - ownPosition, distance, _walls))
        {
            _teamate = null;
            return;
        }

        _controller.SafeWalk(_teamate.Position2D);
        _controller.LookAtPoint(_teamate.Position2D);
    }

    public void Init(AIController owner)
    {
        _controller = owner;
        _vision = _controller.Vision;
        _controller.Vision.OnScan += PickTeamate;
    }

    private void PickTeamate()
    {
        if (_teamate != null)
        {
            return;
        }

        if (_vision.ScanResults.TryGetValue(ScannedUnitType.Ally, out var units) && units.Count > 0)
        {
            for (int i = 0, length = units.Count; i < length; i++)
            {
                if (!units[i].ComponentSystem.TryToGetComponent<LinkedUnit>(out _))
                {
                    _teamate = units[i];
                    return;
                }
            }
        }
    }

    public void PreExecute()
    {
    }

    public void Undo()
    {
    }

    public float CalculateEffectivness()
    {
        return _teamate == null ? -1f : 0.5f;
    }

    private float SafeDistance => _teamate.Size * 2 + _controller.AttachedUnit.Size * 2;
}
