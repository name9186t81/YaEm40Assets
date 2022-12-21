using UnityEngine;

public abstract class UnitComponent : MonoBehaviour2D, IComponent<Unit>
{
    [field: SerializeField] protected Unit Owner { get; private set; }
    public ComponentType ComponentType { get => _componentType; }
    [SerializeField] private ComponentType _componentType;
    protected IController Controller => Owner.CurrentController;

    protected virtual void Start()
    {
        AddToComponentSystem();
    }
    protected abstract void AddToComponentSystem();
    public virtual void Init(Unit owner)
    {
        Owner = owner;
    }

    protected bool TryGetExistingController(out IController controller)
    {
        if(Controller != null)
        {
            controller = Controller;
            return true;
        }
        controller = null;
        return false;
    }
}
