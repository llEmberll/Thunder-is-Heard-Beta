using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SuicidalAI : AbstractAI
{
    /// Самоубийственный ИИ. Атакует если есть цель, иначе максимально приближается к ближайшему врагу,
    /// игнорируя собственную безопасность и возможность быть атакованным

    public override TurnData GetTurn()
    {
        Debug.Log("SuicidalAI GetTurn. SIDE: " + _battleEngine.currentBattleSituation._sideTurn);

        if (_battleEngine.currentBattleSituation.GetUnitsCollectionBySide(_battleEngine.currentBattleSituation._sideTurn).Count < 1) 
        { 
            return new TurnData(); 
        }

        // Сначала проверяем возможность атаки (только по юнитам, исключая здания)
        Dictionary<TurnData, BattleSituation> attackMoves = GetUnitOnlyAttackingSequels(_battleEngine.currentBattleSituation);
        if (attackMoves.Count > 0)
        {
            return GetBestAttack(_battleEngine.currentBattleSituation, attackMoves);
        }

        // Если атаки нет, максимально приближаемся к ближайшему врагу
        Dictionary<TurnData, BattleSituation> movementMoves = _battleEngine.currentBattleSituation.GetAllMovementSequels();
        if (movementMoves.Count < 1)
        {
            return new TurnData();
        }

        return GetSuicidalMovement(_battleEngine.currentBattleSituation, movementMoves);
    }



    private TurnData GetSuicidalMovement(BattleSituation battleSituation, Dictionary<TurnData, BattleSituation> movements)
    {
        // Получаем всех юнитов текущей стороны
        Dictionary<string, ObjectOnBattle> currentSideUnits = battleSituation.GetUnitsCollectionBySide(battleSituation._sideTurn);
        
        // Сортируем юнитов по расстоянию до ближайшего врага (от ближайшего к дальнему)
        var sortedUnits = currentSideUnits.Values
            .Where(unit => unit is UnitOnBattle)
            .Cast<UnitOnBattle>()
            .OrderBy(unit => battleSituation.FindDistanceToMostAccessibleUnitBySide(
                unit.Position.First(), 
                Sides.enemySideBySide[battleSituation._sideTurn]))
            .ToList();

        foreach (UnitOnBattle unit in sortedUnits)
        {
            // Проверяем, может ли этот юнит приблизиться к ближайшему врагу
            TurnData bestMoveForUnit = GetBestMoveForUnitTowardsNearestEnemy(battleSituation, movements, unit);
            if (bestMoveForUnit != null)
            {
                return bestMoveForUnit;
            }
        }

        // Если ни один юнит не может приблизиться, возвращаем пустой ход
        return new TurnData();
    }

    private TurnData GetBestMoveForUnitTowardsNearestEnemy(BattleSituation battleSituation, Dictionary<TurnData, BattleSituation> movements, UnitOnBattle unit)
    {
        // Получаем текущее расстояние до ближайшего врага
        int currentDistance = battleSituation.FindDistanceToMostAccessibleUnitBySide(
            unit.Position.First(), 
            Sides.enemySideBySide[battleSituation._sideTurn]);
        
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
            int newDistance = newBattleSituation.FindDistanceToMostAccessibleUnitBySide(
                newPosition, 
                Sides.enemySideBySide[battleSituation._sideTurn]);
            
            // Вычисляем улучшение расстояния
            int distanceImprovement = currentDistance - newDistance;
            
            // Если это лучшее улучшение, запоминаем ход
            // Игнорируем любые проверки безопасности - просто выбираем максимальное приближение
            if (distanceImprovement > bestDistanceImprovement)
            {
                bestDistanceImprovement = distanceImprovement;
                bestMove = currentMove;
            }
        }

        return bestMove;
    }
} 