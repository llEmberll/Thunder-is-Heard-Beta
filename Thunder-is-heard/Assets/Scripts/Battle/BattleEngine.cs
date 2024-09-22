using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BattleEngine : MonoBehaviour
{
    public Map _map;

    public BattleSituation currentBattleSituation;


    public void Start()
    {
        InitMap();
        InitBattleSituation();
    }

    public void InitMap()
    {
        _map = GameObject.FindGameObjectWithTag(Tags.map).GetComponent<Map>();
    }

    public void InitBattleSituation()
    {
        BattleCacheTable battleTable = Cache.LoadByType<BattleCacheTable>();
        CacheItem cacheItem = battleTable.GetById(FightSceneLoader.parameters._battleId);
        BattleCacheItem battleData = new BattleCacheItem(cacheItem.Fields);

        currentBattleSituation = new BattleSituation();
        currentBattleSituation.InitByBattleDataAndMap(battleData, _map);
    }

    public int CalculateDamageToTarget(UnitOnBattle[] attackersData, Entity target)
    {
        if (target is Build)
        {
            BuildOnBattle buildData = FindBuildByIdAndSide(target.childId, target.side);
            return CalculateDamageToBuild(attackersData, buildData);
        }
        else
        {
            UnitOnBattle unitData = FindUnitByIdAndSide(target.childId, target.side);
            return CalculateDamageToUnit(attackersData, unitData);
        }
    }

    public static int CalculateDamageToBuild(UnitOnBattle[] attackersData, BuildOnBattle build)
    {
        int totalDamage = 0;
        foreach (var attacker in attackersData)
        {
            totalDamage += attacker.damage;
        }

        return totalDamage;
    }

    public static int CalculateDamageToUnit(UnitOnBattle[] attackersData, UnitOnBattle unit)
    {
        int totalDamage = 0;
        foreach (var attacker in attackersData)
        {
            int currentAttackerDamage = attacker.damage;


            // Суммирование влияния от умений и эффектов
            // Вычитание урона из-за умений и эффектов цели

            totalDamage += currentAttackerDamage;
        }

        return totalDamage;
    }

    public static int GetDistanceBetweenPoints(Bector2Int point1, Bector2Int point2)
    {
        int distanceX = Mathf.Abs(point1.x - point2.x);
        int distanceY = Mathf.Abs(point1.y - point2.y);
        return Mathf.Max(distanceX, distanceY);
    }

    public static int GetDistanceBetweenPointAndRectangleOfPoints(Bector2Int point, RectangleBector2Int rectangle)
    {
        // Находим минимальное расстояние по каждой оси
        int minDistanceX = Mathf.Min(
            Mathf.Abs(point.x - rectangle._startPosition.x),
            Mathf.Abs(point.x - (rectangle._startPosition.x + rectangle._size.x))
        );
        int minDistanceY = Mathf.Min(
            Mathf.Abs(point.y - rectangle._startPosition.y),
            Mathf.Abs(point.y - (rectangle._startPosition.y + rectangle._size.y))
        );

        // Возвращаем минимальное из минимальных расстояний
        return Mathf.Min(minDistanceX, minDistanceY);
    }

    public UnitOnBattle FindUnitByIdAndSide(string id, string side)
    {
        Dictionary<string, UnitOnBattle> units;
        if (side == Sides.federation)
        {
            units = currentBattleSituation.federationUnits;
        }
        else if (side == Sides.empire)
        {
            units = currentBattleSituation.empireUnits;
        }
        else if (side == Sides.neutral)
        {
            units = currentBattleSituation.neutralUnits;
        }
        else
        {
            return null;
        }

        if (units.ContainsKey(id))
        {
            return units[id];
        }
        return null;
    }

    public BuildOnBattle FindBuildByIdAndSide(string id, string side)
    {
        Dictionary<string, BuildOnBattle> builds;
        if (side == Sides.federation)
        {
            builds = currentBattleSituation.federationBuilds;
        }
        else if (side == Sides.empire)
        {
            builds = currentBattleSituation.empireBuilds;
        }
        else if (side == Sides.neutral)
        {
            builds = currentBattleSituation.neutralBuilds;
        }
        else
        {
            return null;
        }

        if (builds.ContainsKey(id))
        {
            return builds[id];
        }
        return null;
    }
}
