using Newtonsoft.Json;
using System;


[System.Serializable]
public class BuildOnBattle: ObjectOnBattle
{
    public string workStatus;

    [JsonIgnore]
    public string WorkStatus {  get { return workStatus; } }

    public BuildOnBattle() { }

    public BuildOnBattle(
        string coreBuildId,
        Bector2Int[] buildPosition,
        int buildRotation,
        int buildMaxHealth,
        int buildHealth,
        int buildDamage,
        int buildDistance,
        string buildDoctrine,
        string buildSide, 
        string buildWorkStatus,
        string buildIdOnBattle = null
        )
    {
        coreId = coreBuildId;
        position = buildPosition;
        rotation = buildRotation;
        maxHealth = buildMaxHealth;
        health = buildHealth;
        damage = buildDamage;
        distance = buildDistance;
        doctrine = buildDoctrine;
        side = buildSide;
        workStatus = buildWorkStatus;

        if (buildIdOnBattle == null)
        {
            buildIdOnBattle = Guid.NewGuid().ToString();
        }
        idOnBattle = buildIdOnBattle;
    }

    public BuildOnBattle(Build build)
    {
        coreId = build.CoreId;
        position = Bector2Int.GetVector2IntListAsBector(build.occypiedPoses);
        rotation = build.rotation;
        maxHealth = build.maxHealth;
        health = build.currentHealth;
        damage = build.damage;
        distance = build.distance;
        doctrine = build._doctrine;
        side = build.side;
        workStatus = build.workStatus;

        string buildIdOnBattle = build.ChildId;
        if (buildIdOnBattle == null)
        {
            buildIdOnBattle = Guid.NewGuid().ToString();
        }
        idOnBattle = buildIdOnBattle;
    }


    public override ObjectOnBattle Clone()
    {
        return new BuildOnBattle(
            coreId,
            position,
            rotation,
            maxHealth,
            health,
            damage,
            distance,
            doctrine,
            side,
            workStatus,
            idOnBattle
        );
    }
}
