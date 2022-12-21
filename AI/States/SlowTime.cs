using UnityEngine;

public class SlowTime : IWeightState<AIController>
{
    private float _currentActivationProgress = 1f;
    private float _slowingTime;
    private float _chargingTime;
    private float _slowAmmount;
    private AIController _controller;
    private Timer _slowingTimer;
    private bool _active;

    public SlowTime(float slowingTime, float chargingTime, float slowAmmount)
    {
        _slowingTime = slowingTime;
        _chargingTime = chargingTime;
        _slowAmmount = slowAmmount;
        _slowingTimer = new Timer(_slowingTime);
        _slowingTimer.OnPeriodReached += DisableSlowment;
        _slowingTimer.Stop();
    }

    private void DisableSlowment()
    {
        _active = false;
        ServiceLocator.GetService<SlowMotionNotification>().EndSlowMotion(_slowAmmount);
        _controller.AttachedUnit.LocalTimeScale = 2f;
        _slowingTimer.Stop();
    }

    public float CalculateEffectivness()
    {
        return (_currentActivationProgress < 1f) ? -100f : 500f;
    }

    public void Execute()
    {
        if (_currentActivationProgress > 1f)
        {
            ServiceLocator.GetService<SlowMotionNotification>().DecreaseTimeFlow(_slowAmmount);
            _active = true;
            _controller.AttachedUnit.LocalTimeScale = 2f;
            _currentActivationProgress = 0f;
            _slowingTimer.Start();
        }
    }

    public void Init(AIController owner)
    {
        _controller = owner;
        _controller.OnUpdate += Update;
        _controller.AttachedUnit.Health.OnDeath += Death;
    }

    private void Death(DamageArgs obj)
    {
        if (_active)
        {
            DisableSlowment();
        }
    }

    private void Update(float obj)
    {
        _currentActivationProgress += Time.deltaTime / _chargingTime;
        _slowingTimer.Update(obj);
    }

    public void PreExecute()
    {
    }

    public void Undo()
    {
    }
}
