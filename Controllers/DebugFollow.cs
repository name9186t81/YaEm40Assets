using System;
using UnityEngine;

public class DebugFollow : MonoBehaviour, IController
{
    public event Action<Vector2> MoveInput;
    public event Action<Vector2> LookInput;
    public event Action<CommandKey> OnCommandInput;

    [SerializeField] private Unit _attachedTo;
    [SerializeField] private float _minFollowDistance;
    [SerializeField] private Transform _target;
    private Transform _cached;

    private void Awake()
    {
        if (!_attachedTo.TryChangeController(this))
        {
            Debug.LogWarning($"Cannot apply controller {gameObject.name} to {_attachedTo.name}");
        }
        _cached = transform;
    }
    private void Update()
    {
        if (Vector2.Distance(_cached.position, _target.position) > _minFollowDistance)
        {
            MoveInput?.Invoke(_target.position - _cached.position);
        }
        LookInput?.Invoke(_target.position - _cached.position);
    }
}
