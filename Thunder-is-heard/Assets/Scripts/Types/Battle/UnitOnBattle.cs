using System;


[System.Serializable]
public class UnitOnBattle
{
    public string coreId;
    public string idOnBattle;
    public Bector2Int position;
    public int rotation;
    public int health;
    public string side;
    public SkillOnBattle skillData;

    public UnitOnBattle(
        string coreUnitId, 
        Bector2Int unitPosition,
        int unitRotation, 
        int unitHealth, 
        string unitSide, 
        string unitIdOnBattle = null,
        SkillOnBattle unitSkillData = null
        )
    {
        coreId = coreUnitId;
        position = unitPosition;
        rotation = unitRotation;
        health = unitHealth;
        side = unitSide;
        skillData = unitSkillData;

        if (unitIdOnBattle == null)
        {
            unitIdOnBattle = Guid.NewGuid().ToString();
        }
        idOnBattle = unitIdOnBattle;
    }
}
