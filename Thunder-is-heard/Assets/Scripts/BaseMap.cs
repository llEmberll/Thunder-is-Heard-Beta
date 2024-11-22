using System.Collections.Generic;
using UnityEngine;

public class BaseMap : Map
{
    public override void Awake()
    {
        Init(new Vector2Int(15, 15), Config.terrainsPath["Base"]);
    }

    public void Start()
    {

    }

    public void CreateScenarioForTest()
    {
        ScenarioCacheTable table = Cache.LoadByType<ScenarioCacheTable>();

        ScenarioCacheItem scenarioItem = new ScenarioCacheItem(new Dictionary<string, object>());

        scenarioItem.SetExternalId("7d1660fb-10ad-4a55-9810-2c411120070f");
        scenarioItem.SetName("Scenario1 for tests");
        scenarioItem.SetTerrainPath(Config.terrainsPath["Base"]);
        scenarioItem.SetMapSize(new Bector2Int(new Vector2Int(8, 8)));

        LandingData landingData = new LandingData(
            new Bector2Int[]
            {
                new Bector2Int(new Vector2Int(0, 0)),
                new Bector2Int(new Vector2Int(0, 1)),
                new Bector2Int(new Vector2Int(1, 0)),
                new Bector2Int(new Vector2Int(1, 1))
            },
            6
            );

        scenarioItem.SetLanding(landingData);

        UnitOnBattle[] scenarioUnits = new UnitOnBattle[]
        {
            new UnitOnBattle(
                    coreUnitId: "bd1b7986-cf1a-4d76-8b14-c68bf10f363f",
                    new Bector2Int[] { new Bector2Int(2, 2) },
                    0,
                    3,
                    2,
                    1,
                    2,
                    2,
                    UnitTypes.infantry,
                    Doctrines.land,
                    Sides.empire,
                    unitIdOnBattle: "03a65484-e0c4-4bec-a8eb-277c22a53ef4"
                    )
        };

        scenarioItem.SetUnits(scenarioUnits);

        BuildOnBattle[] scenarioBuilds = new BuildOnBattle[]
        {
            new BuildOnBattle(
                    coreBuildId: "64a4568c-bfaf-408e-9537-8e489ccaca56",
                    new Bector2Int[]
                    {
                        new Bector2Int(3, 3),
                        new Bector2Int(3, 4),
                        new Bector2Int(4, 3),
                        new Bector2Int(4, 4)
                    },
                    180,
                    6,
                    6,
                    0,
                    0,
                    Doctrines.land,
                    Sides.empire,
                    WorkStatuses.idle,
                    buildIdOnBattle: "8f338821-eaf1-4670-8633-fe57ca21ecd0"
                )
        };

        scenarioItem.SetBuilds(scenarioBuilds);


        UnitOnBattle[] stage1Units = new UnitOnBattle[]
        {
            new UnitOnBattle(
                    coreUnitId: "035655f8-a347-4057-87cc-83385fa20660",
                    new Bector2Int[] { new Bector2Int(4, 5) },
                    90,
                    6,
                    6,
                    3,
                    3,
                    4,
                    UnitTypes.vehicle,
                    Doctrines.land,
                    Sides.empire,
                    unitIdOnBattle: "725f70fd-417e-4ca7-9b6f-ff5b19c8e4e6"
                    )
        };

        BuildOnBattle[] stage1Builds = new BuildOnBattle[]
        {
            new BuildOnBattle(
                    coreBuildId: "64a4568c-bfaf-408e-9537-8e489ccaca56",
                    new Bector2Int[]
                    {
                        new Bector2Int(5, 5),
                        new Bector2Int(5, 6),
                        new Bector2Int(6, 5),
                        new Bector2Int(6, 6)
                    },
                    180,
                    6,
                    6,
                    0,
                    0,
                    Doctrines.land,
                    Sides.empire,
                    WorkStatuses.idle,
                    buildIdOnBattle: "68881a80-41e1-41da-8784-bbe4b4bbe3dc"
                )
        };

        string[] stage1ConditionsForVictory = new string[]
        {
            "DestroyAllEnemy"
        };

        string[] stage1ConditionsForDefeat = new string[]
        {
            "DestroyAllAllies"
        };

        AISettings AISettingsForEmpireSide = new AISettings(
            "Waiting",
            Sides.empire,
            null,
            null
            );
        AISettings AISettingsForNeutralSide = new AISettings(
            "Waiting",
            Sides.neutral,
            null,
            null
            );

        StageData[] stages = new StageData[]
        {
            new StageData(
                stage1Units, 
                stage1Builds, 

                new AISettings[] { AISettingsForEmpireSide, AISettingsForNeutralSide },

                stage1ConditionsForDefeat, 
                stage1ConditionsForVictory
                )
        };

        scenarioItem.SetStages( stages );

        table.AddOne(scenarioItem);
        Cache.Save(table);
    }

    public void CreateMissionForTest()
    {
        MissionCacheTable table = Cache.LoadByType<MissionCacheTable>();

        MissionCacheItem missionItem = new MissionCacheItem(new Dictionary<string, object>());

        string missionId = "589dc04e-153d-46b7-bb57-db641aaae115";
        string name = "First step";
        string description = "Description";
        Bector2Int poseOnMap = new Bector2Int(new Vector2Int(-425, -237));
        ResourcesData gives = new ResourcesData(rubCount: 650, expCount: 100);

        missionItem.SetName(name);
        missionItem.SetDescription(description);
        missionItem.SetExternalId(missionId);
        missionItem.SetPoseOnMap(poseOnMap);
        missionItem.SetGives(gives);

        table.AddOne(missionItem);
        Cache.Save(table);
    }

    public void CreateBattleForTest()
    {
        BattleCacheTable table = Cache.LoadByType<BattleCacheTable>();

        BattleCacheItem battleItem = new BattleCacheItem(new Dictionary<string, object>());
        string missionId = "123";
        string turn = Sides.empire;

        CellData[] map = new CellData[]
        {
            new CellData("land", new Bector2Int(new Vector2Int(0, 0))),
            new CellData("water", new Bector2Int(new Vector2Int(0, 1))),
        };

        UnitOnBattle[] units = new UnitOnBattle[]
        {
            new UnitOnBattle(
                coreUnitId: "124", 
                unitPosition: new Bector2Int[] { new Bector2Int(new Vector2Int(0, 0)) }, 
                unitRotation: 0, 
                unitMaxHealth:3, 
                unitHealth: 3, 
                unitDamage: 1, 
                unitDistance: 2, 
                unitMobility: 2,
                UnitTypes.infantry,
                Doctrines.land,
                unitSide: Sides.empire, 
                unitIdOnBattle: "987"
                ),
            new UnitOnBattle(
                coreUnitId: "125",
                unitPosition: new Bector2Int[] { new Bector2Int(new Vector2Int(0, 1)) },
                unitRotation: 0,
                unitMaxHealth:3,
                unitHealth: 2,
                unitDamage: 1,
                unitDistance: 2,
                unitMobility: 2,
                UnitTypes.infantry,
                Doctrines.land,
                unitSide: Sides.federation, "986" , 
                unitSkillsData: new SkillOnBattle[] { new SkillOnBattle("224", 1, false) } 
                )
        };

        Bector2Int[] positionForBuild1 = new Bector2Int[] {new Bector2Int(new Vector2Int(0, 0))};
        Bector2Int[] positionForBuild2 = new Bector2Int[] { new Bector2Int(new Vector2Int(0, 1)), new Bector2Int(new Vector2Int(1, 1)) };

        BuildOnBattle[] builds = new BuildOnBattle[]
        {
            new BuildOnBattle(
                coreBuildId: "124", 
                buildPosition: positionForBuild1, 
                buildRotation: 0, 
                buildMaxHealth: 3,
                buildHealth: 2,
                buildDamage: 0, 
                buildDistance: 0,
                buildDoctrine: Doctrines.land,
                buildSide: Sides.empire, 
                buildWorkStatus: WorkStatuses.idle, 
                buildIdOnBattle: "876"
                ),
            new BuildOnBattle(
                coreBuildId: "125",
                buildPosition: positionForBuild2,
                buildRotation: 0,
                buildMaxHealth: 4,
                buildHealth: 4,
                buildDamage: 0,
                buildDistance: 0,
                buildDoctrine: Doctrines.land,
                buildSide: Sides.federation,
                buildWorkStatus: WorkStatuses.idle,
                buildIdOnBattle: "875"
                ),
        };

        battleItem.SetMissionId(missionId);
        battleItem.SetUnits(units);
        battleItem.SetBuilds(builds);
        battleItem.SetTurn(turn);

        table.AddOne(battleItem);
        Cache.Save(table);
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
        headBuildGives.maxStaff = 50;

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
        headbuild.SetModelPath("Prefabs/Entity/Builds/Headbuild");
        headbuild.SetSize(new Bector2Int(new Vector2Int(3, 3)));
        headbuild.SetCost(headBuildCost);
        headbuild.SetGives(headBuildGives);
        headbuild.SetCreateTime(0);
        headbuild.SetHealth(12);
        headbuild.SetIconSection(Config.resources["UICards"] + "Builds/" + "cards");
        headbuild.SetIconName("headbuild_card");

        BuildCacheItem mine = new BuildCacheItem(new Dictionary<string, object>());

        mine.SetExternalId("4b8a1805-3af8-4144-8bdb-62c93852b443");
        mine.SetName("Mine");
        mine.SetModelPath("Prefabs/Entity/Builds/Mine");
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
        oilStation.SetModelPath("Prefabs/Entity/Builds/OilStation");
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
        office.SetModelPath("Prefabs/Entity/Builds/Office");
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
        tent.SetModelPath("Prefabs/Entity/Builds/Tent");
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
        warehouse.SetModelPath("Prefabs/Entity/Builds/Warehouse");
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
        trainingCenter.SetModelPath("Prefabs/Entity/Builds/TrainingCenter");
        trainingCenter.SetSize(new Bector2Int(new Vector2Int(3, 3)));
        trainingCenter.SetCost(trainingCenterCost);
        trainingCenter.SetGives(trainingCenterGives);
        trainingCenter.SetCreateTime(0);
        trainingCenter.SetHealth(8);
        trainingCenter.SetDistance(0);
        trainingCenter.SetIconSection(Config.resources["UICards"] + "Builds/" + "cards");
        trainingCenter.SetIconName("training_center_card");
        trainingCenter.SetInteractionComponentName("UnitProductionComponent");
        trainingCenter.SetInteractionComponentType(InteractionComponentTypes.infantryUnitProduction);

        BuildCacheItem factory = new BuildCacheItem(new Dictionary<string, object>());

        factory.SetExternalId("7dfc9c80-ddac-45e9-bcdf-61166e068cad");
        factory.SetName("Factory");
        factory.SetModelPath("Prefabs/Entity/Builds/Factory");
        factory.SetSize(new Bector2Int(new Vector2Int(2, 2)));
        factory.SetCost(factoryCost);
        factory.SetGives(factoryGives);
        factory.SetCreateTime(0);
        factory.SetHealth(9);
        factory.SetDistance(0);
        factory.SetIconSection(Config.resources["UICards"] + "Builds/" + "cards");
        factory.SetIconName("factory_card");
        factory.SetInteractionComponentName("UnitProductionComponent");
        factory.SetInteractionComponentType(InteractionComponentTypes.vehicleUnitProduction);


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
        assaulters.SetExternalId("bd1b7986-cf1a-4d76-8b14-c68bf10f363f");
        assaulters.SetName("Assaulters");
        assaulters.SetModelPath("Prefabs/Entity/Units/Assaulters");
        assaulters.SetHealth(3);
        assaulters.SetDamage(1);
        assaulters.SetDistance(2);
        assaulters.SetMobility(2);

        UnitCacheItem osoka = new UnitCacheItem(new Dictionary<string, object>());
        osoka.SetExternalId("035655f8-a347-4057-87cc-83385fa20660");
        osoka.SetName("Osoka");
        osoka.SetModelPath("Prefabs/Entity/Units/Osoka");
        osoka.SetHealth(6);
        osoka.SetDamage(3);
        osoka.SetDistance(3);
        osoka.SetMobility(4);

        UnitCacheItem vortex = new UnitCacheItem(new Dictionary<string, object>());
        vortex.SetExternalId("a480c091-6b22-43f2-b26d-bbd3d2c2905b");
        vortex.SetName("Vortex");
        vortex.SetModelPath("Prefabs/Entity/Units/Vortex");
        vortex.SetHealth(5);
        vortex.SetDamage(2);
        vortex.SetDistance(2);
        vortex.SetMobility(6);

        unitsTable.Add(new CacheItem[3] { assaulters, osoka, vortex });
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


        ShopCacheItem oilStation = new ShopCacheItem(new Dictionary<string, object>()
        {
            { "coreId", "64a4568c-bfaf-408e-9537-8e489ccaca56" },
            { "type", "Build" },
            { "count", 2 }
        }
        );

        ShopCacheItem office = new ShopCacheItem(new Dictionary<string, object>()
        {
            { "coreId", "8878498b-a4bc-4dc8-8f39-bc9e987a689f" },
            { "type", "Build" },
            { "count", 2 }
        }
        );

        ShopCacheItem tent = new ShopCacheItem(new Dictionary<string, object>()
        {
            { "coreId", "ba290dde-968d-46ab-868b-b0f7598a7787" },
            { "type", "Build" },
            { "count", 2 }
        }
        );

        ShopCacheItem warehouse = new ShopCacheItem(new Dictionary<string, object>()
        {
            { "coreId", "3d9f0f22-409e-40d7-8511-f4584b583dc0" },
            { "type", "Build" },
            { "count", 2 }
        }
       );

        ShopCacheItem trainingCenter = new ShopCacheItem(new Dictionary<string, object>()
        {
            { "coreId", "065a4f61-5b0e-450e-b89b-299651f90b4d" },
            { "type", "Build" },
            { "count", 2 }
        }
       );

        ShopCacheItem factory = new ShopCacheItem(new Dictionary<string, object>()
        {
            { "coreId", "7dfc9c80-ddac-45e9-bcdf-61166e068cad" },
            { "type", "Build" },
            { "count", 2 }
        }
       );

        ShopCacheItem osoka = new ShopCacheItem(new Dictionary<string, object>()
        {
            { "coreId", "035655f8-a347-4057-87cc-83385fa20660" },
            { "type", "Unit" },
            { "count", 2 }
        }
        );

        ShopCacheItem vortex = new ShopCacheItem(new Dictionary<string, object>()
        {
            { "coreId", "a480c091-6b22-43f2-b26d-bbd3d2c2905b" },
            { "type", "Unit" },
            { "count", 2 }
        }
        );


        CacheItem[] itemsForAdd = new CacheItem[8] { office, oilStation, trainingCenter, tent, warehouse, factory, osoka, vortex };
        shopTable.Add(itemsForAdd);

        Cache.Save(shopTable);
    }

    public void CreateSteelContracts()
    {
        ContractCacheTable contractsTable = Cache.LoadByType<ContractCacheTable>();

        ContractCacheItem contractOnSteel_1 = new ContractCacheItem(new Dictionary<string, object>());
        contractOnSteel_1.SetExternalId("93970dfe-6f4a-4b5c-8d6d-115dad61423e");
        ResourcesData costForContract_1 = new ResourcesData(rubCount: 500);
        contractOnSteel_1.SetCost(costForContract_1);
        ResourcesData givesForContract_1 = new ResourcesData(steelCount: 2);
        contractOnSteel_1.SetGives(givesForContract_1);
        contractOnSteel_1.SetType(InteractionComponentTypes.steelContract);
        contractOnSteel_1.SetDuration(10);
        contractOnSteel_1.SetName("Steel express");
        contractOnSteel_1.SetIconSection("UIBuildCards");
        contractOnSteel_1.SetIconName("mine_card");

        ContractCacheItem contractOnSteel_2 = new ContractCacheItem(new Dictionary<string, object>());
        contractOnSteel_2.SetExternalId("4766fc0c-db3e-46f8-853a-06dfa7f62d29");
        ResourcesData costForContract_2 = new ResourcesData(rubCount: 1200);
        contractOnSteel_2.SetCost(costForContract_2);
        ResourcesData givesForContract_2 = new ResourcesData(steelCount: 6);
        contractOnSteel_2.SetGives(givesForContract_2);
        contractOnSteel_2.SetType(InteractionComponentTypes.steelContract);
        contractOnSteel_2.SetDuration(25);
        contractOnSteel_2.SetName("Half a shift");
        contractOnSteel_2.SetIconSection("UIBuildCards");
        contractOnSteel_2.SetIconName("mine_card");

        ContractCacheItem contractOnSteel_3 = new ContractCacheItem(new Dictionary<string, object>());
        contractOnSteel_3.SetExternalId("e5215c51-f22f-4655-9d32-6f49fc0510fc");
        ResourcesData costForContract_3 = new ResourcesData(rubCount: 2150);
        contractOnSteel_3.SetCost(costForContract_3);
        ResourcesData givesForContract_3 = new ResourcesData(steelCount: 15);
        contractOnSteel_3.SetGives(givesForContract_3);
        contractOnSteel_3.SetType(InteractionComponentTypes.steelContract);
        contractOnSteel_3.SetDuration(60);
        contractOnSteel_3.SetName("Full shift");
        contractOnSteel_3.SetIconSection("UIBuildCards");
        contractOnSteel_3.SetIconName("mine_card");

        contractsTable.Add(new CacheItem[3] { contractOnSteel_1, contractOnSteel_2, contractOnSteel_3 });
        Cache.Save(contractsTable);
    }

    public void CreateOilContracts()
    {
        ContractCacheTable contractsTable = Cache.LoadByType<ContractCacheTable>();

        ContractCacheItem contractOnOil_1 = new ContractCacheItem(new Dictionary<string, object>());
        contractOnOil_1.SetExternalId("7a837319-57a3-457f-9fab-bd6d8bf35021");
        ResourcesData costForContract_1 = new ResourcesData(rubCount: 600);
        contractOnOil_1.SetCost(costForContract_1);
        ResourcesData givesForContract_1 = new ResourcesData(oilCount: 2);
        contractOnOil_1.SetGives(givesForContract_1);
        contractOnOil_1.SetType(InteractionComponentTypes.oilContract);
        contractOnOil_1.SetDuration(10);
        contractOnOil_1.SetName("Oil express");
        contractOnOil_1.SetIconSection("UIBuildCards");
        contractOnOil_1.SetIconName("oil_station_card");


        ContractCacheItem contractOnOil_2 = new ContractCacheItem(new Dictionary<string, object>());
        contractOnOil_2.SetExternalId("6c6b3001-cfa9-4761-8e40-ae26e88f6ca5");
        ResourcesData costForContract_2 = new ResourcesData(rubCount: 1400);
        contractOnOil_2.SetCost(costForContract_2);
        ResourcesData givesForContract_2 = new ResourcesData(oilCount: 5);
        contractOnOil_2.SetGives(givesForContract_2);
        contractOnOil_2.SetType(InteractionComponentTypes.oilContract);
        contractOnOil_2.SetDuration(30);
        contractOnOil_2.SetName("Half a shift");
        contractOnOil_2.SetIconSection("UIBuildCards");
        contractOnOil_2.SetIconName("oil_station_card");

        ContractCacheItem contractOnOil_3 = new ContractCacheItem(new Dictionary<string, object>());
        contractOnOil_3.SetExternalId("522a3c5c-ce58-42b5-b237-80ac15845b6a");
        ResourcesData costForContract_3 = new ResourcesData(rubCount: 2750);
        contractOnOil_3.SetCost(costForContract_3);
        ResourcesData givesForContract_3 = new ResourcesData(oilCount: 12);
        contractOnOil_3.SetGives(givesForContract_3);
        contractOnOil_3.SetType(InteractionComponentTypes.oilContract);
        contractOnOil_3.SetDuration(65);
        contractOnOil_3.SetName("Full shift");
        contractOnOil_3.SetIconSection("UIBuildCards");
        contractOnOil_3.SetIconName("oil_station_card");

        contractsTable.Add(new CacheItem[3] { contractOnOil_1, contractOnOil_2, contractOnOil_3 });
        Cache.Save(contractsTable);
    }

    public void CreateRubContracts()
    {
        ContractCacheTable contractsTable = Cache.LoadByType<ContractCacheTable>();

        ContractCacheItem contractOnRub_1 = new ContractCacheItem(new Dictionary<string, object>());
        contractOnRub_1.SetExternalId("4a9f0fb4-51fe-452f-9159-6368e00d9b1f");
        ResourcesData costForContract_1 = new ResourcesData(oilCount: 1);
        contractOnRub_1.SetCost(costForContract_1);
        ResourcesData givesForContract_1 = new ResourcesData(rubCount: 550);
        contractOnRub_1.SetGives(givesForContract_1);
        contractOnRub_1.SetType(InteractionComponentTypes.rubContract);
        contractOnRub_1.SetDuration(5);
        contractOnRub_1.SetName("Rub express");
        contractOnRub_1.SetIconSection("UIBuildCards");
        contractOnRub_1.SetIconName("office_card");

        ContractCacheItem contractOnRub_2 = new ContractCacheItem(new Dictionary<string, object>());
        contractOnRub_2.SetExternalId("fc4c6678-2349-4927-a222-3fa0a54b8459");
        ResourcesData costForContract_2 = new ResourcesData(oilCount: 3);
        contractOnRub_2.SetCost(costForContract_2);
        ResourcesData givesForContract_2 = new ResourcesData(rubCount: 1800);
        contractOnRub_2.SetGives(givesForContract_2);
        contractOnRub_2.SetType(InteractionComponentTypes.rubContract);
        contractOnRub_2.SetDuration(25);
        contractOnRub_2.SetName("Half a shift");
        contractOnRub_2.SetIconSection("UIBuildCards");
        contractOnRub_2.SetIconName("office_card");

        ContractCacheItem contractOnRub_3 = new ContractCacheItem(new Dictionary<string, object>());
        contractOnRub_3.SetExternalId("c5f7ec2d-db3a-48a2-8845-51607d342e9c");
        ResourcesData costForContract_3 = new ResourcesData(oilCount: 5);
        contractOnRub_3.SetCost(costForContract_3);
        ResourcesData givesForContract_3 = new ResourcesData(rubCount: 3750);
        contractOnRub_3.SetGives(givesForContract_3);
        contractOnRub_3.SetType(InteractionComponentTypes.rubContract);
        contractOnRub_3.SetDuration(60);
        contractOnRub_3.SetName("Full shift");
        contractOnRub_3.SetIconSection("UIBuildCards");
        contractOnRub_3.SetIconName("office_card");

        contractsTable.Add(new CacheItem[3] { contractOnRub_1, contractOnRub_2, contractOnRub_3 });
        Cache.Save(contractsTable);
    }

    public void CreateUnitProductions()
    {
        UnitProductionCacheTable unitProductionsTable = Cache.LoadByType<UnitProductionCacheTable>();

        UnitProductionCacheItem assaulters = new UnitProductionCacheItem(new Dictionary<string, object>());
        assaulters.SetExternalId("7e940ae4-6d78-40a8-962b-0f1c154cf20d");
        assaulters.SetUnitId("bd1b7986-cf1a-4d76-8b14-c68bf10f363f");

        ResourcesData costForAssaulters = new ResourcesData(rubCount: 300);
        assaulters.SetCost(costForAssaulters);
        assaulters.SetType(InteractionComponentTypes.infantryUnitProduction);
        assaulters.SetDuration(5);
        assaulters.SetName("Train assaulters");
        assaulters.SetIconSection("UIUnitCards");
        assaulters.SetIconName("assaulters");


        UnitProductionCacheItem osoka = new UnitProductionCacheItem(new Dictionary<string, object>());
        osoka.SetExternalId("957bb263-524a-4460-8375-234b86055d7c");
        osoka.SetUnitId("035655f8-a347-4057-87cc-83385fa20660");

        ResourcesData costForOsoka = new ResourcesData(rubCount: 2000, oilCount: 2, steelCount: 4);
        osoka.SetCost(costForOsoka);
        osoka.SetType(InteractionComponentTypes.vehicleUnitProduction);
        osoka.SetDuration(30);
        osoka.SetName("Produce a BMP");
        osoka.SetIconSection("UIUnitCards");
        osoka.SetIconName("osoka");


        UnitProductionCacheItem vortex = new UnitProductionCacheItem(new Dictionary<string, object>());
        vortex.SetExternalId("51112095-826c-4436-91e8-5684dc94ca84");
        vortex.SetUnitId("a480c091-6b22-43f2-b26d-bbd3d2c2905b");

        ResourcesData costForVortex = new ResourcesData(rubCount: 1600, oilCount: 1, steelCount: 2);
        vortex.SetCost(costForVortex);
        vortex.SetType(InteractionComponentTypes.vehicleUnitProduction);
        vortex.SetDuration(20);
        vortex.SetName("Produce a light combat vehicle");
        vortex.SetIconSection("UIUnitCards");
        vortex.SetIconName("vortex");


        unitProductionsTable.Add(new CacheItem[3] { assaulters, vortex, osoka });
        Cache.Save(unitProductionsTable);
    }
}
