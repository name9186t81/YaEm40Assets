using UnityEngine;

public class LinkedUnit : UnitComponent
{
    [SerializeField] private Unit _linkedTo;

    protected override void AddToComponentSystem()
    {
        Owner.ComponentSystem.AddComponent<LinkedUnit>(this);
        Owner.teamNumber = _linkedTo.teamNumber;
        Owner.OnTeamChange += Change;
    }

    private void Change()
    {
        Owner.teamNumber = _linkedTo.teamNumber;
    }

    protected override void Start()
    {
        base.Start();
        Owner.teamNumber = _linkedTo.teamNumber;
    }
}
