using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AbstractAI : AIInterface
{
    public AISettings settings;
    public AISettings Settings {  get {  return settings; } }

    public BattleEngine _battleEngine;


    public virtual void Init()
    {
        InitBattleEngine();
    }

    public void InitBattleEngine()
    {
        _battleEngine = GameObject.FindGameObjectWithTag(Tags.battleEngine).GetComponent<BattleEngine>();
    }

    public abstract TurnData GetTurn();

    public virtual TurnData GetBestAttack(BattleSituation currentBattleSituation, Dictionary<TurnData, BattleSituation> attacks)
    {
        TurnData turnWithGreaterDamage = null;
        int greaterDamage = 0;

        Dictionary<TurnData, BattleSituation> destructions = new Dictionary<TurnData, BattleSituation>();
        foreach (var keyValuePair in attacks)
        {
            if (!keyValuePair.Value.IsObjectExist(keyValuePair.Key._targetIdOnBattle))
            {
                destructions.Add(keyValuePair.Key, keyValuePair.Value);
            }

            List<ObjectOnBattle> attackersData = currentBattleSituation.GetAttackersByTargetId(keyValuePair.Key._targetIdOnBattle);
            if (keyValuePair.Key._activeUnitIdOnBattle != null)
            {
                ObjectOnBattle activeUnitAttacker = currentBattleSituation.GetUnitById(keyValuePair.Key._activeUnitIdOnBattle);
                if (!attackersData.Contains(activeUnitAttacker))
                {
                    attackersData.Add(activeUnitAttacker);
                }
            }

            int currentDamage = BattleEngine.CalculateDamageToTargetById(currentBattleSituation, attackersData.ToArray(), keyValuePair.Key._targetIdOnBattle);
            if (currentDamage > greaterDamage)
            {
                turnWithGreaterDamage = keyValuePair.Key;
                greaterDamage = currentDamage;
            }
        }

        if (destructions.Count > 0) return GetBestDestruction(currentBattleSituation, destructions);

        return turnWithGreaterDamage;
    }

    public virtual TurnData GetBestDestruction(BattleSituation currentBattleSituation, Dictionary<TurnData, BattleSituation> destructions)
    {
        TurnData bestDestruction = null;
        float strongestTargetPower = 0;

        foreach (var keyValuePair in destructions)
        {
            float currentTargetPower = BattleEngine.GetObjectPower(currentBattleSituation, keyValuePair.Key._targetIdOnBattle);
            if (currentTargetPower > strongestTargetPower)
            {
                bestDestruction = keyValuePair.Key;
                strongestTargetPower = currentTargetPower;
            }
        }

        return bestDestruction;
    }

    public virtual TurnData GetBestAttackOnNextTurn(BattleSituation battleSituation, Dictionary<TurnData, BattleSituation> movements)
    {
        TurnData bestMoveWithAttackOnNextTurn = null;
        int greaterDamageByAttackOnNextTurn = 0; /////��������� ��� � ���������� ��������� ������
        int incomingDamage = int.MaxValue; /////��������� ��� � ���������� �������� ������
        int distanceToEnemy = int.MaxValue; ////��������� ��� � ��������� ����������� �� �����

        foreach (var keyValuePair in movements)
        {
            TurnData currentMove = keyValuePair.Key;
            BattleSituation currentBattleSituationWhenNextSideTurn = keyValuePair.Value.Clone();
            currentBattleSituationWhenNextSideTurn.SkipToSideTurn(battleSituation._sideTurn);
            UnitOnBattle currentActiveUnit = currentBattleSituationWhenNextSideTurn.GetUnitById(currentMove._activeUnitIdOnBattle);

            Dictionary<TurnData, BattleSituation> attackMovesInCurrentBattleSituation = currentBattleSituationWhenNextSideTurn.GetAllAttackingSequels();
            if (attackMovesInCurrentBattleSituation.Count > 0)
            {
                TurnData bestAttackInCurrentBattleSituation = GetBestAttack(currentBattleSituationWhenNextSideTurn, attackMovesInCurrentBattleSituation);

                List<ObjectOnBattle> attackersData = currentBattleSituationWhenNextSideTurn.GetAttackersByTargetId(bestAttackInCurrentBattleSituation._targetIdOnBattle);

                if (!attackersData.Contains(currentActiveUnit))
                {
                    attackersData.Add(currentActiveUnit);
                }
                ObjectOnBattle[] activeUnitAttackers = currentBattleSituationWhenNextSideTurn.GetAttackersByTarget(currentActiveUnit).ToArray();
                int damageToActiveUnitInBestAttackInCurrentBattleSituation = BattleEngine.CalculateDamageToTargetById(currentBattleSituationWhenNextSideTurn, activeUnitAttackers, currentMove._activeUnitIdOnBattle);
                if (damageToActiveUnitInBestAttackInCurrentBattleSituation >= currentActiveUnit.Health) continue;
                
                int damageByBestAttackInCurrentBattleSituation = BattleEngine.CalculateDamageToTargetById(currentBattleSituationWhenNextSideTurn, attackersData.ToArray(), bestAttackInCurrentBattleSituation._targetIdOnBattle);
                int currentDistanceToEnemy = currentBattleSituationWhenNextSideTurn.FindDistanceToMostAccessibleUnitBySide(currentActiveUnit.Position.First(), Sides.enemySideBySide[battleSituation._sideTurn]);
                if (damageByBestAttackInCurrentBattleSituation > greaterDamageByAttackOnNextTurn)
                {
                    bestMoveWithAttackOnNextTurn = currentMove;
                    greaterDamageByAttackOnNextTurn = damageByBestAttackInCurrentBattleSituation;
                    incomingDamage = damageToActiveUnitInBestAttackInCurrentBattleSituation;
                    distanceToEnemy = currentDistanceToEnemy;
                }
                else if (damageByBestAttackInCurrentBattleSituation == greaterDamageByAttackOnNextTurn)
                {
                    if ((damageToActiveUnitInBestAttackInCurrentBattleSituation * 3 + currentDistanceToEnemy) < (incomingDamage * 3 + distanceToEnemy))
                    {
                        bestMoveWithAttackOnNextTurn = currentMove;
                        greaterDamageByAttackOnNextTurn = damageByBestAttackInCurrentBattleSituation;
                        incomingDamage = damageToActiveUnitInBestAttackInCurrentBattleSituation;
                        distanceToEnemy = currentDistanceToEnemy;
                    }
                }
            }
        }

        return bestMoveWithAttackOnNextTurn;
    }

    public List<string> GetUniqueUnitIdsByMovements(Dictionary<TurnData, BattleSituation> movements)
    {
        List<string> uniqueActiveUnitIds = new List<string>();

        foreach(TurnData movement in movements.Keys)
        {
            if (!uniqueActiveUnitIds.Contains(movement._activeUnitIdOnBattle))
            {
                uniqueActiveUnitIds.Add(movement._activeUnitIdOnBattle);
            }
        }

        return uniqueActiveUnitIds;
    }

    public TurnData GetMovementWithFastestApproachToAttack(BattleSituation battleSituation, Dictionary<TurnData, BattleSituation> movements)
    {
        List<string> uniqueActiveUnitIds = GetUniqueUnitIdsByMovements(movements);
        TurnData moveWithFastestApproachToAttack = null;
        int minTurnCountForReachAttackDistance = int.MaxValue;

        foreach (string uniqueUnitId in uniqueActiveUnitIds)
        {
            UnitOnBattle currentActiveUnit = battleSituation.GetUnitById(uniqueUnitId);
            MoveForAttackData currentDataForMoveToAttackCurrentNearestUnit = battleSituation.GetDataForMoveToAttackNearestUnit(currentActiveUnit, battleSituation._sideTurn);
            if (currentDataForMoveToAttackCurrentNearestUnit._turnCountForReach < minTurnCountForReachAttackDistance)
            {
                minTurnCountForReachAttackDistance = currentDataForMoveToAttackCurrentNearestUnit._turnCountForReach;

                List<Bector2Int> newBestRoute = currentDataForMoveToAttackCurrentNearestUnit._fullRoute;
                if (newBestRoute.Count >= currentActiveUnit.mobility)
                {
                    newBestRoute.RemoveRange(currentActiveUnit.mobility, newBestRoute.Count - currentActiveUnit.mobility);
                }
                moveWithFastestApproachToAttack = new TurnData(
                    activeUnitIdOnBattle: currentActiveUnit.IdOnBattle,
                    route: newBestRoute);
            }
        }

        return moveWithFastestApproachToAttack;
    }

    public TurnData GetBestOffensiveMovementUnderAttack(BattleSituation battleSituation, Dictionary<TurnData, BattleSituation> movements)
    {
        TurnData bestMoveWithAttackOnNextTurn = GetBestAttackOnNextTurn(battleSituation, movements);
        if (bestMoveWithAttackOnNextTurn != null) return bestMoveWithAttackOnNextTurn;

        return GetMovementWithFastestApproachToAttack(battleSituation, movements);
    }

    /// <summary>
    /// Получает атаки только по юнитам, исключая здания
    /// </summary>
    protected Dictionary<TurnData, BattleSituation> GetUnitOnlyAttackingSequels(BattleSituation battleSituation)
    {
        Dictionary<TurnData, BattleSituation> allAttacks = battleSituation.GetAllAttackingSequels();
        Dictionary<TurnData, BattleSituation> unitOnlyAttacks = new Dictionary<TurnData, BattleSituation>();

        foreach (var attack in allAttacks)
        {
            string targetId = attack.Key._targetIdOnBattle;
            
            // Проверяем, является ли цель юнитом (а не зданием)
            UnitOnBattle targetUnit = battleSituation.GetUnitById(targetId);
            if (targetUnit != null)
            {
                unitOnlyAttacks.Add(attack.Key, attack.Value);
            }
        }

        return unitOnlyAttacks;
    }
}
