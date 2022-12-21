using System;

public class SlowMotionNotification : IService
{
    public event Action<float> OnSlowmotionStarted;
    public event Action<float> OnSlowmotionEnded;
    public float CurrentTimeScale { get; private set; } = 1f;
    public void DecreaseTimeFlow(float scale)
    {
        OnSlowmotionStarted?.Invoke(scale);
        CurrentTimeScale /= scale;
    }

    public void EndSlowMotion(float scale)
    {
        OnSlowmotionEnded?.Invoke(scale);
        CurrentTimeScale *= scale;
    }
}
