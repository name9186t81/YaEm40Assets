using UnityEngine;

public class Jump : IDodgeStrategy
{
    private LayerMask _wallsMask;
    private float _minDistance;
    private Vector2 _savedPoint;
    private AIController _controller;
    private float _jumpCooldown = 1f;
    private float _currentTime;

    public Jump(LayerMask wallMask, float minDistance)
    {
        _wallsMask = wallMask;
        _minDistance = minDistance;
    }

    public bool CanDodge(Unit target)
    {
        if (_currentTime < _jumpCooldown || target == null)
        {
            return false;
        }
        var randomDirection = Vector2Utils.GetRandomPointOnCircle();
        float randomDistance = Random.Range(_minDistance, _minDistance * 2);
        var raycast = Physics2D.Raycast(target.Position2D, randomDirection, randomDistance, _wallsMask);

        _savedPoint = randomDirection * randomDistance;
        if (raycast)
        {
            _savedPoint = raycast.point;
            if (Vector2.Distance(raycast.point, target.Position2D) > _minDistance)
            {
                _currentTime = 0f;
                return true;
            }
            return false;
        }
        _currentTime = 0f;
        return true;
    }

    public void Dodge(Unit target)
    {
        _controller.AttachedUnit.Position2D = _savedPoint;
    }

    public void Init(AIController controller)
    {
        _controller = controller;
        _controller.OnUpdate += Update;
    }

    private void Update(float obj)
    {
        _currentTime += Time.deltaTime;
    }
}
