using UnityEngine;

[CreateAssetMenu(fileName = "Dummy AI")]
public class DummyAI : AIStateMachinePreset
{
    [SerializeField] private float _maxEngagmentRange;

    public override IWeightState<AIController>[] GetStates(Unit unit)
    {
        var rand = Random.Range(0, 2);
        IEngageMovment movment = null;
        if(rand == 0)
        {
            movment = new Engaging(_maxEngagmentRange);
        }
        else
        {
            movment = new Strafing(_maxEngagmentRange, 1f + unit.Size, Random.Range(0.1f, 0.2f));
        }

        return new IWeightState<AIController>[] { new AttackRandomTarget(movment, WallsMask), new IdleWalking(WallsMask, 10f) };
    }
}