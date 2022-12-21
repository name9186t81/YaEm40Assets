using UnityEngine;

public class DestroyOnDeath : MonoBehaviour
{
    [SerializeField] private Unit _unit;
    [SerializeField] private GameObject[] _destroyed;

    private void Start()
    {
        _unit.Health.OnDeath += Death;
    }

    private void Death(DamageArgs obj)
    {
        Destroy(_unit.gameObject);
        for (int i = 0, length = _destroyed.Length; i < length; i++)
        {
            Destroy(_destroyed[i]);
        }
    }
}
