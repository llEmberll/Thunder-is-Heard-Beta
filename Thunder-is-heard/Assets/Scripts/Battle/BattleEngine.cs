using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class BattleEngine : MonoBehaviour
{
    public Map _map;

    public BattleSituation currentBattleSituation;


    public void Start()
    {
        InitMap();
        InitBattleSituation();
        EnableListeners();
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

    public void EnableListeners()
    {
        EventMaster.current.ObjectExposed += OnExposeObject;
    }

    public void DisableListeners()
    {
        EventMaster.current.ObjectExposed -= OnExposeObject;
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

    // Возможно стоит расширить интерфейс до BattleSituation чтобы изменять эффекты
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
        int distanceX = Mathf.Abs(point1._x - point2._x);
        int distanceY = Mathf.Abs(point1._y - point2._y);
        return Mathf.Max(distanceX, distanceY);
    }

    public static int GetDistanceBetweenPointAndRectangleOfPoints(Bector2Int point, RectangleBector2Int rectangle)
    {
        // Находим минимальное расстояние по каждой оси
        int minDistanceX = Mathf.Min(
            Mathf.Abs(point._x - rectangle._startPosition._x),
            Mathf.Abs(point._x - (rectangle._startPosition._x + rectangle._size._x))
        );
        int minDistanceY = Mathf.Min(
            Mathf.Abs(point._y - rectangle._startPosition._y),
            Mathf.Abs(point._y - (rectangle._startPosition._y + rectangle._size._y))
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

    public bool IsPossibleToAttackTarget(Entity entity)
    {
        return currentBattleSituation.attackersByObjectId.ContainsKey(entity.ChildId);
    }

    public List<Cell> GetReachableCellsByUnit(Unit unit)
    {
        List<Bector2Int> possiblePositions = currentBattleSituation.GetReachablePositionsByUnit(unit.ChildId);
        return _map.FindCellsByPosition(possiblePositions).Values.ToList();
    }

    public Dictionary<string, UnitOnBattle> GetAllUnitsInBattle()
    {
        return currentBattleSituation.GetAllUnits();
    }

    public Dictionary<string, BuildOnBattle> GetAllBuildsInBattle()
    {
        return currentBattleSituation.GetAllBuilds();
    }

    public MapOnBattle GetMapOnBattle()
    {
        return currentBattleSituation._map;
    }

    public void OnExposeObject(Entity obj)
    {
        if (obj is Unit unit)
        {
            OnExposeUnit(unit);
        }
        if (obj is Build build)
        {
            OnExposeBuild(build);
        }
    }

    public void OnExposeUnit(Unit unit)
    {
        UnitOnBattle unitOnBattle = new UnitOnBattle(unit);
        currentBattleSituation.AddUnit(unitOnBattle);
    }

    public void OnExposeBuild(Build build)
    {
        BuildOnBattle buildOnBattle = new BuildOnBattle(build);
        currentBattleSituation.AddBuild(buildOnBattle);
    }
}
