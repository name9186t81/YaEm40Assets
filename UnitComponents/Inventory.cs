using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : UnitComponent
{
    [SerializeField] private int _maxInventorySize;
    private int _currentSize;

    [SerializeField] private List<InventoryItem> _items;

    protected override void AddToComponentSystem()
    {
        Owner.ComponentSystem.AddComponent(this);
    }

    public bool CanPickItem(Pickable item) => (_currentSize + item.Item.ReqiuredSpace) > _maxInventorySize;

    public void PickItem(Pickable item)
    {
        if (!CanPickItem(item)) return;
    }
}
