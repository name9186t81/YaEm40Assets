using System;

public interface IHealth
{
    int Max { get; }
    int Current { get; }
    void TakeDamage(DamageArgs args);
    void Heal(int ammount);

    event Action<DamageArgs> OnDamage;
    event Action OnHeal;
    event Action<DamageArgs> OnDeath;
    event Action<DamageArgs> OnPreDamage;
}
