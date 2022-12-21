using UnityEngine;

public class HitCreation : MonoBehaviour, IComponent<Projectile>
{
    public ComponentType ComponentType => ComponentType.Singletone;
    [SerializeField] private GameObject _inited;
    [SerializeField] private Projectile _attached;

    private void Awake()
    {
        _attached.OnHit += Hit;
    }

    public void Init(Projectile owner)
    {
        owner.OnHit += Hit;
    }

    private void Hit(Transform arg1, Vector2 arg2, Vector2 arg3)
    {
        Instantiate(_inited, arg2, Vector2.zero.LookAt2D(arg3), arg1);
    }
}
