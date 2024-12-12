

public abstract class AttackModifier : Skill
{
    public abstract int Multiplier { get; }


    public ICondition[] _conditions;
    public ICondition[] Conditions { get { return _conditions; } }


    public AttackModifier()
    {

    }

    public AttackModifier(
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
        ) : base(coreId, childId, name, isActive, targetType, targetUnitType, targetUnitDoctrine, cooldown, currentCooldown)
    {
        _conditions = conditions;
    }

    public virtual bool IsAllConditionsForWorkingComply(TurnData turnData)
    {
        Entity target = unitsOnFight.FindObjectByChildId(turnData._targetIdOnBattle);
        if (target == null) target = buildsOnFight.FindObjectByChildId(turnData._targetIdOnBattle);

        if (IsTargetCompy(target)) return false;

        foreach (var condition in _conditions)
        {
            if (!condition.IsComply())
            {
                return false;
            }
        }

        return true;
    }
}
