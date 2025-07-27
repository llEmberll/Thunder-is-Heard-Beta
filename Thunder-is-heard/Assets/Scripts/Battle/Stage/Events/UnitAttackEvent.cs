using System.Collections;
using UnityEngine;

public class UnitAttackEvent : IScenarioEvent
{
    public ScenarioEventData EventData { get; private set; }
    public bool IsCompleted { get; private set; }

    private UnitAttackEventData attackData;

    private Scenario _scenario = GameObject.FindGameObjectWithTag(Tags.scenario).GetComponent<Scenario>();
    public Scenario Scenario { get { return _scenario; } }


    public UnitAttackEvent(ScenarioEventData eventData)
    {
        EventData = eventData;
        attackData = eventData as UnitAttackEventData;
        IsCompleted = false;
    }

    public IEnumerator Execute()
    {
        if (attackData == null) yield break;

        // Находим атакующего через Scenario
        Unit attacker = _scenario.FindUnitById(attackData.attackerUnitId);
        
        // Находим цель (может быть Unit или Build)
        Entity target = _scenario.FindUnitById(attackData.targetId);
        if (target == null)
        {
            target = _scenario.FindBuildById(attackData.targetId);
        }

        if (attacker == null || target == null)
        {
            Debug.LogError($"UnitAttackEvent: Cannot find attacker ({attackData.attackerUnitId}) or target ({attackData.targetId})");
            IsCompleted = true;
            yield break;
        }

        // Запускаем анимацию атаки
        attacker.Attack(target);

        // Ждем завершения анимации атаки
        yield return new WaitForSeconds(1f);

        // Если нужно мгновенное убийство
        if (attackData.instantKill)
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