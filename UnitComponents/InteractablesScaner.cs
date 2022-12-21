using System;
using UnityEngine;

public class InteractablesScaner : UnitComponent
{
    [SerializeField] private float _activateRaius;
    [SerializeField] private float _activateDelay;
    [SerializeField] private LayerMask _interactablesMask;
    private Interactable _best;
    private Timer _delay;
    public event Action<Interactable> OnScan;

    private void Awake()
    {
        _delay = new Timer(_activateDelay);
        _delay.OnPeriodReached += Scan;
    }

    private void Scan()
    {
        var overlap = Physics2D.OverlapCircleAll(Owner.Position2D, _activateRaius, _interactablesMask);

        Interactable best = default;
        int max = int.MinValue;

        for (int i = 0, length = overlap.Length; i < length; i++)
        {
            if (overlap[i].TryGetComponent<Interactable>(out var interactable) && interactable.InteractPriority > max && interactable.CanInteract(Owner))
            {
                best = interactable;
                max = interactable.InteractPriority;
            }
        }

        _best = best;
        OnScan?.Invoke(_best);
    }

    private void Update()
    {
        _delay.Update(Time.deltaTime);
    }

    public void Interact()
    {
        if (_best != null && _best.CanInteract(Owner))
        {
            _best.Interact(Owner);
        }
    }

    protected override void AddToComponentSystem()
    {
        Owner.ComponentSystem.AddComponent(this);
    }

    public bool CanInteract() => _best != null;
}
