using UnityEngine;

public interface ISeekStrategy
{
    void Init(AIController controller, Unit lastTarget, Weapon weapon, LayerMask wallsMask);
    //returns false if seeking failed
    bool TrySeek(Vector2 lastTargetPosition);
}