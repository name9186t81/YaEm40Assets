using System;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour2D, IController
{
    public event Action<Vector2> MoveInput;
    public event Action<Vector2> LookInput;
    public event Action<CommandKey> OnCommandInput;

    [SerializeField] private LayerMask _unitsLayerMask;
    [SerializeField] private LayerMask _avoidedInWalk;
    [SerializeField] private AIStateMachinePreset _preset;
    public Unit AttachedUnit;
    private WeightedStateMachine<AIController> _stateMachhine = new WeightedStateMachine<AIController>();
    [field: SerializeField] public AIVision Vision { get; private set; }
    [field: SerializeField] public float EngagmentRadius { get; private set; }
    public bool _enabled = true;
    private Vector2 _positionInlastFrame => Position2D;
    public event Action<float> OnUpdate;

    private void Start()
    {
        if (!AttachedUnit.TryChangeController(this))
        {
            Debug.LogError("Cannot change controller of the " + AttachedUnit.name);
            _enabled = false;
        }

        AttachedUnit.OnControllerChange += ChangeController;
        _stateMachhine.Init(_preset.GetStates(AttachedUnit), this);
        if (!ServiceLocator.TryGetService<SlowMotionNotification>(out _))
        {
            ServiceLocator.AddService<SlowMotionNotification>(new SlowMotionNotification());
        }

        ServiceLocator.GetService<SlowMotionNotification>().OnSlowmotionStarted += SlowMotionStarted;
        ServiceLocator.GetService<SlowMotionNotification>().OnSlowmotionEnded += SlowMotionEnded;
    }

    private void OnEnable()
    {
        if (!ServiceLocator.TryGetService<SlowMotionNotification>(out _))
        {
            ServiceLocator.AddService<SlowMotionNotification>(new SlowMotionNotification());
        }

        AttachedUnit.LocalTimeScale = ServiceLocator.GetService<SlowMotionNotification>().CurrentTimeScale;
    }

    private void OnDestroy()
    {
        ServiceLocator.GetService<SlowMotionNotification>().OnSlowmotionStarted -= SlowMotionStarted;
        ServiceLocator.GetService<SlowMotionNotification>().OnSlowmotionEnded -= SlowMotionEnded;
    }
    private void SlowMotionEnded(float timeScale)
    {
        if (_enabled)
        {
            AttachedUnit.LocalTimeScale *= timeScale;
        }
    }

    private void SlowMotionStarted(float obj)
    {
        if (_enabled)
        {
            AttachedUnit.LocalTimeScale /= obj;
        }
    }

    private void ChangeController(IController old, IController newController)
    {
        _enabled = false;
        AttachedUnit.LocalTimeScale = 1f;

        ServiceLocator.GetService<SlowMotionNotification>().OnSlowmotionStarted -= SlowMotionStarted;
        ServiceLocator.GetService<SlowMotionNotification>().OnSlowmotionEnded -= SlowMotionEnded;
    }

    private void Update()
    {
        if (_enabled)
        {
            _stateMachhine?.Execute();
            OnUpdate?.Invoke(Time.deltaTime);
        }
    }
    public void MoveToPoint(Vector2 point)
    {
        MoveInput?.Invoke(point - AttachedUnit.Position2D);
    }
    public void LookAtPoint(Vector2 point)
    {
        LookInput?.Invoke(point - AttachedUnit.Position2D);
    }
    public void LookAtDirection(Vector2 point)
    {
        LookInput?.Invoke(point);
    }
    public void MoveToDirection(Vector2 direcion)
    {
        MoveInput?.Invoke(direcion);
    }
    public void WriteCommand(CommandKey command)
    {
        OnCommandInput?.Invoke(command);
    }

    public bool SafeToShoot(Vector2 position)
    {
        var raycast = Physics2D.RaycastAll(AttachedUnit.Position2D, position - AttachedUnit.Position2D, Vector2.Distance(position, AttachedUnit.Position2D), _unitsLayerMask);
        if (raycast == null || (raycast.Length == 1 && raycast[0].transform == AttachedUnit.transform) || raycast.Length == 0)
        {
            return true;
        }

        if (raycast.Length == 1 && raycast[0].transform != AttachedUnit.transform)
        {
            return true;
        }

        Unit unit = default;
        if (raycast[1].transform.TryGetComponent<Unit>(out unit))
        {
            return unit.teamNumber != AttachedUnit.teamNumber;
        }

        return true;
    }

    public void SafeWalk(Vector2 position)
    {
        if (HandleWalking(_positionInlastFrame - AttachedUnit.Position2D, AttachedUnit.Size * 2f, out var result, out var isRightPos))
        {
            if (isRightPos)
            {
                MoveToDirection(-AttachedUnit.transform.right);
            }
            else
            {
                MoveToDirection(AttachedUnit.transform.right);
            }
        }
        else
        {
            MoveToPoint(position);
        }
    }

    public bool IsEnemyInRange(out List<Unit> enemys)
    {
        if(Vision.ScanResults.TryGetValue(ScannedUnitType.Enemy, out var list))
        {
            enemys = list;
            return true;
        }
        enemys = null;
        return false;
    }
    public bool HandleWalking(Vector2 direction, float distance, out RaycastHit2D result, out bool rightPosition)
    {
        var raycast1 = Physics2D.RaycastAll(AttachedUnit.Position2D + (Vector2)AttachedUnit.transform.right * AttachedUnit.Size, direction, distance, _avoidedInWalk);
        var raycast2 = Physics2D.RaycastAll(AttachedUnit.Position2D - (Vector2)AttachedUnit.transform.right * AttachedUnit.Size, direction, distance, _avoidedInWalk);

        if((raycast1 == null && raycast2 == null) ||
            (raycast1.Length == 1 && raycast1[0].transform == AttachedUnit) &&
            (raycast2.Length == 1 && raycast2[0].transform == AttachedUnit))
        {
            rightPosition = false;
            result = default;
            return false;
        }

        if(raycast1 != null && raycast1.Length > 1 && raycast1[0].transform == AttachedUnit.transform)
        {
            rightPosition = true;
            result = raycast1[1];
            return true;
        }

        if (raycast2 != null && raycast2.Length > 1 &&  raycast2[0].transform == AttachedUnit.transform)
        {
            rightPosition = false;
            result = raycast2[1];
            return true;
        }

        rightPosition = false;
        result = default;
        return false;
    }

    public float DeltaTime => Time.deltaTime;
    public LayerMask Avoided => _avoidedInWalk;
}
