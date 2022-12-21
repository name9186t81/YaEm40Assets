using UnityEngine;

public class RandomSpread : WeaponSpreadProvider
{
    public override float GetSpreadedAngle(float weaponAngle, float spread)
    {
        return weaponAngle + Random.Range(-spread / 2, spread / 2);
    }
}
