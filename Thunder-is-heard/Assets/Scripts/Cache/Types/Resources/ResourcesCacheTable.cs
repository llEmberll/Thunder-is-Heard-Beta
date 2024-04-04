


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
        ResourcesCacheItem newResourcesData = new ResourcesCacheItem(new Dictionary<string, object>());
        newResourcesData.SetResources(resources);

        Items.Clear();
        AddOne(newResourcesData);
    }
}
