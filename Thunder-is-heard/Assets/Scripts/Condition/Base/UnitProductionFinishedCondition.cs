
using System;

public class UnitProductionFinishedCondition: BasicCondition
{
    public bool firstCheck = true;

    public string _targetUnitProductionId;
    public bool finished = false;


    public UnitProductionFinishedCondition(string targetUnitProductionId) 
    {
        _targetUnitProductionId = targetUnitProductionId;
    }

    public void FirstComplyCheck()
    {
        firstCheck = false;

        finished = IsTargetUnitProductionFinish();

        if (!finished && _isActive)
        {
            EnableListeners();
        }
    }

    public bool IsTargetUnitProductionFinish()
    {
        ProcessOnBaseCacheTable processTable = Cache.LoadByType<ProcessOnBaseCacheTable>();
        CacheItem processData = processTable.FindBySourceId(_targetUnitProductionId);
        return processData == null;
    }

    public void EnableListeners()
    {
        EventMaster.current.ProcessOnBaseFinished += SomeProcessOnBaseFinished;
    }

    public void DisableListeners()
    {
        EventMaster.current.ProcessOnBaseFinished -= SomeProcessOnBaseFinished;
    }

    private bool CheckProcessSourceType(string processType)
    {
        return string.Equals(processType, UnitProductionItem.type, StringComparison.OrdinalIgnoreCase);
    }

    public void SomeProcessOnBaseFinished(ProcessOnBaseCacheItem processData)
    {
        if (!CheckProcessSourceType(processData.GetSource().type)) return;
        if (processData.GetSource().id != _targetUnitProductionId) return;

        finished = true;
        DisableListeners();
    }

    protected override void OnActivate()
    {
        // При активации проверяем текущее состояние
        if (firstCheck)
        {
            FirstComplyCheck();
        }
        if (!finished)
        {
            // Если уже проверяли и производство не завершено, подписываемся на события
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
        finished = false;
        DisableListeners();
    }

    public override bool IsComply()
    {
        if (firstCheck && _isActive)
        {
            FirstComplyCheck();
        }

        return finished;
    }

    public override bool IsRealTimeUpdate()
    {
        return true;
    }
}
