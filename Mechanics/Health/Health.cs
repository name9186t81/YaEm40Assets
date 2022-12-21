using System;
using UnityEngine;

public class Health : IHealth
{
    private int _maxHealth;
    private int _currentHealth;

    public event Action<DamageArgs> OnDamage;
    public event Action<DamageArgs> OnDeath;
    public event Action<DamageArgs> OnPreDamage;
    public event Action OnHeal;

    public Health(int maxHealth, int currentHealth)
    {
        _maxHealth = maxHealth;
        _currentHealth = currentHealth;
    }

    public Health(int maxHealth) : this(maxHealth, maxHealth) { }

    public int Max => _maxHealth;

    public int Current => _currentHealth;

    public virtual void TakeDamage(DamageArgs args)
    {
        OnPreDamage?.Invoke(args);

        if (args.Damage == 0)
        {
            return;
        }

        _currentHealth -= args.Damage;

        if(_currentHealth < 0)
        {
            OnDeath?.Invoke(args);
            return;
        }

        OnDamage?.Invoke(args);
    }

    public void Heal(int ammount)
    {
        _currentHealth += ammount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
        OnHeal?.Invoke();
    }
}
