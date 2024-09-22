using System;


[System.Serializable]
public class UnitOnBattle
{
    public string coreId;
    public string idOnBattle;

    public Bector2Int position;
    public int rotation;

    public int maxHealth;
    public int health;
    public int damage;
    public int distance;
    public int mobility;

    public string side;

    public SkillOnBattle[] skillsData;
    public Effect[] effectsData;

    public UnitOnBattle(
        string coreUnitId, 
        Bector2Int unitPosition,
        int unitRotation, 
        int unitMaxHealth,
        int unitHealth, 
        int unitDamage,
        int unitDistance,
        int unitMobility,
        string unitSide, 
        string unitIdOnBattle = null,
        SkillOnBattle[] unitSkillsData = null,
        Effect[] unitEffects = null
        )
    {
        coreId = coreUnitId;
        position = unitPosition;
        rotation = unitRotation;
        maxHealth = unitMaxHealth;
        health = unitHealth;
        damage = unitDamage;
        distance = unitDistance;
        mobility = unitMobility;
        side = unitSide;
        skillsData = unitSkillsData;
        effectsData = unitEffects;

        if (unitIdOnBattle == null)
        {
            unitIdOnBattle = Guid.NewGuid().ToString();
        }
        idOnBattle = unitIdOnBattle;
    }


    public UnitOnBattle Clone()
    {
        return new UnitOnBattle(
            coreId,
            position,
            rotation,
            maxHealth,
            health,
            damage,
            distance,
            mobility,
            side,
            idOnBattle,
            skillsData,
            effectsData
        );
    }
}
