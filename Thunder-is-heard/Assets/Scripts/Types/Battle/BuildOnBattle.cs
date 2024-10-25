using System;


[System.Serializable]
public class BuildOnBattle
{
    public string coreId;
    public string idOnBattle;

    public Bector2Int[] position;
    public int rotation;

    public int maxHealth;
    public int health;
    public int damage;
    public int distance;

    public string side;
    public string workStatus;

    public BuildOnBattle() { }

    public BuildOnBattle(
        string coreBuildId,
        Bector2Int[] buildPosition,
        int buildRotation,
        int buildMaxHealth,
        int buildHealth,
        int buildDamage,
        int buildDistance,
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
        side = build.side;
        workStatus = build.workStatus;

        string buildIdOnBattle = build.ChildId;
        if (buildIdOnBattle == null)
        {
            buildIdOnBattle = Guid.NewGuid().ToString();
        }
        idOnBattle = buildIdOnBattle;
    }


    public BuildOnBattle Clone()
    {
        return new BuildOnBattle(
            coreId,
            position,
            rotation,
            maxHealth,
            health,
            damage,
            distance,
            side,
            workStatus,
            idOnBattle
        );
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        BuildOnBattle other = (BuildOnBattle)obj;
        return idOnBattle == other.idOnBattle && coreId == other.coreId;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + idOnBattle.GetHashCode();
            hash = hash * 23 + coreId.GetHashCode();
            return hash;
        }
    }
}
