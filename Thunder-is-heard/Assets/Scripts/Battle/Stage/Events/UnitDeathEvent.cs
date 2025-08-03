using System.Collections;
using UnityEngine;

public class UnitDeathEvent : IScenarioEvent
{
    public ScenarioEventData EventData { get; private set; }
    public bool IsCompleted { get; private set; }

    private Scenario _scenario = GameObject.FindGameObjectWithTag(Tags.scenario).GetComponent<Scenario>();
    public Scenario Scenario { get { return _scenario; } }

    public UnitDeathEvent(ScenarioEventData eventData)
    {
        EventData = eventData;
        IsCompleted = false;
    }

    public IEnumerator Execute()
    {
        if (EventData == null || EventData.eventType != "UnitDeath")
        {
            IsCompleted = true;
            yield break;
        }

        // Получаем параметры из EventData
        string unitId = EventData.GetParameter<string>("unitId");
        bool playAnimation = EventData.GetParameter<bool>("playAnimation", true);

        if (string.IsNullOrEmpty(unitId))
        {
            Debug.LogError("UnitDeathEvent: Missing required parameter unitId");
            IsCompleted = true;
            yield break;
        }

        Unit unit = _scenario.FindUnitById(unitId);
        if (unit == null)
        {
            Debug.LogError($"UnitDeathEvent: Cannot find unit {unitId}");
            IsCompleted = true;
            yield break;
        }

        if (playAnimation)
        {
            // Запускаем анимацию смерти
            unit.Die();
            
            // Ждем завершения анимации
            yield return new WaitForSeconds(2f);
        }

        // Логическое удаление юнита через BattleEngine
        BattleEngine.RemoveUnitFromBattle(_scenario._fightDirector._battleEngine.currentBattleSituation, unit.ChildId);
        
        // Синхронизируем данные через FightDirector
        _scenario._fightDirector.SyncBattleDataToCurrentBattleSituation();

        IsCompleted = true;
    }

    public void Cancel()
    {
        IsCompleted = true;
    }
} 