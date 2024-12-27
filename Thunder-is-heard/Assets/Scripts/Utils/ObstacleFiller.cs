using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


public class ObstacleFiller
{


    public static ObstacleOnBattle[] Fill(
        Dictionary<string, int> possibleObjectIdsWithChanceMultiplier, 
        Bector2Int mainRectangleForFill, 
        List<RectangleBector2Int> exclusionZones, 
        float fillChance,
        int[] possibleRotationValues,
        string side
        )
    {
        List<ObstacleOnBattle> obstacles = new List<ObstacleOnBattle>();

        for ( int x = 0; x < mainRectangleForFill._x;  x++ )
        {
            for ( int y = 0; y < mainRectangleForFill._y; y++ )
            {
                Bector2Int currentPosition = new Bector2Int( x, y );
                if (IsPositionOnExclusionZone(exclusionZones, currentPosition)) continue;
                if (!IsNeedFillByChance(fillChance)) continue;

                string objectId = GetRandomObjectIdWithChanceMultiplier(possibleObjectIdsWithChanceMultiplier);
                int rotation = GetRandomRotation(possibleRotationValues);

                ObstacleOnBattle obstacle = CreateObstacle(objectId, currentPosition, rotation, side);
                obstacles.Add( obstacle );
            }
        }

        return obstacles.ToArray();
    }

    public static bool IsPositionOnExclusionZone(List<RectangleBector2Int> zones, Bector2Int position)
    {
        foreach ( RectangleBector2Int zone in zones )
        {
            if (zone.Contains(position)) return true;
        }
        return false;
    }

    public static int GetRandomRotation(int[] possibleValues)
    {
        int index = UnityEngine.Random.Range(0, possibleValues.Length);
        return possibleValues[index];
    }

    public static string GetRandomObjectIdWithChanceMultiplier(Dictionary<string, int> objectIdsWithChanceMultiplier)
    {
        int totalWeight = objectIdsWithChanceMultiplier.Values.Sum();
        int randomWeight = UnityEngine.Random.Range(0, totalWeight);

        foreach (var keyValuePair in objectIdsWithChanceMultiplier)
        {
            randomWeight -= keyValuePair.Value;
            if (randomWeight <= 0)
            {
                return keyValuePair.Key;
            }
        }

        throw new InvalidOperationException("Dictionary is empty or has negative weights.");
    }

    public static bool IsNeedFillByChance(float chance)
    {
        if (chance <= 0f)
        {
            return false;
        }
        else if (chance >= 1f)
        {
            return true;
        }
        else
        {
            return UnityEngine.Random.Range(0f, 1f) < chance;
        }
    }

    public static ObstacleOnBattle CreateObstacle(string coreId, Bector2Int position, int rotation, string side)
    {
        return new ObstacleOnBattle(
                    coreObstacleId: coreId,
                    obstaclePosition: new Bector2Int[] { position },
                    obstacleRotation: rotation,
                    obstacleSide: side,
                    obstacleIdOnBattle: Guid.NewGuid().ToString()
                    );
    }
}
