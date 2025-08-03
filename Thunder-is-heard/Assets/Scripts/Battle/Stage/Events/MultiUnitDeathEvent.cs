using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiUnitDeathEvent : IScenarioEvent
{
    public ScenarioEventData EventData { get; private set; }
    public bool IsCompleted { get; private set; }

    private Scenario _scenario = GameObject.FindGameObjectWithTag(Tags.scenario).GetComponent<Scenario>();
    public Scenario Scenario { get { return _scenario; } }

    public MultiUnitDeathEvent(ScenarioEventData eventData)
    {
        EventData = eventData;
        IsCompleted = false;
    }

    public IEnumerator Execute()
    {
        if (EventData == null || EventData.eventType != "MultiUnitDeath")
        {
            IsCompleted = true;
            yield break;
        }

        // Получаем параметры из EventData
        string[] unitIds = EventData.GetParameter<string[]>("unitIds");
        bool playAnimation = EventData.GetParameter<bool>("playAnimation", true);

        if (unitIds == null || unitIds.Length == 0)
        {
            Debug.LogError("MultiUnitDeathEvent: Missing required parameter unitIds");
            IsCompleted = true;
            yield break;
        }

        var units = new List<Unit>();

        // Находим всех юнитов
        foreach (string unitId in unitIds)
        {
            Unit unit = _scenario.FindUnitById(unitId);
            if (unit == null)
            {
                Debug.LogError($"MultiUnitDeathEvent: Cannot find unit {unitId}");
                continue;
            }
            units.Add(unit);
        }

        if (units.Count == 0)
        {
            Debug.LogError("MultiUnitDeathEvent: No valid units found");
            IsCompleted = true;
            yield break;
        }

        if (playAnimation)
        {
            // Запускаем анимации смерти для всех юнитов одновременно
            foreach (Unit unit in units)
            {
                unit.Die();
            }
            
            // Ждем завершения анимации
            yield return new WaitForSeconds(2f);
        }

        // Логическое удаление всех юнитов через BattleEngine
        foreach (Unit unit in units)
        {
            BattleEngine.RemoveUnitFromBattle(_scenario._fightDirector._battleEngine.currentBattleSituation, unit.ChildId);
        }
        
        // Синхронизируем данные через FightDirector
        _scenario._fightDirector.SyncBattleDataToCurrentBattleSituation();

        IsCompleted = true;
    }

    public void Cancel()
    {
        IsCompleted = true;
    }
} 