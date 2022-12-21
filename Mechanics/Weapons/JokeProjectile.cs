using UnityEngine;

public class JokeProjectile : Projectile
{
    [SerializeField] private int _maxDodges;
    [SerializeField] private Projectile _spawned;
    [SerializeField] private float _spawnDelay;
    private Timer _delayTimer;
    private Pool<Projectile> _projectiles;
    private int _currentDodges;

    protected override void Awake()
    {
        base.Awake();
        _delayTimer = new Timer(_spawnDelay);
        _delayTimer.OnPeriodReached += SpawnProjectile;
        _projectiles = new Pool<Projectile>(CreateProjectile);
    }

    private void SpawnProjectile()
    {
        var direction = Vector2Utils.GetRandomPointOnCircle();
        var projectile = _projectiles.GetFromPool();
        projectile.transform.up = direction;
        projectile.Init(_projectiles, Args, Position2D, 1f);
    }

    public override void Init(Pool<Projectile> pool, DamageArgs args, Vector2 position, float speedModifier)
    {
        base.Init(pool, args, position, speedModifier);
        _delayTimer.Start();
        _currentDodges = 0;
    }

    protected override void Hit(Transform target, Vector2 hit, Vector2 normal)
    {
        _currentDodges++;
        if (_currentDodges == _maxDodges)
        {
            base.Hit(target, hit, normal);
        }

        var reflected = Vector2.Reflect(Cached.up, normal);
        Cached.up = reflected;
        Cached.position = hit;
        Cached.position += Cached.up * 0.1f;
    }

    protected override void Update()
    {
        base.Update();
        _delayTimer.Update(Time.deltaTime);
    }

    private Projectile CreateProjectile() 
    {
        return Instantiate(_spawned, Vector2Utils.GetRandomPointOnCircle(), default, null);
    }
}
