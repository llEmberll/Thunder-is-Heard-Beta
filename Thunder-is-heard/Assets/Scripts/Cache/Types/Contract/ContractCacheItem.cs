using Newtonsoft.Json;
using System;
using System.Collections.Generic;


[System.Serializable]
public class ContractCacheItem : CacheItem
{
    public ContractCacheItem(Dictionary<string, object> objFields) : base(objFields)
    {
        if (!objFields.ContainsKey("type"))
        {
            SetType("Undefined");
        }

        if (!objFields.ContainsKey("cost"))
        {
            SetCost(new ResourcesData());
        }

        if (!objFields.ContainsKey("gives"))
        {
            SetGives(new ResourcesData());
        }

        if (!objFields.ContainsKey("duration"))
        {
            SetDuration(3);
        }

        if (!objFields.ContainsKey("iconSection"))
        {
            SetIconSection("UIBuildCards");
        }

        if (!objFields.ContainsKey("iconName"))
        {
            SetIconSection("");
        }
    }

    public string? GetType()
    {
        return (string?)GetField("type");
    }

    public void SetType(string value)
    {
        SetField("type", value);
    }

    public string? GetIconSection()
    {
        return (string?)GetField("iconSection");
    }

    public void SetIconSection(string value)
    {
        SetField("iconSection", value);
    }

    public string? GetIconName()
    {
        return (string?)GetField("iconName");
    }

    public void SetIconName(string value)
    {
        SetField("iconName", value);
    }

    public ResourcesData GetCost()
    {
        object value = GetField("cost");
        if (value == null)
        {
            return new ResourcesData();
        }

        if (value is ResourcesData typedValue)
        {
            return typedValue;
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

        if (value is ResourcesData typedValue)
        {
            return typedValue;
        }   

        return JsonConvert.DeserializeObject<ResourcesData>(value.ToString());
    }

    public void SetGives(ResourcesData value)
    {
        SetField("gives", value);
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
        clone.SetGives(clone.GetGives().Clone());
        return clone;
    }
}
