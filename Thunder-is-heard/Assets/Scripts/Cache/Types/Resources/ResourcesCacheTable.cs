
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class ResourcesCacheTable: CacheTable
{
    public string name = "Resources";

    public override string Name { get { return name; } }

    public ResourcesData GetResources()
    {
        CacheItem data = null;
        if (Items.Count > 0)
        {
            data = Items.First().Value;
        }

        ResourcesCacheItem resources = new ResourcesCacheItem(new Dictionary<string, object>());
        if (data == null)
        {
            resources = new ResourcesCacheItem(new Dictionary<string, object>());
        }
        else
        {
            resources = new ResourcesCacheItem(data.Fields);
        }

        return resources.GetResources();
    }

    public void SetResources(ResourcesData resources)
    {
        CacheItem data = Items.First().Value;
        ResourcesCacheItem newResourcesData = new ResourcesCacheItem(data.Fields);
        newResourcesData.SetResources(resources);

        Items.Clear();
        AddOne(newResourcesData);
    }


    public string GetBaseName()
    {
        CacheItem data = null;
        if (Items.Count > 0)
        {
            data = Items.First().Value;
        }

        ResourcesCacheItem resources = new ResourcesCacheItem(new Dictionary<string, object>());
        if (data == null)
        {
            resources = new ResourcesCacheItem(new Dictionary<string, object>());
        }
        else
        {
            resources = new ResourcesCacheItem(data.Fields);
        }

        return resources.GetBaseName();
    }

    public void SetBaseName(string value)
    {
        CacheItem data = Items.First().Value;
        ResourcesCacheItem newResourcesData = new ResourcesCacheItem(data.Fields);
        newResourcesData.SetBaseName(value);

        Items.Clear();
        AddOne(newResourcesData);
    }
}
