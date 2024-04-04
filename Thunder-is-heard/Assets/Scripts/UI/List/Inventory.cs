using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : ItemList
{
    public List<InventoryItem> items;
    public Transform content;

    public override void Start()
    {
        EventMaster.current.InventoryChanged += UpdateContent;

        InitContent();
        
        base.Start();
    }

    public void IncreaseItem(string id, string type, int count)
    {
        foreach (var item in items)
        {
            if (item.coreId == id && item.Type == type)
            {
                item.Increment(count);
                break;
            }
        }
    }

    public override void FillContent()
    {
        ClearItems();

        InventoryCacheTable inventoryTable = Cache.LoadByType<InventoryCacheTable>();
        foreach (var inventoryItemAsCacheItem in inventoryTable.Items)
        {
            InventoryCacheItem inventoryItemAsInventoryItem = new InventoryCacheItem(inventoryItemAsCacheItem.Value.Fields);
            string type = inventoryItemAsInventoryItem.GetType();

            CacheTable itemTable = Cache.LoadByName(type);
            CacheItem item = itemTable.GetById(inventoryItemAsInventoryItem.GetCoreId());

            switch (type)
            {
                case "Build":
                    BuildCacheItem buildData = new BuildCacheItem(item.Fields);
                    BuildInventoryItem build = CreateBuild(inventoryItemAsInventoryItem, buildData);
                    items.Add(build);
                    break;
                case "Unit":
                    UnitCacheItem unitData = new UnitCacheItem(item.Fields);
                    UnitInventoryItem unit = CreateUnit(inventoryItemAsInventoryItem, unitData);
                    items.Add(unit);
                    break;

            }
        }
    }

    public void UpdateContent()
    {
        FillContent();
    }

    public BuildInventoryItem CreateBuild(InventoryCacheItem inventoryItemData, BuildCacheItem buildData)
    {
        string id = inventoryItemData.GetExternalId();
        string name = buildData.GetName();
        ResourcesData gives = buildData.GetGives();
        int health = buildData.GetHealth();
        int damage = buildData.GetDamage();
        int distance = buildData.GetDistance();
        int count = inventoryItemData.GetCount();
        string description = buildData.GetDescrption();
        Sprite icon = Resources.Load<Sprite>(buildData.GetIconPath());

        GameObject itemObject = CreateObject(Config.resources["UI" + "Build" + "InventoryItemPrefab"]);
        itemObject.name = name;
        BuildInventoryItem buildComponent = itemObject.GetComponent<BuildInventoryItem>();

        buildComponent.Init(id, name, gives, health, damage, distance, count, description, icon);
        return buildComponent;
    }

    public UnitInventoryItem CreateUnit(InventoryCacheItem inventoryItemData, UnitCacheItem unitData)
    {
        string id = inventoryItemData.GetExternalId();
        string name = unitData.GetName();
        ResourcesData gives = unitData.GetGives();
        int health = unitData.GetHealth();
        int damage = unitData.GetDamage();
        int distance = unitData.GetDistance();
        int mobility = unitData.GetMobility();
        int count = inventoryItemData.GetCount();
        string description = unitData.GetDescrption();
        Sprite icon = Resources.Load<Sprite>(unitData.GetIconPath());

        GameObject itemObject = CreateObject(Config.resources["UI" + "Unit" + "InventoryItemPrefab"]);
        itemObject.name = name;
        UnitInventoryItem unitComponent = itemObject.GetComponent<UnitInventoryItem>();

        unitComponent.Init(id, name, gives, health, damage, distance, mobility, count, description, icon);
        return unitComponent;
    }

    public GameObject CreateObject(string prefabPath)
    {
        GameObject itemPrefab = Resources.Load<GameObject>(prefabPath);
        GameObject itemObject = Instantiate(itemPrefab);
        itemObject.transform.SetParent(content, false);
        return itemObject;
    }

    public void InitContent()
    {
        content = GameObject.FindGameObjectWithTag(Config.tags["inventoryItems"]).transform;
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

        items = new List<InventoryItem>();
    }
}
