using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitMoveEvent : IScenarioEvent
{
    public ScenarioEventData EventData { get; private set; }
    public bool IsCompleted { get; private set; }

    private UnitMoveEventData moveData;

    private Scenario _scenario = GameObject.FindGameObjectWithTag(Tags.scenario).GetComponent<Scenario>();
    public Scenario Scenario { get { return _scenario; } }


    public UnitMoveEvent(ScenarioEventData eventData)
    {
        EventData = eventData;
        moveData = eventData as UnitMoveEventData;
        IsCompleted = false;
    }

    public IEnumerator Execute()
    {
        if (moveData == null || moveData.route == null || moveData.route.Count == 0) 
        {
            IsCompleted = true;
            yield break;
        }

        Unit unit = _scenario.FindUnitById(moveData.unitId);
        if (unit == null)
        {
            Debug.LogError($"UnitMoveEvent: Cannot find unit {moveData.unitId}");
            IsCompleted = true;
            yield break;
        }

        // Конвертируем маршрут в клетки
        Dictionary<Bector2Int, Cell> cellsByPositions = _scenario.Map.FindCellsByPosition(moveData.route);
        if (cellsByPositions.Count() < 1)
        {
            IsCompleted = true;
            yield break;
        }
        List<Cell> route = cellsByPositions.Values.ToList();


        // Устанавливаем скорость движения
        unit.SetMovementSpeed(moveData.moveSpeed);

        // Запускаем движение
        unit.Move(route);

        // Ждем завершения движения
        yield return new WaitUntil(() => !unit._onMove);

        // Синхронизируем данные через FightDirector
        _scenario._fightDirector.SyncBattleDataToCurrentBattleSituation();

        IsCompleted = true;
    }

    public void Cancel()
    {
        IsCompleted = true;
    }
} 