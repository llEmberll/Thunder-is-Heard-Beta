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
}
