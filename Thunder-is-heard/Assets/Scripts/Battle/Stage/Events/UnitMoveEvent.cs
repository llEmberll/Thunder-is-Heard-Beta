using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class UnitMoveEvent : IScenarioEvent
{
    public ScenarioEventData EventData { get; private set; }
    public bool IsCompleted { get; private set; }

    private Scenario _scenario = GameObject.FindGameObjectWithTag(Tags.scenario).GetComponent<Scenario>();
    public Scenario Scenario { get { return _scenario; } }

    public UnitMoveEvent(ScenarioEventData eventData)
    {
        EventData = eventData;
        IsCompleted = false;
    }

    public IEnumerator Execute()
    {
        if (EventData == null || EventData.eventType != "UnitMove") 
        {
            IsCompleted = true;
            yield break;
        }

        // Получаем параметры из EventData
        string unitId = EventData.GetParameter<string>("unitId");
        float moveSpeed = EventData.GetParameter<float>("moveSpeed", 1f);

        // Обрабатываем route как List<object> и конвертируем в List<Bector2Int>
        var routeObjects = EventData.GetParameter<List<object>>("route");
        List<Bector2Int> route = null;
        
        if (routeObjects != null)
        {
            route = new List<Bector2Int>();
            foreach (var obj in routeObjects)
            {
                if (obj is Bector2Int bector)
                {
                    route.Add(bector);
                }
                else if (obj is JObject jObj)
                {
                    // Если это JObject, конвертируем в Bector2Int
                    var x = jObj["x"]?.Value<int>() ?? 0;
                    var y = jObj["y"]?.Value<int>() ?? 0;
                    route.Add(new Bector2Int(x, y));
                }
            }
        }

        if (string.IsNullOrEmpty(unitId) || route == null || route.Count == 0) 
        {
            Debug.LogError("UnitMoveEvent: Missing required parameters");
            IsCompleted = true;
            yield break;
        }

        Unit unit = _scenario.FindUnitById(unitId);
        if (unit == null)
        {
            Debug.LogError($"UnitMoveEvent: Cannot find unit {unitId}");
            IsCompleted = true;
            yield break;
        }

        // Конвертируем маршрут в клетки
        Dictionary<Bector2Int, Cell> cellsByPositions = _scenario.Map.FindCellsByPosition(route);
        if (cellsByPositions.Count() < 1)
        {
            IsCompleted = true;
            yield break;
        }
        List<Cell> routeCells = cellsByPositions.Values.ToList();

        // Устанавливаем скорость движения
        unit.SetMovementSpeed(moveSpeed);

        // Запускаем движение
        unit.Move(routeCells);

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