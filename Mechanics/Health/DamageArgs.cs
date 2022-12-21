using System;

public class DamageArgs : EventArgs
{
    public int Damage;
    public readonly Unit Attacker;
    public readonly DamageType Type;

    public DamageArgs(int damage, Unit attacker)
    {
        Damage = damage;
        Attacker = attacker;
    }
}
