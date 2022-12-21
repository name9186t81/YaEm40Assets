using System;
using UnityEngine;

public class DebugController : MonoBehaviour, IController
{
    public event Action<Vector2> MoveInput;
    public event Action<Vector2> LookInput;
    public event Action<CommandKey> OnCommandInput;

    private void Start()
    {
        if(TryGetComponent<IControllable>(out var c))
        {
            c.TryChangeController(this);
        }
    }
    private void Update()
    {
        Vector2 lookDir = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        LookInput?.Invoke(lookDir);
        if (Input.anyKey)
        {
            Vector2 moveDir = Vector2.zero;
            if (Input.GetKey(KeyCode.W))
            {
                moveDir += Vector2.up;
            }
            if (Input.GetKey(KeyCode.S))
            {
                moveDir += Vector2.down;
            }
            if (Input.GetKey(KeyCode.A))
            {
                moveDir += Vector2.left;
            }
            if (Input.GetKey(KeyCode.D))
            {
                moveDir += Vector2.right;
            }
            MoveInput?.Invoke(moveDir);
        }
    }
}
