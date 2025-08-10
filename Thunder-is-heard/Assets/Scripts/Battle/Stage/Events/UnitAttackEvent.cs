using System.Collections;
using UnityEngine;

public class UnitAttackEvent : IScenarioEvent
{
    public ScenarioEventData EventData { get; private set; }
    public bool IsCompleted { get; private set; }

    private Scenario _scenario = GameObject.FindGameObjectWithTag(Tags.scenario).GetComponent<Scenario>();
    public Scenario Scenario { get { return _scenario; } }

    private BattleEngine _battleEngine = GameObjectUtils.FindGameObjectByTagIncludingInactive(Tags.battleEngine).GetComponent<BattleEngine>();

    public UnitAttackEvent(ScenarioEventData eventData)
    {
        EventData = eventData;
        IsCompleted = false;
        Debug.Log($"[UnitAttackEvent] Created with eventData: {eventData?.eventType}");
    }

    public IEnumerator Execute()
    {
        Debug.Log($"[UnitAttackEvent] Execute started for event type: {EventData?.eventType}");
        
        if (EventData == null || EventData.eventType != "UnitAttack") 
        {
            Debug.LogError($"[UnitAttackEvent] Invalid event data or type: {EventData?.eventType}");
            IsCompleted = true;
            yield break;
        }

        // Получаем параметры из EventData
        string attackerUnitId = EventData.GetParameter<string>("attackerUnitId");
        string targetId = EventData.GetParameter<string>("targetId");
        bool instantKill = EventData.GetParameter<bool>("instantKill", false);

        Debug.Log($"[UnitAttackEvent] Parameters - attackerUnitId: {attackerUnitId}, targetId: {targetId}, instantKill: {instantKill}");

        if (string.IsNullOrEmpty(attackerUnitId) || string.IsNullOrEmpty(targetId))
        {
            Debug.LogError($"UnitAttackEvent: Missing required parameters: attackerUnitId={attackerUnitId}, targetId={targetId}");
            IsCompleted = true;
            yield break;
        }

        // Находим атакующего через Scenario
        Debug.Log($"[UnitAttackEvent] Finding attacker unit with ID: {attackerUnitId}");
        Unit attacker = _scenario.FindUnitById(attackerUnitId);
        
        // Находим цель (может быть Unit или Build)
        Debug.Log($"[UnitAttackEvent] Finding target with ID: {targetId}");
        Entity target = _scenario.FindUnitById(targetId);
        if (target == null)
        {
            Debug.Log($"[UnitAttackEvent] Target not found as unit, trying to find as build");
            target = _scenario.FindBuildById(targetId);
        }

        if (attacker == null || target == null)
        {
            Debug.LogError($"UnitAttackEvent: Cannot find attacker ({attackerUnitId}) or target ({targetId})");
            IsCompleted = true;
            yield break;
        }

        Debug.Log($"[UnitAttackEvent] Found attacker: {attacker.name} and target: {target.name}");

        // Запускаем анимацию атаки
        Debug.Log($"[UnitAttackEvent] Starting attack animation");
        attacker.Attack(target);

        // Ждем завершения анимации атаки
        Debug.Log($"[UnitAttackEvent] Waiting 1 second for attack animation");
        yield return new WaitForSeconds(1f);

        int damage = attacker.damage;
        if (instantKill)
        {
            damage = int.MaxValue;
            Debug.Log($"[UnitAttackEvent] Instant kill enabled, setting damage to max");
        }

        Debug.Log($"[UnitAttackEvent] Applying damage: {damage} to target: {target.name}");
        BattleEngine.OnAttackTarget(_battleEngine.currentBattleSituation, target, damage);

        target.GetDamage(damage);

        Debug.Log($"[UnitAttackEvent] Waiting 2 seconds after damage application");
        yield return new WaitForSeconds(2f);

        // Синхронизируем данные через FightDirector
        Debug.Log($"[UnitAttackEvent] Syncing battle data");
        _scenario._fightDirector.SyncBattleDataToCurrentBattleSituation();

        Debug.Log($"[UnitAttackEvent] Setting IsCompleted to true");
        IsCompleted = true;
        Debug.Log($"[UnitAttackEvent] Execute completed successfully");
    }

    public void Cancel()
    {
        Debug.Log($"[UnitAttackEvent] Cancel called, setting IsCompleted to true");
        IsCompleted = true;
    }
} 