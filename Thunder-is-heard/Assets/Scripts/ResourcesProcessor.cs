using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Reflection;


public class ResourcesProcessor : MonoBehaviour
{
    public ResourcesData resources;
    public ResourcesPanel resourcesUI;

    public void Awake()
    {
        resources = LoadResourcesFromResourcesData();
        resources.Add(LoadResourcesFromObjectsOnBase());

        resourcesUI = GameObject.FindGameObjectWithTag("ResourcesPanel").GetComponent<ResourcesPanel>();
        UpdateResourcesUI();
    }

    public void Start()
    {
        UpdateBaseNameUI();
        EventMaster.current.BaseObjectsChanged += UpdateResourcesFromBaseObjects;
    }

    public void UpdateResourcesUI()
    {
        resourcesUI.UpdateAll(resources);
    }

    public static void ChangeBaseName(string value)
    {
        ResourcesCacheTable resourcesTable = Cache.LoadByType<ResourcesCacheTable>();
        resourcesTable.SetBaseName(value);
        Cache.Save(resourcesTable);
        EventMaster.current.OnChangeBaseName(value);
    }


    public void UpdateBaseNameUI()
    {
        ResourcesCacheTable resourcesTable = Cache.LoadByType<ResourcesCacheTable>();
        string baseName = resourcesTable.GetBaseName();
        EventMaster.current.OnChangeBaseName(baseName);
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

    public ResourcesData LoadResourcesFromResourcesData()
    {
        ResourcesCacheTable resourceTable = Cache.LoadByType<ResourcesCacheTable>();
        return resourceTable.GetResources();
    }

    public void Save()
    {
        ResourcesCacheTable resourceTable = Cache.LoadByType<ResourcesCacheTable>();
        resourceTable.SetResources(resources.GetResourcesWithoutLimits());
        Cache.Save(resourceTable);
        EventMaster.current.OnChangeResources(resources);
    }

    public bool IsAvailableToAddResources(ResourcesData resourcesForAdd)
    {
        ResourcesData newResources = resources.Clone();
        newResources.Add(resourcesForAdd);
        return !newResources.IsOverflow();
    }

    public void AddResources(ResourcesData resourcesForAdd)
    {
        resources.Add(resourcesForAdd);
    }

    public void SubstractResources(ResourcesData resourcesForSubstract)
    {
        resources.Substract(resourcesForSubstract);
        if (!resources.IsValid()) {
            resources.Add(resourcesForSubstract);
            throw new Exception("shortage of resources");
        }

        return;
    }

    public bool IsAvailableToBuy(ResourcesData cost)
    {
        return resources.IsCoveringCost(cost);
    }

    public void UpdateResourcesFromBaseObjects()
    {
        resources = resources.GetResourcesWithoutLimits();
        resources.Add(LoadResourcesFromObjectsOnBase());
        UpdateResourcesUI();

    }

    public static void UpdateResources(Transform resourcesParent, ResourcesData resourcesData)
    {
        if (resourcesParent == null || resourcesData == null) return;
        ClearResources(resourcesParent);

        Type type = resourcesData.GetType();
        FieldInfo[] resources = type.GetFields();

        foreach (FieldInfo resource in resources)
        {
            if (resource.FieldType == typeof(int))
            {
                int value = (int)resource.GetValue(resourcesData);
                if (value != 0)
                {
                    string resourceName = resource.Name;
                    CreateResourceElement(resourceName, value, resourcesParent);
                }
            }
        }
    }

    public static Sprite GetResourceSpriteByName(string name)
    {
        if (name == "exp")
        {
            return ResourcesUtils.LoadIcon(Config.resources["expIcon"], "0");
        }
        Sprite[] icons = Resources.LoadAll<Sprite>(Config.resources["resourcesIcons"]);

        name = name.ToLower();
        foreach (Sprite icon in icons)
        {
            if (name.Contains(icon.name))
            {
                return icon;
            }
        }

        return null;
    }

    public static void CreateResourceElement(string name, int count, Transform parent)
    {
        Sprite resourceIcon = GetResourceSpriteByName(name);

        string beginPrefix = "";
        string endPrefix = "";
        if (parent.tag == Tags.givesList)
        {
            if (count < 0)
            {
                beginPrefix = "- ";
            }
            else
            {
                beginPrefix = "+ ";
            }

            if (name.Contains("max"))
            {
                endPrefix = " max";
            }
        }

        string countText = beginPrefix + count.ToString() + endPrefix;

        GameObject prefab = Resources.Load<GameObject>(Config.resources["resourceForUIItem"]);
        GameObject resourceObject = Instantiate(prefab);
        resourceObject.transform.SetParent(parent, false);

        Image resourceImage = resourceObject.transform.Find("Icon").GetComponent<Image>();
        resourceImage.sprite = resourceIcon;

        GameObject resourceCount = resourceObject.transform.Find("Count").gameObject;
        TMP_Text resourceCountText = resourceCount.GetComponent<TMP_Text>();
        resourceCountText.text = countText;
    }

    public static void ClearResources(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }

    public static Dictionary<string, string> GetFirstNotEmptyResourceData(ResourcesData resourcesData)
    {
        Dictionary<string, string> result = new Dictionary<string, string>()
        {
            { "name", "" },
            { "count", 1.ToString() } 
        };

        Type type = resourcesData.GetType();
        FieldInfo[] resources = type.GetFields();

        foreach (FieldInfo resource in resources)
        {
            if (resource.FieldType == typeof(int))
            {
                int value = (int)resource.GetValue(resourcesData);
                if (value != 0)
                {
                    string resourceName = resource.Name;
                    result["name"] = resourceName.ToLower();
                    result["count"] = value.ToString();
                }
            }
        }

        return result;
    }
}
