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
    public SkillOnBattle[] skillsData;

    public UnitOnBattle(
        string coreUnitId, 
        Bector2Int unitPosition,
        int unitRotation, 
        int unitHealth, 
        string unitSide, 
        string unitIdOnBattle = null,
        SkillOnBattle[] unitSkillsData = null
        )
    {
        coreId = coreUnitId;
        position = unitPosition;
        rotation = unitRotation;
        health = unitHealth;
        side = unitSide;
        skillsData = unitSkillsData;

        if (unitIdOnBattle == null)
        {
            unitIdOnBattle = Guid.NewGuid().ToString();
        }
        idOnBattle = unitIdOnBattle;
    }
}
