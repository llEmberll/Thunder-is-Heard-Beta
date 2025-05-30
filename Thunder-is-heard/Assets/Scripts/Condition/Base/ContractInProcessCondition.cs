using System;

public class ContractInProcessCondition: BasicCondition
{
    public bool firstCheck = true;

    public string _targetContractId;
    public bool process = false;


    public ContractInProcessCondition(string targetContractId) 
    {
        _targetContractId = targetContractId;
        EnableListeners();
    }

    public void FirstComplyCheck()
    {
        firstCheck = false;

        process = IsTargetContractInProcess();
        if (process)
        {
            DisableListeners();
        }
    }

    public bool IsTargetContractInProcess()
    {
        ProcessOnBaseCacheTable processTable = Cache.LoadByType<ProcessOnBaseCacheTable>();
        CacheItem processData = processTable.FindBySourceId(_targetContractId);
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
        return string.Equals(processType, ContractItem.type, StringComparison.OrdinalIgnoreCase);
    }

    public void SomeProcessOnBaseStarted(ProcessOnBaseCacheItem processData)
    {
        if (!CheckProcessSourceType(processData.GetSource().type)) return;
        if (processData.GetSource().id != _targetContractId) return;

        process = true;
        DisableListeners();
    }


    public override bool IsComply()
    {
        if (firstCheck)
        {
            FirstComplyCheck();
        }

        return process;
    }
}
