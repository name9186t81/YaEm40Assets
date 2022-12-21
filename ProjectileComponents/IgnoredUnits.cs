using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoredUnits : UnitComponent
{
    [SerializeField] private List<Unit> _ignored;
    [SerializeField] private Weapon _weapon;

    protected override void AddToComponentSystem()
    {
        Owner.ComponentSystem.AddComponent<IgnoredUnits>(this);
    }

    public Unit[] Ignored => _ignored.ToArray();
}
