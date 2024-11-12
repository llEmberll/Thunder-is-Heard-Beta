using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WaitingAI : AbstractAI
{
    /// Выжидательное поведение. Атакует при возможности. Если под боем, то нападает

    public override TurnData GetTurn()
    {
        Dictionary<TurnData, BattleSituation> attackMoves = _battleEngine.currentBattleSituation.GetAllAttackingSequels();
        if (attackMoves.Count > 0)
        {
            return GetBestAttack(_battleEngine.currentBattleSituation, attackMoves);
        }

        Dictionary<string, ObjectOnBattle> objectsUnderAttack = _battleEngine.currentBattleSituation.GetTargetsBySide(_battleEngine.currentBattleSituation._sideTurn);
        Dictionary<TurnData, BattleSituation> movementMoves = _battleEngine.currentBattleSituation.GetAllMovementSequels();
        if (objectsUnderAttack.Count < 1 || movementMoves.Count < 1)
        {
            //Пас
            return new TurnData();
        }

        return GetBestMovementUnderAttack(_battleEngine.currentBattleSituation, movementMoves);
    }

    public static TurnData GetBestAttack(BattleSituation currentBattleSituation, Dictionary<TurnData, BattleSituation> attacks)
    {
        TurnData turnWithGreaterDamage = null;
        int greaterDamage = 0;

        Dictionary<TurnData, BattleSituation> destructions = new Dictionary<TurnData, BattleSituation>();
        foreach (var keyValuePair in  attacks)
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

    public static TurnData GetBestDestruction(BattleSituation currentBattleSituation, Dictionary<TurnData, BattleSituation> destructions)
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

    public TurnData GetBestMovementUnderAttack(BattleSituation battleSituation, Dictionary<TurnData, BattleSituation> movements)
    {
        TurnData bestMoveWithCounterAttack = null;
        int greaterDamageByCounterAttackOnNextTurn = 0;

        List<string> uniqueActiveUnitIds = new List<string>();

        foreach (var keyValuePair in movements)
        {
            TurnData currentMove = keyValuePair.Key;
            BattleSituation currentBattleSituation = keyValuePair.Value;

            Dictionary<TurnData, BattleSituation> attackMovesInCurrentBattleSituation = currentBattleSituation.GetAllAttackingSequels();
            if (attackMovesInCurrentBattleSituation.Count > 0)
            {
                TurnData bestAttackInCurrentBattleSituation = GetBestAttack(currentBattleSituation, attackMovesInCurrentBattleSituation);

                ObjectOnBattle[] attackersData = currentBattleSituation.attackersByObjectId[bestAttackInCurrentBattleSituation._targetIdOnBattle].ToArray();
                //Если урон по активному юниту в текущем лучшем контратакующем ходу будет смертельным, то damageByBestAttackInCurrentBattleSituation = 0
                //Иначе:
                int damageByBestAttackInCurrentBattleSituation = BattleEngine.CalculateDamageToTargetById(currentBattleSituation, attackersData, currentMove._targetIdOnBattle);

                if (damageByBestAttackInCurrentBattleSituation > greaterDamageByCounterAttackOnNextTurn)
                {
                    bestMoveWithCounterAttack = bestAttackInCurrentBattleSituation;
                    greaterDamageByCounterAttackOnNextTurn = damageByBestAttackInCurrentBattleSituation;
                }
            }
            else
            {
                if (!uniqueActiveUnitIds.Contains(currentMove._activeUnitIdOnBattle)) uniqueActiveUnitIds.Add(currentMove._activeUnitIdOnBattle);
            }
        }

        if (bestMoveWithCounterAttack != null) return bestMoveWithCounterAttack;


        TurnData moveWithFastestApproachToAttack = null;
        int minTurnCountForReachAttackDistance = int.MaxValue;

        foreach (string  uniqueUnitId in uniqueActiveUnitIds)
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
}
