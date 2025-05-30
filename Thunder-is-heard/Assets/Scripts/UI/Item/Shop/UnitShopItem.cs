using UnityEngine;
using TMPro;

public class UnitShopItem: ExposableShopItem
{
    public static string type = "Unit";

    public override string Type { get { return type; } }

    public int mobility;

    public TMP_Text TmpMobility;


    public void Init(string objectId, string objectCoreId, string objectName, ResourcesData objectCost, ResourcesData objectGives, int objectHealth, int objectDamage, int objectDistance, int objectMobility, int objectCount, string objectDescription = "", Sprite objectIcon = null)
    {
        _id = objectId; coreId = objectCoreId; _objName = objectName; _icon = objectIcon; _itemImage.sprite = _icon;

        costData = objectCost;
        givesData = objectGives;
        _description = objectDescription;
        health = objectHealth; damage = objectDamage; distance = objectDistance; mobility = objectMobility; _count = objectCount;

        UpdateUI();
    }

    public override void UpdateUI()
    {
        TmpMobility.text = mobility.ToString();

        base.UpdateUI();
    }
}
