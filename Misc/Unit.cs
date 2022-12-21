using System;
using UnityEngine;
using UnityEngine.Events;

public sealed class Unit : MonoBehaviour2D, IControllable
{
    //used for some calculations
    [SerializeField] private float _unitSize;
    [SerializeField] private bool _allowChangeController;
    [SerializeField] private int _maxHealth;
    [SerializeField] private Reinforcment[] _reinforcments;
    [SerializeField] private UnityEvent OnDeath;
    [SerializeField] private string _name;

    [field: SerializeField] public bool LockRotationWithMovment { get; private set; } = false;
    [field: SerializeField] public bool Loadable { get; private set; }
    [field: SerializeField] public int teamNumber { get; set; }
    private IController _controller;
    private IHealth _unitHealth;
    private int _currentHealth;

    public event Action<Collision2D> OnCollision;
    public ComponentSystem<Unit> ComponentSystem { get; private set; }

    public event Action<IController, IController> OnControllerChange;
    public event Action OnTeamChange;
    public float LocalTimeScale { get; set; } = 1f;

    private void Awake()
    {
        _unitHealth = HealthFactory.CreateHealth(_maxHealth, _reinforcments);
        _unitHealth.TakeDamage(new DamageArgs(1, this));
        ComponentSystem = new ComponentSystem<Unit>(this);
        _unitHealth.OnDeath += Death;
    }

    private void Death(DamageArgs obj)
    {
        OnDeath?.Invoke();
    }

    private void Update()
    {
        _currentHealth = _unitHealth.Current;
    }

    public bool TryChangeController(IController controller)
    {
        if (_allowChangeController || _controller == null)
        {
            OnControllerChange?.Invoke(_controller, controller);
            _controller = controller;
            return true;
        }
        return false;
    }

    public void Init(int teamNumber)
    {
        this.teamNumber = teamNumber;
    }

    protected void ChangeTeamNumber(int newNumber)
    {
        teamNumber = newNumber;
        OnTeamChange?.Invoke();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnCollision?.Invoke(collision);
    }

    public bool SetAllowingControllerChange { set => _allowChangeController = value; }
    public IController CurrentController => _controller;
    public IHealth Health => _unitHealth;
    public float Size => _unitSize;
    public string UnitName => _name;
}
