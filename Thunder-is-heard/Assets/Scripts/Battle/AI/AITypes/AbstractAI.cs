using System.Collections;
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

            ObjectOnBattle[] attackersData = currentBattleSituation.attackersByObjectId[keyValuePair.Key._targetIdOnBattle].ToArray();
            int currentDamage = BattleEngine.CalculateDamageToTargetById(currentBattleSituation, attackersData, keyValuePair.Key._targetIdOnBattle);
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
        int greaterDamageByAttackOnNextTurn = 0; /////Вычислить ход с наибольшим исходящим уроном
        int incomingDamage = int.MaxValue; /////Вычислить ход с наименьшим входящим уроном
        int distanceToEnemy = int.MaxValue; ////Вычислить ход с ближайшим расстоянием от врага

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

                ObjectOnBattle[] attackersData = currentBattleSituationWhenNextSideTurn.attackersByObjectId[bestAttackInCurrentBattleSituation._targetIdOnBattle].ToArray();
                ObjectOnBattle[] activeUnitAttackers = currentBattleSituationWhenNextSideTurn.GetAttackersByTarget(currentActiveUnit).ToArray();
                int damageToActiveUnitInBestAttackInCurrentBattleSituation = BattleEngine.CalculateDamageToTargetById(currentBattleSituationWhenNextSideTurn, activeUnitAttackers, currentMove._activeUnitIdOnBattle);
                if (damageToActiveUnitInBestAttackInCurrentBattleSituation >= currentActiveUnit.Health) continue;
                
                int damageByBestAttackInCurrentBattleSituation = BattleEngine.CalculateDamageToTargetById(currentBattleSituationWhenNextSideTurn, attackersData, bestAttackInCurrentBattleSituation._targetIdOnBattle);
                int currentDistanceToEnemy = currentBattleSituationWhenNextSideTurn.FindDistanceToNearestUnitBySide(currentActiveUnit.Position.First(), Sides.enemySideBySide[battleSituation._sideTurn]);
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
}
