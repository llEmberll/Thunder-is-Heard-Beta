using TMPro;
using UnityEngine;
using System;
using System.Reflection;
using UnityEngine.UI;

public abstract class ShopItem : Item
{
    public ResourcesProcessor resourcesProcessor;

    public string coreId;

    public TMP_Text TmpCount;

    public ResourcesData costData;
    public Transform cost;

    public ResourcesData givesData;
    public Transform gives;

    public override void Awake()
    {
        resourcesProcessor = GameObject.FindGameObjectWithTag("ResourcesProcessor").GetComponent<ResourcesProcessor>();
    }

    public override void Interact()
    {
        if (!IsAvailableToBuy()) return;

        OnBuy();
    }

    public bool IsAvailableToBuy()
    {
        return resourcesProcessor.IsAvailableToBuy(costData);
    }

    public abstract void OnBuy();

    public override void UpdateUI()
    {
        TmpCount.text = _count.ToString();
        TmpDescription.text = _description;

        ResourcesProcessor.UpdateResources(cost, costData);
        ResourcesProcessor.UpdateResources(gives, givesData);

        base.UpdateUI();
    }

    public override void UpdateCount(int newCount)
    {
        base.UpdateCount(newCount);
        TmpCount.text = newCount.ToString();
    }

    public virtual void Substract(int number = 1)
    {
        ShopCacheTable shopItemsTable = Cache.LoadByType<ShopCacheTable>();
        CacheItem cacheItem = shopItemsTable.GetById(_id);
        ShopCacheItem shopItem = new ShopCacheItem(cacheItem.Fields);
        shopItem.SetCount(shopItem.GetCount() - 1);
        if (shopItem.GetCount() < 1)
        {
            shopItemsTable.Delete(new CacheItem[1] { cacheItem});
        }
        else
        {
            cacheItem.fields = shopItem.Fields;
        }

        Cache.Save(shopItemsTable);

        UpdateCount(_count - number);
    }

    public void InitCoreId()
    {
        ShopCacheTable shop = Cache.LoadByType<ShopCacheTable>();
        ShopCacheItem shopItem = new ShopCacheItem(shop.GetById(_id).Fields);
        coreId = shopItem.GetCoreId();
    }
}

