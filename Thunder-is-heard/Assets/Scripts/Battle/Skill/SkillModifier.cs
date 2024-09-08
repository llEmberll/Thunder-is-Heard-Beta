

public abstract class SkillModifier : Skill
{
    public ModifierType _type;
    public ModifierType Type { get { return _type; } }


    public abstract int Multiplier { get; }


    public ICondition[] _conditions;
    public ICondition[] Conditions { get { return _conditions; } }


    public SkillModifier(
        string name, 
        bool isActive, 
        string targetType, 
        int cooldown, 
        int currentCooldown, 
        Effect effect,
        int multiplier,
        ModifierType type, 
        ICondition[] conditions
        ) : base(name, isActive, targetType, cooldown, currentCooldown, effect)
    {
        _type = type;
        _conditions = conditions;
    }

    public SkillModifier(string name, bool isActive, string targetType, int cooldown, int currentCooldown, Effect effect) : base(name, isActive, targetType, cooldown, currentCooldown, effect)
    {
    }
}
