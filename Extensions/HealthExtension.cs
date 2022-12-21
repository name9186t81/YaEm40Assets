public static class HealthExtension
{
    public static float DeltaHealth(this IHealth health) => (float)health.Current / health.Max;
}
