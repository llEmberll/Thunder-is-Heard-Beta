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


    //Реализовать после создания кеша для юнита
    public override void SaveExpose(Bector2Int[] occypation, int rotation)
    {
        PlayerBuildCacheItem exposedBuildData = new PlayerBuildCacheItem(new Dictionary<string, object>());
        exposedBuildData.SetCoreId(id);
        exposedBuildData.SetName(name);
        exposedBuildData.SetPosition(occypation);
        exposedBuildData.SetRotation(rotation);

        PlayerBuildCacheTable exposedBuilds = Cache.LoadByType<PlayerBuildCacheTable>();
        exposedBuilds.Add(new CacheItem[1] { exposedBuildData });
        Cache.Save(exposedBuilds);
    }
}
