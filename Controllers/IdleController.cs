using UnityEngine;
using System;

public class IdleController : MonoBehaviour, IController
{
    public event Action<Vector2> MoveInput;
    public event Action<Vector2> LookInput;
    public event Action<CommandKey> OnCommandInput;
    [SerializeField] private Unit _unit;

    private void Awake()
    {
        _unit.TryChangeController(this);
    }
}