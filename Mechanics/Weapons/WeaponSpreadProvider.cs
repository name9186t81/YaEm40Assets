using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponSpreadProvider : MonoBehaviour
{
    public abstract float GetSpreadedAngle(float weaponAngle, float spread);
}
