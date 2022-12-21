using System;

public interface IControllable
{
    bool TryChangeController(IController controller);
    IController CurrentController { get; }
    /// <summary>
    /// previous and new controller
    /// </summary>
    event Action<IController, IController> OnControllerChange;
}
