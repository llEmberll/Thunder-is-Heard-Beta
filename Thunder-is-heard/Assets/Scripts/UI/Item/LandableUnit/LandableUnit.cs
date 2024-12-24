using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class LandableUnit: ExposableInventoryItem
{
    public bool focusOn = false;

    public static string type = "Unit";

    public override string Type { get { return type; } }

    public int mobility;

    public int staff;
    public TMP_Text TmpStaff;


    public void Init(string inventoryItemId, string objectName, int objectStaff, int objectHealth, int objectDamage, int objectDistance, int objectMobility, int objectCount, Sprite objectIcon = null)
    {
        _id = inventoryItemId; _objName = objectName; _icon = objectIcon; _itemImage.sprite = _icon;
        InitCoreId();

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

    public override void OnPointerEnter(PointerEventData data)
    {
        if (!focusOn)
        {
            EventMaster.current.OnLandableUnitFocus(this);
        }

        focusOn = true;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (focusOn)
        {
            EventMaster.current.OnLandableUnitDefocus(this);
        }

        focusOn = false;
    }
}
