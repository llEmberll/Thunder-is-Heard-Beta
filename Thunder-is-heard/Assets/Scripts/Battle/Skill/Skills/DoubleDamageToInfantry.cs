

public class DoubleDamageToInfantry : AttackModifier
{
    public string name = "DoubleDamageToInfantry";

    public override int Multiplier { get { return 2; } }


    public DoubleDamageToInfantry(
        string coreId,
        string childId,
        string name,
        bool isActive,
        string targetType,
        string targetUnitType,
        string targetUnitDoctrine,
        int cooldown,
        int currentCooldown,
        ICondition[] conditions
        ) : base(coreId, childId,name, isActive, targetType, targetUnitType, targetUnitDoctrine, cooldown, currentCooldown, conditions) 
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
        Entity target = unitsOnFight.FindObjectByChildId(turnData._targetIdOnBattle);
        if (target == null) target = buildsOnFight.FindObjectByChildId(turnData._targetIdOnBattle);

        return IsTargetCompy(target);
    }
}