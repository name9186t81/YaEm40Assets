using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallenUnit : Interactable
{
    [SerializeField] private Unit _attachedTo;

    private void Start()
    {
        _attachedTo.Health.OnDeath += DisableUnit;
        transform.SetParent(null);
        gameObject.SetActive(false);
    }

    private void DisableUnit(DamageArgs obj)
    {
        _attachedTo.gameObject.SetActive(false);
        transform.position = _attachedTo.Position2D;
        gameObject.SetActive(true);
    }

    public override bool CanInteract(Unit unit) => unit.ComponentSystem.TryToGetComponent<Reviver>(out _) && !_attachedTo.gameObject.activeSelf;

    public override void Interact(Unit unit)
    {
        Debug.LogError("Res");
        if (unit.ComponentSystem.TryToGetComponent<Reviver>(out var reviver))
        {
            reviver.Ressurect(_attachedTo, () => gameObject.SetActive(false));
		}
    }
}
