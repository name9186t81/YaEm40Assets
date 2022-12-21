using UnityEngine;

public class HealAura : UnitComponent
{
    [SerializeField] private int _healAmmount;
    [SerializeField] private float _delay;
    [SerializeField] private float _radius;
    private Timer _delayTimer;

    private void Awake()
    {
        _delayTimer = new Timer(_delay);
        _delayTimer.OnPeriodReached += Heal;
    }

    private void Heal()
    {
        var overlap = Physics2D.OverlapCircleAll(Position2D, _radius);

        if (overlap == null) return;

        for (int i = 0, length = overlap.Length; i < length; i++)
        {
            if (overlap[i].TryGetComponent<Unit>(out var unit) && unit.teamNumber == Owner.teamNumber)
            {
                unit.Health.Heal(_healAmmount);
            }
        }
    }

    private void Update()
    {
        _delayTimer.Update(Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _radius);
    }

    protected override void AddToComponentSystem()
    {
        Owner.ComponentSystem.AddComponent<HealAura>(this);
    }

    public float HealRadius => _radius;
}
