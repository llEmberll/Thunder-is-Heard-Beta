using System;


[System.Serializable]
public class BuildOnBattle
{
    public string coreId;
    public string idOnBattle;
    public Bector2Int[] position;
    public int rotation;
    public int health;
    public string side;
    public string workStatus;

    public BuildOnBattle(
        string coreBuildId, 
        Bector2Int[] buildPosition,
        int buildRotation, 
        int buildHealth, 
        string buildSide, 
        string buildWorkStatus,
        string buildIdOnBattle = null
        )
    {
        coreId = coreBuildId;
        position = buildPosition;
        rotation = buildRotation;
        health = buildHealth;
        side = buildSide;
        workStatus = buildWorkStatus;

        if (buildIdOnBattle == null)
        {
            buildIdOnBattle = Guid.NewGuid().ToString();
        }
        idOnBattle = buildIdOnBattle;
    }

}
