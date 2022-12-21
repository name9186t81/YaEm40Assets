using UnityEngine;

public static class QuaternionExtensions
{
    public static Quaternion LookAt2D(this Transform origpos, Vector2 target) => LookAt2D(origpos.position, target);
    public static Quaternion LookAtDirection(this Transform origpos, Vector2 target) => LookAt2D(Vector2.zero, target);
    public static Quaternion LookAt2D(this Vector2 origpos, Vector2 target)
    {
        Vector2 direction = target - origpos;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        return Quaternion.AngleAxis(angle, Vector3.forward);
    }
}