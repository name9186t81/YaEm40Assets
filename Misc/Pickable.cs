using UnityEngine;

public class Pickable : MonoBehaviour2D
{
    [field: SerializeField] public InventoryItem Item { get; private set; }

    public void Pick(Inventory inventory)
    {
        inventory.PickItem(this);
    }
}
