using System.Collections.Generic;
using UnityEngine;

public class Inventory : ItemList
{
    public List<InventoryItem> items;

    public ISubsituableInventoryBehaviour _behaviour;

    public override void Awake()
    {
        ChangeBehaviour();
        base.Awake();
    }

    public override void Start()
    {
        EventMaster.current.InventoryChanged += UpdateContent;

        InitContent();
        
        base.Start();
    }

    public void IncreaseItem(string id, string type, int count)
    {
        foreach (var item in _behaviour.GetItems(this))
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
        items = new List<InventoryItem>();

        InventoryCacheTable inventoryTable = Cache.LoadByType<InventoryCacheTable>();
        foreach (var keyValuePair in inventoryTable.Items)
        {
            InventoryCacheItem inventoryItemData = new InventoryCacheItem(keyValuePair.Value.Fields);
            string type = inventoryItemData.GetType();

            CacheTable itemTable = Cache.LoadByName(type);
            CacheItem item = itemTable.GetById(inventoryItemData.GetCoreId());

            switch (type)
            {
                case "Build":
                    BuildCacheItem buildData = new BuildCacheItem(item.Fields);
                    BuildInventoryItem build = CreateBuild(inventoryItemData, buildData);
                    items.Add(build);
                    break;
                case "Unit":
                    UnitCacheItem unitData = new UnitCacheItem(item.Fields);
                    UnitInventoryItem unit = CreateUnit(inventoryItemData, unitData);
                    items.Add(unit);
                    break;
                case "Material":
                    MaterialCacheItem materialData = new MaterialCacheItem(item.Fields);
                    MaterialInventoryItem material = CreateMaterial(inventoryItemData, materialData);
                    items.Add(material);
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
        string description = buildData.GetDescription();
        Sprite icon = ResourcesUtils.LoadIcon(buildData.GetIconSection(), buildData.GetIconName());

        GameObject itemObject = CreateObject(Config.resources["UI" + "Build" + "InventoryItemPrefab"], content);
        itemObject.name = name;
        BuildInventoryItem buildComponent = itemObject.GetComponent<BuildInventoryItem>();

        buildComponent.Init(
            id, 
            name, 
            gives,
            health, 
            damage, 
            distance, 
            count, 
            description,
            icon
            );
        buildComponent.SetConductor(this);
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
        string description = unitData.GetDescription();
        Sprite icon = ResourcesUtils.LoadIcon(unitData.GetIconSection(), unitData.GetIconName());

        GameObject itemObject = CreateObject(Config.resources["UI" + "Unit" + "InventoryItemPrefab"], content);
        itemObject.name = name;
        UnitInventoryItem unitComponent = itemObject.GetComponent<UnitInventoryItem>();

        unitComponent.Init(
            id, 
            name, 
            gives, 
            health, 
            damage, 
            distance, 
            mobility, 
            count, 
            description,
            icon
            );
        unitComponent.SetConductor(this);
        return unitComponent;
    }

    public MaterialInventoryItem CreateMaterial(InventoryCacheItem inventoryItemData, MaterialCacheItem materialData)
    {
        string id = inventoryItemData.GetExternalId();
        string name = materialData.GetName();
        int count = inventoryItemData.GetCount();
        string description = materialData.GetDescription();
        Sprite icon = ResourcesUtils.LoadIcon(materialData.GetIconSection(), materialData.GetIconName());

        GameObject itemObject = CreateObject(Config.resources["UI" + "Material" + "InventoryItemPrefab"], content);
        itemObject.name = name;
        MaterialInventoryItem materialComponent = itemObject.GetComponent<MaterialInventoryItem>();

        materialComponent.Init(id, name, count, description, icon);
        materialComponent.SetConductor(this);
        return materialComponent;
    }

    public void InitContent()
    {
        content = GameObject.FindGameObjectWithTag(Tags.inventoryItems).transform;
    }

    public void ChangeBehaviour(string name = "Base")
    {
        _behaviour = SubsituableInventoryFactory.GetBehaviourById(name);
        _behaviour.Init();
    }

    public void OnUse(InventoryItem item)
    {
        _behaviour.OnUse(item);
    }
}
