using UnityEngine;

[CreateAssetMenu(fileName = "Turret AI")]
public class TurretAI : AIStateMachinePreset
{
    public override IWeightState<AIController>[] GetStates(Unit unit)
    {
        return new IWeightState<AIController>[]
        { 
            new AttackRandomTarget(new StandingMovment(), WallsMask), 
            new TurretIdle()
        };
    }
}
