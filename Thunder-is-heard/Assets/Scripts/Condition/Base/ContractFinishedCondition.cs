using System;

public class ContractFinishedCondition: BasicCondition
{
    public bool firstCheck = true;

    public string _targetContractId;
    public bool finished = false;


    public ContractFinishedCondition(string targetContractId) 
    {
        _targetContractId = targetContractId;
    }

    public void FirstComplyCheck()
    {
        firstCheck = false;

        finished = IsTargetContractFinish();

        if (!finished && _isActive)
        {
            EnableListeners();
        }
    }

    public bool IsTargetContractFinish()
    {
        ProcessOnBaseCacheTable processTable = Cache.LoadByType<ProcessOnBaseCacheTable>();
        CacheItem processData = processTable.FindBySourceId(_targetContractId);
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
        return string.Equals(processType, ContractItem.type, StringComparison.OrdinalIgnoreCase);
    }

    public void SomeProcessOnBaseFinished(ProcessOnBaseCacheItem processData)
    {
        if (!CheckProcessSourceType(processData.GetSource().type)) return;
        if (processData.GetSource().id != _targetContractId) return;

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
        else if (!finished)
        {
            // Если уже проверяли и контракт не завершен, подписываемся на события
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
