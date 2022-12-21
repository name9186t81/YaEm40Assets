using System;
using System.Threading.Tasks;

public sealed class Timer
{
    private float _period;
    private bool _stopped = false;
    public Timer(float period)
    {
        _period = period;
    }

    public float CurrentTime { get; private set; }
    public float _awaitngCurrentTime { get; private set; }
    public event Action OnPeriodReached;

    public void Update(float delta)
    {
        if (_stopped)
        {
            return;
        }
        CurrentTime += delta;
        if (CurrentTime > _period && _period > -1f)
        {
            OnPeriodReached?.Invoke();
            CurrentTime = 0;
        }
    }

    public void Start()
    {
        _stopped = false;
    }
    public void Stop()
    {
        _stopped = true;
    }
    public async Task WaitForPeriod(float period)
    {
        while (_awaitngCurrentTime < period)
        {
            _awaitngCurrentTime += delta;
            await Task.Delay((int)(delta * 1000f));
        }
    }

    public float Progress => CurrentTime / _period;
    private float delta => UnityEngine.Time.deltaTime;
}