public class ReportTargets : IWeightState<AIController>
{
    private TargetMapProvider _provider;
    private AIVision _vision;
    private AIController _controller;

    public float CalculateEffectivness()
    {
        //auto-state should never be executed
        return float.MinValue;
    }

    public void Execute()
    {
        throw new System.NotImplementedException();
    }

    public void Init(AIController owner)
    {
        _controller = owner;
        _vision = owner.Vision;
        owner.Vision.OnScan += Scan;
        ServiceLocator.TryGetService<TargetMapProvider>(out _provider);
    }

    private void Scan()
    {
        if (_vision.ScanResults.TryGetValue(ScannedUnitType.Enemy, out var enemys))
        {
            for (int i = 0, length = enemys.Count; i < length; i++)
            {
                _provider.AddTarget(_controller.AttachedUnit.teamNumber, enemys[i].Position2D);
            }
        }
    }

    public void PreExecute()
    {
    }

    public void Undo()
    {
    }
}
