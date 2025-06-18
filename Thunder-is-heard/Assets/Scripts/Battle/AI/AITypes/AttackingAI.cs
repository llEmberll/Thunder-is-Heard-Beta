using System.Collections.Generic;
using System.Linq;

public class AttackingAI : AbstractAI
{
    /// Агрессивное поведение. Атакует аккуратно при возможности, иначе подтягивает остальные силы к фронту. При тупике отчаянно атакует

    public override TurnData GetTurn()
    {
        if (_battleEngine.currentBattleSituation.GetUnitsCollectionBySide(_battleEngine.currentBattleSituation._sideTurn).Count < 1) { return new TurnData(); }

        Dictionary<TurnData, BattleSituation> attackMoves = _battleEngine.currentBattleSituation.GetAllAttackingSequels();
        if (attackMoves.Count > 0)
        {
            return GetBestAttack(_battleEngine.currentBattleSituation, attackMoves);
        }

        Dictionary<TurnData, BattleSituation> movementMoves = _battleEngine.currentBattleSituation.GetAllMovementSequels();
        if (movementMoves.Count < 1)
        {
            return new TurnData();
        }

        return GetBestMovement(_battleEngine.currentBattleSituation, movementMoves);
    }

    public TurnData GetBestMovement(BattleSituation battleSituation, Dictionary<TurnData, BattleSituation> movements)
    {
        TurnData bestMoveWithNextTurnAttack = GetBestAttackOnNextTurn(battleSituation, movements);
        if (bestMoveWithNextTurnAttack != null) return bestMoveWithNextTurnAttack;

        Dictionary<string, ObjectOnBattle> objectsUnderAttack = battleSituation.GetUnitsUnderAttackBySide(battleSituation._sideTurn);
        if (objectsUnderAttack.Count > 0)
        {
            return GetMovementWithFastestApproachToAttack(battleSituation, movements);
        }
        else
        {
            return GetBestOffensiveMovement(battleSituation, movements);
        }
    }

    public TurnData GetBestOffensiveMovement(BattleSituation battleSituation, Dictionary<TurnData, BattleSituation> movements)
    {
        TurnData bestMovementWithAccurateBringToNearestEnemy = FindBestMovementWithAccurateBringToNearestEnemy(battleSituation, movements);
        if (bestMovementWithAccurateBringToNearestEnemy != null) return bestMovementWithAccurateBringToNearestEnemy;

        //Если нет безопасного хода, приближающего союзника к врагу
        return GetMovementWithFastestApproachToAttack(battleSituation, movements);
    }


    public TurnData FindBestMovementWithAccurateBringToNearestEnemy(BattleSituation battleSituation, Dictionary<TurnData, BattleSituation> movements)
    {
        TurnData bestMove = null;
        int passedDistance = 0;

        foreach (var keyValuePair in movements)
        {
            TurnData currentMove = keyValuePair.Key;
            BattleSituation currentBattleSituation = keyValuePair.Value;
            UnitOnBattle currentActiveUnit = battleSituation.GetUnitById(currentMove._activeUnitIdOnBattle);

            if (currentBattleSituation.attackersByObjectId.ContainsKey(currentActiveUnit.IdOnBattle)) continue;

            int currentUnitOldDistanceToEnemy = battleSituation.FindDistanceToNearestUnitBySide(currentActiveUnit.Position.First(), Sides.enemySideBySide[battleSituation._sideTurn]);
            int currentUnitNewDistanceToEnemy = currentBattleSituation.FindDistanceToNearestUnitBySide(currentActiveUnit.Position.First(), Sides.enemySideBySide[battleSituation._sideTurn]);
            if (currentUnitOldDistanceToEnemy <= currentUnitNewDistanceToEnemy) continue;

            int currentPassedDistance = currentUnitOldDistanceToEnemy - currentUnitNewDistanceToEnemy;
            if (passedDistance < currentPassedDistance)
            {
                passedDistance = currentPassedDistance;
                bestMove = currentMove;
            }
        }

        return bestMove;
    }
}
