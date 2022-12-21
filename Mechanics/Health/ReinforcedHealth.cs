
public class ReinforcedHealth : Health
{
    private Reinforcment[] _reinforcments;
    public ReinforcedHealth(int maxHealth, Reinforcment[] reinforcments) : base(maxHealth)
    {
        _reinforcments = reinforcments;
    }

    public ReinforcedHealth(int maxHealth, int startHealth, Reinforcment[] reinforcments) : base(maxHealth, startHealth)
    {
        _reinforcments = reinforcments;
    }

    public override void TakeDamage(DamageArgs args)
    {
        if (TryToPickReinforcment(args.Type, out var result))
        {
            args.Damage = (int)(args.Damage * result.DamageMultiplayer);
        }
        base.TakeDamage(args);
    }

    private bool TryToPickReinforcment(DamageType type, out Reinforcment reinforcment)
    {
        for(int i = 0, length = _reinforcments.Length; i < length; i++)
        {
            if(_reinforcments[i].Type == type)
            {
                reinforcment = _reinforcments[i];
                return true;
            }
        }
        reinforcment = default;
        return false;
    }
}