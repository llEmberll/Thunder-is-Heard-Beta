using UnityEngine;
using TMPro;

public class LandableUnit: ExposableInventoryItem
{
    public bool focusOn = false;

    public static string type = "Unit";

    public override string Type { get { return type; } }

    public int mobility;

    public int staff;
    public TMP_Text TmpStaff;

    public void Init(string inventoryItemId, string inventoryItemCoreId, string objectName, int objectStaff, int objectHealth, int objectDamage, int objectDistance, int objectMobility, int objectCount, Sprite objectIcon = null)
    {
        _id = inventoryItemId; _objName = objectName; _icon = objectIcon; _itemImage.sprite = _icon;
        InitCoreId(inventoryItemCoreId);

        staff = objectStaff;
        health = objectHealth; damage = objectDamage; distance = objectDistance; mobility = objectMobility; _count = objectCount;

        UpdateUI();
    }

    public override void UpdateUI()
    {
        TmpName.text = _objName;
        TmpCount.text = "x" + _count.ToString();
        TmpStaff.text = staff.ToString();
    }

    public override void OnInventoryItemAdded(InventoryCacheItem item)
    {
        Debug.Log($"[OnInventoryItemAdded] InventoryCacheItem: {(item == null ? "NULL" : "NOT NULL")}");
        Debug.Log($"[OnInventoryItemAdded] conductor: {(conductor == null ? "NULL" : "NOT NULL")}");

        // ����� �� ��� ���� ��-�� ���� ��� Landing � ���� ������ ��������?

        conductor.OnInventoryItemAdded(this, item);
    }

    public override void Increment(int number = 1)
    {
        conductor.Increment(this, number);
    }
}
