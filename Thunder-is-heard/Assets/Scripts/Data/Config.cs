
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
}