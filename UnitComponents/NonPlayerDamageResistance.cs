using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerDamageResistance : UnitComponent
{
    protected override void AddToComponentSystem()
    {
        Owner.ComponentSystem.AddComponent<NonPlayerDamageResistance>(this);
        Owner.Health.OnPreDamage += Resist;
    }

    private void Resist(DamageArgs obj)
    {
        if (!(obj.Attacker.CurrentController is MobileController))
        {
            obj.Damage = 0;
        }
    }
}
