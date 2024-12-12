using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class BattleSituation
{
    public Dictionary<string, ObjectOnBattle> federationUnits = new Dictionary<string, ObjectOnBattle>();
    public Dictionary<string, ObjectOnBattle> empireUnits = new Dictionary<string, ObjectOnBattle>();
    public Dictionary<string, ObjectOnBattle> neutralUnits = new Dictionary<string, ObjectOnBattle>();

    public Dictionary<string, ObjectOnBattle> federationBuilds = new Dictionary<string, ObjectOnBattle>();
    public Dictionary<string, ObjectOnBattle> empireBuilds = new Dictionary<string, ObjectOnBattle>();
    public Dictionary<string, ObjectOnBattle> neutralBuilds = new Dictionary<string, ObjectOnBattle>();

    public Dictionary<string, List<ObjectOnBattle>> attackersByObjectId = new Dictionary<string, List<ObjectOnBattle>>();

    public string _sideTurn;

    public MapOnBattle _map;


    public void InitByBattleDataAndMap(BattleCacheItem battleData, Map map)
    {
        InitMap(map.Cells);
        InitObstacles(battleData.GetObstacles());

        InitSideTurn(battleData.GetTurn());
        InitUnits(battleData.GetUnits());
        InitBuilds(battleData.GetBuilds());

        UpdateAttackers();
    }

    public void InitMap(Dictionary<Vector2Int, Cell> cellsByPosition)
    {
        CellData[] cells = new CellData[cellsByPosition.Count];

        int index = 0;
        foreach (var pair in cellsByPosition)
        {
            cells[index] = new CellData(Doctrines.land, new Bector2Int(pair.Key), pair.Value.occupied);
            index++;
        }

        _map = new MapOnBattle(cells);
    }

    public void InitMap(CellData[] cells)
    {
        _map = new MapOnBattle(cells);
    }

    public void InitObstacles(ObstacleOnBattle[] obstacles)
    {
        foreach (var obstacle in obstacles)
        {
            foreach (var position in obstacle.Position)
            {
                _map.Cells[position]._isOccypy = true;
            }
        }
    }

    public void InitSideTurn(string sideTurn)
    {
        _sideTurn = sideTurn;
    }

    public void InitUnits(UnitOnBattle[] units)
    {
        federationUnits = new Dictionary<string, ObjectOnBattle>();
        empireUnits = new Dictionary<string, ObjectOnBattle>();
        neutralUnits = new Dictionary<string, ObjectOnBattle>();

        foreach (var unit in units)
        {
            AddUnit(unit);
        }
    }

    public void InitBuilds(BuildOnBattle[] builds)
    {
        federationBuilds = new Dictionary<string, ObjectOnBattle>();
        empireBuilds = new Dictionary<string, ObjectOnBattle>();
        neutralBuilds = new Dictionary<string, ObjectOnBattle>();

        foreach (var build in builds)
        {
            AddBuild(build);
        }
    }

    public void AddUnit(UnitOnBattle unitData) // Добавить юнита в сражение
    {
        Dictionary<string, ObjectOnBattle> unitsBySide = GetUnitsCollectionBySide(unitData.side);
        unitsBySide.Add(unitData.idOnBattle, unitData);
        _map.Cells[unitData.Position.First()]._isOccypy = true;

        UpdateTargetsForAttacker(unitData);
        UpdateAttackersForUnitTarget(unitData);
        // Сделать перерасчет оценки позиции, когда она будет реализована
    }

    public void AddBuild(BuildOnBattle buildData) // Добавить здание в сражение
    {
        Dictionary<string, ObjectOnBattle> buildsBySide = GetBuildsCollectionBySide(buildData.side);
        buildsBySide.Add(buildData.idOnBattle, buildData);
        foreach (var position in buildData.Position)
        {
            _map.Cells[position]._isOccypy = true;
        }

        UpdateAttackersForBuildTarget(buildData);
    }

    public void RemoveUnit(UnitOnBattle unitData) // Удалить юнита из сражения
    {
        Dictionary<string, ObjectOnBattle> unitsBySide = GetUnitsCollectionBySide(unitData.side);
        ClearAttacker(unitData);
        ClearUnitAsTarget(unitData);
        unitsBySide.Remove(unitData.idOnBattle);
        _map.Cells[unitData.Position.First()]._isOccypy = false;

        // Сделать перерасчет оценки позиции, когда она будет реализована
    }

    public void RemoveBuild(BuildOnBattle buildData) // Удалить здание из сражения
    {
        Dictionary<string, ObjectOnBattle> buildsBySide = GetBuildsCollectionBySide(buildData.side);
        ClearBuildAsTarget(buildData);
        buildsBySide.Remove(buildData.idOnBattle);
        foreach (var position in buildData.position)
        {
            _map.Cells[position]._isOccypy = false;
        }
    }

    public void ObjectChangeHealth(string objectId, int newHealth)
    {
        UnitOnBattle unit = GetUnitById(objectId);
        if (unit != null)
        {
            UnitChangeHealth(objectId, newHealth);
            return;
        }

        BuildOnBattle build = GetBuildById(objectId);
        if (build != null)
        {
            BuildChangeHealth(objectId, newHealth);
            return;
        }
    }

    public void UnitChangePosition(string unitId, Bector2Int newPosition)
    {
        Dictionary<string, ObjectOnBattle> unitsCollection = GetUnitsCollectionById(unitId);
        UnitOnBattle unit = unitsCollection[unitId] as UnitOnBattle;

        _map.Cells[unit.Position.First()]._isOccypy = false;
        unit.position = new Bector2Int[] { newPosition };
        _map.Cells[unit.Position.First()]._isOccypy = true;

        unitsCollection[unitId] = unit;
        UpdateAttackersForUnitTarget(unit);
        UpdateTargetsForAttacker(unit);
    }

    public void UnitChangeSide(string unitId, string newSide)
    {
        Dictionary<string, ObjectOnBattle> unitsCollectionByOldSide = GetUnitsCollectionById(unitId);
        UnitOnBattle unit = unitsCollectionByOldSide[unitId] as UnitOnBattle;
        unitsCollectionByOldSide.Remove(unitId);

        Dictionary<string, ObjectOnBattle> unitsByNewSide = GetUnitsCollectionBySide(newSide);
        unitsByNewSide.Add(unitId, unit);
        UpdateAttackers();
    }


    public void UnitChangeHealth(string unitId, int newHealth)
    {
        Dictionary<string, ObjectOnBattle> unitsCollection = GetUnitsCollectionById(unitId);
        UnitOnBattle unit = unitsCollection[unitId] as UnitOnBattle;

        if (newHealth < 1)
        {
            RemoveUnit(unit);
        }
        else
        {
            unit.health = newHealth;
            unitsCollection[unitId] = unit;
            // Сделать перерасчет оценки позиции, когда она будет реализована
        }
    }

    public void UnitChangeAttributes(UnitOnBattle newUnitData)
    {
        Dictionary<string, ObjectOnBattle> unitsForSearchData = GetUnitsCollectionById(newUnitData.idOnBattle);
        UnitOnBattle oldUnitData = unitsForSearchData[newUnitData.idOnBattle] as UnitOnBattle;

        // Сделать перерасчет оценки позиции, когда она будет реализована

        unitsForSearchData[newUnitData.idOnBattle] = newUnitData;

        if (oldUnitData.distance != newUnitData.distance)
        {
            ClearAttacker(newUnitData);
            UpdateTargetsForAttacker(newUnitData);
        }
    }

    public void UnitUseSkill(string unitId, string skillId)
    {

    }

    public void UnitGotEffect(string unitId, string effectId)
    {

    }

    public void UnitLoseEffect(string unitId, string effectId)
    {

    }

    public void BuildChangePosition(string buildId, Bector2Int[] newPosition)
    {
        Dictionary<string, ObjectOnBattle> buildsCollection = GetBuildsCollectionById(buildId);
        BuildOnBattle build = buildsCollection[buildId] as BuildOnBattle;
        build.position = newPosition;
        buildsCollection[buildId] = build;
        UpdateAttackersForBuildTarget(build);
    }

    public void BuildChangeSide(string buildId, string newSide)
    {
        Dictionary<string, ObjectOnBattle> buildsCollectionByOldSide = GetBuildsCollectionById(buildId);
        BuildOnBattle build = buildsCollectionByOldSide[buildId] as BuildOnBattle;
        buildsCollectionByOldSide.Remove(buildId);

        Dictionary<string, ObjectOnBattle> buildsByNewSide = new Dictionary<string, ObjectOnBattle>();
        if (newSide == Sides.federation) buildsByNewSide = federationBuilds;
        if (newSide == Sides.empire) buildsByNewSide = empireBuilds;
        if (newSide == Sides.neutral) buildsByNewSide = neutralBuilds;

        buildsByNewSide.Add(buildId, build);
        UpdateAttackers();
    }

    public void BuildChangeHealth(string buildId, int newHealth)
    {
        Dictionary<string, ObjectOnBattle> buildsCollection = GetBuildsCollectionById(buildId);
        BuildOnBattle build = buildsCollection[buildId] as BuildOnBattle;

        if (newHealth < 1)
        {
            RemoveBuild(build);
        }
        else
        {
            build.health = newHealth;
            buildsCollection[buildId] = build;
            // Сделать перерасчет оценки позиции, когда она будет реализована
        }
    }

    public void BuildChangeAttributes(BuildOnBattle newBuildData)
    {
        Dictionary<string, ObjectOnBattle> buildsForSearchData = GetBuildsCollectionById(newBuildData.idOnBattle);
        ObjectOnBattle oldBuildData = buildsForSearchData[newBuildData.idOnBattle];

        // Сделать перерасчет оценки позиции, когда она будет реализована

        buildsForSearchData[newBuildData.idOnBattle] = newBuildData;
    }


    public void NextTurn()
    {
        this._sideTurn = GetNextSideTurn(this._sideTurn);
        RechargeSkillsCooldown();
    }

    public void SkipToSideTurn(string side)
    {
        if (this._sideTurn == side) return;

        for (int i = 0; i <= Sides.enemySideBySide.Keys.Count; i++)
        {
            NextTurn();
            if (this._sideTurn == side)
            {
                return;
            }
        }

        return;
    }

    public void RechargeSkillsCooldown()
    {
        Dictionary<string, ObjectOnBattle> allUnits = empireUnits.Concat(federationUnits).Concat(neutralUnits).ToDictionary(x => x.Key, x => x.Value);
        foreach (UnitOnBattle unit in allUnits.Values)
        {
            if (unit.skillsData == null || unit.skillsData.Length < 1) continue;
            foreach (SkillOnBattle skill in unit.skillsData)
            {
                skill.cooldown = Mathf.Max(skill.cooldown - 1, 0);
            }
        }
    }

    public void AddAttackerByObjectId(string objectId, ObjectOnBattle attacker)
    {
        if (attackersByObjectId.ContainsKey(objectId))
        {
            List<ObjectOnBattle> newAttackers = attackersByObjectId[objectId];
            if (!newAttackers.Contains(attacker))
            {
                newAttackers.Add(attacker);
            }
        }
        else
        {
            attackersByObjectId.Add(objectId, new List<ObjectOnBattle>() { attacker });
        }
    }


    public void ClearAttacker(ObjectOnBattle attacker)
    {
        var keysToClear = new List<string>();

        foreach (var item in attackersByObjectId)
        {
            if (item.Value.Contains(attacker))
            {
                item.Value.Remove(attacker);
                if (item.Value.Count < 1)
                {
                    keysToClear.Add(item.Key);
                }
            }
        }

        foreach (var key in keysToClear)
        {
            attackersByObjectId.Remove(key);
        }
    }

    public void RemoveAttackerByObjectIdIfExist(string objectId, ObjectOnBattle attacker)
    {
        if (!attackersByObjectId.ContainsKey(objectId)) return;

        List<ObjectOnBattle> newAttackersData = attackersByObjectId[objectId];
        if (!newAttackersData.Contains(attacker)) return;

        newAttackersData.Remove(attacker);
        if (newAttackersData.Count < 1)
        {
            attackersByObjectId.Remove(objectId);
        }
        else
        {
            attackersByObjectId[objectId] = newAttackersData;
        }
    }

    public void ClearUnitAsTarget(UnitOnBattle unitData)
    {
        attackersByObjectId.Remove(unitData.idOnBattle);
    }

    public void ClearBuildAsTarget(BuildOnBattle buildData)
    {
        attackersByObjectId.Remove(buildData.idOnBattle);
    }

    public void UpdateTargetsForAttacker(ObjectOnBattle attacker)
    {
        Dictionary<string, ObjectOnBattle> unitsForSearchData = GetUnitTargetsBySide(attacker.side);
        Dictionary<string, ObjectOnBattle> buildsForSearchData = GetBuildTargetsBySide(attacker.side);

        UnitOnBattle attackerAsUnit = attacker as UnitOnBattle;

        foreach (UnitOnBattle possibleUnitTarget in unitsForSearchData.Values)
        {
            if (CanObjectAttackObject(attackerAsUnit, possibleUnitTarget))
            {
                AddAttackerByObjectId(possibleUnitTarget.idOnBattle, attacker);
            }
            else
            {
                RemoveAttackerByObjectIdIfExist(possibleUnitTarget.idOnBattle, attacker);
            }
        }

        foreach (BuildOnBattle possibleBuildTarget in buildsForSearchData.Values)
        {
            if (CanObjectAttackObject(attackerAsUnit, possibleBuildTarget))
            {
                AddAttackerByObjectId(possibleBuildTarget.idOnBattle, attacker);
            }
            else
            {
                RemoveAttackerByObjectIdIfExist(possibleBuildTarget.idOnBattle, attacker);
            }
        }
    }

    public bool CanObjectAttackObject(ObjectOnBattle attacker, ObjectOnBattle target)
    {
        if (attacker is UnitOnBattle && CanMoveWithAttack(attacker as UnitOnBattle))
        {
            UnitOnBattle attackerAsUnit = attacker as UnitOnBattle;
            if (BattleEngine.GetDistanceBetweenPointAndRectangleOfPoints(attackerAsUnit.Position.First(), new RectangleBector2Int(target.Position)) < attackerAsUnit.Distance + attackerAsUnit.Mobility)
            {
                // Даже в чистом поле невозможно достать до цели
                return false;
            }

            List<Bector2Int> attackerReachablePositions = _map.GetReachablePositions(attackerAsUnit.Position.First(), attackerAsUnit.Mobility);
            foreach (Bector2Int position in attackerReachablePositions)
            {
                if (BattleEngine.GetDistanceBetweenPointAndRectangleOfPoints(position, new RectangleBector2Int(target.Position)) <= attackerAsUnit.Distance)
                {
                    return true;
                }
            }
            return false;
        }
        else
        {
            return BattleEngine.GetDistanceBetweenPointAndRectangleOfPoints(attacker.Position.First(), new RectangleBector2Int(target.Position)) <= attacker.Distance;
        }
    }

    public void UpdateAttackersForBuildTarget(BuildOnBattle build)
    {
        attackersByObjectId.Remove(build.idOnBattle);
        Dictionary<string, ObjectOnBattle> possibleAttackers = GetAttackersBySide(build.side);
        foreach (ObjectOnBattle possibleAttacker in possibleAttackers.Values)
        {
            if (CanObjectAttackObject(possibleAttacker, build)) 
            {
                AddAttackerByObjectId(build.idOnBattle, possibleAttacker);
            }
        }
    }

    public void UpdateAttackersForUnitTarget(UnitOnBattle unit)
    {
        attackersByObjectId.Remove(unit.idOnBattle);
        Dictionary<string, ObjectOnBattle> possibleAttackers = GetAttackersBySide(unit.side);
        foreach (ObjectOnBattle possibleAttacker in possibleAttackers.Values)
        {
            if (CanObjectAttackObject(possibleAttacker, unit))
            {
                AddAttackerByObjectId(unit.idOnBattle, possibleAttacker);
            }
        }
    }

    public void UpdateAttackers() // Обновить атакующих для всех объектов
    {
        attackersByObjectId = new Dictionary<string, List<ObjectOnBattle>>();
        UpdateAttackersForFederationObjects();
        UpdateAttackersForEmpireObjects();
        UpdateAttackersForNeutralObjects();
    }

    public void UpdateAttackersForFederationObjects()
    {
        UpdateAttackersForFederationUnits();
        UpdateAttackersForFederationBuilds();
    }

    public void UpdateAttackersForFederationUnits()
    {
        Dictionary<string, ObjectOnBattle> possibleAttackers = empireUnits;

        foreach (var victimItem in federationUnits)
        {
            foreach (var attackerItem in possibleAttackers)
            {
                if (CanObjectAttackObject(attackerItem.Value, victimItem.Value)) {
                    AddAttackerByObjectId(victimItem.Key, attackerItem.Value);
                }
            }
        }
    }

    public void UpdateAttackersForFederationBuilds()
    {
        Dictionary<string, ObjectOnBattle> possibleAttackers = empireUnits;

        foreach (var victimItem in federationBuilds)
        {
            foreach (var attackerItem in possibleAttackers)
            {
                if (CanObjectAttackObject(attackerItem.Value, victimItem.Value))
                {
                    AddAttackerByObjectId(victimItem.Key, attackerItem.Value);
                }
            }
        }
    }

    public void UpdateAttackersForEmpireObjects()
    {
        UpdateAttackersForEmpireUnits();
        UpdateAttackersForEmpireBuilds();
    }

    public void UpdateAttackersForEmpireUnits()
    {
        Dictionary<string, ObjectOnBattle> possibleAttackers = federationUnits;

        foreach (var victimItem in empireUnits)
        {
            foreach (var attackerItem in possibleAttackers)
            {
                if (CanObjectAttackObject(attackerItem.Value, victimItem.Value))
                {
                    AddAttackerByObjectId(victimItem.Key, attackerItem.Value);
                }
            }
        }
    }

    public void UpdateAttackersForEmpireBuilds()
    {
        Dictionary<string, ObjectOnBattle> possibleAttackers = federationUnits;

        foreach (var victimItem in empireBuilds)
        {
            foreach (var attackerItem in possibleAttackers)
            {
                if (CanObjectAttackObject(attackerItem.Value, victimItem.Value))
                {
                    AddAttackerByObjectId(victimItem.Key, attackerItem.Value);
                }
            }
        }
    }

    public void UpdateAttackersForNeutralObjects()
    {
        UpdateAttackersForNeutralUnits();
        UpdateAttackersForNeutralBuilds();
    }

    public void UpdateAttackersForNeutralUnits()
    {
        Dictionary<string, ObjectOnBattle> possibleAttackers = federationUnits;

        foreach (var victimItem in neutralUnits)
        {
            foreach (var attackerItem in possibleAttackers)
            {
                if (CanObjectAttackObject(attackerItem.Value, victimItem.Value))
                {
                    AddAttackerByObjectId(victimItem.Key, attackerItem.Value);
                }
            }
        }
    }

    public void UpdateAttackersForNeutralBuilds()
    {
        Dictionary<string, ObjectOnBattle> possibleAttackers = federationUnits;

        foreach (var victimItem in neutralBuilds)
        {
            foreach (var attackerItem in possibleAttackers)
            {
                if (CanObjectAttackObject(attackerItem.Value, victimItem.Value))
                {
                    AddAttackerByObjectId(victimItem.Key, attackerItem.Value);
                }
            }
        }
    }


    public bool CanMoveWithAttack(UnitOnBattle unit)
    {
        if (unit.skillsData == null || unit.skillsData.Length < 1) return false;

        foreach (var skill in unit.skillsData)
        {
            SkillCacheTable skillsTable = Cache.LoadByType<SkillCacheTable>();
            CacheItem cacheItem = skillsTable.GetById(skill.coreId);
            SkillCacheItem coreSkillData = new SkillCacheItem(cacheItem.Fields);
            if (coreSkillData.GetName() == "Move with attack") return true;
        }
        return false;
    }

    public Dictionary<string, ObjectOnBattle> GetPossibleTargetsBySide(string side)
    {
        string targetsSide = Sides.enemySideBySide[side];
        Dictionary<string, ObjectOnBattle> buildTargets = GetBuildTargetsBySide(targetsSide);
        Dictionary<string, ObjectOnBattle> unitTargets = GetUnitTargetsBySide(targetsSide);
        Dictionary<string, ObjectOnBattle> allTargets = buildTargets.Concat(unitTargets).ToDictionary(x => x.Key, x => x.Value);
        return allTargets;
    }

    public Dictionary<string, ObjectOnBattle> GetPossibleUnitTargetsBySide(string side)
    {
        string targetsSide = Sides.enemySideBySide[side];
        Dictionary<string, ObjectOnBattle> unitTargets = GetUnitTargetsBySide(targetsSide);
        return unitTargets;
    }

    public Dictionary<string, ObjectOnBattle> GetTargetsUnderAttackBySide(string side)
    {
        Dictionary<string, ObjectOnBattle> possibleTargets = GetPossibleTargetsBySide(side);
        Dictionary<string, ObjectOnBattle> targetsUnderAttack = new Dictionary<string, ObjectOnBattle>();

        foreach (var keyValuePair in possibleTargets)
        {
            string targetId = keyValuePair.Key;
            if (attackersByObjectId.ContainsKey(targetId))
            {
                targetsUnderAttack.Add(targetId, keyValuePair.Value);
            }
        }

        return targetsUnderAttack;
    }

    public Dictionary<string, ObjectOnBattle> GetUnitTargetsUnderAttackBySide(string side)
    {
        Dictionary<string, ObjectOnBattle> possibleTargets = GetPossibleUnitTargetsBySide(side);
        Dictionary<string, ObjectOnBattle> unitTargetsUnderAttack = new Dictionary<string, ObjectOnBattle>();

        foreach (var keyValuePair in possibleTargets)
        {
            string targetId = keyValuePair.Key;
            if (attackersByObjectId.ContainsKey(targetId))
            {
                unitTargetsUnderAttack.Add(targetId, keyValuePair.Value);
            }
        }

        return unitTargetsUnderAttack;
    }

    public BattleSituation Clone()
    {
        // Создаем новую копию BattleSituation
        BattleSituation clone = new BattleSituation();

        // Клонируем _map
        CellData[] clonedCells = new CellData[this._map.Cells.Count];
        int index = 0;
        foreach (var pair in this._map.Cells)
        {
            clonedCells[index] = pair.Value.Clone();
            index++;
        }
        clone.InitMap(clonedCells);

        // Клонируем словари юнитов
        clone.federationUnits = new Dictionary<string, ObjectOnBattle>();
        foreach (var pair in federationUnits)
        {
            clone.federationUnits.Add(pair.Key, pair.Value.Clone());
        }
        clone.empireUnits = new Dictionary<string, ObjectOnBattle>();
        foreach (var pair in empireUnits)
        {
            clone.empireUnits.Add(pair.Key, pair.Value.Clone());
        }
        clone.neutralUnits = new Dictionary<string, ObjectOnBattle>();
        foreach (var pair in neutralUnits)
        {
            clone.neutralUnits.Add(pair.Key, pair.Value.Clone());
        }

        // Клонируем словари зданий
        clone.federationBuilds = new Dictionary<string, ObjectOnBattle>();
        foreach (var pair in federationBuilds)
        {
            clone.federationBuilds.Add(pair.Key, pair.Value.Clone());
        }
        clone.empireBuilds = new Dictionary<string, ObjectOnBattle>();
        foreach (var pair in empireBuilds)
        {
            clone.empireBuilds.Add(pair.Key, pair.Value.Clone());
        }
        clone.neutralBuilds = new Dictionary<string, ObjectOnBattle>();
        foreach (var pair in neutralBuilds)
        {
            clone.neutralBuilds.Add(pair.Key, pair.Value.Clone());
        }

        // Клонируем словарь attackersByObjectId
        clone.attackersByObjectId = new Dictionary<string, List<ObjectOnBattle>>();
        foreach (var pair in attackersByObjectId)
        {
            clone.attackersByObjectId.Add(pair.Key, new List<ObjectOnBattle>());
            foreach (var unit in pair.Value)
            {
                clone.attackersByObjectId[pair.Key].Add(unit.Clone());
            }
        }

        // Клонируем _sideTurn
        clone._sideTurn = _sideTurn;

        return clone;
    }

    public UnitOnBattle FindNearestUnitByPointAndSide(Bector2Int point, string side)
    {
        ObjectOnBattle nearestUnit = null;
        int nearestDistance = int.MaxValue;

        Dictionary<string, ObjectOnBattle> unitsBySide = GetUnitsCollectionBySide(side);
        foreach (var keyValuePair in unitsBySide)
        {
            ObjectOnBattle currentUnit = keyValuePair.Value;
            List<Bector2Int> routeToCurrentUnit = _map.BuildRoute(point, currentUnit.Position.First(), int.MaxValue);
            if (routeToCurrentUnit.Count < nearestDistance) 
            { 
                nearestUnit = currentUnit;
                nearestDistance = routeToCurrentUnit.Count;
            }
        }

        return nearestUnit as UnitOnBattle;
    }

    public int FindDistanceToNearestUnitBySide(Bector2Int point, string side)
    {
        int nearestDistance = int.MaxValue;

        Dictionary<string, ObjectOnBattle> unitsBySide = GetUnitsCollectionBySide(side);
        foreach (var keyValuePair in unitsBySide)
        {
            ObjectOnBattle currentUnit = keyValuePair.Value;
            List<Bector2Int> routeToCurrentUnit = _map.BuildRoute(point, currentUnit.Position.First(), int.MaxValue);
            if (routeToCurrentUnit.Count < nearestDistance)
            {
                nearestDistance = routeToCurrentUnit.Count;
            }
        }

        return nearestDistance;
    }

    public MoveForAttackData GetDataForMoveToAttackNearestUnit(UnitOnBattle attacker, string attackerSide)
    {
        MoveForAttackData moveForAttackData = new MoveForAttackData();
        moveForAttackData._activeUnit = attacker;
        moveForAttackData._fullRoute = new List<Bector2Int>();

        ObjectOnBattle nearestUnit = null;
        int minPassedDistanceToAttack = int.MaxValue;

        Dictionary<string, ObjectOnBattle> unitsBySide = GetUnitsCollectionBySide(Sides.enemySideBySide[attackerSide]);
        foreach (var keyValuePair in unitsBySide)
        {
            ObjectOnBattle currentUnit = keyValuePair.Value;
            List<Bector2Int> shortestRouteToAttackCurrentUnit = _map.BuildRouteForAttackTarget(attacker.Position.First(), currentUnit.Position.First(), attackRange: attacker.Distance, int.MaxValue);
            if (shortestRouteToAttackCurrentUnit.Count < minPassedDistanceToAttack)
            {
                nearestUnit = currentUnit;
                minPassedDistanceToAttack = shortestRouteToAttackCurrentUnit.Count;
                moveForAttackData._fullRoute = shortestRouteToAttackCurrentUnit;
            }
        }

        moveForAttackData._turnCountForReach = minPassedDistanceToAttack / attacker.Mobility;
        if (CanMoveWithAttack(attacker)) moveForAttackData._turnCountForReach -= 1;
        return moveForAttackData;
    }

    public Dictionary<TurnData, BattleSituation> GetAllSequels()
    {
        Dictionary<TurnData, BattleSituation> battleSituationByTurn = new Dictionary<TurnData, BattleSituation>();
        battleSituationByTurn = battleSituationByTurn.Concat(GetAllMovementSequels()).ToDictionary(x => x.Key, x => x.Value);
        battleSituationByTurn = battleSituationByTurn.Concat(GetAllAttackingSequels()).ToDictionary(x => x.Key, x => x.Value);
        battleSituationByTurn = battleSituationByTurn.Concat(GetSequelWithPass()).ToDictionary(x => x.Key, x => x.Value);
        return battleSituationByTurn;
    }

    public Dictionary<TurnData, BattleSituation> GetAllMovementWithAttackSequels() 
    {
        Dictionary<TurnData, BattleSituation> battleSituationByTurn = new Dictionary<TurnData, BattleSituation>();
        Dictionary<string, ObjectOnBattle> buildTargets = new Dictionary<string, ObjectOnBattle>();
        Dictionary<string, ObjectOnBattle> unitTargets = new Dictionary<string, ObjectOnBattle>();
        Dictionary<string, ObjectOnBattle> possibleAttackers = new Dictionary<string, ObjectOnBattle>();

        if (_sideTurn == Sides.federation)
        {
            buildTargets = empireBuilds;
            unitTargets = empireUnits;
            possibleAttackers = federationUnits;
        }
        else if (_sideTurn == Sides.empire)
        {
            buildTargets = federationBuilds;
            unitTargets = federationUnits;
            possibleAttackers = empireUnits;
        }
        else
        {
            buildTargets = federationBuilds.Concat(empireBuilds).ToDictionary(x => x.Key, x => x.Value);
            unitTargets = federationUnits.Concat(empireUnits).ToDictionary(x => x.Key, x => x.Value);
            possibleAttackers = neutralUnits;
        }

        Dictionary<string, ObjectOnBattle> filteredAttackers = new Dictionary<string, ObjectOnBattle>();
        foreach (var item in possibleAttackers)
        {
            if (CanMoveWithAttack(item.Value as UnitOnBattle))
            {
                filteredAttackers.Add(item.Key, item.Value);
            }
        }

        if (filteredAttackers.Count < 1) return battleSituationByTurn;

        foreach (var item in filteredAttackers)
        {
            UnitOnBattle currentAttacker = item.Value as UnitOnBattle;
            List<ObjectOnBattle> targets = GetTargetsByAttacker(currentAttacker);
            if (targets.Count < 1) continue;

            foreach (ObjectOnBattle target in targets)
            {
                int totalDamage = BattleEngine.CalculateDamageToObject(this, attackersByObjectId[target.IdOnBattle].ToArray(), target);

                List<Bector2Int> allPossiblePositionsToMoveAttacker = GetReachablePositionsByUnit(currentAttacker.idOnBattle);
                List<Bector2Int> positionsFromWhichToAttack = new List<Bector2Int>();
                foreach (Bector2Int reachablePosition in allPossiblePositionsToMoveAttacker)
                {
                    if (BattleEngine.GetDistanceBetweenPointAndRectangleOfPoints(reachablePosition, new RectangleBector2Int(target.Position)) <= currentAttacker.Distance)
                    {
                        List<Bector2Int> routeToPosition = _map.BuildRoute(currentAttacker.Position.First(), reachablePosition, currentAttacker.Mobility);

                        BattleSituation currentBattleSituation = this.Clone();
                        currentBattleSituation.UnitChangePosition(currentAttacker.idOnBattle, reachablePosition);
                        currentBattleSituation.ObjectChangeHealth(target.idOnBattle, target.Health - totalDamage);
                        currentBattleSituation.NextTurn();

                        TurnData currentTurn = new TurnData();
                        currentTurn._targetIdOnBattle = target.IdOnBattle;
                        currentTurn._activeUnitIdOnBattle = currentAttacker.IdOnBattle;
                        currentTurn._route = routeToPosition;
                        battleSituationByTurn.Add(currentTurn, currentBattleSituation);
                    }
                }
            }
        }
        return battleSituationByTurn;
    }

    public Dictionary<TurnData, BattleSituation> GetAllAttackingSequels()
    {
        Dictionary<TurnData, BattleSituation> battleSituationByTurn = new Dictionary<TurnData, BattleSituation>();

        Dictionary<string, ObjectOnBattle> buildTargets = new Dictionary<string, ObjectOnBattle>();
        Dictionary<string, ObjectOnBattle> unitTargets = new Dictionary<string, ObjectOnBattle>();
        if (_sideTurn == Sides.federation)
        {
            buildTargets = empireBuilds;
            unitTargets = empireUnits;
        }
        else if (_sideTurn == Sides.empire)
        {
            buildTargets = federationBuilds;
            unitTargets = federationUnits;
        }
        else
        {
            buildTargets = federationBuilds.Concat(empireBuilds).ToDictionary(x => x.Key, x => x.Value);
            unitTargets = federationUnits.Concat(empireUnits).ToDictionary(x => x.Key, x => x.Value);
        }


       foreach (BuildOnBattle buildTarget in buildTargets.Values) 
        {
            if (!attackersByObjectId.ContainsKey(buildTarget.IdOnBattle)) continue;
            int totalDamage = BattleEngine.CalculateDamageToBuild(this, attackersByObjectId[buildTarget.IdOnBattle].ToArray(), buildTarget);
            BattleSituation currentBattleSituation = this.Clone();
            currentBattleSituation.BuildChangeHealth(buildTarget.IdOnBattle, buildTarget.Health - totalDamage);
            currentBattleSituation.NextTurn();

            TurnData currentTurn = new TurnData();
            currentTurn._targetIdOnBattle = buildTarget.IdOnBattle;

            battleSituationByTurn.Add(currentTurn, currentBattleSituation);
        }

        foreach (UnitOnBattle unitTarget in unitTargets.Values)
        {
            if (!attackersByObjectId.ContainsKey(unitTarget.IdOnBattle)) continue;
            int totalDamage = BattleEngine.CalculateDamageToUnit(this, attackersByObjectId[unitTarget.IdOnBattle].ToArray(), unitTarget);
            BattleSituation currentBattleSituation = this.Clone();
            currentBattleSituation.UnitChangeHealth(unitTarget.IdOnBattle, unitTarget.Health - totalDamage);
            currentBattleSituation.NextTurn();

            TurnData currentTurn = new TurnData();
            currentTurn._targetIdOnBattle = unitTarget.IdOnBattle;

            battleSituationByTurn.Add(currentTurn, currentBattleSituation);
        }

        battleSituationByTurn.AddRange(GetAllMovementWithAttackSequels());
        return battleSituationByTurn;
    }

    public Dictionary<TurnData, BattleSituation> GetAllMovementSequels()
    {
        Dictionary<TurnData, BattleSituation> battleSituationByTurn = new Dictionary<TurnData, BattleSituation>();

        Dictionary<string, ObjectOnBattle> possibleMovers = new Dictionary<string, ObjectOnBattle>();
        if (_sideTurn == Sides.federation)
        {
            possibleMovers = federationUnits;
        }
        else if (_sideTurn == Sides.empire) 
        {
            possibleMovers = empireUnits;
        }
        else
        {
            possibleMovers = neutralUnits;
        }

        foreach (var item in possibleMovers)
        {
            battleSituationByTurn = battleSituationByTurn.Concat(GetMovementSequelsByUnit(item.Value as UnitOnBattle)).ToDictionary(x => x.Key, x => x.Value);
        }

        return battleSituationByTurn;
    }

    public Dictionary<TurnData, BattleSituation> GetMovementSequelsByUnit(UnitOnBattle unit)
    {
        Dictionary<TurnData, BattleSituation> battleSituationByTurn = new Dictionary<TurnData, BattleSituation>();

        List<Bector2Int> possibleNewPositions = _map.GetReachablePositions(unit.Position.First(), unit.mobility);
        foreach (Bector2Int position in possibleNewPositions)
        {
            TurnData currentTurn = new TurnData();
            currentTurn._activeUnitIdOnBattle = unit.idOnBattle;
            currentTurn._route = _map.BuildRoute(unit.Position.First(), position, unit.mobility);

            BattleSituation currentBattleSituation = this.Clone();
            currentBattleSituation.UnitChangePosition(unit.idOnBattle, position);
            currentBattleSituation.NextTurn();

            battleSituationByTurn.Add(currentTurn, currentBattleSituation);
        }

        return battleSituationByTurn;
    }

    public Dictionary<TurnData, BattleSituation> GetSequelWithPass()
    {
        TurnData turn = new TurnData();

        BattleSituation battleSituationWithPass = this.Clone();
        battleSituationWithPass.NextTurn();
        return new Dictionary<TurnData, BattleSituation>() { { turn, battleSituationWithPass } };
    }

    public List<ObjectOnBattle> GetAttackersByTarget(ObjectOnBattle target)
    {
        List<ObjectOnBattle> attackers = new List<ObjectOnBattle>();

        if (!attackersByObjectId.ContainsKey(target.IdOnBattle)) return attackers;

        return attackersByObjectId[target.IdOnBattle];
    }


    public List<ObjectOnBattle> GetTargetsByAttacker(ObjectOnBattle attacker)
    {
        List<ObjectOnBattle> targets = new List<ObjectOnBattle>();

        foreach(var keyValuePair in attackersByObjectId)
        {
            string currentTargetId = keyValuePair.Key;
            List<ObjectOnBattle> currentAttackers = keyValuePair.Value;
            if (currentAttackers.Contains(attacker))
            {
                // Найти цель-объект и добавить
                targets.Add(GetObjectById(currentTargetId));
            }
        }

        return targets;
    } 

    public bool IsObjectExist(string objectId)
    {
        List<string> allIds = new List<string>();

        allIds.AddRange(federationUnits.Keys);
        allIds.AddRange(empireUnits.Keys);
        allIds.AddRange(neutralUnits.Keys);

        allIds.AddRange(federationBuilds.Keys);
        allIds.AddRange(empireBuilds.Keys);
        allIds.AddRange(neutralBuilds.Keys);

        return allIds.Contains(objectId);
    }

    public ObjectOnBattle GetObjectById(string id)
    {
        Dictionary<string, ObjectOnBattle> allObjects = federationUnits.Concat(empireUnits).Concat(neutralUnits).Concat(federationBuilds).Concat(empireBuilds).Concat(neutralBuilds).ToDictionary(x => x.Key, x => x.Value);

        if (allObjects.ContainsKey(id)) return allObjects[id];
        return null;
    }

    public UnitOnBattle GetUnitById(string id)
    {
        Dictionary<string, ObjectOnBattle> allUnits = federationUnits.Concat(empireUnits).Concat(neutralUnits).ToDictionary(x => x.Key, x => x.Value);

        if (allUnits.ContainsKey(id))
        {
            ObjectOnBattle obj = allUnits[id];
            UnitOnBattle unit = obj as UnitOnBattle;
            return unit;
        }

        return null;
    }

    public BuildOnBattle GetBuildById(string id)
    {
        Dictionary<string, ObjectOnBattle> allBuilds = federationBuilds.Concat(empireBuilds).Concat(neutralBuilds).ToDictionary(x => x.Key, x => x.Value);

        if (allBuilds.ContainsKey(id)) return allBuilds[id] as BuildOnBattle;
        return null;
    }

    public string GetNextSideTurn(string currentSideTurn)
    {
        return Sides.nextSideTurnByCurrentSide[currentSideTurn];
    }


    public Dictionary<string, ObjectOnBattle> GetUnitsCollectionById(string id)
    {
        Dictionary<string, ObjectOnBattle> allUnits = federationUnits.Concat(empireUnits).Concat(neutralUnits).ToDictionary(x => x.Key, x => x.Value);
        ObjectOnBattle unit = allUnits[id];
        return GetUnitsCollectionBySide(unit.side);
    }

    public Dictionary<string, ObjectOnBattle> GetUnitsCollectionBySide(string side)
    {
        Dictionary<string, ObjectOnBattle> unitsBySide = new Dictionary<string, ObjectOnBattle>();
        if (side == Sides.federation) unitsBySide = federationUnits;
        if (side == Sides.empire) unitsBySide = empireUnits;
        if (side == Sides.neutral) unitsBySide = neutralUnits;
        return unitsBySide;
    }

    public Dictionary<string, ObjectOnBattle> GetBuildsCollectionById(string id)
    {
        Dictionary<string, ObjectOnBattle> allBuilds = federationBuilds.Concat(empireBuilds).Concat(neutralBuilds).ToDictionary(x => x.Key, x => x.Value);
        ObjectOnBattle build = allBuilds[id];
        return GetBuildsCollectionBySide(build.side);
    }

    public Dictionary<string, ObjectOnBattle> GetBuildsCollectionBySide(string side)
    {
        Dictionary<string, ObjectOnBattle> buildsBySide = new Dictionary<string, ObjectOnBattle>();
        if (side == Sides.federation) buildsBySide = federationBuilds;
        if (side == Sides.empire) buildsBySide = empireBuilds;
        if (side == Sides.neutral) buildsBySide = neutralBuilds;
        return buildsBySide;
    }

    public Dictionary<string, ObjectOnBattle> GetUnitTargetsBySide(string side)
    {
        Dictionary<string, ObjectOnBattle> possibleTargets = new Dictionary<string, ObjectOnBattle>();
        if (side == Sides.federation) { possibleTargets = empireUnits.Concat(neutralUnits).ToDictionary(x => x.Key, x => x.Value); }
        if (side == Sides.empire) { possibleTargets = federationUnits; }
        if (side == Sides.neutral) { }
        return possibleTargets;
    }

    public Dictionary<string, ObjectOnBattle> GetBuildTargetsBySide(string side)
    {
        Dictionary<string, ObjectOnBattle> possibleTargets = new Dictionary<string, ObjectOnBattle>();
        if (side == Sides.federation) { possibleTargets = empireBuilds.Concat(neutralBuilds).ToDictionary(x => x.Key, x => x.Value); }
        if (side == Sides.empire) { possibleTargets = federationBuilds; }
        if (side == Sides.neutral) { }
        return possibleTargets;
    }

    public Dictionary<string, ObjectOnBattle> GetAttackersBySide(string side)
    {
        Dictionary<string, ObjectOnBattle> possibleAttackers = new Dictionary<string, ObjectOnBattle>();
        if (side == Sides.federation) { possibleAttackers = empireUnits; }
        if (side == Sides.empire) { possibleAttackers = federationUnits; }
        if (side == Sides.neutral) { possibleAttackers = federationUnits; }
        return possibleAttackers;
    }

    public List<Bector2Int> GetReachablePositionsByUnit(string unitId)
    {
        UnitOnBattle unit = GetUnitsCollectionById(unitId)[unitId] as UnitOnBattle;
        return _map.GetReachablePositions(unit.Position.First(), unit.mobility);
    }

    public Dictionary<string, ObjectOnBattle> GetAllUnits()
    {
        return federationUnits.Concat(empireUnits).Concat(neutralUnits).ToDictionary(x => x.Key, x => x.Value);
    }

    public Dictionary<string, ObjectOnBattle> GetAllBuilds()
    {
        return federationBuilds.Concat(empireBuilds).Concat(neutralBuilds).ToDictionary(x => x.Key, x => x.Value);
    }
}
