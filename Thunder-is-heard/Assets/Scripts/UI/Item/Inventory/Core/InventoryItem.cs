using TMPro;
using UnityEngine;

public abstract class InventoryItem : Item
{
    public string coreId;

    public TMP_Text TmpCount;

    public override void EnableListeners()
    {
        EventMaster.current.InventoryItemAdded += OnInventoryItemAdded;
    }

    public override void DisableListeners()
    {
        EventMaster.current.InventoryItemAdded -= OnInventoryItemAdded;
    }

    public virtual void OnInventoryItemAdded(InventoryCacheItem item)
    {
        if (this.Type.Contains(item.GetType()) && item.GetCoreId() == this.coreId) {
            Increment();
        }
    }

    public abstract override void Interact();

    public override void UpdateUI()
    {
        TmpCount.text = _count.ToString();
        TmpDescription.text = _description;

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
        CacheItem cacheItem = inventoryItemsTable.GetById(_id);
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

        UpdateCount(_count - number);
    }

    public void InitCoreId()
    {
        InventoryCacheTable inventory = Cache.LoadByType<InventoryCacheTable>();
        InventoryCacheItem inventoryItem = new InventoryCacheItem(inventory.GetById(_id).Fields);
        coreId = inventoryItem.GetCoreId();
    }
}

