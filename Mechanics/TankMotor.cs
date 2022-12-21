using UnityEngine;

public class TankMotor : Motor
{
    [SerializeField] private float _angleTreshold;
    protected override void Move(Vector2 dir)
    {
        if (Vector2.Angle(dir, Cached.up) < _angleTreshold)
        {
            base.Move(dir);
        }
        else
        {
            base.Look(dir);
        }
    }
}
