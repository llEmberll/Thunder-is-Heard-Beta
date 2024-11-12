using Microsoft.EntityFrameworkCore.Storage;
using Org.BouncyCastle.Asn1.X509;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;


public class BattleEngine : MonoBehaviour
{
    public Map _map;

    public BattleSituation currentBattleSituation;


    public void Awake()
    {
        
    }

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

    public static int CalculateDamageToEntity(BattleSituation battleSituation, ObjectOnBattle[] attackersData, Entity target)
    {
        if (target is Build)
        {
            BuildOnBattle buildData = FindBuildByIdAndSide(battleSituation, target.childId, target.side);
            return CalculateDamageToBuild(battleSituation, attackersData, buildData);
        }
        else
        {
            UnitOnBattle unitData = FindUnitByIdAndSide(battleSituation, target.childId, target.side);
            return CalculateDamageToUnit(battleSituation, attackersData, unitData);
        }
    }

    public static int CalculateDamageToTargetById(BattleSituation battleSituation, ObjectOnBattle[] attackersData, string targetId)
    {
        UnitOnBattle foundedUnit = battleSituation.GetUnitById(targetId);
        if (foundedUnit != null)
        {
            return CalculateDamageToUnit(battleSituation, attackersData, foundedUnit);
        }

        BuildOnBattle foundedBuild = battleSituation.GetBuildById(targetId);
        if (foundedBuild != null)
        {
            return CalculateDamageToBuild(battleSituation, attackersData, foundedBuild);
        }

        return 0;
    }

    public static int CalculateDamageToBuild(BattleSituation battleSituation, ObjectOnBattle[] attackersData, BuildOnBattle build)
    {
        int totalDamage = 0;
        foreach (var attacker in attackersData)
        {
            totalDamage += attacker.damage;
        }

        return totalDamage;
    }

    // Возможно стоит расширить интерфейс до BattleSituation чтобы изменять эффекты
    public static int CalculateDamageToUnit(BattleSituation battleSituation, ObjectOnBattle[] attackersData, UnitOnBattle unit)
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
            Mathf.Abs(point._x - (rectangle._startPosition._x + (rectangle._size._x - 1)))
        );
        int minDistanceY = Mathf.Min(
            Mathf.Abs(point._y - rectangle._startPosition._y),
            Mathf.Abs(point._y - (rectangle._startPosition._y + (rectangle._size._y - 1)))
        );

        // Возвращаем максимальное из минимальных расстояний
        int maxDistance = Mathf.Max(minDistanceX, minDistanceY);
        return maxDistance;
    }

    public List<Cell> GetCellsByBector2IntPositions(List<Bector2Int> positions)
    {
        List<Vector2Int> routeAsVector2List = Bector2Int.MassiveToVector2Int(positions.ToArray()).ToList();
        return _map.FindCellsByPosition(routeAsVector2List).Values.ToList();
    }

    
    public static float GetObjectPower(BattleSituation battleSituation, string objectId)
    {
        BuildOnBattle foundedBuild = battleSituation.GetBuildById(objectId);
        if (foundedBuild != null)
        {
            return GetBuildPower(battleSituation, foundedBuild);
        }

        UnitOnBattle foundedUnit = battleSituation.GetUnitById(objectId);
        if (foundedUnit != null)
        {
            return GetUnitPower(battleSituation, foundedUnit);
        }

        throw new System.Exception("Объект не найден: " + objectId);
    }

    // Прикрутить учет скилов
    // У скилов реализовать интерфейс получения мощи, использующий реализацию для подсчета
    public static float GetUnitPower(BattleSituation battleSituation, UnitOnBattle unit)
    {
        List<ObjectOnBattle> targets = battleSituation.GetTargetsByAttacker(unit);
        int targetsCount = targets.Count();
        float powerFromTargetsCount = targetsCount > 0 ? Mathf.Log10(targetsCount) : targetsCount;
        int maxDamageOnTarget = CalculateGreatesDamageByAttackerAndTargets(battleSituation, unit, targets.ToArray());
        int totalDamageFromAttackers = CalculateDamageToTargetById(
            battleSituation,
            battleSituation.GetAttackersByTarget(unit).ToArray(),
            unit.IdOnBattle
        );

        float powerFromDistance = unit.Distance * 4;
        float powerFromDamage = unit.Damage + maxDamageOnTarget + powerFromTargetsCount;
        float powerFromHealth = Mathf.Clamp((unit.Health - totalDamageFromAttackers), 0, unit.Health);
        float powerFromMobility = unit.Mobility / 2;
        float powerFromSkills = 0; // Реализовать

        float totalPower = powerFromDistance + powerFromDamage + powerFromHealth + powerFromMobility + powerFromSkills;
        return totalPower;
    }

    public static float GetBuildPower(BattleSituation battleSituation, BuildOnBattle build)
    {
        if (build.Damage == 0 || build.Distance == 0)
        {
            return 0;
        }

        List<ObjectOnBattle> targets = battleSituation.GetTargetsByAttacker(build);
        int targetsCount = targets.Count();
        float powerFromTargetsCount = targetsCount > 0 ? Mathf.Log10(targetsCount) : targetsCount;
        int maxDamageOnTarget = CalculateGreatesDamageByAttackerAndTargets(battleSituation, build, targets.ToArray());
        int totalDamageFromAttackers = CalculateDamageToTargetById(
            battleSituation,
            battleSituation.GetAttackersByTarget(build).ToArray(),
            build.IdOnBattle
        );

        float powerFromDistance = build.Distance * 4;
        float powerFromDamage = build.Damage + maxDamageOnTarget + powerFromTargetsCount;
        float powerFromHealth = Mathf.Clamp((build.Health - totalDamageFromAttackers), 0, build.Health);

        float totalPower = (powerFromDistance + powerFromDamage + powerFromHealth) / 2;
        return totalPower;
    }

    public static int CalculateGreatesDamageByAttackerAndTargets(BattleSituation battleSituation, ObjectOnBattle attacker, ObjectOnBattle[] targets)
    {
        int greatestDamage = 0;
        foreach (ObjectOnBattle target in targets)
        {
            int currentDamage = CalculateDamageToTargetById(battleSituation, new ObjectOnBattle[] { attacker }, target.IdOnBattle);
            if (currentDamage > greatestDamage)
            {
                greatestDamage = currentDamage;
            }
        }

        return greatestDamage;
    }

    public static UnitOnBattle FindUnitByIdAndSide(BattleSituation battleSituation, string id, string side)
    {
        Dictionary<string, ObjectOnBattle> units;
        if (side == Sides.federation)
        {
            units = battleSituation.federationUnits;
        }
        else if (side == Sides.empire)
        {
            units = battleSituation.empireUnits;
        }
        else if (side == Sides.neutral)
        {
            units = battleSituation.neutralUnits;
        }
        else
        {
            return null;
        }

        if (units.ContainsKey(id))
        {
            return units[id] as UnitOnBattle;
        }
        return null;
    }

    public static BuildOnBattle FindBuildByIdAndSide(BattleSituation battleSituation, string id, string side)
    {
        Dictionary<string, ObjectOnBattle> builds;
        if (side == Sides.federation)
        {
            builds = battleSituation.federationBuilds;
        }
        else if (side == Sides.empire)
        {
            builds = battleSituation.empireBuilds;
        }
        else if (side == Sides.neutral)
        {
            builds = battleSituation.neutralBuilds;
        }
        else
        {
            return null;
        }

        if (builds.ContainsKey(id))
        {
            return builds[id] as BuildOnBattle;
        }
        return null;
    }

    public static bool IsPossibleToAttackTarget(BattleSituation battleSituation, Entity entity)
    {
        Dictionary<string, List<ObjectOnBattle>> attackersData = battleSituation.attackersByObjectId;
        return battleSituation.attackersByObjectId.ContainsKey(entity.ChildId);
    }

    public List<Cell> GetReachableCellsByUnit(BattleSituation battleSituation, Unit unit)
    {
        List<Bector2Int> possiblePositions = battleSituation.GetReachablePositionsByUnit(unit.ChildId);
        return _map.FindCellsByPosition(possiblePositions).Values.ToList();
    }

    public static Dictionary<string, UnitOnBattle> GetAllUnitsInBattle(BattleSituation battleSituation)
    {
        var unitsOnBattle = battleSituation.GetAllUnits()
            .Where(kvp => kvp.Value is UnitOnBattle)
            .ToDictionary(kvp => kvp.Key, kvp => (UnitOnBattle)kvp.Value);

        return unitsOnBattle;
    }

    public static Dictionary<string, BuildOnBattle> GetAllBuildsInBattle(BattleSituation battleSituation)
    {
        var buildsOnBattle = battleSituation.GetAllBuilds()
           .Where(kvp => kvp.Value is BuildOnBattle)
           .ToDictionary(kvp => kvp.Key, kvp => (BuildOnBattle)kvp.Value);

        return buildsOnBattle;
    }

    public static MapOnBattle GetMapOnBattle(BattleSituation battleSituation)
    {
        return battleSituation._map;
    }

    public void OnExposeObject(Entity obj)
    {
        Debug.Log("OnExposeObject in battleEngine");

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

    public static void OnReplaceUnit(BattleSituation battleSituation, Unit unit, Bector2Int newPosition)
    {
        battleSituation.UnitChangePosition(unit.ChildId, newPosition);
    }

    public static void OnAttackTarget(BattleSituation battleSituation, Entity entity, int damage)
    {
        int newHealthValue = entity.currentHealth - damage;
        if (entity is Unit) 
        {
            battleSituation.UnitChangeHealth(entity.ChildId, newHealthValue);
        }
        if (entity is Build)
        {
            battleSituation.BuildChangeHealth(entity.ChildId, newHealthValue);
        }
        
    }
}
