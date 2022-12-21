 using UnityEngine;

public abstract class AIStateMachinePreset : ScriptableObject
{
    [SerializeField] protected LayerMask WallsMask;
    public abstract IWeightState<AIController>[] GetStates(Unit unit);
}
