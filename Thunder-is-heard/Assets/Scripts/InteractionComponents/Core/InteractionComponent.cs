using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractionComponent
{
    public string id;
    public string type;

    public virtual void Init(string objectOnBaseId, string componentType)
    {
        id = objectOnBaseId;
        type = componentType;
        EventMaster.current.ProcessOnBaseFinished += OnProcessOnBaseFinished;
        EventMaster.current.ProcessOnBaseStarted += OnProcessOnBaseStarted;
    }

    public void Interact(string workStatus)
    {
        if (workStatus == WorkStatuses.idle) Idle();
        if (workStatus == WorkStatuses.working) Working();
        if (workStatus == WorkStatuses.finished) Finished();
    }

    public abstract void Idle();
    public abstract void Working();
    public abstract void Finished();

    public bool IsProcessBelongsToComponent(string objectOnBaseId, string type)
    {
        return id == objectOnBaseId && ProcessTypes.component == type;
    }

    public void OnProcessOnBaseFinished(ProcessOnBaseCacheItem process)
    {
        if (!IsProcessBelongsToComponent(process.GetObjectOnBaseId(), process.GetProcessType())) return;

        HandleFinishedProcess(process);
        AfterProcessHandle(process.GetExternalId(), process.GetObjectOnBaseId());
    }

    public void OnProcessOnBaseStarted(ProcessOnBaseCacheItem process)
    {
        if (!IsProcessBelongsToComponent(process.GetObjectOnBaseId(), process.GetProcessType())) return;

        ObjectProcessor.DeleteProductsNotificationBySourceObjectId(process.GetObjectOnBaseId());
        EventMaster.current.OnChangeObjectOnBaseWorkStatus(id, WorkStatuses.working);
    }

    public abstract void HandleFinishedProcess(ProcessOnBaseCacheItem processCacheItem);

    public void AfterProcessHandle(string processId, string objectOnBaseId)
    {
        EventMaster.current.OnProcessOnBaseHandle(processId);
        EventMaster.current.OnChangeObjectOnBaseWorkStatus(objectOnBaseId, WorkStatuses.finished);
    }
}