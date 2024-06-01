using System.Collections.Generic;
using UnityEngine;

public class BaseMap : Map
{
    public override void Awake()
    {
        base.Awake();
        InitTerrain();
    }

    public void Start()
    {
    }

    public void InitTerrain()
    {
        Transform terrainParent = transform.Find("Terrain");
        Transform terrainObj = Instantiate(Resources.Load<Terrain>(Config.terrainsPath["Base"]).transform, parent: terrainParent);

        terrainParent.transform.position -= new Vector3(5, 0, 5);

        terrain = terrainObj.GetComponent<Terrain>();
        TerrainData terrainData = terrain.terrainData;
        terrainData.size = new Vector3(5 * 2 + size.x, terrainData.size.y, 5 * 2 + size.y);
        terrain.terrainData = terrainData;
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

        ResourcesData headBuildCost = new ResourcesData();

        ResourcesData oilStationCost = new ResourcesData();

        ResourcesData mineCost = new ResourcesData();

        ResourcesData officeCost = new ResourcesData();

        ResourcesData tentCost = new ResourcesData();

        ResourcesData warehouseCost = new ResourcesData();

        ResourcesData trainingCenterCost = new ResourcesData();

        ResourcesData factoryCost = new ResourcesData();

        mineCost.rub = 750;
        mineCost.oil = 2;
        mineCost.steel = 3;

        oilStationCost.rub = 650;
        oilStationCost.oil = 1;
        oilStationCost.steel = 3;

        officeCost.rub = 1250;
        officeCost.oil = 3;
        officeCost.steel = 5;

        tentCost.rub = 600;
        tentCost.oil = 1;
        tentCost.steel = 2;

        warehouseCost.rub = 500;
        warehouseCost.steel = 3;

        trainingCenterCost.rub = 1600;
        trainingCenterCost.oil = 2;
        trainingCenterCost.steel = 6;

        factoryCost.rub = 3500;
        factoryCost.oil = 5;
        factoryCost.steel = 10;

        ResourcesData headBuildGives = new ResourcesData();
        headBuildGives.maxOil = 8;
        headBuildGives.maxSteel = 10;
        headBuildGives.maxStaff = 10;

        ResourcesData mineGives = new ResourcesData();
        mineGives.maxSteel = 2;

        ResourcesData oilStationGives = new ResourcesData();
        oilStationGives.maxOil = 2;

        ResourcesData officeGives = new ResourcesData();

        ResourcesData tentGives = new ResourcesData();
        tentGives.maxStaff = 5;

        ResourcesData warehouseGives = new ResourcesData();
        warehouseGives.maxOil = 5;
        warehouseGives.maxSteel = 8;

        ResourcesData trainingCenterGives = new ResourcesData();
        trainingCenterGives.maxStaff = 2;

        ResourcesData factoryGives = new ResourcesData();

        BuildCacheItem headbuild = new BuildCacheItem(new Dictionary<string, object>());

        headbuild.SetExternalId("9b2cf240-5f63-4107-8751-eb91b95b94d9");
        headbuild.SetName("Headbuild");
        headbuild.SetModelPath("Prefabs/Entity/Builds/Headbuild/Model");
        headbuild.SetSize(new Bector2Int(new Vector2Int(3, 2)));
        headbuild.SetCost(headBuildCost);
        headbuild.SetGives(headBuildGives);
        headbuild.SetCreateTime(0);
        headbuild.SetHealth(12);
        headbuild.SetIconSection(Config.resources["UICards"] + "Builds/" + "cards");
        headbuild.SetIconName("headbuild_card");

        BuildCacheItem mine = new BuildCacheItem(new Dictionary<string, object>());

        mine.SetExternalId("4b8a1805-3af8-4144-8bdb-62c93852b443");
        mine.SetName("Mine");
        mine.SetModelPath("Prefabs/Entity/Builds/Mine/Model");
        mine.SetSize(new Bector2Int(new Vector2Int(2, 2)));
        mine.SetCost(mineCost);
        mine.SetGives(mineGives);
        mine.SetCreateTime(0);
        mine.SetHealth(6);
        mine.SetDistance(0);
        mine.SetIconSection(Config.resources["UICards"] + "Builds/" + "cards");
        mine.SetIconName("mine_card");
        mine.SetInteractionComponentName("ContractComponent");
        mine.SetInteractionComponentType(InteractionComponentTypes.steelContract);

        BuildCacheItem oilStation = new BuildCacheItem(new Dictionary<string, object>());

        oilStation.SetExternalId("64a4568c-bfaf-408e-9537-8e489ccaca56");
        oilStation.SetName("OilStation");
        oilStation.SetModelPath("Prefabs/Entity/Builds/OilStation/Model");
        oilStation.SetSize(new Bector2Int(new Vector2Int(2, 2)));
        oilStation.SetCost(oilStationCost);
        oilStation.SetGives(oilStationGives);
        oilStation.SetCreateTime(0);
        oilStation.SetHealth(6);
        oilStation.SetDistance(0);
        oilStation.SetIconSection(Config.resources["UICards"] + "Builds/" + "cards");
        oilStation.SetIconName("oil_station_card");
        oilStation.SetInteractionComponentName("ContractComponent");
        oilStation.SetInteractionComponentType(InteractionComponentTypes.oilContract);

        BuildCacheItem office = new BuildCacheItem(new Dictionary<string, object>());

        office.SetExternalId("8878498b-a4bc-4dc8-8f39-bc9e987a689f");
        office.SetName("Office");
        office.SetModelPath("Prefabs/Entity/Builds/Office/Model");
        office.SetSize(new Bector2Int(new Vector2Int(3, 3)));
        office.SetCost(officeCost);
        office.SetGives(officeGives);
        office.SetCreateTime(0);
        office.SetHealth(8);
        office.SetDistance(0);
        office.SetIconSection(Config.resources["UICards"] + "Builds/" + "cards");
        office.SetIconName("office_card");
        office.SetInteractionComponentName("ContractComponent");
        office.SetInteractionComponentType(InteractionComponentTypes.rubContract);

        BuildCacheItem tent = new BuildCacheItem(new Dictionary<string, object>());

        tent.SetExternalId("ba290dde-968d-46ab-868b-b0f7598a7787");
        tent.SetName("Tent");
        tent.SetModelPath("Prefabs/Entity/Builds/Tent/Model");
        tent.SetSize(new Bector2Int(new Vector2Int(2, 3)));
        tent.SetCost(tentCost);
        tent.SetGives(tentGives);
        tent.SetCreateTime(0);
        tent.SetHealth(5);
        tent.SetDistance(0);
        tent.SetIconSection(Config.resources["UICards"] + "Builds/" + "cards");
        tent.SetIconName("tent_card");

        BuildCacheItem warehouse = new BuildCacheItem(new Dictionary<string, object>());

        warehouse.SetExternalId("3d9f0f22-409e-40d7-8511-f4584b583dc0");
        warehouse.SetName("Warehouse");
        warehouse.SetModelPath("Prefabs/Entity/Builds/Warehouse/Model");
        warehouse.SetSize(new Bector2Int(new Vector2Int(2, 3)));
        warehouse.SetCost(warehouseCost);
        warehouse.SetGives(warehouseGives);
        warehouse.SetCreateTime(0);
        warehouse.SetHealth(7);
        warehouse.SetDistance(0);
        warehouse.SetIconSection(Config.resources["UICards"] + "Builds/" + "cards");
        warehouse.SetIconName("warehouse_card");

        BuildCacheItem trainingCenter = new BuildCacheItem(new Dictionary<string, object>());

        trainingCenter.SetExternalId("065a4f61-5b0e-450e-b89b-299651f90b4d");
        trainingCenter.SetName("TrainingCenter");
        trainingCenter.SetModelPath("Prefabs/Entity/Builds/TrainingCenter/Model");
        trainingCenter.SetSize(new Bector2Int(new Vector2Int(3, 3)));
        trainingCenter.SetCost(trainingCenterCost);
        trainingCenter.SetGives(trainingCenterGives);
        trainingCenter.SetCreateTime(0);
        trainingCenter.SetHealth(8);
        trainingCenter.SetDistance(0);
        trainingCenter.SetIconSection(Config.resources["UICards"] + "Builds/" + "cards");
        trainingCenter.SetIconName("training_center_card");

        BuildCacheItem factory = new BuildCacheItem(new Dictionary<string, object>());

        factory.SetExternalId("065a4f61-5b0e-450e-b89b-299651f90b4d");
        factory.SetName("Factory");
        factory.SetModelPath("Prefabs/Entity/Builds/factory/Model");
        factory.SetSize(new Bector2Int(new Vector2Int(2, 2)));
        factory.SetCost(factoryCost);
        factory.SetGives(factoryGives);
        factory.SetCreateTime(0);
        factory.SetHealth(9);
        factory.SetDistance(0);
        factory.SetIconSection(Config.resources["UICards"] + "Builds/" + "cards");
        factory.SetIconName("factory_card");

        Debug.Log("build created and prepared");

        CacheItem[] itemsForAdd = new CacheItem[8] { headbuild, mine , oilStation , office, tent, warehouse , trainingCenter, factory};
        buildTable.Add(itemsForAdd);

        Debug.Log("build added to table");

        Cache.Save(buildTable);

    }

    public void CreateUnits()
    {
        UnitCacheTable unitsTable = Cache.LoadByType<UnitCacheTable>();
        UnitCacheItem assaulters = new UnitCacheItem(new Dictionary<string, object>());
        assaulters.SetName("Assaulters");
        assaulters.SetModelPath("Prefabs/Entity/Units/Assaulters/Model");
        assaulters.SetHealth(3);
        assaulters.SetDamage(1);
        assaulters.SetDistance(2);
        assaulters.SetMobility(2);

        unitsTable.Add(new CacheItem[1] { assaulters });
        Cache.Save(unitsTable);
    }

    public void CreateMaterial()
    {
        MaterialCacheTable materialsTable = Cache.LoadByType<MaterialCacheTable>();
        MaterialCacheItem megaphone = new MaterialCacheItem(new Dictionary<string, object>());
        megaphone.SetName("Megaphone");

        materialsTable.Add(new CacheItem[1] { megaphone });
        Cache.Save(materialsTable);
    }

    public void CreateInventory()
    {
        InventoryCacheTable inventoryTable = Cache.LoadByType<InventoryCacheTable>();

        InventoryCacheItem headbuild = new InventoryCacheItem(new Dictionary<string, object>()
        {
            { "coreId", "9b2cf240-5f63-4107-8751-eb91b95b94d9" },
            { "type", "Build" },
            { "count", 2 }
        }
        );

        InventoryCacheItem mine = new InventoryCacheItem(new Dictionary<string, object>()
        {
            { "coreId", "4b8a1805-3af8-4144-8bdb-62c93852b443" },
            { "type", "Build" },
            { "count", 2 }
        }
        );

        CacheItem[] itemsForAdd = new CacheItem[2] { mine, headbuild };
        inventoryTable.Add(itemsForAdd);

        Cache.Save(inventoryTable);
    }

    public void CreateInventoryUnit()
    {
        InventoryCacheTable inventoryTable = Cache.LoadByType<InventoryCacheTable>();
        InventoryCacheItem assaulters = new InventoryCacheItem(new Dictionary<string, object>()
        {
            { "coreId", "bd1b7986-cf1a-4d76-8b14-c68bf10f363f" },
            { "type", "Unit" },
            { "count", 2 }
        }
        );

        inventoryTable.Add(new CacheItem[1] { assaulters });
        Cache.Save(inventoryTable);
    }

    public void CreateInventoryMaterial()
    {
        InventoryCacheTable inventoryTable = Cache.LoadByType<InventoryCacheTable>();
        InventoryCacheItem megaphone = new InventoryCacheItem(new Dictionary<string, object>()
        {
            { "coreId", "31391161-4be3-4149-833d-7fdde496946c" },
            { "type", "Material" },
            { "count", 1 }
        }
        );

        inventoryTable.Add(new CacheItem[1] { megaphone });
        Cache.Save(inventoryTable);
    }

    public void CreateShopMaterial()
    {
        ShopCacheTable shopTable = Cache.LoadByType<ShopCacheTable>();

        ShopCacheItem megaphone = new ShopCacheItem(new Dictionary<string, object>()
        {
            { "coreId", "31391161-4be3-4149-833d-7fdde496946c" },
            { "type", "Material" },
            { "count", 2 }
        }
        );


        CacheItem[] itemsForAdd = new CacheItem[1] { megaphone };
        shopTable.Add(itemsForAdd);

        Cache.Save(shopTable);
    }


    public void CreateShop()
    {
        ShopCacheTable shopTable = Cache.LoadByType<ShopCacheTable>();


        ShopCacheItem mine = new ShopCacheItem(new Dictionary<string, object>()
        {
            { "coreId", "4b8a1805-3af8-4144-8bdb-62c93852b443" },
            { "type", "Build" },
            { "count", 2 }
        }
        );

        ShopCacheItem assaulters = new ShopCacheItem(new Dictionary<string, object>()
        {
            { "coreId", "bd1b7986-cf1a-4d76-8b14-c68bf10f363f" },
            { "type", "Unit" },
            { "count", 2 }
        }
        );


        CacheItem[] itemsForAdd = new CacheItem[2] { mine, assaulters };
        shopTable.Add(itemsForAdd);

        Cache.Save(shopTable);
    }

    public void CreateContracts()
    {
        ContractCacheTable contractsTable = Cache.LoadByType<ContractCacheTable>();

        ContractCacheItem contractOnSteel_1 = new ContractCacheItem(new Dictionary<string, object>());
        ResourcesData costForContract_1 = new ResourcesData(rubCount: 500);
        contractOnSteel_1.SetCost(costForContract_1);
        ResourcesData givesForContract_1 = new ResourcesData(steelCount: 2);
        contractOnSteel_1.SetGives(givesForContract_1);
        contractOnSteel_1.SetType("Steel");
        contractOnSteel_1.SetDuration(10);
        contractOnSteel_1.SetName("Steel express");
        contractOnSteel_1.SetIconSection("UIBuildCards");

        ContractCacheItem contractOnSteel_2 = new ContractCacheItem(new Dictionary<string, object>());
        ResourcesData costForContract_2 = new ResourcesData(rubCount: 1200);
        contractOnSteel_2.SetCost(costForContract_2);
        ResourcesData givesForContract_2 = new ResourcesData(steelCount: 6);
        contractOnSteel_2.SetGives(givesForContract_2);
        contractOnSteel_2.SetType("Steel");
        contractOnSteel_2.SetDuration(25);
        contractOnSteel_2.SetName("Half a shift");
        contractOnSteel_2.SetIconSection("UIBuildCards");

        ContractCacheItem contractOnSteel_3 = new ContractCacheItem(new Dictionary<string, object>());
        ResourcesData costForContract_3 = new ResourcesData(rubCount: 2150);
        contractOnSteel_3.SetCost(costForContract_3);
        ResourcesData givesForContract_3 = new ResourcesData(steelCount: 15);
        contractOnSteel_3.SetGives(givesForContract_3);
        contractOnSteel_3.SetType("Steel");
        contractOnSteel_3.SetDuration(60);
        contractOnSteel_3.SetName("Full shift");
        contractOnSteel_3.SetIconSection("UIBuildCards");

        contractsTable.Add(new CacheItem[] { contractOnSteel_1, contractOnSteel_2, contractOnSteel_3 });
        Cache.Save(contractsTable);
    }
}
