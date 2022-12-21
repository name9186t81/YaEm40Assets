public static class HealthFactory
{
    public static IHealth CreateHealth(int maxHealth, int currentHealth, Reinforcment[] reinforcments)
    {
        if (reinforcments != null)
        {
            return new ReinforcedHealth(maxHealth, currentHealth, reinforcments);
        }

        return new Health(maxHealth, currentHealth);
    }

    public static IHealth CreateHealth(int maxHealth, Reinforcment[] reinforcments) => CreateHealth(maxHealth, maxHealth, reinforcments);
    public static IHealth CreateHealth(int maxHealth) => new Health(maxHealth);
}
