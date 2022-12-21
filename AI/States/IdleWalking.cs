using UnityEngine;

public class IdleWalking : IWeightState<AIController>
{
    private LayerMask _wallsMask;
    private float _pickRadius;
    private Vector2 _previousPosition;
    private const int MAX_POINT_CHANGE_TRIES = 16;
    private const float TRESHOLD = 1.5f;

    //TODO: move this into AI controller
    private Vector2 _positionInLastFrame;
    private AIController _controller;
    private Unit _owner;

    public IdleWalking(LayerMask wallsMask, float pickRadius)
    {
        _wallsMask = wallsMask;
        _pickRadius = pickRadius;
    }

    public float CalculateEffectivness()
    {
        return 0f;
    }

    public void Execute()
    {
        if(Vector2.Distance(_previousPosition, _owner.Position2D) < TRESHOLD + _owner.Size)
        {
            TryToPickRandomPoint();
        }

        if(_controller.HandleWalking(_positionInLastFrame - _owner.Position2D, _owner.Size * 2f, out var result, out var isRightPos))
        {
            //Debug.Log(_owner.name + " is evading");
            if (isRightPos)
            {
                //Debug.Log(_owner.name + " moving to the " + -_owner.transform.right);
                _controller.MoveToDirection(-_owner.transform.right);
            }
            else
            {
                //Debug.Log(_owner.name + " moving to the " + _owner.transform.right);
                _controller.MoveToDirection(_owner.transform.right);
            }
        }
        else
        {
            _controller.MoveToPoint(_previousPosition);
        }
        _controller.LookAtPoint(_previousPosition);
        _positionInLastFrame = _previousPosition;
    }

    public void Init(AIController owner)
    {
        _controller = owner;
        _owner = _controller.AttachedUnit;
        _owner.OnCollision += HandleCollision;
    }

    private void HandleCollision(Collision2D obj)
    {

    }

    private void TryToPickRandomPoint()
    {

        for (int i = 0; i < MAX_POINT_CHANGE_TRIES; i++)
        {
            Vector2 direction = Vector2Utils.GetRandomPointOnCircle();
            float radius = RandomPickingRadius;

            var raycast = Physics2D.Raycast(_owner.Position2D, direction, radius, _wallsMask);

            if (raycast)
            {
                continue;
            }

            _previousPosition = direction * radius - direction * _owner.Size + _owner.Position2D;
            return;
        }
        Debug.LogWarning($"Unit {_owner.gameObject.name} cannot pick random point to walk");
    }

    public void PreExecute()
    {
        TryToPickRandomPoint();
    }

    public void Undo()
    {
    }

    private float RandomPickingRadius => Random.Range(0f, _pickRadius);
}
