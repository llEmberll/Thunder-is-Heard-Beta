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
        SkillOnBattle[] unitSkillsData = null
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

        if (unit._skills == null ||  unit._skills.Length < 1)
        {
            skillsData = null;
        }
        else
        {
            SkillOnBattle[] collectedSkillsData = new SkillOnBattle[unit._skills.Length];

            int index = 0;
            foreach (Skill skill in unit._skills)
            {
                collectedSkillsData[index] = new SkillOnBattle(skill);
            }
            skillsData = collectedSkillsData;
        }
       
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
            skillsData
        );
    }
}
