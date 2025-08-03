using UnityEngine;

public class MaterialInventoryItem: InventoryItem
{
    public static string type = "Material";

    public override string Type { get { return type; } }


    public void Init(string objectId, string objectCoreId, string objectName, int objectCount, string objectDescription = "", Sprite objectIcon = null)
    {
        _id = objectId; _objName = objectName; _icon = objectIcon; _itemImage.sprite = _icon;
        InitCoreId(objectCoreId);

        _description = objectDescription;
        _count = objectCount;

        UpdateUI();
    }
}
