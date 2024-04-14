using TMPro;
using UnityEngine;

public abstract class InventoryItem : Item
{
    public string coreId;

    public TMP_Text TmpCount, TmpDescription;

    public string description;

    public abstract override void Interact();

    public override void UpdateUI()
    {
        TmpCount.text = count.ToString();
        TmpDescription.text = description;

        base.UpdateUI();
    }

    public override void UpdateCount(int newCount)
    {
        base.UpdateCount(newCount);
        TmpCount.text = newCount.ToString();
    }

    public void Substract(int number = 1)
    {
        InventoryCacheTable inventoryItemsTable = Cache.LoadByType<InventoryCacheTable>();
        CacheItem cacheItem = inventoryItemsTable.GetById(id);
        InventoryCacheItem inventoryItem = new InventoryCacheItem(cacheItem.Fields);
        inventoryItem.SetCount(inventoryItem.GetCount() - 1);
        if (inventoryItem.GetCount() < 1)
        {
            inventoryItemsTable.Delete(new CacheItem[1] { cacheItem});
        }
        else
        {
            cacheItem.fields = inventoryItem.Fields;
        }

        Cache.Save(inventoryItemsTable);

        UpdateCount(count - number);
    }

    public void InitCoreId()
    {
        InventoryCacheTable inventory = Cache.LoadByType<InventoryCacheTable>();
        InventoryCacheItem inventoryItem = new InventoryCacheItem(inventory.GetById(id).Fields);
        coreId = inventoryItem.GetCoreId();
    }
}

