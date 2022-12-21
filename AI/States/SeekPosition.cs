using UnityEngine;

public class SeekPosition : ISeekStrategy
{
    private AIController _controller;
    private LayerMask _wallMask;
    private Unit _lastTarget;

    public void Init(AIController controller, Unit lastTarget, Weapon weapon, LayerMask wallsMask)
    {
        _controller = controller;
        _wallMask = wallsMask;
        _lastTarget = lastTarget;
    }

    public bool TrySeek(Vector2 lastTargetPosition)
    {
        //TODO: use handlewalking method from aiController
        _controller.MoveToPoint(lastTargetPosition);
        _controller.LookAtPoint(lastTargetPosition);

        Vector2 ownPosition = _controller.AttachedUnit.Position2D;
        if (Vector2.Distance(ownPosition, lastTargetPosition) < (_controller.AttachedUnit.Size))
        {
            return false;
        }
        return !Physics2D.Raycast(ownPosition, lastTargetPosition.GetDirection(ownPosition, false),
            Vector2.Distance(ownPosition, _lastTarget.Position2D), _wallMask);
    }
}
