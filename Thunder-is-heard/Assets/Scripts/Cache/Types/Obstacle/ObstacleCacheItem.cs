using Newtonsoft.Json;
using System.Collections.Generic;


[System.Serializable]
public class ObstacleCacheItem : CacheItem
{
    public ObstacleCacheItem(Dictionary<string, object> objFields) : base(objFields)
    {
        if (!objFields.ContainsKey("size"))
        {
            SetSize(new Bector2Int(1, 1));
        }

        if (!objFields.ContainsKey("demolitionCost"))
        {
            SetDemolitionCost(new ResourcesData());
        }

        if (!objFields.ContainsKey("iconSection"))
        {
            SetIconSection("");
        }

        if (!objFields.ContainsKey("iconName"))
        {
            SetIconName("");
        }

        if (!objFields.ContainsKey("modelPath"))
        {
            SetModelPath("");
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

    public Bector2Int GetSize()
    {
        object value = GetField("size");
        if (value == null)
        {
            return null;
        }

        if (value is Bector2Int typedValue)
        {
            return typedValue;
        }

        return JsonConvert.DeserializeObject<Bector2Int>(value.ToString());
    }

    public void SetSize(Bector2Int value)
    {
        SetField("size", value);
    }

    public ResourcesData GetDemolitionCost()
    {
        object value = GetField("demolitionCost");
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

    public void SetDemolitionCost(ResourcesData value)
    {
        SetField("demolitionCost", value);
    }

    public override CacheItem Clone()
    {
        ObstacleCacheItem clone = new ObstacleCacheItem(fields);
        clone.SetDemolitionCost(clone.GetDemolitionCost().Clone());
        return clone;
    }
}
