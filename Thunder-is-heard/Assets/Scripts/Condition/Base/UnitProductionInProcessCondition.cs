
public class UnitProductionInProcessCondition: BasicCondition
{
    public bool firstCheck = true;

    public string _targetUnitProductionId;
    public bool process = false;


    public UnitProductionInProcessCondition(string targetUnitProductionId) 
    {
        _targetUnitProductionId = targetUnitProductionId;
    }

    public void FirstComplyCheck()
    {
        firstCheck = false;

        process = IsTargetUnitProductionInProcess();

        if (!process)
        {
            EnableListeners();
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

    public void SomeProcessOnBaseStarted(ProcessOnBaseCacheItem processData)
    {
        if (processData.GetProcessType() != UnitProductionItem.type) return;
        if (processData.GetSource().id != _targetUnitProductionId) return;

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
