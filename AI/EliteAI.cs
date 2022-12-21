using UnityEngine;

[CreateAssetMenu(fileName = "Elite AI")]
public class EliteAI : AIStateMachinePreset
{
    [SerializeField] private float _maxEngagmentRange;
    [SerializeField, Range(0, 1f)] private float _preFireChance;

    public override IWeightState<AIController>[] GetStates(Unit unit)
    {
        return new IWeightState<AIController>[] { new EliteAttacker(new Strafing(7f, 4f, 0.05f), new Jump(WallsMask, 2f), WallsMask), new IdleWalking(WallsMask, 10f), new SlowTime(0.8f, 1f, 64f) };
    }
}