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
            SetStartTime(Time.realtimeSinceStartup);
        }

        if (!objFields.ContainsKey("endTime"))
        {
            SetEndTime(Time.realtimeSinceStartup);
        }

        if (!objFields.ContainsKey("objectOnBaseId"))
        {
            SetObjectOnBaseId(null);
        }

        if (!objFields.ContainsKey("processType"))
        {
            SetProcessType(null);
        }

        if (!objFields.ContainsKey("processData"))
        {
            SetProcessData(null);
        }
    }

    public float GetStartTime()
    {
        object value = GetField("startTime");
        return Convert.ToInt32(value);
    }

    public void SetStartTime(float value)
    {
        SetField("startTime", value);
    }

    public float GetEndTime()
    {
        object value = GetField("endTime");
        return Convert.ToInt32(value);
    }

    public void SetEndTime(float value)
    {
        SetField("endTime", value);
    }

    public string GetObjectOnBaseId()
    {
        object value = GetField("objectOnBaseId");
        return value == null ? (string)value : null;
    }

    public void SetObjectOnBaseId(string value)
    {
        SetField("objectOnBaseId", value);
    }

    public string GetProcessType()
    {
        object value = GetField("processType");
        return value == null ? (string)value : null;
    }

    public void SetProcessType(string value)
    {
        SetField("processType", value);
    }

    public string GetProcessData()
    {
        object value = GetField("processData");
        return value == null ? (string)value : null;
    }

    public void SetProcessData(string value)
    {
        SetField("processData", value);
    }

    public override CacheItem Clone()
    {
        ProcessOnBaseCacheItem clone = new ProcessOnBaseCacheItem(fields);
        return clone;
    }
}
