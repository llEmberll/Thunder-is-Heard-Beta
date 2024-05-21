using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;


[System.Serializable]
public class ProcessOnBaseCacheItem : CacheItem
{
    public ProcessOnBaseCacheItem(Dictionary<string, object> objFields) : base(objFields)
    {
        if (!objFields.ContainsKey("startTime"))
        {
            SetStartTime((int)Time.realtimeSinceStartup);
        }

        if (!objFields.ContainsKey("endTime"))
        {
            SetEndTime((int)Time.realtimeSinceStartup);
        }

        if (!objFields.ContainsKey("objectOnBaseId"))
        {
            SetObjectOnBaseId(null);
        }

        if (!objFields.ContainsKey("processType"))
        {
            SetProcessType(null);
        }

        if (!objFields.ContainsKey("source"))
        {
            SetSource(null);
        }
    }

    public int GetStartTime()
    {
        object value = GetField("startTime");
        return Convert.ToInt32(value);
    }

    public void SetStartTime(int value)
    {
        SetField("startTime", value);
    }

    public int GetEndTime()
    {
        object value = GetField("endTime");
        return Convert.ToInt32(value);
    }

    public void SetEndTime(int value)
    {
        SetField("endTime", value);
    }

    public string GetObjectOnBaseId()
    {
        return (string)GetField("objectOnBaseId");
    }

    public void SetObjectOnBaseId(string value)
    {
        SetField("objectOnBaseId", value);
    }

    public string GetProcessType()
    {
        return (string)GetField("processType");
    }

    public void SetProcessType(string value)
    {
        SetField("processType", value);
    }

    public ProcessSource GetSource()
    {
        object value = GetField("source");
        if (value == null)
        {
            return null;
        }

        return JsonConvert.DeserializeObject<ProcessSource>(value.ToString());
    }

    public void SetSource(ProcessSource value)
    {
        SetField("source", value);
    }

    public override CacheItem Clone()
    {
        ProcessOnBaseCacheItem clone = new ProcessOnBaseCacheItem(fields);
        return clone;
    }
}
