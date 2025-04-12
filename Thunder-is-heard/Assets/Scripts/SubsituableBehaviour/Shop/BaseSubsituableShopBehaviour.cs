using System.Collections.Generic;
using UnityEngine;

public class BaseSubsituableShopBehaviour : ISubsituableShopBehaviour
{
    public ResourcesProcessor resourcesProcessor;

    public virtual List<ShopItem> GetItems(Shop conductor)
    {
        return conductor.items;
    }

    public virtual void Init(Shop conductor)
    {
        resourcesProcessor = GameObject.FindGameObjectWithTag(Tags.resourcesProcessor).GetComponent< ResourcesProcessor >();
        FillContent(conductor);
    }

    public virtual bool IsAvailableToBuy(ShopItem item)
    {
        return resourcesProcessor.IsAvailableToBuy(item.costData);
    }

    public virtual void OnBuy(ShopItem item)
    {
        if (item is ExposableShopItem) OnBuyExposableItem(item as ExposableShopItem);
        else
        {
            ObjectProcessor.OnBuyMaterial(item.coreId);
            resourcesProcessor.SubstractResources(item.costData);
            resourcesProcessor.Save();

            item.Substract();
        }
    }

    public virtual void OnBuyExposableItem(ExposableShopItem item)
    {
        item.CreatePreview();

        EventMaster.current.OnBuildMode();
        EventMaster.current.ToggledOffBuildMode += item.OnCancelExposing;
        EventMaster.current.ObjectExposed += item.OnObjectExposed;
    }

    public virtual void Toggle(Shop conductor)
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

    public virtual void FillContent(Shop conductor)
    {
        conductor.ClearItems();
        conductor.items = new List<ShopItem>();

        //Согласно рангу, проверить лимиты купленных объектов и сделать нужное количество

        ShopCacheTable shopTable = Cache.LoadByType<ShopCacheTable>();
        foreach (var keyValuePair in shopTable.Items)
        {
            ShopCacheItem shopItemData = new ShopCacheItem(keyValuePair.Value.Fields);
            string type = shopItemData.GetType();

            CacheTable itemTable = Cache.LoadByName(type);
            CacheItem item = itemTable.GetById(shopItemData.GetCoreId());

            switch (type)
            {
                case "Build":
                    BuildCacheItem buildData = new BuildCacheItem(item.Fields);
                    BuildShopItem build = conductor.CreateBuild(shopItemData, buildData);
                    conductor.items.Add(build);
                    break;
                case "Unit":
                    UnitCacheItem unitData = new UnitCacheItem(item.Fields);
                    UnitShopItem unit = conductor.CreateUnit(shopItemData, unitData);
                    conductor.items.Add(unit);
                    break;
                case "Material":
                    MaterialCacheItem materialData = new MaterialCacheItem(item.Fields);
                    MaterialShopItem material = conductor.CreateMaterial(shopItemData, materialData);
                    conductor.items.Add(material);
                    break;
            }
        }
    }
}
