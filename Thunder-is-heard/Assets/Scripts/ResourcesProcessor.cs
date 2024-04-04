using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesProcessor : MonoBehaviour
{
    ResourcesData resources;

    public void Awake()
    {
        resources = LoadResources();
    }

    public ResourcesData LoadResources()
    {
        ResourcesCacheTable resourceTable = Cache.LoadByType<ResourcesCacheTable>();
        return resourceTable.GetResources();
    }

    public void Save()
    {
        ResourcesCacheTable resourceTable = Cache.LoadByType<ResourcesCacheTable>();
        resourceTable.SetResources(resources);
        Cache.Save(resourceTable);
    }

    public void AddResources(ResourcesData resourcesForAdd)
    {
        resources.Add(resourcesForAdd);
    }

    public void SubstractResources(ResourcesData resourcesForSubstract)
    {
        resources.Substract(resourcesForSubstract);
        if (!resources.IsValid()) {
            throw new Exception("shortage of resources");
        }

        return;
    }
}
