
public class ContractFinishedCondition: BasicCondition
{
    public string _targetContractId;
    public bool finished = false;


    public ContractFinishedCondition(string targetContractId) 
    {
        _targetContractId = targetContractId;

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

    public void SomeProcessOnBaseFinished(ProcessOnBaseCacheItem processData)
    {
        if (processData.GetProcessType() != ContractItem.type) return;
        if (processData.GetSource().id != _targetContractId) return;

        finished = true;
        DisableListeners();
    }


    public override bool IsComply()
    {
        return finished;
    }
}
