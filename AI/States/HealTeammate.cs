using UnityEngine;

public class HealTeammate : IWeightState<AIController>
{
    private Unit _mostInjuredTeamate;
    private float _mostInjuredTeamateDeltaHealth;
    private AIController _controller;
    private Unit _attached;
    private AIVision _vision;
    private LayerMask _walls;

    public HealTeammate(LayerMask walls)
    {
        _walls = walls;
    }

    public float CalculateEffectivness()
    {
        return (_mostInjuredTeamate != null) ? 1.5f * (1 - _mostInjuredTeamateDeltaHealth) : -1f;
    }

    public void Execute()
    {
        if (_mostInjuredTeamate == null)
        {
            return;
        }

        if (Physics2D.Raycast(_attached.Position2D, _mostInjuredTeamate.Position2D - _attached.Position2D, Vector2.Distance(_attached.Position2D, _mostInjuredTeamate.Position2D), _walls))
        {
            _mostInjuredTeamate = null;
            return;
        }

        _controller.MoveToPoint(_mostInjuredTeamate.Position2D);
        _controller.LookAtPoint(_mostInjuredTeamate.Position2D);
    }

    public void Init(AIController owner)
    {
        _controller = owner;
        _attached = owner.AttachedUnit;
        _vision = owner.Vision;
        _controller.Vision.OnScan += Scan;
    }

    private void Scan()
    {
        if (_vision.ScanResults.TryGetValue(ScannedUnitType.Ally, out var list))
        {
            Unit mostInjured = default;
            float lowestDelta = 2f;
            for (int i = 0, length = list.Count; i < length; i++)
            {
                float currentDelta = list[i].Health.DeltaHealth();
                if (currentDelta < lowestDelta && 
                    !Physics2D.Raycast(
                        _attached.Position2D, 
                        list[i].Position2D - _attached.Position2D, 
                        Vector2.Distance(list[i].Position2D, _attached.Position2D),
                        _walls))
                {
                    mostInjured = list[i];
                    lowestDelta = currentDelta;
                }
            }
            _mostInjuredTeamateDeltaHealth = lowestDelta;
            _mostInjuredTeamate = mostInjured;
        }
    }

    public void PreExecute()
    {
    }

    public void Undo()
    {
    }
}
