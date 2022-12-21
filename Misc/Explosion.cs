using UnityEngine;

public class Explosion : MonoBehaviour2D
{
    [SerializeField] private int _damage;
    [SerializeField] private float _radius;
    [SerializeField] private float _maxScale;
    [SerializeField] private float _explosionTime;
    private bool _inited = false;

    private void Update()
    {
        if (_inited)
        {
            Cached.localScale += DeltaScale * Time.deltaTime * Vector3.one;
        }
    }

    private void OnEnable()
    {
        Cached.localScale = Vector2.zero;
    }

    private void OnDisable()
    {
        _inited = false;
    }
    public void Init(Unit attacker)
    {
        Cached.localScale = Vector2.zero;
        var overlap = Physics2D.OverlapCircleAll(Position2D, _radius);

        if (overlap != null)
        {
            for (int i = 0, length = overlap.Length; i < length; i++)
            {
                if (overlap[i].transform.TryGetComponent<Unit>(out var unit))
                {
                    unit.Health.TakeDamage(new DamageArgs(_damage, attacker));
                }
            }
        }
        _inited = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }

    private float DeltaScale => _maxScale / _explosionTime;
}
