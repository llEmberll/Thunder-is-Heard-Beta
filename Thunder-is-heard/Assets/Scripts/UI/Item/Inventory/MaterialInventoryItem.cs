using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MaterialInventoryItem: InventoryItem
{
    public static string type = "Material";

    public override string Type { get { return type; } }


    public void Init(string objectId, string objectName, int objectCount, string objectDescription = "", Sprite objectIcon = null)
    {
        id = objectId; objName = objectName; icon = objectIcon; itemImage.sprite = icon;
        InitCoreId();

        description = objectDescription;
        count = objectCount;

        UpdateUI();
    }

    public override void Interact()
    {
    }
}
