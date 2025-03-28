using System.Collections.Generic;
using UnityEngine;

public class BaseSubsituableShopBehaviour : ISubsituableShopBehaviour
{
    public ResourcesProcessor resourcesProcessor;

    public virtual List<ShopItem> GetItems(Shop conductor)
    {
        return conductor.items;
    }

    public virtual void Init()
    {
        resourcesProcessor = GameObject.FindGameObjectWithTag(Tags.resourcesProcessor).GetComponent< ResourcesProcessor >();
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
}
