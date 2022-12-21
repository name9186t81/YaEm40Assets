using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Motor : UnitComponent
{
    [SerializeField, Range(0, 10)] protected float SpeedLoss;
    [SerializeField] protected float MaxSpeed;
    [SerializeField] protected float MaxAccelaration;
    [SerializeField] protected float MaxRotationSpeed;
    protected Rigidbody2D Rigidbody2D;
    private Vector2 _velocity;
    private Vector2 _lastInput;

    public override void Init(Unit owner)
    {
        base.Init(owner);
        owner.OnControllerChange += OnControllerChange;
        if(TryGetExistingController(out var controller))
        {
            OnControllerChange(null, controller);
        }
        Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    protected virtual void FixedUpdate()
    {
        if (Rigidbody2D.IsSleeping()) return;
        Rigidbody2D.velocity = _velocity;
        if (_lastInput == Vector2.zero)
        {
            _velocity = Vector2.Lerp(_velocity, Vector2.zero, SpeedLoss * Time.deltaTime);
        }
        _lastInput = Vector2.zero;
    }
    protected virtual void Move(Vector2 dir)
    {
        _lastInput = dir.normalized;
        _velocity = _lastInput * MaxSpeed * Owner.LocalTimeScale;
        //_velocity.x = Mathf.MoveTowards(_velocity.x, velocity.x, maxSpeedChange);
        //_velocity.y = Mathf.MoveTowards(_velocity.y, velocity.y, maxSpeedChange);
    }
    protected void Look(Vector2 dir)
    {
        Cached.rotation = Quaternion.Lerp(Cached.rotation, Cached.LookAtDirection(dir), MaxRotationSpeed * Time.deltaTime * Owner.LocalTimeScale);
    }

    private void OnDisable()
    {
        if (Controller != null)
        {
            Controller.LookInput -= Look;
            Controller.MoveInput -= Move;
        }
    }

    private void OnEnable()
    {
        if (Controller != null)
        {
            Controller.LookInput += Look;
            Controller.MoveInput += Move;
        }
    }

    protected virtual void OnControllerChange(IController old, IController newc)
    {
        if (old != null)
        {
            old.LookInput -= Look;
            old.MoveInput -= Move;
        }
        if (newc != null)
        {
            newc.LookInput += Look;
            newc.MoveInput += Move;
        }
    }

    protected override void AddToComponentSystem()
    {
        Owner.ComponentSystem.AddComponent(this);
    }
    public virtual float GetMaxSpeed()
    {
        return MaxAccelaration;
    }
    public virtual float GetCurrentSpeed()
    {
        return Rigidbody2D.velocity.magnitude;
    }
    private float maxSpeedChange => MaxAccelaration * Time.deltaTime * Owner.LocalTimeScale;
}
