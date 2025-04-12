using System.Collections.Generic;

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
                    BuildInventoryItem build = conductor.CreateBuild(inventoryItemData, buildData);
                    conductor.items.Add(build);
                    break;
                case "Unit":
                    UnitCacheItem unitData = new UnitCacheItem(item.Fields);
                    UnitInventoryItem unit = conductor.CreateUnit(inventoryItemData, unitData);
                    conductor.items.Add(unit);
                    break;
                case "Material":
                    MaterialCacheItem materialData = new MaterialCacheItem(item.Fields);
                    MaterialInventoryItem material = conductor.CreateMaterial(inventoryItemData, materialData);
                    conductor.items.Add(material);
                    break;
            }
        }
    }
}
