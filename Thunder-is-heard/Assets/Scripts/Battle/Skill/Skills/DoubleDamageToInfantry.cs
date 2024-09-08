

public class DoubleDamageToInfantry : SkillModifier
{
    public string name = "DoubleDamageToInfantry";

    public override int Multiplier { get { return 2; } }


    public DoubleDamageToInfantry(
        string name,
        bool isActive,
        string targetType,
        int cooldown,
        int currentCooldown,
        Effect effect,
        ModifierType type,
        ICondition[] conditions
        ) : base(name, isActive, targetType, cooldown, currentCooldown, effect)
    {
        _type = type;
        _conditions = conditions;
    }
    

    public override bool CanUse()
    {
        return true;
    }

    public override void Use()
    {
        
    }
}