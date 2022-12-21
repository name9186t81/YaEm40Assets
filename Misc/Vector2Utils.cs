using UnityEngine;

public static class Vector2Utils
{
    public static Vector2 GetRandomPointOnCircle()
    {
        float randomAngle = Random.Range(0, Mathf.PI * 2);
        return Vector2.up * Mathf.Sin(randomAngle) + Vector2.right * Mathf.Cos(randomAngle);
    }
    public static Vector2 GetRandomPointInsideCircle(float radius)
    {
        float randomAngle = Random.Range(0, Mathf.PI * 2);
        return (Vector2.up * Mathf.Sin(randomAngle) + Vector2.right * Mathf.Cos(randomAngle)) * Random.Range(0f, radius);
    }
}