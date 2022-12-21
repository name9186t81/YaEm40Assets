using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrielndyFireResistance : UnitComponent
{
    protected override void AddToComponentSystem()
    {
        Owner.ComponentSystem.AddComponent<FrielndyFireResistance>(this);
        Owner.Health.OnPreDamage += Resist;
    }

    private void Resist(DamageArgs obj)
    {
        if (obj.Attacker.teamNumber == Owner.teamNumber)
        {
            obj.Damage = 0;
        }
    }
}
