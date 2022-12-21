using UnityEngine;

public class TurretIdle : IWeightState<AIController>
{
    private float _spinSpeed = 45f;
    private AIController _owner;
    private float _previousAngle;

    public float CalculateEffectivness()
    {
        return (_owner.IsEnemyInRange(out _)) ? 0f : 1f;
    }

    public void Execute()
    {
        _previousAngle += SpinSpeedInRads * _owner.DeltaTime;

        Vector2 direction = new Vector2(Mathf.Sin(_previousAngle), Mathf.Cos(_previousAngle));
        _owner.LookAtPoint(direction + _owner.AttachedUnit.Position2D);
    }

    public void Init(AIController owner)
    {
        _owner = owner;
    }

    public void PreExecute()
    {
    }

    public void Undo()
    {
    }

    private float SpinSpeedInRads => _spinSpeed * Mathf.Deg2Rad;
}
