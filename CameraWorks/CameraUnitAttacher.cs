using UnityEngine;
using System;

public class CameraUnitAttacher : MonoBehaviour
{
    [SerializeField] private Unit _unit;
    [SerializeField] private MobileController _playerController;
    public event Action<Unit> OnUnitChange;

    public bool ChangeUnit(Unit newUnit)
    {
        if (newUnit.TryChangeController(_playerController) && !newUnit.TryGetComponent<LinkedUnit>(out _))
        {
            if (_unit != null)
            {
                _unit.SetAllowingControllerChange = true;
                _unit.TryChangeController(_unit.GetComponent<AIController>());
                _unit.GetComponent<AIController>()._enabled = true;
            }
            _unit = newUnit;
            OnUnitChange?.Invoke(newUnit);
            _unit.SetAllowingControllerChange = false;
            return true;
        }
        return false;
    }

    //TODO: remove this debug only
    private void LateUpdate()
    {
        if (_unit == null || !_unit.gameObject.activeSelf)
        {
            var units = MonoBehaviour.FindObjectsOfType<Unit>();

            for (int i = 0, length = units.Length; i < length; i++)
            {
                var unit = units[i];
                if (ChangeUnit(unit))
                {
                    break;
                }
            }
        }
    }

    public Unit Attached => _unit;
}
