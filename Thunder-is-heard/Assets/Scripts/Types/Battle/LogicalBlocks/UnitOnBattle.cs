using Newtonsoft.Json;
using System;


[System.Serializable]
public class UnitOnBattle: ObjectOnBattle
{
    public int mobility;

    [JsonIgnore]
    public int Mobility {  get { return mobility; } }


    public string type;

    [JsonIgnore]
    public string Type {  get { return type; } }


    public SkillOnBattle[] skillsData;

    [JsonIgnore]
    public SkillOnBattle[] SkillsData { get { return skillsData; } }


    public Effect[] effectsData;

    [JsonIgnore]
    public Effect[] EffectsData { get { return effectsData; } }



    public UnitOnBattle() { }

    public UnitOnBattle(
        string coreUnitId, 
        Bector2Int[] unitPosition,
        int unitRotation, 
        int unitMaxHealth,
        int unitHealth, 
        int unitDamage,
        int unitDistance,
        int unitMobility,
        string unitType,
        string unitDoctrine,
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
        type = unitType;
        doctrine = unitDoctrine;
        side = unitSide;
        skillsData = unitSkillsData;
        effectsData = unitEffects;

        if (unitIdOnBattle == null)
        {
            unitIdOnBattle = Guid.NewGuid().ToString();
        }
        idOnBattle = unitIdOnBattle;
    }

    public UnitOnBattle(Unit unit)
    {
        coreId = unit.CoreId;
        position = new Bector2Int[] { new Bector2Int(unit.center) };
        rotation = unit.rotation;
        maxHealth = unit.maxHealth;
        health = unit.currentHealth;
        damage = unit.damage;
        distance = unit.distance;
        mobility = unit.mobility;
        type = unit._unitType;
        doctrine = unit._doctrine;
        side = unit.side;

        //skillsData = реализовать;
        //effectsData = реализовать;

        string unitIdOnBattle = unit.ChildId;
        if (unitIdOnBattle == null)
        {
            unitIdOnBattle = Guid.NewGuid().ToString();
        }
        idOnBattle = unitIdOnBattle;
    }


    public override ObjectOnBattle Clone()
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
            type,
            doctrine,
            side,
            idOnBattle,
            skillsData,
            effectsData
        );
    }
}
