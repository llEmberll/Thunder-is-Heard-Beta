using System.Collections;
using UnityEngine;

public class UnitRotateEvent : IScenarioEvent
{
    public ScenarioEventData EventData { get; private set; }
    public bool IsCompleted { get; private set; }

    private UnitRotateEventData rotateData;

    private Scenario _scenario = GameObject.FindGameObjectWithTag(Tags.scenario).GetComponent<Scenario>();
    public Scenario Scenario { get { return _scenario; } }



    public UnitRotateEvent(ScenarioEventData eventData)
    {
        EventData = eventData;
        rotateData = eventData as UnitRotateEventData;
        IsCompleted = false;
    }

    public IEnumerator Execute()
    {
        if (rotateData == null)
        {
            IsCompleted = true;
            yield break;
        }

        Unit unit = _scenario.FindUnitById(rotateData.unitId);
        if (unit == null)
        {
            Debug.LogError($"UnitRotateEvent: Cannot find unit {rotateData.unitId}");
            IsCompleted = true;
            yield break;
        }

        if (rotateData.targetPosition != null)
        {
            // Поворачиваемся к указанной позиции
            unit.RotateToTarget(rotateData.targetPosition.ToVector2Int());
        }
        else
        {
            // Поворачиваемся на конкретный угол
            unit.SetRotation(rotateData.rotation);
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