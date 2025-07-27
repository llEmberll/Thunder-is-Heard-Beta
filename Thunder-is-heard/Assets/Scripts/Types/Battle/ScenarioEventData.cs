using System.Collections.Generic;


[System.Serializable]
public abstract class ScenarioEventData
{
    public string eventType;
    public float delay = 0f; // Задержка перед выполнением события
    public bool waitForCompletion = true; // Ждать ли завершения события перед следующим
    public bool executeInParallel = false; // Выполнять ли событие параллельно с другими

    public ScenarioEventData(string type, float delay = 0f, bool waitForCompletion = true, bool executeInParallel = false)
    {
        eventType = type;
        this.delay = delay;
        this.waitForCompletion = waitForCompletion;
        this.executeInParallel = executeInParallel;
    }
}

[System.Serializable]
public class UnitAttackEventData : ScenarioEventData
{
    public string attackerUnitId;
    public string targetId;
    public bool instantKill = false; // Мгновенное убийство цели

    public UnitAttackEventData() : base("UnitAttack") { }
    
    public UnitAttackEventData(string attackerUnitId, string targetId, bool instantKill = false, float delay = 0f, bool waitForCompletion = true, bool executeInParallel = false) 
        : base("UnitAttack", delay, waitForCompletion, executeInParallel)
    {
        this.attackerUnitId = attackerUnitId;
        this.targetId = targetId;
        this.instantKill = instantKill;
    }
}

[System.Serializable]
public class MultiUnitAttackEventData : ScenarioEventData
{
    public string[] attackerUnitIds; // Массив атакующих юнитов
    public string[] targetIds; // Массив целей (должен соответствовать количеству атакующих)
    public bool instantKill = false; // Мгновенное убийство целей

    public MultiUnitAttackEventData() : base("MultiUnitAttack", executeInParallel: true) { }
    
    public MultiUnitAttackEventData(string[] attackerUnitIds, string[] targetIds, bool instantKill = false, float delay = 0f, bool waitForCompletion = true, bool executeInParallel = true) 
        : base("MultiUnitAttack", delay, waitForCompletion, executeInParallel)
    {
        this.attackerUnitIds = attackerUnitIds;
        this.targetIds = targetIds;
        this.instantKill = instantKill;
    }
}

[System.Serializable]
public class UnitMoveEventData : ScenarioEventData
{
    public string unitId;
    public List<Bector2Int> route;
    public float moveSpeed = 1f;

    public UnitMoveEventData() : base("UnitMove") { }
    
    public UnitMoveEventData(string unitId, List<Bector2Int> route, float moveSpeed = 1f, float delay = 0f, bool waitForCompletion = true, bool executeInParallel = false) 
        : base("UnitMove", delay, waitForCompletion, executeInParallel)
    {
        this.unitId = unitId;
        this.route = route;
        this.moveSpeed = moveSpeed;
    }
}

[System.Serializable]
public class UnitRotateEventData : ScenarioEventData
{
    public string unitId;
    public Bector2Int targetPosition; // Позиция, к которой нужно повернуться
    public int rotation; // Или конкретный угол

    public UnitRotateEventData() : base("UnitRotate") { }
    
    public UnitRotateEventData(string unitId, Bector2Int targetPosition, int rotation = 0, float delay = 0f, bool waitForCompletion = true, bool executeInParallel = false) 
        : base("UnitRotate", delay, waitForCompletion, executeInParallel)
    {
        this.unitId = unitId;
        this.targetPosition = targetPosition;
        this.rotation = rotation;
    }
    
    public UnitRotateEventData(string unitId, int rotation, float delay = 0f, bool waitForCompletion = true, bool executeInParallel = false) 
        : base("UnitRotate", delay, waitForCompletion, executeInParallel)
    {
        this.unitId = unitId;
        this.rotation = rotation;
    }
}

[System.Serializable]
public class UnitDeathEventData : ScenarioEventData
{
    public string unitId;
    public bool playAnimation = true;

    public UnitDeathEventData() : base("UnitDeath") { }
    
    public UnitDeathEventData(string unitId, bool playAnimation = true, float delay = 0f, bool waitForCompletion = true, bool executeInParallel = false) 
        : base("UnitDeath", delay, waitForCompletion, executeInParallel)
    {
        this.unitId = unitId;
        this.playAnimation = playAnimation;
    }
}

[System.Serializable]
public class MultiUnitDeathEventData : ScenarioEventData
{
    public string[] unitIds; // Массив юнитов для уничтожения
    public bool playAnimation = true;

    public MultiUnitDeathEventData() : base("MultiUnitDeath", executeInParallel: true) { }
    
    public MultiUnitDeathEventData(string[] unitIds, bool playAnimation = true, float delay = 0f, bool waitForCompletion = true, bool executeInParallel = true) 
        : base("MultiUnitDeath", delay, waitForCompletion, executeInParallel)
    {
        this.unitIds = unitIds;
        this.playAnimation = playAnimation;
    }
}

[System.Serializable]
public class WaitEventData : ScenarioEventData
{
    public float waitTime = 1f;

    public WaitEventData() : base("Wait") { }
    
    public WaitEventData(float waitTime, float delay = 0f, bool waitForCompletion = true, bool executeInParallel = false) 
        : base("Wait", delay, waitForCompletion, executeInParallel)
    {
        this.waitTime = waitTime;
    }
}