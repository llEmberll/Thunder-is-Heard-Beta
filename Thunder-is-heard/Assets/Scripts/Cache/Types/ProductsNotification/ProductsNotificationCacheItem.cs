using Newtonsoft.Json;
using System;
using System.Collections.Generic;


[System.Serializable]
public class ProductsNotificationCacheItem : CacheItem
{
    public ProductsNotificationCacheItem(Dictionary<string, object> objFields) : base(objFields)
    {
        if (!objFields.ContainsKey("sourceObjectId"))
        {
            SetSourceObjectId(null);
        }

        if (!objFields.ContainsKey("type"))
        {
            SetType("Undefined");
        }

        if (!objFields.ContainsKey("gives") || objFields["gives"] == null)
        {
            SetGives(new ResourcesData());
        }

        if (!objFields.ContainsKey("iconSection"))
        {
            SetIconSection("");
        }

        if (!objFields.ContainsKey("iconName"))
        {
            SetIconName("");
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

    public string? GetUnitId()
    {
        return (string?)GetField("unitId");
    }

    public void SetUnitId(string value)
    {
        SetField("unitId", value);
    }

    public string GetSourceObjectId()
    {
        return (string)GetField("sourceObjectId");
    }

    public void SetSourceObjectId(string value)
    {
        SetField("sourceObjectId", value);
    }

    public ResourcesData GetGives()
    {
        object value = GetField("gives");
        if (value == null)
        {
            return new ResourcesData();
        }

        try
        {
            return JsonConvert.DeserializeObject<ResourcesData>(value.ToString());
        }
        catch
        {
            return (ResourcesData)value;
        }
        
    }

    public void SetGives(ResourcesData value)
    {
        SetField("gives", value);
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

    public override CacheItem Clone()
    {
        ProductsNotificationCacheItem clone = new ProductsNotificationCacheItem(fields);
        clone.SetGives(clone.GetGives().Clone());
        return clone;
    }
}
