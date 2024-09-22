

public class DoubleDamageToInfantry : AttackModifier
{
    public string name = "DoubleDamageToInfantry";

    public override int Multiplier { get { return 2; } }


    public DoubleDamageToInfantry(
        string name,
        bool isActive,
        string targetType,
        string targetUnitType,
        string targetUnitDoctrine,
        int cooldown,
        int currentCooldown,
        Effect effect,
        ICondition[] conditions
        ) : base(name, isActive, targetType, targetUnitType, targetUnitDoctrine, cooldown, currentCooldown, effect, conditions) 
    {
    }
    

    public override bool CanUse()
    {
        return true;
    }

    public override void Use()
    {
        
    }

    public override bool IsAllConditionsForWorkingComply(TurnData turnData)
    {
        return IsTargetCompy(turnData._target);
    }
}