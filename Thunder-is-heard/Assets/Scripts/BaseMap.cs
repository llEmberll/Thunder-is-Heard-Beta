using System.Collections.Generic;
using UnityEngine;
using System.Text.Json;
using Newtonsoft.Json;

public class BaseMap : Map
{
    public override void Awake()
    {
        base.Awake();
        
    }

    public void Start()
    {
    }

    public void CreateResources()
    {
        ResourcesCacheTable resourceTable = Cache.LoadByType<ResourcesCacheTable>();

        CacheItem data = resourceTable.GetById("5u5df540-5f63-3107-8781-eb91b95b84i1");

        ResourcesCacheItem resources = new ResourcesCacheItem(new Dictionary<string, object>());
        if (data == null)
        {
            resources = new ResourcesCacheItem(new Dictionary<string, object>());
            resources.SetExternalId("5u5df540-5f63-3107-8781-eb91b95b84i1");
            resources.SetResources(new ResourcesData());
        }
        else
        {
            resources = new ResourcesCacheItem(data.Fields);
            
        }

        resources.SetResources(new ResourcesData());
        resourceTable.Add(new CacheItem[1] { resources });
        Cache.Save(resourceTable);

        ResourcesCacheTable newResourcesTable = Cache.LoadByType<ResourcesCacheTable>();
        CacheItem newData = newResourcesTable.GetById("5u5df540-5f63-3107-8781-eb91b95b84i1");
        ResourcesCacheItem newResources = new ResourcesCacheItem(newData.Fields);
        ResourcesData resourcesData = newResources.GetResources();

    }

    public void CreateBuilds()
    {
        BuildCacheTable buildTable = Cache.LoadByType<BuildCacheTable>();
        PlayerBuildCacheTable playerBuildTable = Cache.LoadByType<PlayerBuildCacheTable>();

        ResourcesData cost = new ResourcesData();
        ResourcesData gives = new ResourcesData();

        PlayerBuildCacheItem headbuildOnBase = new PlayerBuildCacheItem(new Dictionary<string, object>());

        headbuildOnBase.SetExternalId("9b2cf240-5f63-4107-8751-eb91b95b94d9");
        headbuildOnBase.SetName("Headbuild");
        headbuildOnBase.SetRotation(0);
        headbuildOnBase.SetPosition(new Bector2Int[4]
        {
            new Bector2Int(new Vector2Int(0, 1)),
            new Bector2Int(new Vector2Int(1, 0)),
            new Bector2Int(new Vector2Int(1, 1)),
            new Bector2Int(new Vector2Int(2, 0))
        });



        BuildCacheItem headbuild = new BuildCacheItem(new Dictionary<string, object>());

        headbuild.SetExternalId("9b2cf240-5f63-4107-8751-eb91b95b94d9");
        headbuild.SetName("Headbuild");
        headbuild.SetModelPath("Prefabs/Entity/Builds/Headbuild/Model");
        headbuild.SetSize(new Bector2Int(new Vector2Int(3, 2)));
        headbuild.SetCost(cost);
        headbuild.SetGives(gives);
        headbuild.SetCreateTime(0);
        headbuild.SetHealth(0);

        BuildCacheItem mine = new BuildCacheItem(new Dictionary<string, object>());

        mine.SetExternalId("4b8a1805-3af8-4144-8bdb-62c93852b443");
        mine.SetName("Mine");
        mine.SetModelPath("Prefabs/Entity/Builds/Mine/Model");
        mine.SetSize(new Bector2Int(new Vector2Int(2, 2)));
        mine.SetCost(cost);
        mine.SetGives(gives);
        mine.SetCreateTime(0);
        mine.SetHealth(0);
        mine.SetDistance(0);

        Debug.Log("build created and prepared");

        CacheItem[] itemsForAdd = new CacheItem[2] { headbuild, mine };
        CacheItem[] baseItemsForAdd = new CacheItem[1] { headbuildOnBase };
        buildTable.Add(itemsForAdd);
        playerBuildTable.Add(baseItemsForAdd);

        Debug.Log("build added to table");

        Debug.Log("Now count: " +  buildTable.Items.Count);

        Cache.Save(buildTable);
        Cache.Save(playerBuildTable);


        Debug.Log("table saved in cache");


        BuildCacheTable newBuildTable = Cache.LoadByType<BuildCacheTable>();
        PlayerBuildCacheTable newPlayerBuildTable = Cache.LoadByType<PlayerBuildCacheTable>();

        CacheItem savedHeadbuildItem = newBuildTable.GetById("9b2cf240-5f63-4107-8751-eb91b95b94d9");
        BuildCacheItem savedHeadbuild =  new BuildCacheItem(savedHeadbuildItem.Fields);

        CacheItem savedHeadbuildItemOnBase = newPlayerBuildTable.GetById("9b2cf240-5f63-4107-8751-eb91b95b94d9");
        PlayerBuildCacheItem savedHeadbuildOnBase = new PlayerBuildCacheItem(savedHeadbuildItemOnBase.Fields);

        Bector2Int dict = savedHeadbuild.GetSize();
        ResourcesData savedCost = savedHeadbuild.GetCost();

        int headbuildOnBaseRotation = savedHeadbuildOnBase.GetRotation();
        Bector2Int[] position = savedHeadbuildOnBase.GetPosition();
    }

    public void CreateInventory()
    {
        InventoryCacheTable inventoryTable = Cache.LoadByType<InventoryCacheTable>();

        InventoryCacheItem headbuild = new InventoryCacheItem(new Dictionary<string, object>()
        {
            { "coreId", "9b2cf240-5f63-4107-8751-eb91b95b94d9" },
            { "type", "Build" },
            { "count", 1 }
        }
        );

        InventoryCacheItem mine = new InventoryCacheItem(new Dictionary<string, object>()
        {
            { "coreId", "4b8a1805-3af8-4144-8bdb-62c93852b443" },
            { "type", "Build" },
            { "count", 1 }
        }
        );

        CacheItem[] itemsForAdd = new CacheItem[2] { headbuild, mine };
        inventoryTable.Add(itemsForAdd);

        Cache.Save(inventoryTable);
    }
}
