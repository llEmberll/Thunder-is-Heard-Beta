using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnitShopItem: ExposableShopItem
{
    public static string type = "Unit";

    public override string Type { get { return type; } }

    public int mobility;

    public TMP_Text TmpMobility;


    public void Init(string objectId, string objectName, ResourcesData objectCost, ResourcesData objectGives, int objectHealth, int objectDamage, int objectDistance, int objectMobility, int objectCount, string objectDescription = "", Sprite objectIcon = null)
    {
        id = objectId; objName = objectName; icon = objectIcon; itemImage.sprite = icon;
        InitCoreId();

        costData = objectCost;
        givesData = objectGives;
        description = objectDescription;
        health = objectHealth; damage = objectDamage; distance = objectDistance; mobility = objectMobility; count = objectCount;

        UpdateUI();
    }

    public override void UpdateUI()
    {
        TmpMobility.text = mobility.ToString();

        base.UpdateUI();
    }
}
