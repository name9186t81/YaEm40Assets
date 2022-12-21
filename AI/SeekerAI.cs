using UnityEngine;

[CreateAssetMenu(fileName = "Seeker AI")]
public class SeekerAI : AIStateMachinePreset
{
    [SerializeField] private float _maxEngagmentRange;
    [SerializeField, Range(0, 1f)] private float _preFireChance;

    public override IWeightState<AIController>[] GetStates(Unit unit)
    {
        var rand = Random.Range(0, 2);
        IEngageMovment movment = null;
        if (rand == 0)
        {
            movment = new Engaging(_maxEngagmentRange);
        }
        else
        {
            movment = new Strafing(_maxEngagmentRange, 1f + unit.Size, Random.Range(0.05f, 0.1f));
        }

        ISeekStrategy seekStrat = default;
        rand = Random.Range(0, 2);
        if (rand == 0)
        {
            seekStrat = new Camp(1f, _preFireChance);
        }
        else
        {
            seekStrat = new SeekPosition();
        }
        rand = Random.Range(0, 8);
        if (rand == 0)
        {
            return new IWeightState<AIController>[] { new TargetSeeker(movment, WallsMask, seekStrat), new IdleWalking(WallsMask, 10f), new FollowTeamate(WallsMask), new ReportTargets(), new HuntTarget(WallsMask) };
        }
        return new IWeightState<AIController>[] { new TargetSeeker(movment, WallsMask, seekStrat), new IdleWalking(WallsMask, 10f), new ReportTargets(), new HuntTarget(WallsMask) };
    }
}