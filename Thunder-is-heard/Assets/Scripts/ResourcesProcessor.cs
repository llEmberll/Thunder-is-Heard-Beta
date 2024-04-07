using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesProcessor : MonoBehaviour
{
    public ResourcesData resources;
    public ResourcesPanel resourcesUI;

    public void Awake()
    {
        resources = LoadResourcesFromResourcesData();
        resources.Add(LoadResourcesFromObjectsOnBase());

        resourcesUI = GameObject.FindGameObjectWithTag("ResourcesPanel").GetComponent<ResourcesPanel>();
        UpdateUI();
    }

    public void UpdateUI()
    {
        resourcesUI.UpdateAll(resources);
    }

    public ResourcesData LoadResourcesFromObjectsOnBase()
    {
        ResourcesData result = new ResourcesData();
        result.Add(LoadResourcesFromBuilds());
        result.Add(LoadResourcesFromUnits());

        return result;
    }

    public ResourcesData LoadResourcesFromBuilds()
    {
        ResourcesData resourcesFromBuilds = new ResourcesData();

        PlayerBuildCacheTable tableOfBuildsOnBase = Cache.LoadByType<PlayerBuildCacheTable>();
        BuildCacheTable tableOfBuilds = Cache.LoadByType<BuildCacheTable>();

        foreach (var keyValuePair in tableOfBuildsOnBase.Items)
        {
            CacheItem currentItem = tableOfBuilds.GetById(keyValuePair.Value.GetCoreId());
            if (currentItem != null)
            {
                BuildCacheItem buildCoreData = new BuildCacheItem(currentItem.Fields);
                ResourcesData gives = buildCoreData.GetGives();
                resourcesFromBuilds.Add(gives);
            }
        }

        return resourcesFromBuilds;
    }

    public ResourcesData LoadResourcesFromUnits()
    {
        ResourcesData resourcesFromUnits = new ResourcesData();

        PlayerUnitCacheTable tableOfUnitsOnBase = Cache.LoadByType<PlayerUnitCacheTable>();
        UnitCacheTable tableOfUnits = Cache.LoadByType<UnitCacheTable>();

        foreach (var keyValuePair in tableOfUnitsOnBase.Items)
        {
            CacheItem currentItem = tableOfUnits.GetById(keyValuePair.Value.GetCoreId());
            if (currentItem != null)
            {
                UnitCacheItem unitCoreData = new UnitCacheItem(currentItem.Fields);
                ResourcesData gives = unitCoreData.GetGives();
                resourcesFromUnits.Add(gives);
            }
        }

        return resourcesFromUnits;
    }

    public int LoadStaffFromBaseData()
    {
        return 0;
    }

    public ResourcesData LoadResourcesFromResourcesData()
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
