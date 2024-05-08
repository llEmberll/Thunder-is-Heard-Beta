using Newtonsoft.Json;
using System;
using System.Collections.Generic;


[System.Serializable]
public class UnitProductionCacheItem : CacheItem
{
    public UnitProductionCacheItem(Dictionary<string, object> objFields) : base(objFields)
    {
        if (!objFields.ContainsKey("type"))
        {
            SetType("Undefined");
        }

        if (!objFields.ContainsKey("cost"))
        {
            SetCost(new ResourcesData());
        }

        if (!objFields.ContainsKey("unitId"))
        {
            SetUnitId("");
        }

        if (!objFields.ContainsKey("duration"))
        {
            SetDuration(3);
        }

        if (!objFields.ContainsKey("iconPath"))
        {
            SetIconPath("");
        }
    }

    public string? GetIconPath()
    {
        return (string?)GetField("iconPath");
    }

    public void SetIconPath(string value)
    {
        SetField("iconPath", value);
    }

    public string? GetType()
    {
        return (string?)GetField("type");
    }

    public void SetType(string value)
    {
        SetField("type", value);
    }

    public ResourcesData GetCost()
    {
        object value = GetField("cost");
        if (value == null)
        {
            return new ResourcesData();
        }

        return JsonConvert.DeserializeObject<ResourcesData>(value.ToString());
    }

    public void SetCost(ResourcesData value)
    {
        SetField("cost", value);
    }

    public string GetUnitId()
    {
        return (string)GetField("unitId");
    }

    public void SetUnitId(string value)
    {
        SetField("unitId", value);
    }

    public int GetDuration()
    {
        object value = GetField("duration");
        return (value != null) ? Convert.ToInt32(value) : 3;
    }

    public void SetDuration(int value)
    {
        SetField("duration", value);
    }

    public override CacheItem Clone()
    {
        UnitCacheItem clone = new UnitCacheItem(fields);
        clone.SetCost(clone.GetCost().Clone());
        return clone;
    }
}
