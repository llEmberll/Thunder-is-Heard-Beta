using Newtonsoft.Json;
using System;


[System.Serializable]
public class ObstacleOnBattle: ObjectOnBattle
{
    [JsonIgnore]
    public int maxHealth;

    [JsonIgnore]
    public int health;

    [JsonIgnore]
    public int damage;

    [JsonIgnore]
    public int distance;


    public ObstacleOnBattle() { }

    public ObstacleOnBattle(
        string coreObstacleId,
        Bector2Int[] obstaclePosition,
        int obstacleRotation,
        string obstacleSide, 
        string obstacleIdOnBattle = null
        )
    {
        coreId = coreObstacleId;
        position = obstaclePosition;
        rotation = obstacleRotation;
        side = obstacleSide;

        if (obstacleIdOnBattle == null)
        {
            obstacleIdOnBattle = Guid.NewGuid().ToString();
        }
        idOnBattle = obstacleIdOnBattle;
    }

    public ObstacleOnBattle(Obstacle obstacle)
    {
        coreId = obstacle.CoreId;
        position = Bector2Int.GetVector2IntListAsBector(obstacle.occypiedPoses);
        rotation = obstacle.rotation;
        side = obstacle.side;

        string obstacleIdOnBattle = obstacle.ChildId;
        if (obstacleIdOnBattle == null)
        {
            obstacleIdOnBattle = Guid.NewGuid().ToString();
        }
        idOnBattle = obstacleIdOnBattle;
    }


    public override ObjectOnBattle Clone()
    {
        return new ObstacleOnBattle(
            coreId,
            position,
            rotation,
            side,
            idOnBattle
        );
    }
}
