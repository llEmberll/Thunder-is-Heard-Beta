
public class UnitProductionFinishedCondition: BasicCondition
{
    public string _targetUnitProductionId;
    public bool finished = false;


    public UnitProductionFinishedCondition(string targetUnitProductionId) 
    {
        _targetUnitProductionId = targetUnitProductionId;

        finished = IsTargetUnitProductionFinish();

        if (!finished)
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

    public void SomeProcessOnBaseFinished(ProcessOnBaseCacheItem processData)
    {
        if (processData.GetProcessType() != UnitProductionItem.type) return;
        if (processData.GetSource().id != _targetUnitProductionId) return;

        finished = true;
        DisableListeners();
    }


    public override bool IsComply()
    {
        return finished;
    }
}
