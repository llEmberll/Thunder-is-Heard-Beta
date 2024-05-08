using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessWorker : MonoBehaviour
{
    public float currentTime;

    public void Start()
    {
        currentTime = Time.realtimeSinceStartup;
        EventMaster.current.ProcessOnBaseHandled += ClearHandledProcess;
    }

    public static void CreateProcess(
        string processName, 
        string processType, 
        string objectOnBaseId, 
        float startTime, 
        float EndTime, 
        string processData = null
        )
    {
        ProcessOnBaseCacheTable processesTable = Cache.LoadByType<ProcessOnBaseCacheTable>();

        ProcessOnBaseCacheItem process = new ProcessOnBaseCacheItem(new Dictionary<string, object>());
        process.SetName( processName );
        process.SetProcessType( processType );
        process.SetObjectOnBaseId( objectOnBaseId );
        process.SetStartTime( startTime );
        process.SetEndTime( EndTime );
        process.SetProcessData( processData );

        processesTable.AddOne( process );
        Cache.Save(processesTable);

        EventMaster.current.OnProcessOnBaseStart( process );
    }

    public void ClearHandledProcess(string processId)
    {
        ProcessOnBaseCacheTable processTable = Cache.LoadByType<ProcessOnBaseCacheTable>();
        processTable.DeleteById(processId);
        Cache.Save(processTable);
    }

    protected void Update()
    {
        UpdateCurrentTime();
        FindAndHandleFinishedProcesses();
    }

    public void UpdateCurrentTime()
    {
        currentTime = Time.realtimeSinceStartup;
    }

    public void FindAndHandleFinishedProcesses()
    {
        ProcessOnBaseCacheTable processTable = Cache.LoadByType<ProcessOnBaseCacheTable>();
        foreach (CacheItem processCacheItem in processTable.Items.Values)
        {
            ProcessOnBaseCacheItem process = new ProcessOnBaseCacheItem(processCacheItem.Fields);
            if (process.GetEndTime()  > currentTime)
            {
                EventMaster.current.OnProcessOnBaseFinish(process);
            }
        }
    }
}
