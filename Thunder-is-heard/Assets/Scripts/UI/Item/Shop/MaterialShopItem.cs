using UnityEngine;

public class MaterialShopItem: ShopItem
{
    public static string type = "Material";

    public override string Type { get { return type; } }


    public void Init(string objectId, string objectName, ResourcesData objectCost, int objectCount, string objectDescription = "", Sprite objectIcon = null)
    {
        _id = objectId; _objName = objectName; _icon = objectIcon; _itemImage.sprite = _icon;
        InitCoreId();

        costData = objectCost;
        _description = objectDescription;
        _count = objectCount;

        UpdateUI();
    }
}
