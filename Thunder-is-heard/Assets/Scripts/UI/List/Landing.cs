using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class Landing : ItemList
{
    public List<LandableUnit> items;
    public Transform content;

    public TMP_Text TmpHealth;
    public TMP_Text TmpDamage;
    public TMP_Text TmpDistance;
    public TMP_Text TmpMobility;


    public override void Start()
    {
        InitContent();
        InitEvents();

        base.Start();
    }

    public void InitEvents()
    {
        EventMaster.current.LandableUnitFocused += OnLandableUnitFocus;
        EventMaster.current.LandableUnitDefocused += OnLandableUnitDefocus;
    }

    public void OnLandableUnitFocus(LandableUnit target)
    {
        TmpHealth.text = target.health.ToString();
        TmpDamage.text = target.damage.ToString();
        TmpDistance.text = target.distance.ToString();
        TmpMobility.text = target.mobility.ToString();
    }

    public void OnLandableUnitDefocus(LandableUnit target)
    {
        TmpHealth.text = "-";
        TmpDamage.text = "-";
        TmpDistance.text = "-";
        TmpMobility.text = "-";
    }

    public override void FillContent()
    {
        ClearItems();

        InventoryCacheTable inventoryTable = Cache.LoadByType<InventoryCacheTable>();
        foreach (var keyValuePair in inventoryTable.Items)
        {
            InventoryCacheItem inventoryItemData = new InventoryCacheItem(keyValuePair.Value.Fields);
            string type = inventoryItemData.GetType();

            CacheTable itemTable = Cache.LoadByName(type);
            CacheItem item = itemTable.GetById(inventoryItemData.GetCoreId());

            switch (type)
            {
                case "Unit":
                    UnitCacheItem unitData = new UnitCacheItem(item.Fields);
                    LandableUnit unit = CreateUnit(inventoryItemData, unitData);
                    items.Add(unit);
                    break;
            }
        }
    }

    public LandableUnit CreateUnit(InventoryCacheItem inventoryItemData, UnitCacheItem unitData)
    {
        string id = inventoryItemData.GetExternalId();
        string name = unitData.GetName();
        int staff = unitData.GetGives().staff;
        int health = unitData.GetHealth();
        int damage = unitData.GetDamage();
        int distance = unitData.GetDistance();
        int mobility = unitData.GetMobility();
        int count = inventoryItemData.GetCount();
        Sprite icon = Resources.Load<Sprite>(unitData.GetIconPath());

        GameObject itemObject = CreateObject(Config.resources["UI" + "Unit" + "LandablePrefab"], content);
        itemObject.name = name;
        LandableUnit unitComponent = itemObject.GetComponent<LandableUnit>();

        unitComponent.Init(
            id,
            name,
            staff,
            health,
            damage,
            distance,
            mobility,
            count,
            icon
            );
        return unitComponent;
    }

    public void InitContent()
    {
        content = GameObject.FindGameObjectWithTag(Tags.landableUnits).transform;
    }

    public void ClearItems()
    {
        GameObject[] children = content.gameObject.GetComponentsInChildren<Transform>(true)
            .Where(obj => obj != content)
            .Select(obj => obj.gameObject)
            .ToArray();

        foreach (GameObject child in children)
        {
            Destroy(child);
        }

        items = new List<LandableUnit>();
    }
}
