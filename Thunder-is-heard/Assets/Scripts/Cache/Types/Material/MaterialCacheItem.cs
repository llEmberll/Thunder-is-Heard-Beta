using Newtonsoft.Json;
using System;
using System.Collections.Generic;


[System.Serializable]
public class MaterialCacheItem : CacheItem
{
    public MaterialCacheItem(Dictionary<string, object> objFields) : base(objFields)
    {
        if (!objFields.ContainsKey("cost"))
        {
            SetCost(new ResourcesData());
        }

        if (!objFields.ContainsKey("createTime"))
        {
            SetCreateTime(3);
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

    public int GetCreateTime()
    {
        object value = GetField("createTime");
        return (value != null) ? Convert.ToInt32(value) : 0;
    }

    public void SetCreateTime(int value)
    {
        SetField("createTime", value);
    }

    public override CacheItem Clone()
    {
        UnitCacheItem clone = new UnitCacheItem(fields);
        clone.SetCost(clone.GetCost().Clone());
        return clone;
    }
}