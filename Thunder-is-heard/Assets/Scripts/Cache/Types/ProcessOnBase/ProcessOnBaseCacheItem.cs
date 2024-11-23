using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ProcessOnBaseCacheItem : CacheItem
{
    public ProcessOnBaseCacheItem(Dictionary<string, object> objFields) : base(objFields)
    {
        if (!objFields.ContainsKey("startTime"))
        {
            SetStartTime(DateTime.Now);
        }

        if (!objFields.ContainsKey("endTime"))
        {
            SetEndTime(DateTime.Now);
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

    public DateTime GetStartTime()
    {
        string dateTimeAsString = GetField("startTime").ToString();
        return DateTime.Parse(dateTimeAsString);
    }

    public void SetStartTime(DateTime value)
    {
        string stringValue = value.ToString("o");

        SetField("startTime", stringValue);
    }

    public DateTime GetEndTime()
    {
        string dateTimeAsString = GetField("endTime").ToString();
        return DateTime.Parse(dateTimeAsString);
    }

    public void SetEndTime(DateTime value)
    {
        string stringValue = value.ToString("o");

        SetField("endTime", stringValue);
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
