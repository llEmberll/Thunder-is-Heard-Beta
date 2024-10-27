using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleSituation
{
    public Dictionary<string, UnitOnBattle> federationUnits = new Dictionary<string, UnitOnBattle>();
    public Dictionary<string, UnitOnBattle> empireUnits = new Dictionary<string, UnitOnBattle>();
    public Dictionary<string, UnitOnBattle> neutralUnits = new Dictionary<string, UnitOnBattle>();

    public Dictionary<string, BuildOnBattle> federationBuilds = new Dictionary<string, BuildOnBattle>();
    public Dictionary<string, BuildOnBattle> empireBuilds = new Dictionary<string, BuildOnBattle>();
    public Dictionary<string, BuildOnBattle> neutralBuilds = new Dictionary<string, BuildOnBattle>();

    public Dictionary<string, List<UnitOnBattle>> attackersByObjectId = new Dictionary<string, List<UnitOnBattle>>();

    public string _sideTurn;

    public MapOnBattle _map;


    public void InitByBattleDataAndMap(BattleCacheItem battleData, Map map)
    {
        InitMap(map.Cells);
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

    public void InitSideTurn(string sideTurn)
    {
        _sideTurn = sideTurn;
    }

    public void InitUnits(UnitOnBattle[] units)
    {
        federationUnits = new Dictionary<string, UnitOnBattle>();
        empireUnits = new Dictionary<string, UnitOnBattle>();
        neutralUnits = new Dictionary<string, UnitOnBattle>();

        foreach (var unit in units)
        {
            AddUnit(unit);
        }
    }

    public void InitBuilds(BuildOnBattle[] builds)
    {
        federationBuilds = new Dictionary<string, BuildOnBattle>();
        empireBuilds = new Dictionary<string, BuildOnBattle>();
        neutralBuilds = new Dictionary<string, BuildOnBattle>();

        foreach (var build in builds)
        {
            AddBuild(build);
        }
    }

    public void AddUnit(UnitOnBattle unitData) // Добавить юнита в сражение
    {
        Dictionary<string, UnitOnBattle> unitsBySide = GetUnitsCollectionBySide(unitData.side);
        unitsBySide.Add(unitData.idOnBattle, unitData);
        _map.Cells[unitData.position]._isOccypy = true;

        UpdateTargetsForAttacker(unitData);
        UpdateAttackersForUnitTarget(unitData);
        // Сделать перерасчет оценки позиции, когда она будет реализована
    }

    public void AddBuild(BuildOnBattle buildData) // Добавить здание в сражение
    {
        Dictionary<string, BuildOnBattle> buildsBySide = GetBuildsCollectionBySide(buildData.side);
        buildsBySide.Add(buildData.idOnBattle, buildData);
        foreach (var position in buildData.position)
        {
            _map.Cells[position]._isOccypy = true;
        }

        UpdateAttackersForBuildTarget(buildData);
    }

    public void RemoveUnit(UnitOnBattle unitData) // Удалить юнита из сражения
    {
        Dictionary<string, UnitOnBattle> unitsBySide = GetUnitsCollectionBySide(unitData.side);
        ClearAttacker(unitData);
        ClearUnitAsTarget(unitData);
        unitsBySide.Remove(unitData.idOnBattle);
        _map.Cells[unitData.position]._isOccypy = false;

        // Сделать перерасчет оценки позиции, когда она будет реализована
    }

    public void RemoveBuild(BuildOnBattle buildData) // Удалить здание из сражения
    {
        Dictionary<string, BuildOnBattle> buildsBySide = GetBuildsCollectionBySide(buildData.side);
        ClearBuildAsTarget(buildData);
        buildsBySide.Remove(buildData.idOnBattle);
        foreach (var position in buildData.position)
        {
            _map.Cells[position]._isOccypy = false;
        }
    }

    public void UnitChangePosition(string unitId, Bector2Int newPosition)
    {
        Dictionary<string, UnitOnBattle> unitsCollection = GetUnitsCollectionById(unitId);
        UnitOnBattle unit = unitsCollection[unitId];

        _map.Cells[unit.position]._isOccypy = false;
        unit.position = newPosition;
        _map.Cells[unit.position]._isOccypy = true;

        unitsCollection[unitId] = unit;
        UpdateAttackersForUnitTarget(unit);
        UpdateTargetsForAttacker(unit);
    }

    public void UnitChangeSide(string unitId, string newSide)
    {
        Dictionary<string, UnitOnBattle> unitsCollectionByOldSide = GetUnitsCollectionById(unitId);
        UnitOnBattle unit = unitsCollectionByOldSide[unitId];
        unitsCollectionByOldSide.Remove(unitId);

        Dictionary<string, UnitOnBattle> unitsByNewSide = GetUnitsCollectionBySide(newSide);
        unitsByNewSide.Add(unitId, unit);
        UpdateAttackers();
    }


    public void UnitChangeHealth(string unitId, int newHealth)
    {
        Dictionary<string, UnitOnBattle> unitsCollection = GetUnitsCollectionById(unitId);
        UnitOnBattle unit = unitsCollection[unitId];
        
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
        Dictionary<string, UnitOnBattle> unitsForSearchData = GetUnitsCollectionById(newUnitData.idOnBattle);
        UnitOnBattle oldUnitData = unitsForSearchData[newUnitData.idOnBattle];

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
        Dictionary<string, BuildOnBattle> buildsCollection = GetBuildsCollectionById(buildId);
        BuildOnBattle build = buildsCollection[buildId];
        build.position = newPosition;
        buildsCollection[buildId] = build;
        UpdateAttackersForBuildTarget(build);
    }

    public void BuildChangeSide(string buildId, string newSide)
    {
        Dictionary<string, BuildOnBattle> buildsCollectionByOldSide = GetBuildsCollectionById(buildId);
        BuildOnBattle build = buildsCollectionByOldSide[buildId];
        buildsCollectionByOldSide.Remove(buildId);

        Dictionary<string, BuildOnBattle> buildsByNewSide = new Dictionary<string, BuildOnBattle>();
        if (newSide == Sides.federation) buildsByNewSide = federationBuilds;
        if (newSide == Sides.empire) buildsByNewSide = empireBuilds;
        if (newSide == Sides.neutral) buildsByNewSide = neutralBuilds;

        buildsByNewSide.Add(buildId, build);
        UpdateAttackers();
    }

    public void BuildChangeHealth(string buildId, int newHealth)
    {
        Dictionary<string, BuildOnBattle> buildsCollection = GetBuildsCollectionById(buildId);
        BuildOnBattle build = buildsCollection[buildId];

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
        Dictionary<string, BuildOnBattle> buildsForSearchData = GetBuildsCollectionById(newBuildData.idOnBattle);
        BuildOnBattle oldBuildData = buildsForSearchData[newBuildData.idOnBattle];

        // Сделать перерасчет оценки позиции, когда она будет реализована

        buildsForSearchData[newBuildData.idOnBattle] = newBuildData;
    }


    public void NextTurn()
    {
        this._sideTurn = GetNextSideTurn(this._sideTurn);
        RechargeSkillsCooldown();
    }

    public void RechargeSkillsCooldown()
    {
        Dictionary<string, UnitOnBattle> allUnits = empireUnits.Concat(federationUnits).Concat(neutralUnits).ToDictionary(x => x.Key, x => x.Value);
        foreach (UnitOnBattle unit in allUnits.Values)
        {
            foreach (SkillOnBattle skill in unit.skillsData)
            {
                skill.cooldown = Mathf.Max(skill.cooldown - 1, 0);
            }
        }
    }

    public void AddAttackerByObjectId(string objectId, UnitOnBattle attacker)
    {
        if (attackersByObjectId.ContainsKey(objectId))
        {
            List<UnitOnBattle> newAttackers = attackersByObjectId[objectId];
            if (!newAttackers.Contains(attacker))
            {
                newAttackers.Add(attacker);
            }
        }
        else
        {
            attackersByObjectId.Add(objectId, new List<UnitOnBattle>() { attacker });
        }
    }


    public void ClearAttacker(UnitOnBattle attacker)
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

    public void RemoveAttackerByObjectIdIfExist(string objectId, UnitOnBattle attacker)
    {
        if (!attackersByObjectId.ContainsKey(objectId)) return;

        List<UnitOnBattle> newAttackersData = attackersByObjectId[objectId];
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

    public void UpdateTargetsForAttacker(UnitOnBattle attacker)
    {
        //Dictionary<string, UnitOnBattle> possibleAttackers =  empireUnits.Concat(federationUnits).ToDictionary(x => x.Key, x => x.Value);
        Dictionary<string, UnitOnBattle> unitsForSearchData = GetUnitTargetsBySide(attacker.side);
        Dictionary<string, BuildOnBattle> buildsForSearchData = GetBuildTargetsBySide(attacker.side);

        foreach (UnitOnBattle possibleUnitTarget in unitsForSearchData.Values)
        {
            if (BattleEngine.GetDistanceBetweenPoints(attacker.position, possibleUnitTarget.position) <= attacker.distance)
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
            if (BattleEngine.GetDistanceBetweenPointAndRectangleOfPoints(attacker.position, new RectangleBector2Int(possibleBuildTarget.position)) <= attacker.distance)
            {
                AddAttackerByObjectId(possibleBuildTarget.idOnBattle, attacker);
            }
            else
            {
                RemoveAttackerByObjectIdIfExist(possibleBuildTarget.idOnBattle, attacker);
            }
        }
    }

    public void UpdateAttackersForBuildTarget(BuildOnBattle build)
    {
        attackersByObjectId.Remove(build.idOnBattle);
        Dictionary<string, UnitOnBattle> possibleAttackers = GetAttackersBySide(build.side);
        foreach (UnitOnBattle possibleAttacker in  possibleAttackers.Values)
        {
            int minDistanceForAttack = BattleEngine.GetDistanceBetweenPointAndRectangleOfPoints(possibleAttacker.position, new RectangleBector2Int(build.position));
            if (minDistanceForAttack <= possibleAttacker.distance)
            {
                AddAttackerByObjectId(build.idOnBattle, possibleAttacker);
            }
        }
    }

    public void UpdateAttackersForUnitTarget(UnitOnBattle unit)
    {
        attackersByObjectId.Remove(unit.idOnBattle);
        Dictionary<string, UnitOnBattle> possibleAttackers = GetAttackersBySide(unit.side);
        foreach (UnitOnBattle possibleAttacker in possibleAttackers.Values)
        {
            int minDistanceForAttack = BattleEngine.GetDistanceBetweenPoints(unit.position, possibleAttacker.position);
            if (minDistanceForAttack <= possibleAttacker.distance)
            {
                AddAttackerByObjectId(unit.idOnBattle, possibleAttacker);
            }
        }
    }

    public void UpdateAttackers() // Обновить атакующих для всех объектов
    {
        attackersByObjectId = new Dictionary<string, List<UnitOnBattle>>();
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
        Dictionary<string, UnitOnBattle> possibleAttackers = empireUnits;

        foreach (var victimItem in federationUnits)
        {
            foreach (var attackerItem in possibleAttackers)
            {
                int minDistanceForAttack = BattleEngine.GetDistanceBetweenPoints(victimItem.Value.position, attackerItem.Value.position);
                if (minDistanceForAttack <= attackerItem.Value.distance) {
                    AddAttackerByObjectId(victimItem.Key, attackerItem.Value);
                }
            }
        }
    }

    public void UpdateAttackersForFederationBuilds()
    {
        Dictionary<string, UnitOnBattle> possibleAttackers = empireUnits;

        foreach (var victimItem in federationBuilds)
        {
            foreach (var attackerItem in possibleAttackers)
            {
                int minDistanceForAttack = BattleEngine.GetDistanceBetweenPointAndRectangleOfPoints(attackerItem.Value.position, new RectangleBector2Int(victimItem.Value.position));
                if (minDistanceForAttack <= attackerItem.Value.distance)
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
        Dictionary<string, UnitOnBattle> possibleAttackers = federationUnits;

        foreach (var victimItem in empireUnits)
        {
            foreach (var attackerItem in possibleAttackers)
            {
                int minDistanceForAttack = BattleEngine.GetDistanceBetweenPoints(victimItem.Value.position, attackerItem.Value.position);
                if (minDistanceForAttack <= attackerItem.Value.distance)
                {
                    AddAttackerByObjectId(victimItem.Key, attackerItem.Value);
                }
            }
        }
    }

    public void UpdateAttackersForEmpireBuilds()
    {
        Dictionary<string, UnitOnBattle> possibleAttackers = federationUnits;

        foreach (var victimItem in empireBuilds)
        {
            foreach (var attackerItem in possibleAttackers)
            {
                int minDistanceForAttack = BattleEngine.GetDistanceBetweenPointAndRectangleOfPoints(attackerItem.Value.position, new RectangleBector2Int(victimItem.Value.position));
                if (minDistanceForAttack <= attackerItem.Value.distance)
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
        //Dictionary<string, UnitOnBattle> possibleAttackers =  empireUnits.Concat(federationUnits).ToDictionary(x => x.Key, x => x.Value);
        Dictionary<string, UnitOnBattle> possibleAttackers = federationUnits;

        foreach (var victimItem in neutralUnits)
        {
            foreach (var attackerItem in possibleAttackers)
            {
                int minDistanceForAttack = BattleEngine.GetDistanceBetweenPoints(victimItem.Value.position, attackerItem.Value.position);
                if (minDistanceForAttack <= attackerItem.Value.distance)
                {
                    AddAttackerByObjectId(victimItem.Key, attackerItem.Value);
                }
            }
        }
    }

    public void UpdateAttackersForNeutralBuilds()
    {
        //Dictionary<string, UnitOnBattle> possibleAttackers =  empireUnits.Concat(federationUnits).ToDictionary(x => x.Key, x => x.Value);
        Dictionary<string, UnitOnBattle> possibleAttackers = federationUnits;

        foreach (var victimItem in neutralBuilds)
        {
            foreach (var attackerItem in possibleAttackers)
            {
                int minDistanceForAttack = BattleEngine.GetDistanceBetweenPointAndRectangleOfPoints(attackerItem.Value.position, new RectangleBector2Int(victimItem.Value.position));
                if (minDistanceForAttack <= attackerItem.Value.distance)
                {
                    AddAttackerByObjectId(victimItem.Key, attackerItem.Value);
                }
            }
        }
    }


    public BattleSituation Clone()
    {
        // Создаем новую копию BattleSituation
        BattleSituation clone = new BattleSituation();

        // Клонируем _map
        CellData[] clonedCells = new CellData[clone._map.Cells.Count];
        int index = 0;
        foreach (var pair in clone._map.Cells)
        {
            clonedCells[index] = pair.Value.Clone();
            index++;
        }
        clone.InitMap(clonedCells);

        // Клонируем словари юнитов
        clone.federationUnits = new Dictionary<string, UnitOnBattle>();
        foreach (var pair in federationUnits)
        {
            clone.federationUnits.Add(pair.Key, pair.Value.Clone());
        }
        clone.empireUnits = new Dictionary<string, UnitOnBattle>();
        foreach (var pair in empireUnits)
        {
            clone.empireUnits.Add(pair.Key, pair.Value.Clone());
        }
        clone.neutralUnits = new Dictionary<string, UnitOnBattle>();
        foreach (var pair in neutralUnits)
        {
            clone.neutralUnits.Add(pair.Key, pair.Value.Clone());
        }

        // Клонируем словари зданий
        clone.federationBuilds = new Dictionary<string, BuildOnBattle>();
        foreach (var pair in federationBuilds)
        {
            clone.federationBuilds.Add(pair.Key, pair.Value.Clone());
        }
        clone.empireBuilds = new Dictionary<string, BuildOnBattle>();
        foreach (var pair in empireBuilds)
        {
            clone.empireBuilds.Add(pair.Key, pair.Value.Clone());
        }
        clone.neutralBuilds = new Dictionary<string, BuildOnBattle>();
        foreach (var pair in neutralBuilds)
        {
            clone.neutralBuilds.Add(pair.Key, pair.Value.Clone());
        }

        // Клонируем словарь attackersByObjectId
        clone.attackersByObjectId = new Dictionary<string, List<UnitOnBattle>>();
        foreach (var pair in attackersByObjectId)
        {
            clone.attackersByObjectId.Add(pair.Key, new List<UnitOnBattle>());
            foreach (var unit in pair.Value)
            {
                clone.attackersByObjectId[pair.Key].Add(unit.Clone());
            }
        }

        // Клонируем _sideTurn
        clone._sideTurn = _sideTurn;

        return clone;
    }

    public List<BattleSituation> GetAllSequels()
    {
        List<BattleSituation> battleSituations = new List<BattleSituation>();
        battleSituations.AddRange(GetAllMovementSequels());
        battleSituations.AddRange(GetAllAttackingSequels());
        battleSituations.Add(GetSequelWithPass());
        return battleSituations;
    }

    public List<BattleSituation> GetAllAttackingSequels()
    {
        List<BattleSituation> battleSituations = new List<BattleSituation>();

        Dictionary<string, BuildOnBattle> possibleBuildTargets = new Dictionary<string, BuildOnBattle>();
        Dictionary<string, UnitOnBattle> possibleUnitTargets = new Dictionary<string, UnitOnBattle>();
        if (_sideTurn == Sides.federation)
        {
            possibleBuildTargets = empireBuilds;
            possibleUnitTargets = empireUnits;
        }
        else if (_sideTurn == Sides.empire)
        {
            possibleBuildTargets = federationBuilds;
            possibleUnitTargets = federationUnits;
        }
        else
        {
            possibleBuildTargets = federationBuilds.Concat(empireBuilds).ToDictionary(x => x.Key, x => x.Value);
            possibleUnitTargets = federationUnits.Concat(empireUnits).ToDictionary(x => x.Key, x => x.Value);
        }

       foreach (BuildOnBattle buildTarget in possibleBuildTargets.Values) 
        {
            int totalDamage = BattleEngine.CalculateDamageToBuild(attackersByObjectId[buildTarget.idOnBattle].ToArray(), buildTarget);
            BattleSituation currentBattleSituation = this.Clone();
            currentBattleSituation.BuildChangeHealth(buildTarget.idOnBattle, buildTarget.health - totalDamage);
            currentBattleSituation.NextTurn();
            battleSituations.Add(currentBattleSituation);
        }

        foreach (UnitOnBattle unitTarget in possibleUnitTargets.Values)
        {
            int totalDamage = BattleEngine.CalculateDamageToUnit(attackersByObjectId[unitTarget.idOnBattle].ToArray(), unitTarget);
            BattleSituation currentBattleSituation = this.Clone();
            currentBattleSituation.UnitChangeHealth(unitTarget.idOnBattle, unitTarget.health - totalDamage);
            currentBattleSituation.NextTurn();
            battleSituations.Add(currentBattleSituation);
        }
        return battleSituations;
    }

    public List<BattleSituation> GetAllMovementSequels()
    {
        List<BattleSituation> battleSituations = new List<BattleSituation>();

        Dictionary<string, UnitOnBattle> possibleMovers = new Dictionary<string, UnitOnBattle>();
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
            battleSituations.AddRange(GetMovementSequelsByUnit(item.Value));
        }

        return battleSituations;
    }

    public List<BattleSituation> GetMovementSequelsByUnit(UnitOnBattle unit)
    {
        List<BattleSituation> battleSituations = new List<BattleSituation>();

        List<Bector2Int> possibleNewPositions = _map.GetReachablePositions(unit.position, unit.mobility);
        foreach (Bector2Int position in possibleNewPositions)
        {
            BattleSituation currentBattleSituation = this.Clone();
            currentBattleSituation.UnitChangePosition(unit.idOnBattle, position);
            currentBattleSituation.NextTurn();

            battleSituations.Add(currentBattleSituation);
        }

        return battleSituations;
    }

    public BattleSituation GetSequelWithPass()
    {
        BattleSituation battleSituationWithPass = this.Clone();
        battleSituationWithPass.NextTurn();
        return battleSituationWithPass;
    }

    public string GetNextSideTurn(string currentSideTurn)
    {
        return SideTurnsQueue.nextSideTurnByCurrentSide[currentSideTurn];
    }


    public Dictionary<string, UnitOnBattle> GetUnitsCollectionById(string id)
    {
        Dictionary<string, UnitOnBattle> allUnits = federationUnits.Concat(empireUnits).Concat(neutralUnits).ToDictionary(x => x.Key, x => x.Value);
        UnitOnBattle unit = allUnits[id];
        return GetUnitsCollectionBySide(unit.side);
    }

    public Dictionary<string, UnitOnBattle> GetUnitsCollectionBySide(string side)
    {
        Dictionary<string, UnitOnBattle> unitsBySide = new Dictionary<string, UnitOnBattle>();
        if (side == Sides.federation) unitsBySide = federationUnits;
        if (side == Sides.empire) unitsBySide = empireUnits;
        if (side == Sides.neutral) unitsBySide = neutralUnits;
        return unitsBySide;
    }

    public Dictionary<string, BuildOnBattle> GetBuildsCollectionById(string id)
    {
        Dictionary<string, BuildOnBattle> allBuilds = federationBuilds.Concat(empireBuilds).Concat(neutralBuilds).ToDictionary(x => x.Key, x => x.Value);
        BuildOnBattle build = allBuilds[id];
        return GetBuildsCollectionBySide(build.side);
    }

    public Dictionary<string, BuildOnBattle> GetBuildsCollectionBySide(string side)
    {
        Dictionary<string, BuildOnBattle> buildsBySide = new Dictionary<string, BuildOnBattle>();
        if (side == Sides.federation) buildsBySide = federationBuilds;
        if (side == Sides.empire) buildsBySide = empireBuilds;
        if (side == Sides.neutral) buildsBySide = neutralBuilds;
        return buildsBySide;
    }

    public Dictionary<string, UnitOnBattle> GetUnitTargetsBySide(string side)
    {
        Dictionary<string, UnitOnBattle> possibleTargets = new Dictionary<string, UnitOnBattle>();
        if (side == Sides.federation) { possibleTargets = empireUnits.Concat(neutralUnits).ToDictionary(x => x.Key, x => x.Value); }
        if (side == Sides.empire) { possibleTargets = federationUnits; }
        if (side == Sides.neutral) { }
        return possibleTargets;
    }

    public Dictionary<string, BuildOnBattle> GetBuildTargetsBySide(string side)
    {
        Dictionary<string, BuildOnBattle> possibleTargets = new Dictionary<string, BuildOnBattle>();
        if (side == Sides.federation) { possibleTargets = empireBuilds.Concat(neutralBuilds).ToDictionary(x => x.Key, x => x.Value); }
        if (side == Sides.empire) { possibleTargets = federationBuilds; }
        if (side == Sides.neutral) { }
        return possibleTargets;
    }

    public Dictionary<string, UnitOnBattle> GetAttackersBySide(string side)
    {
        Dictionary<string, UnitOnBattle> possibleAttackers = new Dictionary<string, UnitOnBattle>();
        if (side == Sides.federation) { possibleAttackers = empireUnits; }
        if (side == Sides.empire) { possibleAttackers = federationUnits; }
        if (side == Sides.neutral) { possibleAttackers = federationUnits; }
        return possibleAttackers;
    }

    public List<Bector2Int> GetReachablePositionsByUnit(string unitId)
    {
        UnitOnBattle unit = GetUnitsCollectionById(unitId)[unitId];
        return _map.GetReachablePositions(unit.position, unit.mobility);
    }

    public Dictionary<string, UnitOnBattle> GetAllUnits()
    {
        return federationUnits.Concat(empireUnits).Concat(neutralUnits).ToDictionary(x => x.Key, x => x.Value);
    }

    public Dictionary<string, BuildOnBattle> GetAllBuilds()
    {
        return federationBuilds.Concat(empireBuilds).Concat(neutralBuilds).ToDictionary(x => x.Key, x => x.Value);
    }
}
