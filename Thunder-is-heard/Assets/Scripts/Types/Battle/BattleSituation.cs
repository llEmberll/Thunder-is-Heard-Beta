using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleSituation
{
    public Dictionary<Bector2Int, CellData> battleField;

    public Dictionary<string, UnitOnBattle> federationUnits;
    public Dictionary<string, UnitOnBattle> empireUnits;
    public Dictionary<string, UnitOnBattle> neutralUnits;

    public Dictionary<string, BuildOnBattle> federationBuilds;
    public Dictionary<string, BuildOnBattle> empireBuilds;
    public Dictionary<string, BuildOnBattle> neutralBuilds;

    public Dictionary<string, List<UnitOnBattle>> attackersByObjectId;

    public string _sideTurn;


    public void InitByBattleDataAndMap(BattleCacheItem battleData, Map map)
    {
        InitBattleField(map.Cells);
        InitSideTurn(battleData.GetTurn());
        InitUnits(battleData.GetUnits());
        InitBuilds(battleData.GetBuilds());

        UpdateAttackers();
    }

    public void InitBattleField(Dictionary<Vector2Int, Cell> cellsByPosition)
    {
        battleField = new Dictionary<Bector2Int, CellData>();
        foreach (var pair in cellsByPosition)
        {
            battleField.Add(new Bector2Int(pair.Key), new CellData(Doctrines.land, new Bector2Int(pair.Key), pair.Value.occupied));
        }
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
            if (unit.side == Sides.federation) federationUnits.Add(unit.idOnBattle, unit);
            if (unit.side == Sides.empire) empireUnits.Add(unit.idOnBattle, unit);
            if (unit.side == Sides.neutral) neutralUnits.Add(unit.idOnBattle, unit);
        }
    }

    public void InitBuilds(BuildOnBattle[] builds)
    {
        federationBuilds = new Dictionary<string, BuildOnBattle>();
        empireBuilds = new Dictionary<string, BuildOnBattle>();
        neutralBuilds = new Dictionary<string, BuildOnBattle>();

        foreach (var build in builds)
        {
            if (build.side == Sides.federation) federationBuilds.Add(build.idOnBattle, build);
            if (build.side == Sides.empire) empireBuilds.Add(build.idOnBattle, build);
            if (build.side == Sides.neutral) neutralBuilds.Add(build.idOnBattle, build);
        }
    }

    public void AddUnit(UnitOnBattle unitData) // Добавить юнита в сражение
    {

    }

    public void AddBuild(BuildOnBattle buildData) // Добавить здание в сражение
    {

    }

    public void RemoveUnit(UnitOnBattle unitData) // Удалить юнита из сражения
    {

    }

    public void RemoveBuild(BuildOnBattle buildData) // Удалить здание из сражения
    {

    }


    public void UnitDataChanged(UnitOnBattle unitData)
    {
         
    } // Изменить данные о юните -> | позиция | поворот | здоровье | информация об умениях | сторона

    public void BuildDataChanged(BuildOnBattle buildData)
    {

    } // // Изменить данные о здании -> | позиция | поворот | здоровье | рабочий статус | сторона


    public void AddAttackerByObjectId(string objectId, UnitOnBattle attacker)
    {
        if (attackersByObjectId.ContainsKey(objectId))
        {
            List<UnitOnBattle> newAttackers = attackersByObjectId[objectId];
            newAttackers.Add(attacker);
        }
        else
        {
            attackersByObjectId.Add(objectId, new List<UnitOnBattle>() { attacker });
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

        // Клонируем battleField
        clone.battleField = new Dictionary<Bector2Int, CellData>();
        foreach (var pair in battleField)
        {
            clone.battleField.Add(pair.Key, pair.Value.Clone());
        }

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

    }

    public List<BattleSituation> GetAllMovementSequels()
    {

    }

    public BattleSituation GetSequelWithPass()
    {
        BattleSituation battleSituationWithPass = this.Clone();
        battleSituationWithPass._sideTurn = SideTurnsQueue.nextSideTurnByCurrentSide[battleSituationWithPass._sideTurn];
        return battleSituationWithPass;
    }
}
