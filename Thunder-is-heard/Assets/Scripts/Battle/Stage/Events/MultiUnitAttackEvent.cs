using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiUnitAttackEvent : IScenarioEvent
{
    public ScenarioEventData EventData { get; private set; }
    public bool IsCompleted { get; private set; }

    private Scenario _scenario = GameObject.FindGameObjectWithTag(Tags.scenario).GetComponent<Scenario>();
    public Scenario Scenario { get { return _scenario; } }

    public MultiUnitAttackEvent(ScenarioEventData eventData)
    {
        EventData = eventData;
        IsCompleted = false;
    }

    public IEnumerator Execute()
    {
        if (EventData == null || EventData.eventType != "MultiUnitAttack") 
        {
            IsCompleted = true;
            yield break;
        }

        // Получаем параметры из EventData
        string[] attackerUnitIds = EventData.GetParameter<string[]>("attackerUnitIds");
        string[] targetIds = EventData.GetParameter<string[]>("targetIds");
        bool instantKill = EventData.GetParameter<bool>("instantKill", false);

        if (attackerUnitIds == null || targetIds == null)
        {
            Debug.LogError("MultiUnitAttackEvent: Missing required parameters");
            IsCompleted = true;
            yield break;
        }

        if (attackerUnitIds.Length != targetIds.Length)
        {
            Debug.LogError($"MultiUnitAttackEvent: Mismatch between attackers ({attackerUnitIds.Length}) and targets ({targetIds.Length})");
            IsCompleted = true;
            yield break;
        }

        // Находим всех атакующих и целей
        var attackers = new List<Unit>();
        var targets = new List<Entity>();

        for (int i = 0; i < attackerUnitIds.Length; i++)
        {
            Unit attacker = _scenario.FindUnitById(attackerUnitIds[i]);
            if (attacker == null)
            {
                Debug.LogError($"MultiUnitAttackEvent: Cannot find attacker {attackerUnitIds[i]}");
                continue;
            }

            Entity target = _scenario.FindUnitById(targetIds[i]);
            if (target == null)
            {
                target = _scenario.FindBuildById(targetIds[i]);
            }
            if (target == null)
            {
                Debug.LogError($"MultiUnitAttackEvent: Cannot find target {targetIds[i]}");
                continue;
            }

            attackers.Add(attacker);
            targets.Add(target);
        }

        if (attackers.Count == 0)
        {
            Debug.LogError("MultiUnitAttackEvent: No valid attacker-target pairs found");
            IsCompleted = true;
            yield break;
        }

        // Запускаем все атаки одновременно
        for (int i = 0; i < attackers.Count; i++)
        {
            attackers[i].Attack(targets[i]);
        }

        // Ждем завершения анимации атаки
        yield return new WaitForSeconds(1f);

        // Если нужно мгновенное убийство
        if (instantKill)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                BattleEngine.RemoveObjectFromBattle(_scenario._fightDirector._battleEngine.currentBattleSituation, targets[i].ChildId);
                targets[i].Die();
            }

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