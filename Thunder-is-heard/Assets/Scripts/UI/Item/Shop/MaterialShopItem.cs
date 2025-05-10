using UnityEngine;

public class MaterialShopItem: ShopItem
{
    public static string type = "Material";

    public override string Type { get { return type; } }


    public void Init(string objectId, string objectCoreId, string objectName, ResourcesData objectCost, int objectCount, string objectDescription = "", Sprite objectIcon = null)
    {
        _id = objectId; coreId = objectCoreId; _objName = objectName; _icon = objectIcon; _itemImage.sprite = _icon;

        costData = objectCost;
        _description = objectDescription;
        _count = objectCount;

        UpdateUI();
    }
}
