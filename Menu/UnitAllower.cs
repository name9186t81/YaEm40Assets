using UnityEngine;
using UnityEngine.UI;

public class UnitAllower : MonoBehaviour
{
    [field: SerializeField] public string Name { get; private set; }
    [SerializeField] private Toggle _toggle;

    public bool isOn => _toggle.isOn;
}
