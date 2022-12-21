using UnityEngine;

public static class VectorExtensions
{
    public static Vector2 GetDirection(this Vector2 from, Vector2 to, bool normalized = true)
    {
        if (normalized)
        {
            return (from - to).normalized;
        }
        else
        {
            return from - to;
        }
    }

    public static Vector2 GetDirectionFromAngle(float angle)
    {
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }

    public static float RelativeDistance(this Vector2 left, Vector2 right)
    {
        float x = Mathf.Abs(left.x - right.x);
        float y = Mathf.Abs(left.y - right.y);
        return x + y;
    }

    public static Vector2 Rotate(this Vector2 vector, float angle)
    {
        return new Vector2(vector.x * Mathf.Cos(angle) - vector.y * Mathf.Sin(angle), vector.x * Mathf.Sin(angle) + vector.y * Mathf.Cos(angle));
    }
}
