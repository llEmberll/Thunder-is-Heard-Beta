using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessWorker : MonoBehaviour
{
    public float currentTime;

    public void Start()
    {
        currentTime = Time.realtimeSinceStartup;
    }

    public static void CreateProcess(
        string processName, 
        string processType, 
        string objectOnBaseId, 
        int startTime, 
        int EndTime, 
        ProcessSource source = null
        )
    {
        ProcessOnBaseCacheTable processesTable = Cache.LoadByType<ProcessOnBaseCacheTable>();

        ProcessOnBaseCacheItem process = new ProcessOnBaseCacheItem(new Dictionary<string, object>());
        process.SetName( processName );
        process.SetProcessType( processType );
        process.SetObjectOnBaseId( objectOnBaseId );
        process.SetStartTime( startTime );
        process.SetEndTime( EndTime );
        process.SetSource( source );

        processesTable.AddOne( process );
        Cache.Save(processesTable);

        EventMaster.current.OnProcessOnBaseStart( process );
    }

    public void ClearProcess(string processId)
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
            if (currentTime > process.GetEndTime())
            {
                EventMaster.current.OnProcessOnBaseFinish(process);
                ClearProcess(process.GetExternalId());
            }
        }
    }
}
