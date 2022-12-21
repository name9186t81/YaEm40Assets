using UnityEngine;

public abstract class Interactable : MonoBehaviour2D
{
    [SerializeField] private int _interactPriority;
    [field: SerializeField] public float AIPriority { get; private set; }

    public abstract void Interact(Unit unit);
    public abstract bool CanInteract(Unit unit);

    public int InteractPriority => _interactPriority;
}
