using System;
using UnityEngine;

public interface IController
{
    event Action<Vector2> MoveInput;
    event Action<Vector2> LookInput;
    event Action<CommandKey> OnCommandInput;
}
