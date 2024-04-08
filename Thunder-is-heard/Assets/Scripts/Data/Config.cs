
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
        {"materialPreview", "Materials/Preview/" },
        {"defaultCellMaterial", "Materials/Cell/Basic" },
        {"landableCellMaterial", "Materials/Cell/Landable" },
        { "prefabs", "Prefabs/" },
        { "entityPrefabs", "Prefabs/Entity/" },
        { "prefabPreview", "Prefabs/Custom/Preview" },
        { "emptyPrefab", "Prefabs/Entity/Basic/Empty" },
        { "UIBuildInventoryItemPrefab", "Prefabs/UI/GeneratedItems/Inventory/BuildItem" },
        { "UIUnitInventoryItemPrefab", "Prefabs/UI/GeneratedItems/Inventory/UnitItem" },
        { "landableUnitItem", "Prefabs/UI/GeneratedItems/UnitForLanding/LandableUnitItem" },
        { "ranks", "Textures/Interface/Panels/Ranks" }
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

    public static Dictionary<int, int> maxExpByRank = new Dictionary<int, int>
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

    public static Dictionary<string, string> tags = new Dictionary<string, string>
    {
        {"federation", "Federation" },
        {"empire", "Empire" },
        {"mission", "Mission" },
        { "inventoryItems", "InventoryItems" }
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
        {"Base", "Prefabs/Terrain/Base/Terrain" },
        { "Common", "Prefabs/Terrain/" }
    };
}
