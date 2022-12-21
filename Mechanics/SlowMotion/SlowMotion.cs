using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//TODO: create a ISlowMotionProvider interface with same functionality
public class SlowMotion : MonoBehaviour
{
    [SerializeField] private CameraUnitAttacher _attacher;
    [SerializeField] private Image _displayer;
    [SerializeField] private float _fullRechargeTime;
    [SerializeField] private int _chargesCount;
    [SerializeField] private float _slowedTimeValue;
    [SerializeField] private float _duration;
    [SerializeField] private bool _attachedUnitAnneffected;

    private Unit _unit;
    private float _currentCharge;
    private bool _activated;
    private WaitForSeconds _delay;

    private void Start()
    {
        ChangeUnit(_attacher.Attached);
        _attacher.OnUnitChange += ChangeUnit;
        _delay = new WaitForSeconds(_duration);
    }

    private void ChangeUnit(Unit obj)
    {
        _unit = obj;
        if (_activated && _attachedUnitAnneffected && obj != null)
        {
            obj.LocalTimeScale = 1 / _slowedTimeValue;
        }
    }

    private void Update()
    {
        _displayer.fillAmount = _currentCharge;
        if (_activated)
        {
            return;
        }
        _currentCharge += Time.deltaTime / _fullRechargeTime;
        _currentCharge = Mathf.Clamp01(_currentCharge);
    }

    public void Activate()
    {
        if (_currentCharge > ChargeCost && !_activated)
        {
            _currentCharge -= ChargeCost;
            ServiceLocator.GetService<SlowMotionNotification>().DecreaseTimeFlow(_slowedTimeValue);
            StartCoroutine(WaitForEndSlowMotion());
            _activated = true;
        }
    }

    private IEnumerator WaitForEndSlowMotion()
    {
        yield return _delay;
        _activated = false;
        ServiceLocator.GetService<SlowMotionNotification>().EndSlowMotion(_slowedTimeValue);
    }
    private void OnValidate()
    {
        if (_displayer != null)
        {
            if (_displayer.type != Image.Type.Filled)
            {
                Debug.LogWarning("Image of " + gameObject.name + " supposed to filled changing it...");
                _displayer.type = Image.Type.Filled;
            }
        }
    }
    private float ChargeCost => 1f / _chargesCount;
}
