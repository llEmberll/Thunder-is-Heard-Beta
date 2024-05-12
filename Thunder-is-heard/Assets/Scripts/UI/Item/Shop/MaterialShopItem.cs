using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MaterialShopItem: ShopItem
{
    public static string type = "Material";

    public override string Type { get { return type; } }


    public void Init(string objectId, string objectName, ResourcesData objectCost, int objectCount, string objectDescription = "", Sprite objectIcon = null)
    {
        id = objectId; objName = objectName; icon = objectIcon; itemImage.sprite = icon;
        InitCoreId();

        costData = objectCost;
        description = objectDescription;
        count = objectCount;

        UpdateUI();
    }

    public override void OnBuy()
    {
        ObjectProcessor.OnBuyMaterial(coreId);
        resourcesProcessor.SubstractResources(costData);
        resourcesProcessor.Save();

        Substract();
    }
}
