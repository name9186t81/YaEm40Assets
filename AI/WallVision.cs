using UnityEngine;

public class WallVision : AIVision
{
    [SerializeField] private LayerMask _walls;

    protected override void Scan()
    {
        if (!Enabled) return;

        var overlap = Physics2D.OverlapCircleAll(Owner.Position2D, ScanRadius);

        if (overlap == null) return;

        ClearDicitonary();
        CircilarScan();
        ClearDicitonary(ScannedUnitType.Enemy);

        for (int i = 0, length = overlap.Length; i < length; i++)
        {
            if (overlap[i].TryGetComponent<Unit>(out var unit) && unit.teamNumber != Owner.teamNumber)
            {
                if (Physics2D.Raycast(Owner.Position2D, unit.Position2D - Owner.Position2D, Vector2.Distance(unit.Position2D, Owner.Position2D), _walls))
                {
                    continue;
                }

                AddToDictionary(unit);
            }
        }

        InitOnScan();
    }
}
