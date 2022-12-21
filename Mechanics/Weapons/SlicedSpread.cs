using UnityEngine;

public class SlicedSpread : WeaponSpreadProvider
{
    [SerializeField] private int _stepsCount;
    private int _lastStep;
    public override float GetSpreadedAngle(float weaponAngle, float spread)
    {
        float angle = weaponAngle + (-spread / 2 + spread * _lastStep / _stepsCount);
        _lastStep++;
        if (_lastStep > _stepsCount)
        {
            _lastStep = 0;
        }
        return angle;
    }
}
