using System.Collections.Generic;
using UnityEngine;

public class BaseSubsituableInventoryBehaviour : ISubsituableInventoryBehaviour
{
    public virtual List<InventoryItem> GetItems(Inventory conductor)
    {
        return conductor.items;
    }

    public virtual void Init(Inventory conductor)
    {
        FillContent(conductor);
    }

    public virtual void OnUse(InventoryItem item)
    {
        if (item is ExposableInventoryItem) OnUseExposableItem(item as ExposableInventoryItem);
        else
        {
            // No interact
        }
    }

    public virtual void OnUseExposableItem(ExposableInventoryItem item)
    {
        item.CreatePreview();

        EventMaster.current.OnBuildMode();
        EventMaster.current.ToggledOffBuildMode += item.OnCancelExposing;
        EventMaster.current.ObjectExposed += item.OnObjectExposed;
    }

    public virtual void Toggle(Inventory conductor)
    {
        if (conductor.gameObject.activeSelf)
        {
            conductor.Hide();
        }
        else
        {
            conductor.Show();
        }
    }

    public virtual void FillContent(Inventory conductor)
    {
        conductor.ClearItems();
        conductor.items = new List<InventoryItem>();

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
                    BuildInventoryItem build = CreateBuild(conductor, inventoryItemData, buildData);
                    conductor.items.Add(build);
                    break;
                case "Unit":
                    UnitCacheItem unitData = new UnitCacheItem(item.Fields);
                    UnitInventoryItem unit = CreateUnit(conductor, inventoryItemData, unitData);
                    conductor.items.Add(unit);
                    break;
                case "Material":
                    MaterialCacheItem materialData = new MaterialCacheItem(item.Fields);
                    MaterialInventoryItem material = CreateMaterial(conductor, inventoryItemData, materialData);
                    conductor.items.Add(material);
                    break;
            }
        }
    }

    public virtual BuildInventoryItem CreateBuild(Inventory conductor, InventoryCacheItem inventoryItemData, BuildCacheItem buildData)
    {
        string id = inventoryItemData.GetExternalId();
        string coreId = inventoryItemData.GetCoreId();
        string name = buildData.GetName();
        ResourcesData gives = buildData.GetGives();
        int health = buildData.GetHealth();
        int damage = buildData.GetDamage();
        int distance = buildData.GetDistance();
        int count = inventoryItemData.GetCount();
        string description = buildData.GetDescription();
        Sprite icon = ResourcesUtils.LoadIcon(buildData.GetIconSection(), buildData.GetIconName());

        GameObject itemObject = Inventory.CreateObject(Config.resources["UI" + "Build" + "InventoryItemPrefab"], conductor.content);
        itemObject.name = name;
        BuildInventoryItem buildComponent = itemObject.GetComponent<BuildInventoryItem>();

        buildComponent.Init(
            id, 
            coreId,
            name, 
            gives,
            health, 
            damage, 
            distance, 
            count, 
            description,
            icon
            );
        buildComponent.SetConductor(conductor);
        return buildComponent;
    }

    public virtual UnitInventoryItem CreateUnit(Inventory conductor, InventoryCacheItem inventoryItemData, UnitCacheItem unitData)
    {
        string id = inventoryItemData.GetExternalId();
        string coreId = inventoryItemData.GetCoreId();
        string name = unitData.GetName();
        ResourcesData gives = unitData.GetGives();
        int health = unitData.GetHealth();
        int damage = unitData.GetDamage();
        int distance = unitData.GetDistance();
        int mobility = unitData.GetMobility();
        int count = inventoryItemData.GetCount();
        string description = unitData.GetDescription();
        Sprite icon = ResourcesUtils.LoadIcon(unitData.GetIconSection(), unitData.GetIconName());

        GameObject itemObject = Inventory.CreateObject(Config.resources["UI" + "Unit" + "InventoryItemPrefab"], conductor.content);
        itemObject.name = name;
        UnitInventoryItem unitComponent = itemObject.GetComponent<UnitInventoryItem>();

        unitComponent.Init(
            id, 
            coreId,
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
        unitComponent.SetConductor(conductor);
        return unitComponent;
    }

    public virtual MaterialInventoryItem CreateMaterial(Inventory conductor, InventoryCacheItem inventoryItemData, MaterialCacheItem materialData)
    {
        string id = inventoryItemData.GetExternalId();
        string coreId = inventoryItemData.GetCoreId();
        string name = materialData.GetName();
        int count = inventoryItemData.GetCount();
        string description = materialData.GetDescription();
        Sprite icon = ResourcesUtils.LoadIcon(materialData.GetIconSection(), materialData.GetIconName());

        GameObject itemObject = Inventory.CreateObject(Config.resources["UI" + "Material" + "InventoryItemPrefab"], conductor.content);
        itemObject.name = name;
        MaterialInventoryItem materialComponent = itemObject.GetComponent<MaterialInventoryItem>();

        materialComponent.Init(id, coreId, name, count, description, icon);
        materialComponent.SetConductor(conductor);
        return materialComponent;
    }

    public virtual void CreatePreview(Inventory conductor, ExposableInventoryItem item)
    {
        CacheTable itemsTable = Cache.LoadByName(item.Type);

        CacheItem needleItemData = itemsTable.GetById(item.coreId);
        if (needleItemData == null)
        {
            Debug.Log("CreatePreview | Can't find item by id: " + item.coreId);
            item.Finish();
            return;
        }

        string modelPath = (string)needleItemData.GetField("modelPath") + "/" + Tags.federation;
        Vector2Int size = item.GetSize(needleItemData).ToVector2Int();
        Transform model = ObjectProcessor.CreateModel(modelPath, 0).transform;

        ObjectPreview preview = ObjectPreview.Create();
        preview.Init(item._objName, item.Type, item.coreId, size, model);
    }

    public virtual void OnObjectExposed(Inventory conductor, ExposableInventoryItem item, Entity obj)
    {
        if (obj.CoreId == item.coreId && obj.Type.Contains(item.Type))
        {
            if (item._count < 2)
            {
                item.Finish();
            }

            item.Substract();
        }

        else
        {
            item.Continue();
        }
    }

    public virtual void Substract(Inventory conductor, InventoryItem item, int number = 1)
    {
        InventoryCacheTable inventoryItemsTable = Cache.LoadByType<InventoryCacheTable>();
        CacheItem cacheItem = inventoryItemsTable.GetById(item._id);
        InventoryCacheItem inventoryItem = new InventoryCacheItem(cacheItem.Fields);
        inventoryItem.SetCount(inventoryItem.GetCount() - 1);
        if (inventoryItem.GetCount() < 1)
        {
            inventoryItemsTable.Delete(new CacheItem[1] { cacheItem });
        }
        else
        {
            cacheItem.fields = inventoryItem.Fields;
        }

        Cache.Save(inventoryItemsTable);

        item.UpdateCount(item._count - number);
    }

    public virtual void OnInventoryItemAdded(Inventory conductor, InventoryItem sourceItem, InventoryCacheItem addedItem)
    {
        sourceItem.OnInventoryItemAdded(addedItem);
    }

    public virtual void Increment(Inventory conductor, InventoryItem item, int number = 1)
    {
        item.Increment(number);
    }
}
