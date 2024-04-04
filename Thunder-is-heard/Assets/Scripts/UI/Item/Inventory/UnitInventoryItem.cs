using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnitInventoryItem: ExposableInventoryItem
{
    public static string type = "Unit";

    public override string Type { get { return type; } }

    public int mobility;

    public TMP_Text TmpMobility;


    public void Init(string objectId, string objectName, ResourcesData objectGives, int objectHealth, int objectDamage, int objectDistance, int objectMobility, int objectCount, string objectDescription = "", Sprite objectIcon = null)
    {
        id = objectId; objName = objectName; icon = objectIcon;
        InitCoreId();

        gives = objectGives;
        description = objectDescription;
        health = objectHealth; damage = objectDamage; distance = objectDistance; mobility = objectMobility; count = objectCount;

        UpdateUI();
    }

    public override void UpdateUI()
    {
        TmpMobility.text = mobility.ToString();

        base.UpdateUI();
    }


    public override void SaveExpose(Bector2Int[] occypation, int rotation)
    {
        PlayerUnitCacheItem exposedUnitData = new PlayerUnitCacheItem(new Dictionary<string, object>());
        exposedUnitData.SetCoreId(coreId);
        exposedUnitData.SetName(name);
        exposedUnitData.SetPosition(occypation);
        exposedUnitData.SetRotation(rotation);

        PlayerUnitCacheTable exposedUnits = Cache.LoadByType<PlayerUnitCacheTable>();
        exposedUnits.Add(new CacheItem[1] { exposedUnitData });
        Cache.Save(exposedUnits);
    }
}
