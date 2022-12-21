using UnityEngine;

public class ProjectileTrailCleaner : MonoBehaviour
{
    [SerializeField] private TrailRenderer _renderer;
    [SerializeField] private Projectile _projectile;

    private void Start()
    {
        _projectile.OnInit += Init;
        Init();
    }

    private void Init()
    {
        _renderer.Clear();
    }
}
