using UnityEngine;

public class AstarProvider : MonoBehaviour, IService
{
    [SerializeField] private AstarAbstract _astar;

    private void Awake()
    {
        if (ServiceLocator.TryGetService<AstarProvider>(out var provider))
        {
            provider._astar = _astar;
        }
        else
        {
            ServiceLocator.AddService<AstarProvider>(this);
        }
    }

    public AstarAbstract PathFinding => _astar;
}
