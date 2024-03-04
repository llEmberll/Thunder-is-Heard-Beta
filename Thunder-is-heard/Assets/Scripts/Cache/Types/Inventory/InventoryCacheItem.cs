using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCacheItem : CacheItem
{
    public InventoryCacheItem(Dictionary<string, object> objFields) : base(objFields)
    {
        if (!objFields.ContainsKey("count"))
        {
            SetCount(1);
        }
    }

    public string? GetCoreId()
    {
        return (string?)GetField("coreId");
    }

    public void SetCoreId(string value)
    {
        SetField("coreId", value);
    }

    public string? GetType()
    {
        return (string?)GetField("type");
    }

    public void SetType(string value)
    {
        SetField("type", value);
    }

    public int GetCount()
    {
        object value = GetField("count");
        return (value != null) ? Convert.ToInt32(value) : 1;
    }

    public void SetCount(int value)
    {
        SetField("count", value);
    }
}
