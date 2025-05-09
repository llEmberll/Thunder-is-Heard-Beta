using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseSubsituableUnitProductionsBehaviour : ISubsituableUnitProductionsBehaviour
{
    public ResourcesProcessor resourcesProcessor;



    public virtual void Init(UnitProductions conductor)
    {
        resourcesProcessor = GameObject.FindGameObjectWithTag(Tags.resourcesProcessor).GetComponent<ResourcesProcessor>();
        FillContent(conductor);
    }


    public virtual List<UnitProductionItem> GetItems(UnitProductions conductor)
    {
        return conductor.items;
    }

    public bool IsAvailableToBuy(UnitProductionItem item)
    {
        return resourcesProcessor.IsAvailableToBuy(item.costData);
    }

    public virtual void OnBuy(UnitProductionItem item)
    {
        DateTime startTime = DateTime.Now;
        DateTime endTime = startTime.AddSeconds(item._duration);

        ProcessWorker.CreateProcess(
            item.Type,
            ProcessTypes.component,
            item._sourceObjectId,
            startTime,
            endTime,
            new ProcessSource(item.Type, item._id)
            );
        resourcesProcessor.SubstractResources(item.costData);
        resourcesProcessor.Save();
    }

    public virtual void Toggle(UnitProductions conductor)
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

    public virtual void FillContent(UnitProductions conductor)
    {
        conductor.ClearItems();
        conductor.items = new List<UnitProductionItem>();

        UnitProductionCacheTable unitProductionTable = Cache.LoadByType<UnitProductionCacheTable>();
        foreach (var keyValuePair in unitProductionTable.Items)
        {
            UnitProductionCacheItem unitProductionData = new UnitProductionCacheItem(keyValuePair.Value.Fields);
            if (unitProductionData.GetType() != conductor._unitProductionType)
            {
                continue;
            }

            conductor.items.Add(conductor.CreateUnitProductionItem(unitProductionData));
        }
    }

    public virtual void OnInteractWithIdleComponent(UnitProductionComponent component)
    {
        component.ToggleUI();
        component._conductor.Init(component.type, component.id);
    }

    public virtual void OnInteractWithWorkingComponent(UnitProductionComponent component)
    {
        //Показать оставшееся время выполнения
        Debug.Log("working...");
    }

    public virtual void OnInteractWithFinishedComponent(UnitProductionComponent component)
    {
        ProductsNotificationCacheTable productsNotificationCacheTable = Cache.LoadByType<ProductsNotificationCacheTable>();
        ProductsNotificationCacheItem productsCollectionData = productsNotificationCacheTable.FindBySourceObjectId(component.id);
        if (productsCollectionData == null)
        {
            throw new System.NotImplementedException("Unit production component waiting for collection, but collectionData not found");
        }

        ObjectProcessor.AddUnitToInventory(productsCollectionData.GetUnitId());
        ObjectProcessor.DeleteProductsNotificationByItemId(productsCollectionData.GetExternalId());
        ObjectProcessor.CreateProductsNotification(component.id, ProductsNotificationTypes.idle);
        EventMaster.current.OnCollectUnit(productsCollectionData);
        EventMaster.current.OnChangeObjectOnBaseWorkStatus(component.id, WorkStatuses.idle);
    }
}
