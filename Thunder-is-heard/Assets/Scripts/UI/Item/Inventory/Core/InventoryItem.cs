using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class InventoryItem : Item
{
    public TMP_Text TmpCount, TmpDescription;

    public int count;

    public string description;

    public abstract string Type { get; }

    public abstract override void Interact();

    public override void UpdateUI()
    {
        TmpCount.text = count.ToString();
        TmpDescription.text = description;

        base.UpdateUI();
    }

    public void UpdateCount(int newCount)
    {
        count = newCount;
        TmpCount.text = newCount.ToString();
        if (count < 1)
        {
            Destroy(this.gameObject);
        }
    }

    public void Substract(int number = 1)
    {
        InventoryCacheTable inventoryItemsTable = Cache.LoadByType<InventoryCacheTable>();
        CacheItem cacheItem = inventoryItemsTable.GetByCoreId(id);
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

    public void Increment(int number = 1)
    {
        UpdateCount(count + number);
    }
}

