

public class MoveWithAttack : Skill
{
    public string name = "Атака в движении";


    public MoveWithAttack()
    {

    }

    public MoveWithAttack(
        string coreId,
        string childId,
        string name,
        bool isActive,
        string targetType,
        string targetUnitType,
        string targetUnitDoctrine,
        int cooldown,
        int currentCooldown
        ) : base(coreId, childId, name, isActive, targetType, targetUnitType, targetUnitDoctrine, cooldown, currentCooldown) 
    {
    }
    

    public override bool CanUse()
    {
        return true;
    }

    public override void Use()
    {
        
    }
}