using System.Collections.Generic;
using System.Linq;

public class PursuingAI : AbstractAI
{
    /// Преследующий ИИ. Приближает юнитов как можно ближе к указанному объекту

    public override TurnData GetTurn()
    {
        if (_battleEngine.currentBattleSituation.GetUnitsCollectionBySide(_battleEngine.currentBattleSituation._sideTurn).Count < 1) 
        { 
            return new TurnData(); 
        }

        // Проверяем, есть ли объект для преследования
        if (settings.targetObjectIds == null || settings.targetObjectIds.Length == 0)
        {
            return new TurnData();
        }

        string targetObjectId = settings.targetObjectIds[0];
        ObjectOnBattle targetObject = _battleEngine.currentBattleSituation.GetUnitById(targetObjectId);
        if (targetObject == null)
        {
            targetObject = _battleEngine.currentBattleSituation.GetBuildById(targetObjectId);
        }

        if (targetObject == null)
        {
            return new TurnData();
        }

        // Получаем позицию цели
        Bector2Int targetPosition = targetObject.Position.First();

        // Получаем все возможные движения
        Dictionary<TurnData, BattleSituation> movementMoves = _battleEngine.currentBattleSituation.GetAllMovementSequels();
        if (movementMoves.Count < 1)
        {
            return new TurnData();
        }

        return GetBestPursuitMovement(_battleEngine.currentBattleSituation, movementMoves, targetPosition);
    }

    private TurnData GetBestPursuitMovement(BattleSituation battleSituation, Dictionary<TurnData, BattleSituation> movements, Bector2Int targetPosition)
    {
        // Получаем всех юнитов текущей стороны
        Dictionary<string, ObjectOnBattle> currentSideUnits = battleSituation.GetUnitsCollectionBySide(battleSituation._sideTurn);
        
        // Сортируем юнитов по расстоянию до цели (от ближайшего к дальнему)
        var sortedUnits = currentSideUnits.Values
            .Where(unit => unit is UnitOnBattle)
            .Cast<UnitOnBattle>()
            .OrderBy(unit => BattleEngine.GetDistanceBetweenPoints(unit.Position.First(), targetPosition))
            .ToList();

        foreach (UnitOnBattle unit in sortedUnits)
        {
            // Проверяем, может ли этот юнит приблизиться к цели
            TurnData bestMoveForUnit = GetBestMoveForUnitTowardsTarget(battleSituation, movements, unit, targetPosition);
            if (bestMoveForUnit != null)
            {
                return bestMoveForUnit;
            }
        }

        // Если ни один юнит не может приблизиться, возвращаем пустой ход
        return new TurnData();
    }

    private TurnData GetBestMoveForUnitTowardsTarget(BattleSituation battleSituation, Dictionary<TurnData, BattleSituation> movements, UnitOnBattle unit, Bector2Int targetPosition)
    {
        // Получаем текущее расстояние до цели
        int currentDistance = BattleEngine.GetDistanceBetweenPoints(unit.Position.First(), targetPosition);
        
        // Ищем движения для этого конкретного юнита
        var unitMoves = movements.Where(kvp => kvp.Key._activeUnitIdOnBattle == unit.IdOnBattle).ToList();
        
        if (unitMoves.Count == 0)
        {
            return null;
        }

        TurnData bestMove = null;
        int bestDistanceImprovement = 0;

        foreach (var move in unitMoves)
        {
            TurnData currentMove = move.Key;
            BattleSituation newBattleSituation = move.Value;
            
            // Получаем новую позицию юнита после движения
            UnitOnBattle unitAfterMove = newBattleSituation.GetUnitById(unit.IdOnBattle);
            if (unitAfterMove == null) continue;
            
            Bector2Int newPosition = unitAfterMove.Position.First();
            int newDistance = BattleEngine.GetDistanceBetweenPoints(newPosition, targetPosition);
            
            // Вычисляем улучшение расстояния
            int distanceImprovement = currentDistance - newDistance;
            
            // Если это лучшее улучшение, запоминаем ход
            if (distanceImprovement > bestDistanceImprovement)
            {
                bestDistanceImprovement = distanceImprovement;
                bestMove = currentMove;
            }
        }

        return bestMove;
    }
}
