
using System;

public class UnitProductionInProcessCondition: BasicCondition
{
    public bool firstCheck = true;

    public string _targetUnitProductionId;
    public bool process = false;


    public UnitProductionInProcessCondition(string targetUnitProductionId) 
    {
        _targetUnitProductionId = targetUnitProductionId;
        // Убираем EnableListeners() из конструктора - теперь это будет в OnActivate
    }

    public void FirstComplyCheck()
    {
        firstCheck = false;

        process = IsTargetUnitProductionInProcess();

        if (process)
        {
            DisableListeners();
        }
    }

    public bool IsTargetUnitProductionInProcess()
    {
        ProcessOnBaseCacheTable processTable = Cache.LoadByType<ProcessOnBaseCacheTable>();
        CacheItem processData = processTable.FindBySourceId(_targetUnitProductionId);
        return processData != null;
    }

    public void EnableListeners()
    {
        EventMaster.current.ProcessOnBaseStarted += SomeProcessOnBaseStarted;
    }

    public void DisableListeners()
    {
        EventMaster.current.ProcessOnBaseStarted -= SomeProcessOnBaseStarted;
    }

    private bool CheckProcessSourceType(string processType)
    {
        return string.Equals(processType, UnitProductionItem.type, StringComparison.OrdinalIgnoreCase);
    }

    public void SomeProcessOnBaseStarted(ProcessOnBaseCacheItem processData)
    {
        if (!CheckProcessSourceType(processData.GetSource().type)) return;
        if (processData.GetSource().id != _targetUnitProductionId) return;

        process = true;
        DisableListeners();
    }

    protected override void OnActivate()
    {
        // При активации проверяем текущее состояние
        if (firstCheck)
        {
            FirstComplyCheck();
        }
        else if (!process)
        {
            // Если уже проверяли и производство не в процессе, подписываемся на события
            EnableListeners();
        }
    }
    
    protected override void OnDeactivate()
    {
        DisableListeners();
    }
    
    protected override void OnReset()
    {
        firstCheck = true;
        process = false;
        DisableListeners();
    }

    public override bool IsComply()
    {
        if (firstCheck && _isActive)
        {
            FirstComplyCheck();
        }

        return process;
    }

    public override bool IsRealTimeUpdate()
    {
        return true;
    }
}
