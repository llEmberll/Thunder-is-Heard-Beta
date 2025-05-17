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

        if (!finished)
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

    private bool CheckProcessType(string processType)
    {
        return string.Equals(processType, ContractItem.type, StringComparison.OrdinalIgnoreCase);
    }

    public void SomeProcessOnBaseFinished(ProcessOnBaseCacheItem processData)
    {
        if (!CheckProcessType(processData.GetProcessType())) return;
        if (processData.GetSource().id != _targetContractId) return;

        finished = true;
        DisableListeners();
    }


    public override bool IsComply()
    {
        if (firstCheck)
        {
            FirstComplyCheck();
        }

        return finished;
    }
}
