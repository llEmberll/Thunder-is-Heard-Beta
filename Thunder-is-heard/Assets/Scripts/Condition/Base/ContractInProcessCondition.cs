
public class ContractInProcessCondition: BasicCondition
{
    public string _targetContractId;
    public bool process = false;


    public ContractInProcessCondition(string targetContractId) 
    {
        _targetContractId = targetContractId;

        process = IsTargetContractInProcess();

        if (!process)
        {
            EnableListeners();
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

    public void SomeProcessOnBaseStarted(ProcessOnBaseCacheItem processData)
    {
        if (processData.GetProcessType() != ContractItem.type) return;
        if (processData.GetSource().id != _targetContractId) return;

        process = true;
        DisableListeners();
    }


    public override bool IsComply()
    {
        return process;
    }
}
