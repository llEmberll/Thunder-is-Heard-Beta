using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseSubsituableContractsBehaviour : ISubsituableContractsBehaviour
{
    public ResourcesProcessor resourcesProcessor;



    public virtual void Init(Contracts conductor)
    {
        resourcesProcessor = GameObject.FindGameObjectWithTag(Tags.resourcesProcessor).GetComponent<ResourcesProcessor>();
        FillContent(conductor);
    }


    public List<ContractItem> GetItems(Contracts conductor)
    {
        return conductor.items;
    }

    public bool IsAvailableToBuy(ContractItem item)
    {
        return resourcesProcessor.IsAvailableToBuy(item.costData);
    }

    public virtual void OnBuy(ContractItem item)
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

    public virtual void Toggle(Contracts conductor)
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

    public virtual void FillContent(Contracts conductor)
    {
        conductor.ClearItems();
        conductor.items = new List<ContractItem>();

        ContractCacheTable contractTable = Cache.LoadByType<ContractCacheTable>();
        foreach (var keyValuePair in contractTable.Items)
        {
            ContractCacheItem contractData = new ContractCacheItem(keyValuePair.Value.Fields);
            if (!IsContractTypeMatch(contractData.GetType(), conductor._contractType) || !CheckContractRequirements(contractData))
            {
                continue;
            }

            conductor.items.Add(conductor.CreateContractItem(contractData));
        }
    }

    public bool IsContractTypeMatch(string contractType, string targetType)
    {
        return string.Equals(contractType, targetType, StringComparison.OrdinalIgnoreCase);
    }

    public virtual bool CheckContractRequirements(ContractCacheItem contractData)
    {
        ContractRequirementsCacheTable requirementsTable = Cache.LoadByType<ContractRequirementsCacheTable>();
        CacheItem cacheItem = requirementsTable.GetById(contractData.GetExternalId());
        if (cacheItem == null)
        {
            Debug.Log("Can't find requirements for contract: " +  contractData.GetExternalId());
            return false;
        }

        ContractRequirementsCacheItem requirementsData = new ContractRequirementsCacheItem(cacheItem.Fields);
        string[] circumstances = requirementsData.GetData().Circumstances;
        return CheckContractCircumstancesRequirements(circumstances);
    }

    public virtual bool CheckContractCircumstancesRequirements(string[] circumstances)
    {
        return circumstances.Length == 0;
    }

    public virtual void OnInteractWithIdleComponent(ContractComponent component)
    {
        component.ToggleUI();
        component._conductor.Init(component.type, component.id);
    }

    public virtual void OnInteractWithWorkingComponent(ContractComponent component)
    {
        //Показать оставшееся время выполнения
        Debug.Log("working...");
    }

    public virtual void OnInteractWithFinishedComponent(ContractComponent component)
    {
        ProductsNotificationCacheTable productsNotificationCacheTable = Cache.LoadByType<ProductsNotificationCacheTable>();
        ProductsNotificationCacheItem productsCollectionData = productsNotificationCacheTable.FindBySourceObjectId(component.id);
        if (productsCollectionData == null)
        {
            throw new System.NotImplementedException("Contract component waiting for collection, but collectionData not found");
        }

        if (resourcesProcessor.IsAvailableToAddResources(productsCollectionData.GetGives()))
        {
            resourcesProcessor.AddResources(productsCollectionData.GetGives());
            resourcesProcessor.Save();
            ObjectProcessor.DeleteProductsNotificationByItemId(productsCollectionData.GetExternalId());
            ObjectProcessor.CreateProductsNotification(component.id, ProductsNotificationTypes.idle);
            EventMaster.current.OnCollectProducts(productsCollectionData);
            EventMaster.current.OnChangeObjectOnBaseWorkStatus(component.id, WorkStatuses.idle);
        }

        else
        {
            Debug.Log("Сбор невозможен");

        }
    }
}
