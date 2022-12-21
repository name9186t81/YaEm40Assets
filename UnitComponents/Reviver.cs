using UnityEngine;
using System.Collections;
using System;

public class Reviver : UnitComponent
{
    [SerializeField] private float _reviveTime;
    [SerializeField] private bool _lockMovment;
    private WaitForSeconds _delay;

    public void Ressurect(Unit fallen, Action OnRevive)
    {
        StartCoroutine(ReviveCoroutine(fallen, OnRevive));
    }

    private IEnumerator ReviveCoroutine(Unit unit, Action OnRevive)
    {
        yield return _delay;

        unit.gameObject.SetActive(true);
        unit.Health.Heal((int)(unit.Health.Max / 2f));
		OnRevive?.Invoke();
    }

    protected override void AddToComponentSystem()
    {
        _delay = new WaitForSeconds(_reviveTime);
        Owner.ComponentSystem.AddComponent(this);
    }
}
