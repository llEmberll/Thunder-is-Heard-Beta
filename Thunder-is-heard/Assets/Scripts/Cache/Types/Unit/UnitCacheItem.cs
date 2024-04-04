using Newtonsoft.Json;
using System;
using System.Collections.Generic;


[System.Serializable]
public class UnitCacheItem : CacheItem
{
    public UnitCacheItem(Dictionary<string, object> objFields) : base(objFields)
    {
        if (!objFields.ContainsKey("cost"))
        {
            SetCost(new ResourcesData());
        }

        if (!objFields.ContainsKey("gives"))
        {
            SetGives(new ResourcesData());
        }

        if (!objFields.ContainsKey("createTime"))
        {
            SetCreateTime(3);
        }


        if (!objFields.ContainsKey("health"))
        {
            SetHealth(1);
        }

        if (!objFields.ContainsKey("damage"))
        {
            SetDamage(1);
        }

        if (!objFields.ContainsKey("distance"))
        {
            SetDistance(1);
        }

        if (!objFields.ContainsKey("mobility"))
        {
            SetMobility(1);
        }

        if (!objFields.ContainsKey("size"))
        {
            SetSize(new Bector2Int(new UnityEngine.Vector2Int(1, 1)));
        }
    }

    public string? GetModelPath()
    {
        return (string?)GetField("modelPath");
    }

    public void SetModelPath(string value)
    {
        SetField("modelPath", value);
    }

    public string? GetIconPath()
    {
        return (string?)GetField("iconPath");
    }

    public void SetIconPath(string value)
    {
        SetField("iconPath", value);
    }


    public Bector2Int GetSize()
    {
        object value = GetField("size");
        if (value == null)
        {
            return null;
        }

        return JsonConvert.DeserializeObject<Bector2Int>(value.ToString());
    }

    public void SetSize(Bector2Int value)
    {
        SetField("size", value);
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

    public ResourcesData GetGives()
    {
        object value = GetField("gives");
        if (value == null)
        {
            return new ResourcesData();
        }

        return JsonConvert.DeserializeObject<ResourcesData>(value.ToString());
    }

    public void SetGives(ResourcesData value)
    {
        SetField("gives", value);
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

    public int GetHealth()
    {
        object value = GetField("health");
        return (value != null) ? Convert.ToInt32(value) : 1;
    }

    public void SetHealth(int value)
    {
        SetField("health", value);
    }

    public int GetDamage()
    {
        object value = GetField("damage");
        return (value != null) ? Convert.ToInt32(value) : 1;
    }

    public void SetDamage(int value)
    {
        SetField("damage", value);
    }

    public int GetDistance()
    {
        object value = GetField("distance");
        return (value != null) ? Convert.ToInt32(value) : 1;
    }

    public void SetDistance(int value)
    {
        SetField("distance", value);
    }

    public int GetMobility()
    {
        object value = GetField("mobility");
        return (value != null) ? Convert.ToInt32(value) : 1;
    }

    public void SetMobility(int value)
    {
        SetField("mobility", value);
    }

    public override CacheItem Clone()
    {
        UnitCacheItem clone = new UnitCacheItem(fields);
        clone.SetCost(clone.GetCost().Clone());
        clone.SetGives(clone.GetGives().Clone());
        return clone;
    }
}
