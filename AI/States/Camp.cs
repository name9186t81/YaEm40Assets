using UnityEngine;
using Random = UnityEngine.Random;

public class Camp : ISeekStrategy
{
    private float _maxSeekingTime;
    private float _currentSeekingTime;
    private float _shootChance;
    private AIController _controller;
    private Weapon _weapon;
    private LayerMask _walls;
    private Unit _lastTarget;

    public Camp(float maxSeekingTime, float shootChance)
    {
        _maxSeekingTime = maxSeekingTime;
        _shootChance = Mathf.Clamp01(shootChance);
    }

    public void Init(AIController controller, Unit lastTarget, Weapon weapon, LayerMask mask)
    {
        _controller = controller;
        _weapon = weapon;
        _walls = mask;
        _lastTarget = lastTarget;
    }

    public bool TrySeek(Vector2 lastTargetPosition)
    {
        Vector2 ownPosition = _controller.AttachedUnit.Position2D;
        if (!Physics2D.Raycast(ownPosition, lastTargetPosition.GetDirection(ownPosition, false),
            Vector2.Distance(ownPosition, _lastTarget.Position2D), _walls))
        {
            return false;
        }

        _controller.LookAtPoint(lastTargetPosition);
        if (Random.Range(0, 1f) < _shootChance)
        {
            _weapon.Attack();
        }

        _currentSeekingTime += Time.deltaTime;
        _controller.MoveToPoint(ownPosition);

        if (_currentSeekingTime > _maxSeekingTime)
        {
            _currentSeekingTime = 0f;
            return false;
        }
        return true;
    }
}
