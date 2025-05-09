using Newtonsoft.Json;
using System.Collections.Generic;


[System.Serializable]
public class ResourcesCacheItem : CacheItem
{
    public ResourcesCacheItem(Dictionary<string, object> objFields) : base(objFields)
    {
    }

    public ResourcesData GetResources()
    {
        object value = GetField("resources");
        if (value == null)
        {
            return new ResourcesData();
        }

        return JsonConvert.DeserializeObject<ResourcesData>(value.ToString());
    }

    public void SetResources(ResourcesData resources)
    {
        SetField("resources", resources);
    }

    public string GetBaseName()
    {
        return (string?)GetField("baseName");
    }

    public void SetBaseName(string value)
    {
        SetField("baseName", value);
    }

    public override CacheItem Clone()
    {
        ResourcesCacheItem clone = new ResourcesCacheItem(fields);
        clone.SetResources(GetResources().Clone());
        return clone;
    }
}
