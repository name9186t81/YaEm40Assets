using UnityEngine;
using System.Collections;

public class RangedWeapom : Weapon
{
    private Pool<Projectile> _projectilePool;
    [SerializeField] private Projectile _projectile;
    [SerializeField, Range(1, 100)] private int _bulletsPerShot;
    [SerializeField] private int _maxCapacity;
    public int CurrentCapacity { get; private set; }
    [SerializeField] private float _delayBetweenShots;
    [SerializeField] private float _reloadingTime;
    [SerializeField] private float _delayBetweenFire;
    [SerializeField] private float _projectileSpeedModifier = 1f;
    [Header("Spread")]
    [SerializeField] private WeaponSpreadProvider _spreadProvider;
    [SerializeField] private float _spread;

    private WaitForSeconds _shotsDelay;
    private WaitForSeconds _fireDelay;
    private bool _canShoot = true;
    private bool _isFiring = false;

    public override void Init(Unit owner)
    {
        _projectilePool = new Pool<Projectile>(CreateProjectile);
        _shotsDelay = new WaitForSeconds(_delayBetweenShots);
        _fireDelay = new WaitForSeconds(_delayBetweenFire);
        CurrentCapacity = _maxCapacity;
        base.Init(owner);
    }

    public override void Attack()
    {
        if (!CanAttack())
        {
            return;
        }
        StartCoroutine(StartFiring());
    }

    private IEnumerator StartFiring()
    {
        if (_isFiring)
        {
            yield break;
        }
        _isFiring = true;
        _canShoot = false;
        for (int i = 0; i < _bulletsPerShot; i++)
        {
            var projectile = _projectilePool.GetFromPool();
            CurrentCapacity -= 1;
            InitProjectile(projectile);
            InvokeFire();
            if (CurrentCapacity < 0)
            {
                yield break;
            }
            if (_delayBetweenShots > 0f)
            {
                yield return new WaitForSeconds(_delayBetweenShots / Owner.LocalTimeScale);
            }
        }
        InvokeFireFinished();
        _isFiring = false;
        yield return new WaitForSeconds(_delayBetweenFire / Owner.LocalTimeScale);
        _canShoot = true;
    }

    private void OnDisable()
    {
        _canShoot = false;
    }

    private void OnEnable()
    {
        _canShoot = true;
    }

    private void InitProjectile(Projectile projectile)
    {
        projectile.Init(_projectilePool, FormArgs, _shootPosition.position, _projectileSpeedModifier);
        projectile.gameObject.SetActive(true);
        projectile.transform.rotation = Quaternion.Euler(0, 0, _spreadProvider.GetSpreadedAngle(_shootPosition.eulerAngles.z, _spread));
    }
    private Projectile CreateProjectile()
    {
        var projectile = Instantiate(_projectile, _shootPosition.position, default, null);
        projectile.Init(_projectilePool, FormArgs, _shootPosition.position, _projectileSpeedModifier);
        return projectile;
    }

    private float RandomSpread => Random.Range(-_spread, _spread);
    public override bool CanAttack() => _canShoot && CurrentCapacity > 0;

    protected override void AddToComponentSystem()
    {
        if (Owner.ComponentSystem.TryToGetComponent<Weapon>(out _))
        {
            Owner.ComponentSystem.RemoveComponent<Weapon>();
        }
        Owner.ComponentSystem.AddComponent(typeof(Weapon), this);
    }

    public override bool IsLockedInRotation() => _isFiring;
    public float Spread => _spread;
    private DamageArgs FormArgs => new DamageArgs(Damage, Owner);
}
