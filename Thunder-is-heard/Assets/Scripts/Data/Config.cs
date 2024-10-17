
using System.Collections.Generic;

public static class Config
{
    public static Dictionary<string, string> dataBase = new Dictionary<string, string>
    {
        {"host", "localhost" },
        { "username", "root" },
        { "password", "thunderisheard" },
        {"database",  "thunderisheard"},
        { "port", "3306" },
    };

    public static Dictionary<string, string> localDataBase = new Dictionary<string, string>
    {
        {"tablesPath", "LocalDatabase/Tables/" }
    };

    public static Dictionary<string, string> streamingAssets = new Dictionary<string, string>
    {
        {"cachePath", "Cache/" }
    };


    public static Dictionary<string, string> resources = new Dictionary<string, string>
    {
        {"materials", "Materials/" },
        {"UICards", "Textures/Interface/Cards/" },
        {"materialPreview", "Materials/Preview/" },
        {"defaultCellMaterial", "Materials/Cell/Basic" },
        {"landableCellMaterial", "Materials/Cell/Landable" },
        { "prefabs", "Prefabs/" },
        { "entityPrefabs", "Prefabs/Entity/" },
        { "prefabPreview", "Prefabs/Custom/Preview" },
        { "emptyPrefab", "Prefabs/Entity/Basic/Empty" },
        { "fightProcessorPrefab", "Prefabs/Battle/FightProcessor" },
        { "resourceForUIItem", "Prefabs/UI/GeneratedItems/Common/Resource/ResourceElement" },

        { "UIBuildInventoryItemPrefab", "Prefabs/UI/GeneratedItems/Inventory/BuildItem" },
        { "UIUnitInventoryItemPrefab", "Prefabs/UI/GeneratedItems/Inventory/UnitItem" },
        { "UIMaterialInventoryItemPrefab", "Prefabs/UI/GeneratedItems/Inventory/MaterialItem" },
        { "UIBuildShopItemPrefab", "Prefabs/UI/GeneratedItems/Shop/BuildItem" },
        { "UIUnitShopItemPrefab", "Prefabs/UI/GeneratedItems/Shop/UnitItem" },
        { "UIContractItemPrefab", "Prefabs/UI/GeneratedItems/Contract/ContractItem" },
        { "UIUnitProductionItemPrefab", "Prefabs/UI/GeneratedItems/UnitProduction/UnitItem" },
        { "UIMaterialShopItemPrefab", "Prefabs/UI/GeneratedItems/Shop/MaterialItem" },
        { "UIMissionItemPrefab", "Prefabs/UI/GeneratedItems/Campany/MissionItem" },
        { "UIUnitLandablePrefab", "Prefabs/UI/GeneratedItems/LandableUnit/LandableUnit" },

        { "UIProductsNotificationPrefab", "Prefabs/UI/GeneratedItems/Common/ProductsNotification/ProductsNotification" },
        { "UIBuildCards", "Textures/Interface/Cards/Builds/cards/" },
        { "UIUnitCards", "Textures/Interface/Cards/Unit/cards/" },
        { "landableUnitItem", "Prefabs/UI/GeneratedItems/UnitForLanding/LandableUnitItem" },
        { "ranks", "Textures/Interface/Icons/Ranks/Ranks" },
        { "resourcesIcons", "Textures/Interface/Resources/Resources" },

        { "enemySelector", "Textures/Interface/Selections/EnemySelector" },
        { "allySelector", "Textures/Interface/Selections/AllySelector" },
        { "neutralSelector", "Textures/Interface/Selections/NeutralSelector" },
        { "attackableSelector", "Textures/Interface/Selections/AttackableSelector" },
        { "attackRadius", "Textures/Interface/Selections/AttackRadius" },

        { "routeImage", "Textures/Interface/Selections/RouteImage" },
        { "overRouteImage", "Textures/Interface/Selections/OverRouteImage" },

        { "productsCollectionBackgroundIconIdle", "Textures/Interface/Icons/ProductsCollection/ProductsCollectionBackgroundIdle" },
        { "productsCollectionBackgroundIconAllow", "Textures/Interface/Icons/ProductsCollection/ProductsCollectionBackgroundAllow" },
        { "productsCollectionBackgroundIconForbidden", "Textures/Interface/Icons/ProductsCollection/ProductsCollectionBackgroundForbidden" },

        { "missionIconsSection", "Textures/Interface/Icons/Campany/MissionIcons" },
    };

    public static Dictionary<string, string> mapResources = new Dictionary<string, string>
    {
        { "terrainParent", "Prefabs/Map/TerrainParent/Terrain" },
        { "cellsParent", "Prefabs/Map/CellsParent/Cells" },
        { "cellPrefab", "Prefabs/Cell" },
    };

    public static Dictionary<int, int> ranksByExp = new Dictionary<int, int>
    {
        { 0, 0 },
        { 250, 1 },
        { 500, 2 },
        { 1100, 3 },
        { 2050, 4 },
        { 3400, 5 },
        { 5200, 6 },
        { 8600, 7 },
        { 12500, 8 },
        { 18250, 9 },
        { 25700, 10 },
        { 34000, 11 },
        { 44100, 12 },
        { 55500, 13 },
        { 68450, 14 },
        { 84000, 15 },
        { 105800, 16 },
        { 130000, 17 }
    };

    public static Dictionary<string, string> exposableObjectsTypeToObjectsOnSceneTag = new Dictionary<string, string>
    {
        {"PlayerBuild", "BuildsOnScene" },
        {"Build", "BuildsOnScene" },
        {"PlayerUnit", "UnitsOnScene" },
        { "Unit", "UnitsOnScene" }
    };

    public static Dictionary<string, string> terrainsPath = new Dictionary<string, string>
    {
        { "Base", "Prefabs/Terrain/Base/Terrain" },
        { "Common", "Prefabs/Terrain/" }
    };
}
