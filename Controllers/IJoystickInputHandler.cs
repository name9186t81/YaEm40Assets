using System;
using UnityEngine;

namespace Joysticks
{
    public interface IJoystickInputHandler
    {
        event Action OnRelease;
        event Action<Vector2> OnMove;
    }
}