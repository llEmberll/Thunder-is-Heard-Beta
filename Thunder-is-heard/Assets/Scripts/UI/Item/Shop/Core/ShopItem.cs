using TMPro;
using UnityEngine;
using System;
using System.Reflection;
using UnityEngine.UI;

public abstract class ShopItem : Item
{
    public ResourcesProcessor resourcesProcessor;

    public string coreId;

    public TMP_Text TmpCount, TmpDescription;

    public string description;

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
        TmpCount.text = count.ToString();
        TmpDescription.text = description;

        UpdateResources(cost, costData);
        UpdateResources(gives, givesData);

        base.UpdateUI();
    }

    public virtual void UpdateResources(Transform resourcesParent, ResourcesData resourcesData)
    {
        if (resourcesParent == null || resourcesData == null) return;  
        ClearResources(resourcesParent);

        Type type = resourcesData.GetType();
        FieldInfo[] resources = type.GetFields();

        foreach (FieldInfo resource in resources)
        {
            if (resource.FieldType == typeof(int))
            {
                int value = (int)resource.GetValue(resourcesData);
                if (value != 0)
                {
                    string resourceName = resource.Name;
                    CreateResourceElement(resourceName, value, resourcesParent);
                }
            }
        }
    }

    public void CreateResourceElement(string name, int count, Transform parent)
    {
        Sprite[] icons = Resources.LoadAll<Sprite>(Config.resources["resourcesIcons"]);
        Sprite resourceIcon = null;

        name = name.ToLower();
        foreach (Sprite icon in icons)
        {
            if (name.Contains(icon.name))
            {
                resourceIcon = icon;
            }
        }

        string beginPrefix = "";
        string endPrefix = "";
        if (parent == gives)
        {
            if (count < 0)
            {
                beginPrefix = "- ";
            }
            else
            {
                beginPrefix = "+ ";
            }

            if (name.Contains("max"))
            {
                endPrefix = " max";
            }
        }

        string countText = beginPrefix + count.ToString() + endPrefix;

        GameObject prefab = Resources.Load<GameObject>(Config.resources["resourceForUIItem"]);
        GameObject resourceObject = Instantiate(prefab);
        resourceObject.transform.SetParent(parent, false);

        Image resourceImage = resourceObject.transform.Find("Icon").GetComponent<Image>();
        resourceImage.sprite = resourceIcon;

        GameObject resourceCount = resourceObject.transform.Find("Count").gameObject;
        TMP_Text resourceCountText = resourceCount.GetComponent<TMP_Text>();
        resourceCountText.text = countText;
    }

    public void ClearResources(Transform parent)
    {
        foreach (Transform child in parent) 
        {
            Destroy(child.gameObject);
        }
    }

    public override void UpdateCount(int newCount)
    {
        base.UpdateCount(newCount);
        TmpCount.text = newCount.ToString();
    }

    public virtual void Substract(int number = 1)
    {
        ShopCacheTable shopItemsTable = Cache.LoadByType<ShopCacheTable>();
        CacheItem cacheItem = shopItemsTable.GetById(id);
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

        UpdateCount(count - number);
    }

    public void InitCoreId()
    {
        ShopCacheTable shop = Cache.LoadByType<ShopCacheTable>();
        ShopCacheItem shopItem = new ShopCacheItem(shop.GetById(id).Fields);
        coreId = shopItem.GetCoreId();
    }
}

