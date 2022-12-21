using System;
using UnityEngine;
using Joysticks;

public class MobileController : MonoBehaviour, IController
{
    [SerializeField] private CameraUnitAttacher _attacher;
    public event Action<Vector2> MoveInput;
    public event Action<Vector2> LookInput;
    public event Action<CommandKey> OnCommandInput;
    
    public IJoystickInputHandler AttackJoystick => _attackJoystickData;
    public IJoystickInputHandler MovmentJoystick => _moveJoystickData;

    [SerializeField] private Joystick _attackJoystick;
    [SerializeField] private Joystick _moveJoystick;

    private JoystickData _attackJoystickData = new JoystickData();
    private JoystickData _moveJoystickData = new JoystickData();

    private Unit _attached;
    private bool _canRotate = true;
    private void Awake()
    {
        MovmentJoystick.OnMove += MovmentJoystickChange;
        AttackJoystick.OnMove += HandleLookPosition;
        _attached = _attacher.Attached;
        _attacher.OnUnitChange += ChangeUnit;
    }

    private void ChangeUnit(Unit obj)
    {
        _attached = obj;
    }

    private void MovmentJoystickChange(Vector2 obj)
    {
        MoveInput?.Invoke(obj);
        HandleLookPosition(obj);
    }

    private void HandleLookPosition(Vector2 obj)
    {
        if (_attached == null) return;
        //attack joystick priority
        if (!TryToLookWithJoystick(_attackJoystickData))
        {
            if(!_attached.LockRotationWithMovment)
                TryToLookWithJoystick(_moveJoystickData);
        }
    }

    private bool TryToLookWithJoystick(JoystickData data)
    {
        Vector2 pos = data.LastPosition;
        if (pos != Vector2.zero && _canRotate)
        {
            LookInput?.Invoke(pos);
            return true;
        }
        return false;
    }

    public void AllowRotation(bool val)
    {
        _canRotate = val;
    }
    private void Update()
    {
        _attackJoystickData.Update(_attackJoystick.Direction);
        _moveJoystickData.Update(_moveJoystick.Direction);
    }

    private class JoystickData : IJoystickInputHandler
    {
        public Vector2 LastPosition;
        public event Action OnRelease;
        public event Action<Vector2> OnMove;

        public void Update(Vector2 position)
        {
            if(position == Vector2.zero && LastPosition != Vector2.zero)
            {
                OnRelease?.Invoke();
            }
            else if (position != Vector2.zero)
            {
                OnMove?.Invoke(position);
            }
            LastPosition = position;
        }
    }
}
