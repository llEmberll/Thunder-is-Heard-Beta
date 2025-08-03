using System.Collections;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class UnitRotateEvent : IScenarioEvent
{
    public ScenarioEventData EventData { get; private set; }
    public bool IsCompleted { get; private set; }

    private Scenario _scenario = GameObject.FindGameObjectWithTag(Tags.scenario).GetComponent<Scenario>();
    public Scenario Scenario { get { return _scenario; } }

    public UnitRotateEvent(ScenarioEventData eventData)
    {
        EventData = eventData;
        IsCompleted = false;
    }

    public IEnumerator Execute()
    {
        if (EventData == null || EventData.eventType != "UnitRotate")
        {
            IsCompleted = true;
            yield break;
        }

        // Получаем параметры из EventData
        string unitId = EventData.GetParameter<string>("unitId");
        int rotation = EventData.GetParameter<int>("rotation", 0);

        // Обрабатываем targetPosition
        Bector2Int targetPosition = null;
        var targetPositionObj = EventData.GetParameter<object>("targetPosition");
        if (targetPositionObj is Bector2Int bector)
        {
            targetPosition = bector;
        }
        else if (targetPositionObj is JObject jObj)
        {
            // Если это JObject, конвертируем в Bector2Int
            var x = jObj["x"]?.Value<int>() ?? 0;
            var y = jObj["y"]?.Value<int>() ?? 0;
            targetPosition = new Bector2Int(x, y);
        }

        if (string.IsNullOrEmpty(unitId))
        {
            Debug.LogError("UnitRotateEvent: Missing required parameter unitId");
            IsCompleted = true;
            yield break;
        }

        Unit unit = _scenario.FindUnitById(unitId);
        if (unit == null)
        {
            Debug.LogError($"UnitRotateEvent: Cannot find unit {unitId}");
            IsCompleted = true;
            yield break;
        }

        if (targetPosition != null)
        {
            // Поворачиваемся к указанной позиции
            unit.RotateToTarget(targetPosition.ToVector2Int());
        }
        else
        {
            // Поворачиваемся на конкретный угол
            unit.SetRotation(rotation);
        }

        // Небольшая пауза для завершения поворота
        yield return new WaitForSeconds(0.3f);

        IsCompleted = true;
    }

    public void Cancel()
    {
        IsCompleted = true;
    }
} 