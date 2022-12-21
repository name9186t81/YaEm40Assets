using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private CameraUnitAttacher _attacher;
    [SerializeField] private Slider _healthDisplayer;
    private Unit _unit;

    private void Start()
    {
        if (_attacher.Attached != null)
        {
            _unit = _attacher.Attached;
            _unit.Health.OnDamage += Attacked;
            _unit.Health.OnHeal += UpdateBar;
        }
        _attacher.OnUnitChange += Change;
    }

    private void Attacked(DamageArgs obj)
    {
        UpdateBar();
    }

    private void UpdateBar()
    {
        _healthDisplayer.value = _unit.Health.DeltaHealth();
    }
    private void Change(Unit obj)
    {
        if (_unit != null)
        {
            _unit.Health.OnDamage -= Attacked;
            _unit.Health.OnHeal -= UpdateBar;
        }
        _unit = obj;
        _unit.Health.OnDamage += Attacked;
        _unit.Health.OnHeal += UpdateBar;
        UpdateBar();
    }
}
