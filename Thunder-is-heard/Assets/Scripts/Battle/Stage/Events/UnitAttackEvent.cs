using System.Collections;
using UnityEngine;

public class UnitAttackEvent : IScenarioEvent
{
    public ScenarioEventData EventData { get; private set; }
    public bool IsCompleted { get; private set; }

    private Scenario _scenario = GameObject.FindGameObjectWithTag(Tags.scenario).GetComponent<Scenario>();
    public Scenario Scenario { get { return _scenario; } }

    public UnitAttackEvent(ScenarioEventData eventData)
    {
        EventData = eventData;
        IsCompleted = false;
    }

    public IEnumerator Execute()
    {
        if (EventData == null || EventData.eventType != "UnitAttack") yield break;

        // Получаем параметры из EventData
        string attackerUnitId = EventData.GetParameter<string>("attackerUnitId");
        string targetId = EventData.GetParameter<string>("targetId");
        bool instantKill = EventData.GetParameter<bool>("instantKill", false);

        if (string.IsNullOrEmpty(attackerUnitId) || string.IsNullOrEmpty(targetId))
        {
            Debug.LogError($"UnitAttackEvent: Missing required parameters: attackerUnitId={attackerUnitId}, targetId={targetId}");
            IsCompleted = true;
            yield break;
        }

        // Находим атакующего через Scenario
        Unit attacker = _scenario.FindUnitById(attackerUnitId);
        
        // Находим цель (может быть Unit или Build)
        Entity target = _scenario.FindUnitById(targetId);
        if (target == null)
        {
            target = _scenario.FindBuildById(targetId);
        }

        if (attacker == null || target == null)
        {
            Debug.LogError($"UnitAttackEvent: Cannot find attacker ({attackerUnitId}) or target ({targetId})");
            IsCompleted = true;
            yield break;
        }

        // Запускаем анимацию атаки
        attacker.Attack(target);

        // Ждем завершения анимации атаки
        yield return new WaitForSeconds(1f);

        // Если нужно мгновенное убийство
        if (instantKill)
        {
            BattleEngine.RemoveObjectFromBattle(_scenario._fightDirector._battleEngine.currentBattleSituation, target.ChildId);

            // Визуальное уничтожение через правильный метод
            target.Die();

            // Ждем завершения анимации смерти
            yield return new WaitForSeconds(2f);

            // Синхронизируем данные через FightDirector
            _scenario._fightDirector.SyncBattleDataToCurrentBattleSituation();
        }

        IsCompleted = true;
    }

    public void Cancel()
    {
        IsCompleted = true;
    }
} 