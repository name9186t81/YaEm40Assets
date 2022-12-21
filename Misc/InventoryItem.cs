using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private Pickable _dropVariant;
    [field: SerializeField] public int ReqiuredSpace { get; private set; }


    public void Drop()
    {
        gameObject.SetActive(false);
        _dropVariant.gameObject.SetActive(true);
    }
}
